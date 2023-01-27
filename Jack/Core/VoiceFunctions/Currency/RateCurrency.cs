using Jack.Core.Dune;
using Jack.Tools.Web;
using Jack.Tools.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Jack.Core.VoiceFunctions.Currency
{
    class RateCurrency
    {
        #region Переменные

        private enum RateCurrencyType : Byte
        {
            USD,
            EUR,
            KZT,
            CNY
        }

        private const String CurrencyServiceUrl = "http://www.cbr.ru/scripts/XML_daily.asp";

        #endregion

        public static String GetRateCurrencyAnswer(String voice)
        {
            if (String.IsNullOrEmpty(voice))
            {
                return String.Empty;
            }

            var rateCurrencyType = GetRateCurrencyType(voice);

            if (rateCurrencyType is null)
            {
                return String.Empty;
            }

            var currencyRate = GetCurrencyRate(CurrencyServiceUrl, rateCurrencyType.ToString(), Byte.MinValue).Result;

            if (currencyRate.Item1 == -1)
            {
                return String.Empty;
            }

            return $"{currencyRate.Item2.ToString().Replace(",", " точка ")} рублей";
        }
        
        private static RateCurrencyType? GetRateCurrencyType(String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return null;
            }

            foreach (var targetСurrencyNode in Commands.CommandDictionary.Elements("Сurrency"))
            {
                for (Int32 targetСurrency = 0; targetСurrency < targetСurrencyNode.Nodes().Count(); targetСurrency++)
                {
                    var tempElement = targetСurrencyNode.Elements().ElementAt(targetСurrency);

                    if (XMLTools.TextIsContains(text, tempElement))
                    {
                        if (!Enum.TryParse(tempElement.Name.LocalName, out RateCurrencyType rateCurrencyType))
                        {
                            break;
                        }

                        return rateCurrencyType;
                    }
                }
            }

            return null;
        }

        private static async Task<(Int32, Single)> GetCurrencyRate(String currencyServiceUrl, String curencyCode, Int32 decimalRound)
        {
            var currencyRates = await GetActualRate(currencyServiceUrl);

            if (currencyRates is null)
            {
                return (-1, -1);
            }

            var currencyRate = currencyRates.FirstOrDefault(n => n.CurrencyCode == curencyCode);

            if (!Int32.TryParse(currencyRate.Nominal, out var nominal))
            {
                return (-1, -1);
            }

            var value = currencyRate.Value.Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo?.NumberDecimalSeparator ?? String.Empty);

            if (!Single.TryParse(value, out var resultVal))
            {
                return (-1, -1);
            }

            return (nominal, (Single)Math.Round(resultVal, 1));
        }

        private static async Task<CurrencyRateModel[]> GetActualRate(String currencyServiceUrl)
        {
            try
            {
                //var request = (HttpWebRequest)WebRequest.Create(currencyServiceUrl);
                if (!WebTools.RequestTryCreate(currencyServiceUrl, out HttpWebRequest request))
                {
                    return null;
                }

                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var response = await request.GetResponseAsync())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var xmlDoc = await reader.ReadToEndAsync();
                            var xdoc = XDocument.Parse(xmlDoc);

                            // получаем корневой узел
                            XElement? rates = xdoc.Element("ValCurs");
                            var currencyRates = new List<CurrencyRateModel>();

                            if (rates != null)
                            {
                                // проходим по всем элементам person
                                foreach (var rate in rates.Elements("Valute"))
                                {
                                    var code = rate.Element("CharCode");
                                    var name = rate.Element("Name");
                                    var nom = rate.Element("Nominal");
                                    var val = rate.Element("Value");

                                    var newRate = new CurrencyRateModel()
                                    {
                                        CurrencyCode = code?.Value,
                                        CurrencyName = name?.Value,
                                        Nominal = nom?.Value,
                                        Value = val?.Value
                                    };

                                    currencyRates.Add(newRate);
                                }
                            }

                            return currencyRates.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from {currencyServiceUrl}: {ex.Message}");

                return null;
            }
        }
    }
}