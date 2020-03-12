using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using static Xamarin.Forms.BindableProperty;

namespace X1Viewer.Utils
{
    public class ScalebarControl : SKCanvasView
    {

        public ScalebarControl()
        {
            PaintSurface += ScalebarControl_PaintSurface;
        }


        public static BindableProperty LineColorProperty = Create(@"LineColor", typeof(SKColor), typeof(ScalebarControl), SKColors.White, propertyChanged: LineColorChanged);
        public static BindableProperty LabelColorProperty = Create(@"LabelColor", typeof(SKColor), typeof(ScalebarControl), SKColors.White, propertyChanged: LabelColorChanged);
        public static BindableProperty ScaleValueProperty = Create(@"ScaleValue", typeof(double), typeof(ScalebarControl), 1.0, propertyChanged: ScaleValueChanged);
        public static BindableProperty ScaleUnitProperty = Create(@"ScaleUnit", typeof(string), typeof(ScalebarControl), @"μm", propertyChanged: ScaleUnitChanged);
        public static BindableProperty TextFontSizeProperty = Create(@"TextFontSize", typeof(int), typeof(ScalebarControl), 12, propertyChanged: TextFontSizeChanged);
        double _pixelSize = 3.45 * 40 / 105;
        //private IImageDisplayControl _imageDisplayControl;

        static void LineColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = bindable as ScalebarControl;
            if (c != null)
            {
                c.LineColor = (SKColor)newValue;
                c.InvalidateSurface();
            }
        }

