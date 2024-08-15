using System;
using System.IO;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

namespace Spycraft
{
    public class AviPlayer
    {
        private readonly LibVLC _libVLC;
        private readonly MediaPlayer _mediaPlayer;
    

        public AviPlayer(LibVLC libVLC, MediaPlayer mediaPlayer)
        {
            _libVLC = libVLC;
            _mediaPlayer = mediaPlayer;
        }

        public bool IsAviFile(string filePath)
        {
            return Path.GetExtension(filePath).Equals(".avi", StringComparison.OrdinalIgnoreCase);
        }

        public async void PlayAVI(string filePath)
        {
            Media videoMedia = new Media(_libVLC, filePath, FromType.FromPath);
            
            videoMedia.AddOption(":file-caching=1");
            
            _mediaPlayer.Play(videoMedia);
        }

        public async void PlayAVI(string filePath, double startFrame, double endFrame)
        {
            Media videoMedia = new Media(_libVLC, filePath, FromType.FromPath);
            double startSecond = startFrame / 15;
            double endSecond = endFrame / 15;
            videoMedia.AddOption(":file-caching=1");
            videoMedia.AddOption(":start-time="+startSecond);
            videoMedia.AddOption(":stop-time="+endSecond);
            _mediaPlayer.Play(videoMedia);
        }
    }
}
