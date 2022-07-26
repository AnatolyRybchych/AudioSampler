using AudioSampler.PlatformDependenent;
using AudioSampler.WaveMath;
using System;
using System.Runtime.InteropServices;
using static AudioSampler.AudioFile.Wave;
using static AudioSampler.WaveMath.AudioWave;

namespace AudioSampler
{
	class Program
	{
		static void Main(string[] args)
		{
			TimeSpan time = TimeSpan.Zero;
			List<WaveSample> samples = new List<WaveSample>();
            while (time < TimeSpan.FromSeconds(2.0))
            {
				samples.Add(new WaveSample((Math.Sin(Math.PI * 200 * (time / TimeSpan.FromSeconds(1))) + 1.0) * 0.5, 0.0));

				time += AudioWave.Period;
            }

			new AudioPlayer().PlayWave(new AudioWave(samples.ToArray()).ToWave(TimeSpan.FromSeconds(2), false).GetDataWithHeader());
		}
	}
}