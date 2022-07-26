using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AudioSampler.AudioFile
{
    public class Wave
    {


		[StructLayout(LayoutKind.Sequential)]
		internal class WavHeader
		{
			public const UInt32 ChunkIdRIFF = 0x46464952;
			public const UInt32 ChunkFormatWave = 0x45564157;
			public const UInt32 SubchunkIdFmt = 0x20746d66;
			public const UInt16 AudioFormatPCM = 1;

			public WavHeader(Stream src)
            {
				var headerSize = Marshal.SizeOf(this);

				var buffer = new byte[headerSize];
				src.Read(buffer, 0, headerSize);

				var headerPtr = Marshal.AllocHGlobal(headerSize);
				Marshal.Copy(buffer, 0, headerPtr, headerSize);
				Marshal.PtrToStructure(headerPtr, this);
				Marshal.FreeHGlobal(headerPtr);
			}

			public WavHeader(int dataSize, int numChannels = 2, int bitsPerSample = 16, int sampleRate = 44100)
            {
				_ChunkId = ChunkIdRIFF;
				_ChunkSize = (uint)(4 + (8 + _Subchunk1Size) + (8 + dataSize));
				_Format = ChunkFormatWave;
				_Subchunk1Id = SubchunkIdFmt;
				_AudioFormat = AudioFormatPCM;
				_NumChannels = (UInt16)numChannels;
				_SampleRate = (UInt16)sampleRate;
				_ByteRate = (UInt32)(sampleRate * numChannels * bitsPerSample / 8);
				_BlockAlign = (UInt16)(bitsPerSample * numChannels / 8);
				_BitsPerSample = (UInt16)(bitsPerSample);

				_Subchunk2Size = (uint)dataSize;
			}

			public void Write(Stream dst)
            {
				var headerSize = Marshal.SizeOf(this);

				var headerPtr = Marshal.AllocHGlobal(headerSize);
				Marshal.StructureToPtr(this, headerPtr, false);

				byte[] data = new byte[headerSize];
				Marshal.Copy(headerPtr, data, 0, data.Length);
				Marshal.FreeHGlobal(headerPtr);
			}

			public UInt32 _ChunkId;
			public UInt32 _ChunkSize;
			public UInt32 _Format;
			public UInt32 _Subchunk1Id;
			public UInt32 _Subchunk1Size;
			public UInt16 _AudioFormat;
			public UInt16 _NumChannels;
			public UInt32 _SampleRate;
			public UInt32 _ByteRate;
			public UInt16 _BlockAlign;
			public UInt16 _BitsPerSample;
			public UInt32 _Subchunk2Id;
			public UInt32 _Subchunk2Size;
		}
	}
}
