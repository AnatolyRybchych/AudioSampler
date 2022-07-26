using AudioSampler.AudioFile;
using AudioSampler.PlatformDependenent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioSampler.WaveMath
{
    public class AudioWave//
    {
        public WaveSample[] Samples;
        //192000 hz
        public static readonly TimeSpan Period = TimeSpan.FromSeconds(5.208333333333333e-06);

        public AudioWave(WaveSample[] samples)
        {
            Samples = samples;
        }

        public Wave ToWave(TimeSpan duration, bool looped, int sampleRate = 44100, int bitsPerSample = 16)
        {
            int srcSampleRate = (int)((double)TimeSpan.TicksPerSecond / Period.Ticks);

            List<WaveSample> dstSamples = Samples.Where((sample, id) => id % ((srcSampleRate / sampleRate) + 1) == 0).ToList();
            if (dstSamples.Count == 0) dstSamples.Add(new WaveSample(0.0, 0.0));

            TimeSpan dstSampleDuration = TimeSpan.FromSeconds(1.0 / sampleRate);

            int requiredSamplesCount = (int)(duration / dstSampleDuration);

            if(requiredSamplesCount < dstSamples.Count)
            {
                dstSamples = dstSamples.Where((s,i) => i < requiredSamplesCount).ToList();
            }
            else
            {
                int samplesLack = requiredSamplesCount - dstSamples.Count;
                int currLack = samplesLack;
                while (currLack > 0)
                {
                    dstSamples.Add(dstSamples[samplesLack - currLack]);
                    currLack--;
                }
            }

            List<byte> data = new List<byte>();

            switch (bitsPerSample)
            {
                case 8:
                    foreach(var sample in dstSamples)
                    {
                        data.Add((byte)(sample.SampleLeft * 255));
                        data.Add((byte)(sample.SampleRight * 255));
                    }
                    break;
                case 16:
                    foreach (var sample in dstSamples)
                    {
                        short valueL = (short)((sample.SampleLeft - 0.5) * 2.0 * short.MaxValue);
                        short valueR = (short)((sample.SampleRight - 0.5) * 2.0 * short.MaxValue);
                        data.Add((byte)(valueL & 0x00ff));
                        data.Add((byte)(valueL >> 8));
                        data.Add((byte)(valueR & 0x00ff));
                        data.Add((byte)(valueR >> 8));
                    }
                    break;
                case 32:
                    foreach (var sample in dstSamples)
                    {
                        int valueL = (int)((sample.SampleLeft - 0.5) * 2.0 * int.MaxValue);
                        int valueR = (int)((sample.SampleRight - 0.5) * 2.0 * int.MaxValue);
                        data.Add((byte)(valueL & 0x000000ff));
                        data.Add((byte)((valueL >> 8) & 0x000000ff));
                        data.Add((byte)((valueL >> 16) & 0x000000ff));
                        data.Add((byte)((valueL >> 24) & 0x000000ff));
                        data.Add((byte)(valueR & 0x000000ff));
                        data.Add((byte)((valueR >> 8) & 0x000000ff));
                        data.Add((byte)((valueR >> 16) & 0x000000ff));
                        data.Add((byte)((valueR >> 24) & 0x000000ff));
                    }
                    break;
                default:
                    throw new Exception("bitsPerSample");
            }

            return new Wave(
            new Wave.WavHeader(
                (int)(dstSamples.Count * 2 * bitsPerSample / 8),
                2,
                bitsPerSample,
                44100), data.ToArray()
           );
        }

        public class WaveSample
        {
            public ClamedFloat SampleLeft;
            public ClamedFloat SampleRight;

            public WaveSample(ClamedFloat sampleLeft, ClamedFloat sampleRight)
            {
                SampleLeft = sampleLeft;
                SampleRight = sampleRight;
            }
        }
    }
}
