// Checker material class
using System;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;
using Primitive = photontracer.objects.Primitive;

namespace photontracer.material
{
	public class CheckerMaterial:Material
	{
		virtual public float Spacing
		{
			set
			{
				this.spacing_ = value;
			}
			
		}
		virtual public RGBColor OtherDiffuseColor
		{
			set
			{
				this.otherDiffuseColor_ = value;
			}
			
		}
		private RGBColor otherDiffuseColor_;
		private float spacing_;
		
		public CheckerMaterial():base()
		{
			
			OtherDiffuseColor = new RGBColor();
			Spacing = 1;
		}
		
		public CheckerMaterial(RGBColor ambientColor, RGBColor diffuseColor, RGBColor specularColor, RGBColor transmissionColor, float phongExponent, float IOR, RGBColor otherDiffuseColor, float spacing):base(ambientColor, diffuseColor, specularColor, transmissionColor, phongExponent, IOR)
		{
			
			OtherDiffuseColor = otherDiffuseColor;
			Spacing = spacing;
		}
		
		public CheckerMaterial(Material material, RGBColor otherDiffuseColor, float spacing):base(material)
		{
			
			OtherDiffuseColor = new RGBColor(otherDiffuseColor);
			Spacing = spacing;
		}
		
		public override RGBColor diffuseColor(Vector3D localPoint)
		{
			int value;
			
			value = (int) System.Math.Round(localPoint.x() / spacing_) + (int) System.Math.Round(localPoint.y() / spacing_) + (int) System.Math.Round(localPoint.z() / spacing_);
			
			return ((value & 1) == 0)?diffuseColor():otherDiffuseColor();
		}
		
		public virtual float spacing()
		{
			return spacing_;
		}
		
		public virtual RGBColor otherDiffuseColor()
		{
			return otherDiffuseColor_;
		}
	}
}