using Jack.Core.Settings;
using PluginInterface;
using System;

namespace Jack.Tools.StringTLS
{
    class StringTools
    {
        /// <summary>
        /// Проверяет на наличие изменений между двумя массивами.
        /// </summary>
        /// <param name="arrOne">Первый массив</param>
        /// <param name="arrTwo">Второй массив</param>
        /// <returns>False - если различия есть, иначе - True</returns>
        public static Boolean CompareArray(String[] arrOne, String[] arrTwo)
        {
            if (arrOne == null || arrTwo == null ||
                arrOne.Length != arrTwo.Length)
            {
                return false;
            }

            for (Int32 i = 0; i < arrOne.Length; i++)
            {
                if (arrOne[i] != arrTwo[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static Int16 GetValueFromStr(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return 0;
            }

            Int16 result = 0;

            var stringToNumberConvertor = TextToNumberFactory.GetTextToNumberConvertor(new VoiceAssistantSettings().SpeakerCulture);

            foreach (var item in value.Split(' '))
            {
                result += (Int16)stringToNumberConvertor.ConvertStringToNumber(item, 90);
            }

            return result;
        }

        /// <summary>
        /// Формирует массив строк в строку.
        /// </summary>
        /// <param name="stringsArr">Целевой массив строк</param>
        /// <returns>Итоговая строка</returns>
        public static String GetDataFromStrArr(String[] stringsArr)
        {
            if (stringsArr == null)
            {
                return String.Empty;
            }

            var targetStr = String.Empty;

            foreach (var item in stringsArr)
            {
                targetStr += $"{item} ";
            }

            return targetStr.Trim(' ');
        }

        /// <summary>
        /// Проверка строки на пустые символы.
        /// </summary>
        /// <param name="str">Целевая  строка</param>
        /// <returns>True - проверка пройдена, иначе - False</returns>
        public static Boolean StringValidation(String str)
        {
            return !String.IsNullOrEmpty(str) && str.Replace(" ", String.Empty).Replace(" ", String.Empty) != String.Empty;
        }

        /// <summary>
        /// Проверка на разрешенные символы.
        /// </summary>
        /// <param name="str">Целевая строка</param>
        /// <param name="exclusiveCharacters">Массив допустимых символов</param>
        /// <returns>True - если присутствуют лишние символы, иначе - False</returns>
        public static Boolean CharacterCheck(String str, String[] exclusiveCharacters)
        {
            if (exclusiveCharacters == null || String.IsNullOrEmpty(str))
            {
                return true;
            }

            foreach (var item in exclusiveCharacters)
            {
                str = str.Replace(item, String.Empty);
            }

            if (str != String.Empty)
            {
                return true;
            }

            return false;
        }

        public static String RemoveCharacter(String origString, UInt16 length)
        {
            if (origString is null ||
                origString.Length <= length)
            {
                return String.Empty;
            }

            return origString.Remove(origString.Length - length, length);
        }

        /// <summary>
        /// Возвращает случайный элемент массива.
        /// </summary>
        /// <param name="arrayWords">Массив строк.</param>
        /// <returns>Случайный элемент из массива.</returns>
        public static String GiveRandText(String[] arrayWords)
        {
            if (arrayWords.Length <= 0 || arrayWords == null)
            {
                return String.Empty;
            }

            return arrayWords[new Random().Next(0, arrayWords.Length - 1)];
        }
    }
}
