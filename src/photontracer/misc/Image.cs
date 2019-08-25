// Image class
using System;

namespace photontracer.misc
{	
	public class Image:System.Windows.Forms.PictureBox 
	{
		private const int DEFWIDTH = 320;
		private const int DEFHEIGHT = 240;
		private int width_, height_;
		private RGBColor[][] pixels;
		private System.Drawing.Bitmap  offImg;

		public Image():base()
		{
			setDimensions(DEFWIDTH, DEFHEIGHT);
		}
		
		public Image(int width, int height):base()
		{
			setDimensions(width, height);
		}

		public virtual int width()
		{
			return width_;
		}
		
		public virtual int height()
		{
			return height_;
		}
		
		protected override System.Drawing.Size DefaultSize
		{
			get
			{
				return new System.Drawing.Size(width(),height());
			}
		}

		public virtual void  setDimensions(int width, int height)
		{
			this.width_ = width;
			this.height_ = height;
			
			Size = new System.Drawing.Size(width, height);
			//SetClientSizeCore (width, height);
			//SetBounds (0, 0, width, height);
			//Visible = true;
			//ClientSize = new System.Drawing.Size(width, height);
		}
		
		public virtual void  initializeFramebuffer()
		{
			createFramebuffer();
			
			pixels = new RGBColor[width_][];
			for (int i = 0; i < width_; i++)
			{
				pixels[i] = new RGBColor[height_];
			}
			
			for (int i = 0; i < width_; i++)
				for (int j = 0; j < height_; j++)
					setPixel(i, j, new RGBColor());
		}
		
		public virtual RGBColor getPixel(int x, int y)
		{
			if (x < 0 || x >= width_ || y < 0 || y >= height_)
			{
				System.Console.Out.WriteLine("Warning! (x,y) out of bounds.");
				
				return new RGBColor();
			}
			
			return pixels[x][y];
		}
		
		public virtual void  setPixel(int x, int y, RGBColor color)
		{
			if (x < 0 || x >= width_ || y < 0 || y >= height_)
			{
				//System.out.println ("Warning! (x,y) out of bounds.");
			}
			else
			{
				RGBColor clampedColor, screenColor;
				
				pixels[x][y] = color;
				
				if (offImg != null)
				{
					clampedColor = new RGBColor(color);
					clampedColor.clamp();
					
					screenColor = clampedColor.convertTo24Bits();
					
					offImg.SetPixel (x, y, System.Drawing.Color.FromArgb ((int) screenColor.red(), (int) screenColor.green(), (int) screenColor.blue()));
				}
			}
		}
		
		public virtual void  createFramebuffer()
		{
			offImg = new System.Drawing.Bitmap (width_, height_);
		}

		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs e) 
		{
		}
			
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) 
		{
			if (offImg != null && Visible)
			{
				e.Graphics.DrawImageUnscaled (offImg, 0, 0);
			}
		}

		private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			System.Drawing.Imaging.ImageCodecInfo[] encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();

			for (int j = 0; j < encoders.Length; j++)
			{
				if (encoders[j].MimeType == mimeType)
					return encoders[j];
			} 
			
			return null;
		}

		public virtual void  saveJPEG(System.String filename)
		{
			try
			{
				// Set the quality to 100 (must be a long)
				System.Drawing.Imaging.Encoder qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
				System.Drawing.Imaging.EncoderParameter ratio = new System.Drawing.Imaging.EncoderParameter(qualityEncoder, 100L);
				// Add the quality parameter to the list
				System.Drawing.Imaging.EncoderParameters codecParams = new System.Drawing.Imaging.EncoderParameters(1);
				codecParams.Param[0] = ratio;

				// Get Codec Info using MIME type
				System.Drawing.Imaging.ImageCodecInfo JpegCodecInfo = GetEncoderInfo("image/jpeg");

				//
				System.Console.Out.WriteLine("Saving '" + filename + "'.");
				
				offImg.Save (filename, JpegCodecInfo, codecParams);
				
				System.Console.Out.WriteLine("JPEG image saved.");
			}
			catch (System.Exception e)
			{
				System.Console.Out.WriteLine(e);
			}
		}
	
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Image] - <" + width_ + ", " + height_ + ">").ToString();
		}

	}
}