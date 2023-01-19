using System;
using System.Threading;
using Jack.Core.ThreadUtils;
using System.Windows.Controls;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using Jack.Tools.StringTLS;
using NAudio.Wave;
using Vosk;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Jack.Core.Settings;

namespace Jack.Core.Dune
{
    class SpeechEngine
    {
        #region Переменные

        private static VoskRecognizer _voiceRecognition;
        private static WaveOut _waveOut;
        private static VoiceAssistantSettings _appConfig;
        private static AudioOutSingleton _audioOut;
        private static WaveInEvent _waveIn;
        private static readonly object SyncRoot = new Object();

        public static Boolean RecognizeState = true;
        private static SpeechSynthesizer Synthesizer = new SpeechSynthesizer();

        #endregion

        static SpeechEngine()
        {
            _appConfig = new VoiceAssistantSettings();
            SetDefoltInputDevice();
            SetDefoltOutDevice();

            _waveIn = InitAudioInput();
            _voiceRecognition = InitSpeechToText();
            if (_voiceRecognition == null)
                return;

            _audioOut = InitTextToSpeech(_waveOut);

            AddSpeechRecogniz();
            SetDefaultSynthesizer();
            SetSynthesizerRate(Byte.MinValue);
            GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.HelloAnswer), MainWindow.GetInstance().DuneAnswer);
            StartRecognize();
        }

        private static void ProcessAudioInput(object s, WaveInEventArgs waveEventArgs)
        {
            lock (SyncRoot)
            {
                VoskResult newWords;
                // simulated words from external source
                if (s is VoskResult simulatedInput)
                {
                    newWords = simulatedInput;
                }
                // naturally spoken words
                else
                {
                    var recognitionResult = _voiceRecognition.AcceptWaveform(waveEventArgs.Buffer, waveEventArgs.BytesRecorded);

                    if (!recognitionResult)
                    {
                        return;
                    }

                    var jsonResult = _voiceRecognition.Result();
                    newWords = JsonConvert.DeserializeObject<VoskResult>(jsonResult);
                }

                if (newWords == null || string.IsNullOrEmpty(newWords.text))
                {
                    return;
                }

                Console.WriteLine($"Recognized words: {newWords.text}");
                Commands.CommandProcessing(newWords.text);
            }
        }

        private static AudioOutSingleton InitTextToSpeech(WaveOut waveOut)
        {
            var synthesizer = new SpeechSynthesizer();
            Console.WriteLine("\r\nAvailable voices:");

            var voiceSelected = false;
            foreach (var voice in synthesizer.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                Console.WriteLine(
                    $"- Id: {info.Id} | Name: {info.Name} | Age: {info.Age} | Gender: {info.Gender} | Culture: {info.Culture} ");

                if (!string.IsNullOrEmpty(_appConfig.VoiceName))
                {
                    if (info.Name == _appConfig.VoiceName)
                    {
                        synthesizer.SelectVoice(_appConfig.VoiceName);
                        voiceSelected = true;
                    }
                }

                if (!voiceSelected
                    && !string.IsNullOrEmpty(_appConfig.SpeakerCulture)
                    && info.Culture.Name.StartsWith(_appConfig.SpeakerCulture))
                {
                    synthesizer.SelectVoice(info.Name);
                }
            }

            var builder = new PromptBuilder();
            Console.WriteLine($"Selected voice: {synthesizer.Voice.Name}");

            //create audio output interface singleton
            var audioOut = AudioOutSingleton.GetInstance(_appConfig.SpeakerCulture, synthesizer, builder, waveOut,
                _appConfig.AudioOutSampleRate);

            return audioOut;
        }

        private static VoskRecognizer InitSpeechToText()
        {
            // set -1 to disable logging messages
            Vosk.Vosk.SetLogLevel(_appConfig.VoskLogLevel);
            VoskRecognizer rec;

            if (!Directory.Exists(_appConfig.ModelFolder))
            {
                Console.WriteLine($"Voice recognition model folder missing: {_appConfig.ModelFolder}");

                return null;
            }

            try
            {
                var model = new Model(_appConfig.ModelFolder);
                rec = new VoskRecognizer(model, _appConfig.AudioInSampleRate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't load model (could it be missing?): {ex.Message}");

                return null;
            }

            rec.SetMaxAlternatives(0);
            rec.SetWords(true);

            return rec;
        }

        private static WaveOut InitAudioOutput()
        {
            var audioDeviceNumber = WaveOut.DeviceCount;
            var recordOutputs = new Dictionary<int, string>(audioDeviceNumber + 1);
            var selectedDevice = -1;
            Console.WriteLine("\r\nAvailable output devices:");

            for (var n = -1; n < audioDeviceNumber; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                recordOutputs.Add(n, caps.ProductName);
                Console.WriteLine($"- {n}: {caps.ProductName}");

                if (!string.IsNullOrEmpty(_appConfig.SelectedAudioOutDevice) &&
                    caps.ProductName.StartsWith(_appConfig.SelectedAudioOutDevice))
                {
                    selectedDevice = n;
                }
            }

            recordOutputs.TryGetValue(selectedDevice, out var outDevice);

            var waveOut = new WaveOut
            {
                DeviceNumber = selectedDevice
            };

            Console.WriteLine($"Selected output device: {outDevice}");

            return waveOut;
        }

        private static WaveInEvent InitAudioInput()
        {
            var recordDeviceNumber = WaveIn.DeviceCount;
            var recordInputs = new Dictionary<int, string>(recordDeviceNumber + 1);
            Console.WriteLine("\r\nAvailable input devices:");
            var selectedDevice = -1;

            for (var n = -1; n < recordDeviceNumber; n++)
            {
                var caps = WaveIn.GetCapabilities(n);
                recordInputs.Add(n, caps.ProductName);
                Console.WriteLine($"- {n}: {caps.ProductName}");

                if (!string.IsNullOrEmpty(_appConfig.SelectedAudioInDevice) &&
                    caps.ProductName.StartsWith(_appConfig.SelectedAudioInDevice))
                {
                    selectedDevice = n;
                }
            }

            recordInputs.TryGetValue(selectedDevice, out var inDevice);

            var waveIn = new WaveInEvent
            {
                DeviceNumber = selectedDevice,
                WaveFormat = new WaveFormat(_appConfig.AudioInSampleRate, 1)
            };

            Console.WriteLine($"Selected input device: {inDevice}");
            Console.WriteLine($"Stream settings: {waveIn.WaveFormat}");

            return waveIn;
        }

        #region Методы

        public static Boolean InitSpeaker() => true;

        public static void ChangeRecognizeState()
        {
            RecognizeState = !RecognizeState;
        }

        //private static void DetectChangeMicrophone()
        //{
        //    var recordDeviceNumber = WaveIn.DeviceCount;

        //    while (true)
        //    {
        //        var tmpDeviceNumber = WaveIn.DeviceCount;

        //        if (recordDeviceNumber != tmpDeviceNumber)
        //        {
        //            recordDeviceNumber = tmpDeviceNumber;
        //            StopRecognize();
        //            Thread.Sleep(100);
        //            SetDefInputDevice();
        //            Thread.Sleep(100);
        //            SetRecognizeMode(RecognizeMode.Multiple);
        //        }

        //        Thread.Sleep(5000);
        //    }
        //}

        public static Boolean SetSynthesizerRate(Int16 rate)
        {
            try
            {
                Synthesizer.Rate = rate;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean SetDefoltInputDevice()
        {
            try
            {
                _waveIn = InitAudioInput();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean SetDefoltOutDevice()
        {
            try
            {
                _waveOut = InitAudioOutput();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean SetDefaultSynthesizer()
        {
            if (Synthesizer is null)
            {
                return false;
            }

            try
            {
                Synthesizer.SetOutputToDefaultAudioDevice();
                RecognizeState = true;
            }
            catch
            {
                return false;
            }
            
            return true;
        }

        private static Boolean AddSpeechRecogniz()
        {
            try
            {
                _waveIn.DataAvailable += ProcessAudioInput;
                _waveIn.RecordingStopped += (s, a) => { _waveIn.Dispose(); };
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void GiveSpeackText(String answer, TextBlock textBlock)
        {
            if (String.IsNullOrEmpty(answer) || 
                textBlock is null)
            {
                return;
            }

            var tmpTheard = new Thread(() =>
            {
                SpeackText(answer, textBlock);
            });

            tmpTheard.IsBackground = true;
            tmpTheard.Start();
        }

        public static void GiveSpeackText(String answer)
        {
            var textBlock = MainWindow.GetInstance().DuneAnswer;

            if (String.IsNullOrEmpty(answer) || textBlock is null)
            {
                return ;
            }

            var tmpTheard = new Thread(() =>
            {
                SpeackText(answer, textBlock);
            });

            tmpTheard.IsBackground = true;
            tmpTheard.Start();
        }

        private static void SpeackText(String text, TextBlock textBlock)
        {
            if (String.IsNullOrEmpty(text) || 
                textBlock is null)
            {
                return;
            }

            Invokes.EditTextBlock_Text(textBlock, text);

            if (RecognizeState)
            {
                Thread.Sleep(100);
                StopRecognize();
                SynthesizerSpeack(text);
                if (RecognizeState)
                {
                    StartRecognize();
                }
            }

            Thread.Sleep(100);
            Invokes.EditTextBlock_Text(textBlock, "...");
        }

        private static Boolean SynthesizerSpeack(String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            try
            {
                _audioOut?.Speak(text);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean StopRecognize()
        {
            try
            {
                _waveIn.StopRecording();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean StartRecognize()
        {
            try
            {
                _waveIn.StartRecording();
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}