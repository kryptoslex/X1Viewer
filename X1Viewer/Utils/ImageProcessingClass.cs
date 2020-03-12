using System;
using System.Collections.Generic;
using SkiaSharp;

namespace X1Viewer.Utils
{
    public class ImageProcessingClass
    {
        public ImageProcessingClass()
        {
        }



		public static SKImage Combine(string[] files)
		{
			//read all images into memory
			List<SKBitmap> images = new List<SKBitmap>();
			SKImage finalImage = null;

			try
			{
				int width = 0;
				int height = 0;

				foreach (string image in files)
				{
					//create a bitmap from the file and add it to the list
					SKBitmap bitmap = SKBitmap.Decode(image);

					//update the size of the final bitmap
					width += bitmap.Width;
					height += bitmap.Height;

					images.Add(bitmap);
				}

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
					foreach (SKBitmap image in images)
					{
						canvas.DrawBitmap(image, SKRect.Create(offset, offsetTop, image.Width, image.Height));
						offsetTop = offsetTop > 0 ? 0 : image.Height / 2;
						offset += (int)(image.Width / 1.6);
					}

					// return the surface as a manageable image
					finalImage = tempSurface.Snapshot();
				}

				//return the image that was just drawn
				return finalImage;
			}
			finally
			{
				//clean up memory
				foreach (SKBitmap image in images)
				{
					image.Dispose();
				}
			}
		}
		
	}
}
