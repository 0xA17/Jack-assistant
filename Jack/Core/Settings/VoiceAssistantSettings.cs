using System;

namespace Jack.Core.Settings
{
    public class VoiceAssistantSettings
    {
        //[JsonProperty(Required = Required.Always)]
        public String ModelFolder = "model";
        public String SelectedAudioInDevice = "";
        public String SelectedAudioOutDevice = "";
        public Int32 AudioInSampleRate = 16000;
        public Int32 AudioOutSampleRate = 44100;
        public String[] CallSign = { "Вася" };
        public Int32 DefaultSuccessRate = 90;
        public String VoiceName = "Evgeniy-Rus";
        public String SpeakerCulture = "ru-RU";
        public String PluginsFolder = "plugins";
        public String PluginFileMask = "*Plugin.dll";
        public String StartSound = "AssistantStart.wav";
        public String MisrecognitionSound = "Misrecognition.wav";
        public Int32 CommandAwaitTime = 10;
        public Int32 NextWordAwaitTime = 3;
        public Int32 VoskLogLevel = -1;
        public String CommandNotRecognizedMessage = "Команда не распознана";
        public String CommandNotFoundMessage = "Команда не найдена";
        public Boolean AllowPluginsToListenToSound = false;
        public Boolean AllowPluginsToListenToWords = false;
    }
}
