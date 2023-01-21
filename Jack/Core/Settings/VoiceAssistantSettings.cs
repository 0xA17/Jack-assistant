using System;

namespace Jack.Core.Settings
{
    public class VoiceAssistantSettings
    {
        //[JsonProperty(Required = Required.Always)]
        public String ModelFolder = "LightModel";//LightModel//model
        public String SelectedAudioInDevice = "";
        public String SelectedAudioOutDevice = "";
        public Int32 AudioInSampleRate = 16000;
        public Int32 AudioOutSampleRate = 44100;
        public Int32 DefaultSuccessRate = 90;
        public String VoiceName = "Evgeniy-Rus";
        public String SpeakerCulture = "ru-RU";
        public Int32 CommandAwaitTime = 10;
        public Int32 NextWordAwaitTime = 3;
        public Int32 VoskLogLevel = -1;
    }
}