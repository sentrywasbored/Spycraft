using LibVLCSharp.Shared;
using LibVLCSharp.Shared.MediaPlayerElement;
using LibVLCSharp.WPF;
using LibVLCSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Spycraft
{
    internal class VobPlayer
    {
        private readonly LibVLC _libVLC;
        private readonly MediaPlayer _mediaPlayer;

        public VobPlayer(LibVLC libVLC, MediaPlayer mediaPlayer)
        {
            _libVLC = libVLC;
            _mediaPlayer = mediaPlayer;
        }

        public bool IsVobFile(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".vob", StringComparison.OrdinalIgnoreCase);
        }

        public async void PlayVob(string filePath)
        {
            Media videoMedia = new Media(_libVLC, filePath, FromType.FromPath);
            _mediaPlayer.Play(videoMedia);
        }

        public async void PlayVob(string filePath, double startFrame, double endFrame)
        {
            Media videoMedia = new Media(_libVLC, filePath, FromType.FromPath);
            double startSecond = startFrame / 29.97f;
            double endSecond = endFrame / 29.97f; 
            videoMedia.AddOption(":start-time="+startSecond);
            videoMedia.AddOption(":stop-time="+endSecond);
            _mediaPlayer.Play(videoMedia);
        }
    }
}
