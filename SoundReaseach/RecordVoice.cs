using NAudio.Wave;
using System;
using System.IO;

namespace SoundReaseach
{
    public class RecordVoice : IDisposable
    {
        private WaveInEvent waveIn;
        private MemoryStream memoryStream;
        private WaveFileWriter writer;
        private bool isRecording;

        public byte[] RecordedBytes { get; private set; }

        public RecordVoice()
        {
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 1); // 44.1 kHz, Mono
            waveIn.DataAvailable += OnDataAvailable;
            waveIn.RecordingStopped += OnRecordingStopped;
            memoryStream = new MemoryStream();
            isRecording = false;
        }

        public void StartRecording()
        {
            if (isRecording)
                return;

            memoryStream.SetLength(0); // Clear previous recordings
            writer = new WaveFileWriter(new IgnoreDisposeStream(memoryStream), waveIn.WaveFormat);
            waveIn.StartRecording();
            isRecording = true;
        }

        public void StopRecording()
        {
            if (!isRecording)
                return;

            waveIn.StopRecording();
            isRecording = false;
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (writer != null)
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                writer.Flush();
            }
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            writer?.Dispose();
            writer = null;

            // Store the recorded bytes
            RecordedBytes = memoryStream.ToArray();
        }

        public void Dispose()
        {
            waveIn?.Dispose();
            writer?.Dispose();
            memoryStream?.Dispose();
        }

        // Helper class to prevent MemoryStream from being disposed by WaveFileWriter
        private class IgnoreDisposeStream : Stream
        {
            private readonly Stream _innerStream;

            public IgnoreDisposeStream(Stream inner)
            {
                _innerStream = inner;
            }

            public override bool CanRead => _innerStream.CanRead;
            public override bool CanSeek => _innerStream.CanSeek;
            public override bool CanWrite => _innerStream.CanWrite;
            public override long Length => _innerStream.Length;
            public override long Position { get => _innerStream.Position; set => _innerStream.Position = value; }

            public override void Flush() => _innerStream.Flush();

            public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);

            public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);

            public override void SetLength(long value) => _innerStream.SetLength(value);

            public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                // Do not dispose the inner stream
            }
        }
    }
}
