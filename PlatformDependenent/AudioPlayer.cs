using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AudioSampler.PlatformDependenent
{
    public class AudioPlayer
    {
        public AudioPlayer()
        {

        }

        private const uint SND_MEMMORY = 0x0004;

        [DllImport("Winmm.dll")]
        private static extern bool sndPlaySound([MarshalAs(UnmanagedType.LPArray)] byte[] sound, uint options = SND_MEMMORY);

        public void PlayWave(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);

            sndPlaySound(data);
        }

        public void PlayWave(byte[] data)
        {
            sndPlaySound(data);
        }
    }
}
