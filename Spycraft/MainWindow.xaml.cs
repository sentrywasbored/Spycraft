using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Pfim;
using LibVLCSharp.Shared;
using Microsoft.Win32;
using System.Windows.Media;

namespace Spycraft
{
    public partial class MainWindow : Window
    {
        private LibVLC _libVLC;
        private LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
        private VobPlayer _vobPlayer;
        private AviPlayer _aviPlayer;

        public MainWindow()
        {
            InitializeComponent();
            Core.Initialize();
            _libVLC = new LibVLC();
            _mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVLC);
            VlcView.MediaPlayer = _mediaPlayer;
            _vobPlayer = new VobPlayer(_libVLC, _mediaPlayer);
            _aviPlayer = new AviPlayer(_libVLC, _mediaPlayer);
            imageControl.Source = LoadTGA(@".\VOL\60500.TGA");
            startbox.Text = "-1";
            endbox.Text = "-1";
            
        }

        public BitmapSource LoadTGA(string filePath)
        {
            using (var image = Pfim.Pfimage.FromFile(filePath))
            {
                PixelFormat format;
                byte[] data;

                switch (image.Format)
                {
                    case Pfim.ImageFormat.Rgba32:
                        format = PixelFormats.Bgra32;
                        data = image.Data;
                        break;
                    case Pfim.ImageFormat.Rgb24:
                        format = PixelFormats.Bgr24;
                        data = image.Data;
                        break;
                    case Pfim.ImageFormat.R5g5b5:
                        format = PixelFormats.Bgr555;
                        data = ConvertR5g5b5ToBgr555(image.Data);
                        break;
                    default:
                        throw new NotSupportedException($"Image format not supported: {image.Format}");
                }

                return BitmapSource.Create(
                    image.Width,
                    image.Height,
                    96,
                    96,
                    format,
                    null,
                    data,
                    image.Stride);
            }
        }



        private byte[] ConvertR5g5b5ToBgr555(byte[] data)
        {
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i += 2)
            {
                ushort pixel = BitConverter.ToUInt16(data, i);
                ushort b = (ushort)((pixel & 0x7C00) >> 10);
                ushort g = (ushort)((pixel & 0x03E0) >> 5);
                ushort r = (ushort)(pixel & 0x001F);
                ushort bgr555 = (ushort)((b << 10) | (g << 5) | r);
                byte[] converted = BitConverter.GetBytes(bgr555);
                result[i] = converted[0];
                result[i + 1] = converted[1];
            }
            return result;
        }



        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @".\AVIWAV\" + filebox.Text;
            double startDouble = Convert.ToDouble(startbox.Text);
            double endDouble = Convert.ToDouble(endbox.Text);
            if (_aviPlayer.IsAviFile(filePath))
            {
                if (startbox.Text == "-1" || endbox.Text == "-1")
                {
                    _aviPlayer.PlayAVI(filePath);
                }
                else
                {
                    _aviPlayer.PlayAVI(filePath, startDouble, endDouble);
                }
            }
            else if (_vobPlayer.IsVobFile(filePath))
            {
                if (startbox.Text == "-1" || endbox.Text == "-1")
                {
                    _vobPlayer.PlayVob(filePath);
                }
                else
                {
                    _vobPlayer.PlayVob(filePath, startDouble, endDouble);
                }
            }
            else
            {
                MessageBox.Show("Unrecognized Filetype");
            }

           // _aviPlayer.PlayAVI(@".\AVIWAV\41.AVI");
           
            
            /*OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "MPEG2|*.vob|DUCK|*.avi|WAV|*.wav|All Files|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {

                string filePath = openFileDialog.FileName;
                Trace.WriteLine(filePath);

                  if (_aviPlayer.IsAviFile(filePath))
                  {
                     _aviPlayer.PlayAVI(@"J:\\test1.vob");
                  }
                  else
                  {
                      Media media = new Media(_libVLC, filePath, FromType.FromPath);
                     _mediaPlayer.Play(media);
                  }
            }*/
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {

           // _aviPlayer.PlayAVI(@".\AVIWAV\79000.AVI", 1380, 1433);
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
            }

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Stop();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _mediaPlayer.Dispose();
            _libVLC.Dispose();
            base.OnClosed(e);
        }

        private void FileBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void StartBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void endbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}