using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace X1Viewer.Views
{
    public partial class CapturedImageView : ContentPage
    {
        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 8;
        private const double OVERSHOOT = 0.15;
        private double StartScale;
        private double LastX, LastY;

        public CapturedImageView(Image image)
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine(image.ToString()) ;
            imageNameLabel.Text = image.Source.ToString();
            CapturedImage.Source = image.Source;
            OnPropertyChanged();
        }

        async void BackButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            var pinch = new PinchGestureRecognizer();
            pinch.PinchUpdated += OnPinchUpdated;
            CapturedImage.GestureRecognizers.Add(pinch);

            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            CapturedImage.GestureRecognizers.Add(pan);

            var tap = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            tap.Tapped += OnTapped;
            CapturedImage.GestureRecognizers.Add(tap);
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (CapturedImage.Scale > MIN_SCALE || CapturedImage.Scale < MIN_SCALE)
            {
                CapturedImage.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                CapturedImage.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.5; //TODO tapped position
                CapturedImage.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (CapturedImage.Scale > MIN_SCALE)
                switch (e.StatusType)
                {
                    case GestureStatus.Started:
                        LastX = CapturedImage.TranslationX;
                        LastY = CapturedImage.TranslationY;
                        break;
                    case GestureStatus.Running:
                        CapturedImage.TranslationX = Clamp(LastX + e.TotalX * CapturedImage.Scale, -Width / 2, Width / 2);
                        CapturedImage.TranslationY = Clamp(LastY + e.TotalY * CapturedImage.Scale, -Height / 2, Height / 2);
                        break;
                }
        }

        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            switch (e.Status)
            {
                case GestureStatus.Started:
                    StartScale = CapturedImage.Scale;
                    AnchorX = e.ScaleOrigin.X;
                    AnchorY = e.ScaleOrigin.Y;
                    break;
                case GestureStatus.Running:
                    double current = CapturedImage.Scale + (e.Scale - 1) * StartScale;
                    CapturedImage.Scale = Clamp(current, MIN_SCALE * (1 - OVERSHOOT), MAX_SCALE * (1 + OVERSHOOT));
                    break;
                case GestureStatus.Completed:
                    if (Scale > MAX_SCALE)
                        CapturedImage.ScaleTo(MAX_SCALE, 250, Easing.SpringOut);
                    else if (Scale < MIN_SCALE)
                        CapturedImage.ScaleTo(MIN_SCALE, 250, Easing.SpringOut);

                    break;
            }
        }

        private T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
        {
            if (value.CompareTo(minimum) < 0)
                return minimum;
            else if (value.CompareTo(maximum) > 0)
                return maximum;
            else
                return value;
        }
    }
}
