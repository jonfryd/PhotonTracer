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
				this.otherDiffuseColor_Renamed_Field = value;
			}
			
		}
		virtual public Vector3D Tiling
		{
			set
			{
				this.tiling_Renamed_Field = value;
			}
			
		}
		virtual public Vector3D Offset
		{
			set
			{
				this.offset_Renamed_Field = new Vector3D(value);
				this.offset_Renamed_Field.add(0.5);
			}
			
		}
		virtual public bool Mirror
		{
			set
			{
				this.mirror_Renamed_Field = value;
			}
			
		}
		virtual public bool Tile
		{
			set
			{
				this.tile_Renamed_Field = value;
			}
			
		}
		private RGBColor otherDiffuseColor_Renamed_Field;
		//UPGRADE_TODO: Class 'java.awt.image.BufferedImage' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
//		private BufferedImage texture;
		private System.Drawing.Bitmap texture;
		private int textureWidth, textureHeight;
		private float fTextureWidth, fTextureHeight;
		private Vector3D tiling_Renamed_Field;
		private Vector3D offset_Renamed_Field;
		private bool mirror_Renamed_Field;
		private bool tile_Renamed_Field;
		
		//UPGRADE_NOTE: Field 'EnclosingInstance' was added to class 'Bool' to access its enclosing instance. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1019"'
		internal class Bool
		{
			private void  InitBlock(TextureMaterial enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private TextureMaterial enclosingInstance;
			public TextureMaterial Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			internal bool value_Renamed;
			
			internal Bool(TextureMaterial enclosingInstance)
			{
				InitBlock(enclosingInstance);
				setBool(true);
			}
			
			internal Bool(TextureMaterial enclosingInstance, bool value_Renamed)
			{
				InitBlock(enclosingInstance);
				setBool(value_Renamed);
			}
			
			internal virtual bool bool_Renamed()
			{
				return value_Renamed;
			}
			
			internal virtual void  setBool(bool value_Renamed)
			{
				this.value_Renamed = value_Renamed;
			}
		}
		
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
				
				//UPGRADE_TODO: Method 'java.awt.image.BufferedImage.getWidth' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
				textureWidth = texture.Width;
				//UPGRADE_TODO: Method 'java.awt.image.BufferedImage.getHeight' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
				textureHeight = texture.Height;
				
				fTextureWidth = textureWidth;
				fTextureHeight = textureHeight;
			}
			catch (System.Exception e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.WriteLine' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
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
			
			//UPGRADE_TODO: Method 'java.awt.image.BufferedImage.getRGB' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1095"'
			lock (this)
			{
				RGB = texture.GetPixel (u, v).ToArgb ();
			}
			
			texel = new RGBColor((RGB >> 16) & 0xff, (RGB >> 8) & 0xff, RGB & 0xff);
			
			return texel.convertFrom24Bits();
		}
		
		private int ifloor(int a, int b)
		{
			return (a < 0)?((a / b) - 1):(a / b);
		}
		
		private int correctTexel(int value_Renamed, Bool wrappedObj, int max)
		{
			bool wrapped;
			int i = value_Renamed % max;
			
			wrapped = false;
			
			if (value_Renamed < 0)
			{
				i += (max - 1);
				
				wrapped = true;
			}
			else if (value_Renamed >= max)
			{
				wrapped = true;
			}
			
			if (mirror())
			{
				if ((ifloor(value_Renamed, max) & 1) == 0)
				{
					i = (max - 1) - i;
				}
			}
			
			wrappedObj.setBool(wrapped);
			
			return i;
		}
		
		private RGBColor lerpTexel(Vector3D uvcoord)
		{
			RGBColor lerpColor;
			float u, v;
			int iu, iv, iu2, iv2;
			float uv, iuv, uiv, iuiv;
			float iiu, iiv;
			float fu, fv;
			bool uWrapped;
			bool u2Wrapped;
			bool vWrapped;
			bool v2Wrapped;
			Bool boolObj = new Bool(this);
			
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			u = (float) uvcoord.x() * fTextureWidth;
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			v = (float) uvcoord.y() * fTextureHeight;
			
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			iu = (int) u;
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			iv = (int) v;
			
			fu = u - iu;
			fv = v - iv;
			
			iu2 = correctTexel(iu + 1, boolObj, textureWidth);
			u2Wrapped = boolObj.bool_Renamed();
			iv2 = correctTexel(iv + 1, boolObj, textureHeight);
			v2Wrapped = boolObj.bool_Renamed();
			iu = correctTexel(iu, boolObj, textureWidth);
			uWrapped = boolObj.bool_Renamed();
			iv = correctTexel(iv, boolObj, textureHeight);
			vWrapped = boolObj.bool_Renamed();
			
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
			
			finalLocalPoint = localPoint.scaleNew(tiling_Renamed_Field);
			finalLocalPoint.add(offset_Renamed_Field);
			
			return lerpTexel(finalLocalPoint);
		}
		
		public virtual RGBColor otherDiffuseColor()
		{
			return otherDiffuseColor_Renamed_Field;
		}
		
		public virtual Vector3D tiling()
		{
			return tiling_Renamed_Field;
		}
		
		public virtual Vector3D offset()
		{
			return offset_Renamed_Field;
		}
		
		public virtual bool mirror()
		{
			return mirror_Renamed_Field;
		}
		
		public virtual bool tile()
		{
			return tile_Renamed_Field;
		}
	}
}