        static void LabelColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = bindable as ScalebarControl;
            if (c != null)
            {
                c.LabelColor = (SKColor)newValue;
                c.InvalidateSurface();
            }
        }

        static void ScaleValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = bindable as ScalebarControl;
            if (c != null)
            {
                c.ScaleValue = (double)newValue;
                c.InvalidateSurface();
            }
        }

        static void ScaleUnitChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = bindable as ScalebarControl;
            if (c != null)
            {
                c.ScaleUnit = newValue as string;
                c.InvalidateSurface();
            }
        }

        static void TextFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var c = bindable as ScalebarControl;
            if (c != null)
            {
                c.TextFontSize = (int)newValue;
                c.InvalidateSurface();
            }
        }

        public SKColor LineColor
        {
            get { return (SKColor)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }


        public SKColor LabelColor
        {
            get { return (SKColor)GetValue(LabelColorProperty); }
            set { SetValue(LabelColorProperty, value); }
        }
        public double ScaleValue
        {
            get { return (double)GetValue(ScaleValueProperty); }
            set { SetValue(ScaleValueProperty, value); }
        }

        public string ScaleUnit
        {
            get { return (string)GetValue(ScaleUnitProperty); }
            set { SetValue(ScaleUnitProperty, value); }
        }
        public int TextFontSize
        {
            get { return (int)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }

        public double PixelSize
        {
            get { return _pixelSize; }
            set
            {
                if (Math.Abs(_pixelSize - value) > double.Epsilon)
                {
                    _pixelSize = value;
                    InvalidateSurface();
                }
            }
        }

        double DisplayScaleFactor = 1.0;// => X1CoreFactory.Instance.DisplayWindow != null ? X1CoreFactory.Instance.DisplayWindow.DisplayScaleFactor : 1.0;
        //public IImageDisplayControl ImageDisplayControl
        //{
        //    get { return _imageDisplayControl; }
        //    set
        //    {
        //        if (_imageDisplayControl != value)
        //        {
        //            _imageDisplayControl = value;
        //            if (_imageDisplayControl != null)
        //            {
        //                _imageDisplayControl.OnPaint += (s, e) =>
        //                {
        //                    var imageZoom = _imageDisplayControl.ZoomFactor;
        //                    var bound = _imageDisplayControl.Bound;
        //                    if (Math.Abs(imageZoom - _imageDisplayZoom) > double.Epsilon ||
        //                        Math.Abs(_bound.Left - bound.Left) > double.Epsilon ||
        //                        Math.Abs(_bound.Top - bound.Top) > double.Epsilon ||
        //                        Math.Abs(_bound.Width - bound.Width) > double.Epsilon ||
        //                        Math.Abs(_bound.Height - bound.Height) > double.Epsilon)
        //                    {
        //                        _imageDisplayZoom = imageZoom;
        //                        _bound = bound;
        //                        if (Math.Abs(_bound.Left + 1) > double.Epsilon)
        //                        {
        //                            var newX = (bound.Left + bound.Width) / DisplayScaleFactor + XOffset;
        //                            TranslationX = newX - X;
        //                        }

        //                        if (Math.Abs(_bound.Top + 1) > double.Epsilon)
        //                        {
        //                            var newY = bound.Top / DisplayScaleFactor + YOffset;
        //                            TranslationY = newY - Y;
        //                        }
        //                        InvalidateSurface();
        //                    }
        //                };
        //            }
        //        }
        //    }
        //}

        public double XOffset { get; set; } = -120;

        public double YOffset { get; set; } = 50;


        double _imageDisplayZoom = 1;

        SKRect _bound = new SKRect(-1, -1, -1, -1);
        int DisplayLength
        {
            get
            {
                List<int> sizes = new List<int> { 5000, 1000, 500, 200, 100, 50, 30, 20 };
                //1/5 to 1/10 of image
                foreach (var size in sizes)
                {
                    if (GetScreenDrawLength(size) < 150)
                    {
                        return size;
                    }
                }
                return sizes[sizes.Count - 1];
            }
        }

        int GetScreenDrawLength(int length)
        {
            return (int)(length * 2 * ScaleValue / PixelSize / _imageDisplayZoom);
        }

        void ScalebarControl_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            SKPaint textPaint = new SKPaint
            {
                Color = LabelColor,
                TextSize = TextFontSize * (float)DisplayScaleFactor,
                TextAlign = SKTextAlign.Center,
                Typeface = SKTypeface.FromFamilyName("Segoe UI", SKTypefaceStyle.Normal)
            };

            SKPaint thinLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = LineColor,
                StrokeWidth = 2 * (float)DisplayScaleFactor
            };

            canvas.Clear(SKColor.Empty);

            int displayLength = DisplayLength;
            //1/5 to 1/10 of image

            //IMPORTANT:length is calculated without display scale factor 
            int length = GetScreenDrawLength(DisplayLength);

            int left = 16;

            var x = left + (int)(length / 2.0);
            var y = info.Height - 20 * (float)DisplayScaleFactor;

            // Display text
            canvas.DrawText($@"{displayLength:0} {ScaleUnit}", x, y, textPaint);
            // Display thick line
            float scaleFactor = (float)DisplayScaleFactor;
            canvas.DrawLine(left, 5 * scaleFactor, left, 20 * scaleFactor, thinLinePaint);//left
            canvas.DrawLine(left + 2, 15 * scaleFactor, left + length - 2, 15 * scaleFactor, thinLinePaint); //middle
            canvas.DrawLine(left + length, 5 * scaleFactor, left + length, 20 * scaleFactor, thinLinePaint); //right
        }

        public SKImage BurnIn(string fileName)
        {
            //read all images into memory
            SKImage finalImage = null;
            //create a bitmap from the file and add it to the list
            SKBitmap bitmap = SKBitmap.Decode(fileName);
            try
            {
                int width = 0;
                int height = 0;
                //update the size of the final bitmap
                width += bitmap.Width;
                height += bitmap.Height;

                //get a surface so we can draw an image
                using (var tempSurface = SKSurface.Create(new SKImageInfo(width, height)))
                {
                    //get the drawing canvas of the surface
                    var canvas = tempSurface.Canvas;

                    //set background color
                    canvas.Clear(SKColors.Transparent);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    int offsetTop = 0;
                    canvas.DrawBitmap(bitmap, SKRect.Create(offset, offsetTop, bitmap.Width, bitmap.Height));
                    offsetTop = offsetTop > 0 ? 0 : bitmap.Height / 2;
                    offset += (int)(bitmap.Width / 1.6);

                    SKPaint textPaint = new SKPaint
                    {
                        Color = LabelColor,
                        TextSize = TextFontSize * (float)DisplayScaleFactor,
                        TextAlign = SKTextAlign.Center,
                        Typeface = SKTypeface.FromFamilyName("Segoe UI", SKTypefaceStyle.Normal)
                    };

                    SKPaint thinLinePaint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = LineColor,
                        StrokeWidth = 2 * (float)DisplayScaleFactor
                    };

                    int displayLength = DisplayLength;
                    //1/5 to 1/10 of image

                    //IMPORTANT:length is calculated without display scale factor 
                    int length = GetScreenDrawLength(DisplayLength);

                    int left = 16;

                    var x = left + (int)(length / 2.0);
                    var y = 25;//(int)height - 20 * (float)DisplayScaleFactor;

                    // Display text
                    canvas.DrawText($@"{displayLength:0} {ScaleUnit}", x, y, textPaint);
                    // Display thick line
                    float scaleFactor = (float)DisplayScaleFactor;
                    canvas.DrawLine(left, 5 * scaleFactor, left, 20 * scaleFactor, thinLinePaint);//left
                    canvas.DrawLine(left + 2, 15 * scaleFactor, left + length - 2, 15 * scaleFactor, thinLinePaint); //middle
                    canvas.DrawLine(left + length, 5 * scaleFactor, left + length, 20 * scaleFactor, thinLinePaint); //right
                    // return the surface as a manageable image
                    finalImage = tempSurface.Snapshot();
                }

                //return the image that was just drawn
                return finalImage;
            }
            finally
            {
               
                bitmap.Dispose();
                
            }
        }

    }
}
