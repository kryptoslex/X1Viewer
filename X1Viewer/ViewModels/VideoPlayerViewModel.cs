using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using LibVLCSharp.Shared;
using X1Viewer.Views;
using Xamarin.Forms;

namespace X1Viewer.ViewModels
{
    public class VideoPlayerViewModel : INotifyPropertyChanged
    {
        private static VideoPlayerViewModel _instance;
        private bool isDebug = false;

        ICommand _captureCommand;
        ICommand _recordCommand;

        public static VideoPlayerViewModel Instance => _instance ?? (_instance = new VideoPlayerViewModel());
        public event PropertyChangedEventHandler PropertyChanged;

        private LibVLC LibVLC { get; set; }
        Media currentMedia = null;
        private MediaPlayer _mediaPlayer;
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set
            {
                _mediaPlayer = value;
            }
        }

        public void OnAppearing()
        {
            GalleryBtnImageSource = GetImgSource();
        }

        private void Set<T>(string propertyName, ref T field, T value)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public string _streamUrl = "http://commondatastorage.googleapis.com/gtv-videos-bucket/CastVideos/dash/BigBuckBunnyVideo.mp4";

        public string StreamUrl
        {
            get { return _streamUrl; }
            set
            {
                if (_streamUrl != value)
                {
                    _streamUrl = value;
                }
            }
        }

        public bool _isRecording;

        public bool IsRecording
        {
            get => _isRecording;
            set
            {
                if (_isRecording != value)
                {
                    _isRecording = value;
                    OnPropertyChanged();

                }
            }
        }

        ImageSource _galleryBtnImageSource;

        public ImageSource GalleryBtnImageSource
        {
            get
            {
                _galleryBtnImageSource = GetImgSource();
                return _galleryBtnImageSource;
            }
            set
            {
                if (_galleryBtnImageSource != value)
                {
                    _galleryBtnImageSource = value;
                    OnPropertyChanged(nameof(GalleryBtnImageSource));

                }
            }
        }

        public VideoPlayerViewModel()
        {
            Console.WriteLine("------------------------ libvlc -------------------------");

            if (MediaPlayer != null)
            {
                MediaPlayer.Dispose();
                MediaPlayer = null;
            }
            if (LibVLC != null)
            {
                LibVLC.Dispose();
                LibVLC = null;
            }

            var optionsList = new List<string>();
            optionsList.Add("--input-repeat=65000"); //need a large number to make it look infinite
            LibVLC = new LibVLC(optionsList.ToArray());
            if (isDebug)
            {
                LibVLC.Log += (sender, e) => Console.WriteLine($"[{e.Level}] {e.Module}:{e.Message}");

            }


            MediaPlayer = new MediaPlayer(LibVLC);
            MediaPlayer.SnapshotTaken += MediaPlayer_SnapTaken;
        }


        private void MediaPlayer_SnapTaken(object sender, EventArgs e)
        {
            Debug.WriteLine("Snap taken, refresh UI");

            GalleryBtnImageSource = GetImgSource();
        }

        public ICommand RecordCommand
        {
            get
            {
                return _recordCommand ??
                     (_recordCommand =
                         new Command(
                           a =>
                           {

                               Debug.WriteLine("StreamUrl: " + StreamUrl);
                               Debug.WriteLine("isRecording: " + IsRecording);
                               IsRecording = !IsRecording;
                               Media mediaVid = MediaPlayer.Media;

                               mediaVid.AddOption("--sout-keep");
                               mediaVid.AddOption("--security-policy=1");
                               mediaVid.AddOption("--enable-sout");
                               mediaVid.AddOption("--enable-libdvdpsi");
                               mediaVid.AddOption("--enable-dvbpsi");
                               mediaVid.AddOption("--demux=avformat");
                               mediaVid.AddOption("--no-audio");

                               if (IsRecording)
                               {
                                   if (LibVLC != null)
                                   {
                                       mediaVid.AddOption(":sout=#transcode{vcodec=h264,scale=1}:duplicate{dst=display,select=video,dst=std{mux=avformat,access=file{no-overwrite},dst=" + generateFileName(".mp4") + "}}");
                                       MediaPlayer.Play(mediaVid);
                                       Debug.WriteLine("---------------------------------- START RECORDING --------------------------");

                                   }
                               }
                               else
                               {
                                   PlayMedia();
                                   Debug.WriteLine("---------------------------------- END RECORDING --------------------------");
                               }
                           }));
            }
        }

        public ICommand CaptureCommand
        {
            get
            {
                return _captureCommand ??
                       (_captureCommand =
                           new Command(async a =>
                           {
                               try
                               {

                                   bool result = MediaPlayer.TakeSnapshot(0, generateFileName(".jpg"), 0, 0);
                                   if (result)
                                   {
                                       //await CenterMessageController.DisplayAlert("Success", "Image captured!", "OK");
                                       //X1AppDataModel.Instance.GalleryViewModel.RefreshCommand.Execute(null);
                                   }
                                   else
                                   {
                                      // await CenterMessageController.DisplayAlert("Error", "Sorry image not captured!", "OK");
                                   }

                               }
                               catch (Exception ex)
                               {
                                   Debug.WriteLine(ex.Message);
                               }


                           }));
            }
        }

