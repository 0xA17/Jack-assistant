using System;

namespace PluginInterface
{
    public interface IAudioOutSingleton
    {
        void PlayFile(String audioFile, Boolean exclusive = true);
        void Speak(String text, Int16 rate = 0, Boolean exclusive = true);
        void PlayDataBuffer(Byte[] data, Boolean exclusive = true);
    }
}