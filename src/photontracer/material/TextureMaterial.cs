// Texture material class
using System;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;
using Primitive = photontracer.objects.Primitive;

namespace photontracer.material
{
	
	public class TextureMaterial:Material
	{
		virtual public RGBColor OtherDiffuseColor
		{
			set
			{
				this.otherDiffuseColor_ = value;
			}
			
		}
		virtual public Vector3D Tiling
		{
			set
			{
				this.tiling_ = value;
			}
			
		}
		virtual public Vector3D Offset
		{
			set
			{
				this.offset_ = new Vector3D(value);
				this.offset_.add(0.5);
			}
			
		}
		virtual public bool Mirror
		{
			set
			{
				this.mirror_ = value;
			}
			
		}
		virtual public bool Tile
		{
			set
			{
				this.tile_ = value;
			}
			
		}
		private RGBColor otherDiffuseColor_;
//		private BufferedImage texture;
		private System.Drawing.Bitmap texture;
		private int textureWidth, textureHeight;
		private float fTextureWidth, fTextureHeight;
		private Vector3D tiling_;
		private Vector3D offset_;
		private bool mirror_;
		private bool tile_;
		
		public TextureMaterial():base()
		{	
			OtherDiffuseColor = new RGBColor();
			Tiling = new Vector3D(1, 1, 0);
			Offset = new Vector3D();
			Mirror = false;
			Tile = true;
		}
		
		public TextureMaterial(RGBColor ambientColor, RGBColor diffuseColor, RGBColor specularColor, RGBColor transmissionColor, float phongExponent, float IOR, RGBColor otherDiffuseColor, Vector3D tiling, Vector3D offset, bool mirror, bool tile, System.String filename):base(ambientColor, diffuseColor, specularColor, transmissionColor, phongExponent, IOR)
		{
			OtherDiffuseColor = otherDiffuseColor;
			Tiling = tiling;
			Offset = offset;
			Mirror = mirror;
			Tile = tile;

			loadJPEG(filename);
		}
		
		public TextureMaterial(Material material, RGBColor otherDiffuseColor, Vector3D tiling, Vector3D offset, bool mirror, bool tile, System.String filename):base(material)
		{	
			OtherDiffuseColor = new RGBColor(otherDiffuseColor);
			Tiling = tiling;
			Offset = offset;
			Mirror = mirror;
			Tile = tile;
			loadJPEG(filename);
		}
		
		public virtual void  loadJPEG(System.String filename)
		{
			try
			{
				//System.Drawing.Image.FromFile (filename.ToString ());
				texture = new System.Drawing.Bitmap (filename.ToString ());
				//texture = new System.Drawing.Bitmap (100,100);
				
				textureWidth = texture.Width;
				textureHeight = texture.Height;
				
				fTextureWidth = textureWidth;
				fTextureHeight = textureHeight;
			}
			catch (System.Exception e)
			{
				System.Console.Out.WriteLine(e);
				System.Environment.Exit(- 1);
			}
		}
		
		
		private RGBColor getColor(bool wrapped, int u, int v)
		{
			RGBColor texel;
			int RGB;
			
			if (!tile() && wrapped)
			{
				return diffuseColor();
			}
			
            RGB = texture.GetPixel (u, v).ToArgb ();
			
			texel = new RGBColor((RGB >> 16) & 0xff, (RGB >> 8) & 0xff, RGB & 0xff);
			
			return texel.convertFrom24Bits();
		}
		
		private int ifloor(int a, int b)
		{
			return (a < 0) ? ((a / b) - 1) : (a / b);
		}
		
		private int correctTexel(int value, ref bool wrapped, int max)
		{
			int i = value % max;
			
			wrapped = false;
			
			if (value < 0)
			{
				i += (max - 1);
				
				wrapped = true;
			}
			else if (value >= max)
			{
				wrapped = true;
			}
			
			if (mirror())
			{
				if ((ifloor(value, max) & 1) == 0)
				{
					i = (max - 1) - i;
				}
			}
			
			return i;
		}
		
		private RGBColor lerpTexel(Vector3D uvcoord)
		{
			RGBColor lerpColor;
			float u, v;
			int iu, iv, iu2, iv2;
			float uv, iuv, uiv, iuiv;
			//float iiu, iiv;
			float fu, fv;
			bool uWrapped = false;
			bool u2Wrapped = false;
			bool vWrapped = false;
			bool v2Wrapped = false;

			u = (float) uvcoord.x() * fTextureWidth;
			v = (float) uvcoord.y() * fTextureHeight;
			
			iu = (int) u;
			iv = (int) v;
			
			fu = u - iu;
			fv = v - iv;
			
			iu2 = correctTexel(iu + 1, ref u2Wrapped, textureWidth);
			iv2 = correctTexel(iv + 1, ref v2Wrapped, textureHeight);
			iu = correctTexel(iu, ref uWrapped, textureWidth);
			iv = correctTexel(iv, ref vWrapped, textureHeight);
			
			if (fu < 0.0f)
			{
				fu += 1.0f;
			}
			if (fv < 0.0f)
			{
				fv += 1.0f;
			}
			
			uv = fu * fv;
			iuv = (1.0f - fu) * fv;
			uiv = fu * (1 - fv);
			iuiv = (1.0f - fu) * (1.0f - fv);
			
			lerpColor = getColor((uWrapped | vWrapped), iu, iv).scaleNew(iuiv);
			lerpColor.add(getColor((u2Wrapped | vWrapped), iu2, iv).scaleNew(uiv));
			lerpColor.add(getColor((uWrapped | v2Wrapped), iu, iv2).scaleNew(iuv));
			lerpColor.add(getColor((u2Wrapped | v2Wrapped), iu2, iv2).scaleNew(uv));
			
			return lerpColor;
		}
		
		public override RGBColor diffuseColor(Vector3D localPoint)
		{
			Vector3D finalLocalPoint;
			
			finalLocalPoint = localPoint.scaleNew(tiling_);
			finalLocalPoint.add(offset_);
			
			return lerpTexel(finalLocalPoint);
		}
		
		public virtual RGBColor otherDiffuseColor()
		{
			return otherDiffuseColor_;
		}
		
		public virtual Vector3D tiling()
		{
			return tiling_;
		}
		
		public virtual Vector3D offset()
		{
			return offset_;
		}
		
		public virtual bool mirror()
		{
			return mirror_;
		}
		
		public virtual bool tile()
		{
			return tile_;
		}
	}
}