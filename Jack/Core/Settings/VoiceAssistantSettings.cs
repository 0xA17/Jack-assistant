using System;

namespace Jack.Core.Settings
{
    static class VoiceAssistantSettings
    {
        public static String ModelFolder = "LightModel";//LightModel//model
        public static String SelectedAudioInDevice = "";
        public static String SelectedAudioOutDevice = "";
        public static Int32 AudioInSampleRate = 16000;
        public static Int32 AudioOutSampleRate = 44100;
        public static String VoiceName = "Artemiy";//Artemiy//Evgeniy-Rus
        public static String SpeakerCulture = "ru-RU";
        public static Int32 VoskLogLevel = -1;
    }
}