        private string generateFileName(string extension = ".jpg")
        {
            string documents = "";

            if (Device.RuntimePlatform == Device.iOS)
            {
                documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }

            //use epoch time to be more universal
            long epochTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var filename = documents + "/leicaX1_" + epochTime.ToString() + extension;
            Debug.WriteLine("Generated file path: " + filename);
            return filename;

        }

        

        Comparison<FileInfo> CompareLatest = new Comparison<FileInfo>(delegate (FileInfo a, FileInfo b)
        {
            return DateTime.Compare(a.LastWriteTime, b.LastWriteTime);
        });

        private ImageSource GetImgSource()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var files = new DirectoryInfo(documents).GetFiles("*.*");
                Array.Sort(files, CompareLatest);
                if (files.Length > 0)
                {
                    var imageLast = files[files.Length - 1].FullName;
                    return imageLast;
                }
            }
            return null;

        }


        public void PlayMedia(string mediaUrl = "")
        {
            if (!string.IsNullOrEmpty(mediaUrl))
            {
                StreamUrl = mediaUrl;
            }

            if (MediaPlayer != null)
            {
                if (MediaPlayer.IsPlaying)
                {
                    MediaPlayer.Stop();
                }
            }
            currentMedia = new Media(LibVLC, StreamUrl, FromType.FromLocation);
            MediaPlayer.Play(currentMedia);
            MediaPlayer.Position = 0;
        }


        public void Stop()
        {
            _mediaPlayer.Stop();
        }

        public string Message
        {
            get => _message;
            set => Set(nameof(Message), ref _message, value);
        }

        string _message = string.Empty;

        long _finalTime;
        bool _timeChanged;

        int _finalVolume;
        bool _volumeChanged;

        string FormatSeekingMessage(long timeDiff, long finalTime, Direction direction)
        {
            var timeDiffTimeSpan = TimeSpan.FromMilliseconds((double)new decimal(timeDiff));
            var finalTimeSpan = TimeSpan.FromMilliseconds((double)new decimal(finalTime));
            var diffSign = direction == Direction.Right ? "+" : "-";
            return $"Seeking ({direction} swipe): {diffSign}{timeDiffTimeSpan.Minutes}:{Math.Abs(timeDiffTimeSpan.Seconds)} ({finalTimeSpan.Minutes}:{Math.Abs(finalTimeSpan.Seconds)})";
        }

        string FormatVolumeMessage(int volume, Direction direction) => $"Volume ({direction} swipe): {volume}%";

        bool WillOverflow => _finalTime > TimeSpan.MaxValue.TotalMilliseconds || _finalTime < TimeSpan.MinValue.TotalMilliseconds;

        int VolumeRangeCheck(int volume)
        {
            if (volume < 0)
                volume = 0;
            else if (volume > 200)
                volume = 200;
            return volume;
        }

        internal void OnGesture(PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    if (e.TotalX < 0 && Math.Abs(e.TotalX) > Math.Abs(e.TotalY))
                    {
                        var timeDiff = Convert.ToInt64(e.TotalX * 1000);
                        _finalTime = MediaPlayer.Time + timeDiff;

                        if (WillOverflow)
                            break;

                        Message = FormatSeekingMessage(timeDiff, _finalTime, Direction.Left);
                        _timeChanged = true;
                    }
                    else if (e.TotalX > 0 && Math.Abs(e.TotalX) > Math.Abs(e.TotalY))
                    {
                        var timeDiff = Convert.ToInt64(e.TotalX * 1000);
                        _finalTime = MediaPlayer.Time + timeDiff;

                        if (WillOverflow)
                            break;

                        Message = FormatSeekingMessage(timeDiff, _finalTime, Direction.Right);
                        _timeChanged = true;
                    }
                    else if (e.TotalY < 0 && Math.Abs(e.TotalY) > Math.Abs(e.TotalX))
                    {
                        var volume = (int)(MediaPlayer.Volume + e.TotalY * -1);
                        _finalVolume = VolumeRangeCheck(volume);

                        Message = FormatVolumeMessage(_finalVolume, Direction.Top);
                        _volumeChanged = true;
                    }
                    else if (e.TotalY > 0 && e.TotalY > Math.Abs(e.TotalX))
                    {
                        var volume = (int)(MediaPlayer.Volume + e.TotalY * -1);
                        _finalVolume = VolumeRangeCheck(volume);

                        Message = FormatVolumeMessage(_finalVolume, Direction.Bottom);
                        _volumeChanged = true;
                    }
                    break;
                case GestureStatus.Started:
                case GestureStatus.Canceled:
                    Message = string.Empty;
                    break;
                case GestureStatus.Completed:
                    if (_timeChanged)
                        MediaPlayer.Time = _finalTime;
                    if (_volumeChanged && MediaPlayer.Volume != _finalVolume)
                        MediaPlayer.Volume = _finalVolume;

                    Message = string.Empty;
                    _timeChanged = false;
                    _volumeChanged = false;
                    break;
            }
        }

        enum Direction
        {
            Left,
            Right,
            Top,
            Bottom
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var ev = PropertyChanged;
            ev?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
