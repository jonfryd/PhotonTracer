// Image class
using System;
namespace photontracer.misc
{
	
	//UPGRADE_ISSUE: Class 'java.awt.Canvas' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javaawtCanvas"'
	public class Image:System.Windows.Forms.PictureBox 
	{
		private const int DEFWIDTH = 320;
		private const int DEFHEIGHT = 240;
		private int width_Renamed_Field, height_Renamed_Field;
		private RGBColor[][] pixels;
		//UPGRADE_TODO: Class 'java.awt.image.BufferedImage' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
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
			return width_Renamed_Field;
		}
		
		public virtual int height()
		{
			return height_Renamed_Field;
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
			this.width_Renamed_Field = width;
			this.height_Renamed_Field = height;
			
			//UPGRADE_TODO: Method 'java.awt.Component.setSize' was converted to 'System.Windows.Forms.Control.Size' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
			Size = new System.Drawing.Size(width, height);
			//SetClientSizeCore (width, height);
			//SetBounds (0, 0, width, height);
			//Visible = true;
			//ClientSize = new System.Drawing.Size(width, height);
		}
		
		public virtual void  initializeFramebuffer()
		{
			createFramebuffer();
			
			pixels = new RGBColor[width_Renamed_Field][];
			for (int i = 0; i < width_Renamed_Field; i++)
			{
				pixels[i] = new RGBColor[height_Renamed_Field];
			}
			
			for (int i = 0; i < width_Renamed_Field; i++)
				for (int j = 0; j < height_Renamed_Field; j++)
					setPixel(i, j, new RGBColor());
		}
		
		public virtual RGBColor getPixel(int x, int y)
		{
			if (x < 0 || x >= width_Renamed_Field || y < 0 || y >= height_Renamed_Field)
			{
				System.Console.Out.WriteLine("Warning! (x,y) out of bounds.");
				
				return new RGBColor();
			}
			
			return pixels[x][y];
		}
		
		public virtual void  setPixel(int x, int y, RGBColor color)
		{
			if (x < 0 || x >= width_Renamed_Field || y < 0 || y >= height_Renamed_Field)
			{
				//System.out.println ("Warning! (x,y) out of bounds.");
			}
			else
			{
				RGBColor clampedColor, screenColor;
				int biColor;
				
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
			offImg = new System.Drawing.Bitmap (width_Renamed_Field, height_Renamed_Field);
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
				//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.WriteLine' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				System.Console.Out.WriteLine(e);
			}
		}
	
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Image] - <" + width_Renamed_Field + ", " + height_Renamed_Field + ">").ToString();
		}

	}
}