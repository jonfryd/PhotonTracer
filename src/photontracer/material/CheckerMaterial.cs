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
				this.spacing_Renamed_Field = value;
			}
			
		}
		virtual public RGBColor OtherDiffuseColor
		{
			set
			{
				this.otherDiffuseColor_Renamed_Field = value;
			}
			
		}
		private RGBColor otherDiffuseColor_Renamed_Field;
		private float spacing_Renamed_Field;
		
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
			int value_Renamed;
			
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			value_Renamed = (int) System.Math.Round(localPoint.x() / spacing_Renamed_Field) + (int) System.Math.Round(localPoint.y() / spacing_Renamed_Field) + (int) System.Math.Round(localPoint.z() / spacing_Renamed_Field);
			
			return ((value_Renamed & 1) == 0)?diffuseColor():otherDiffuseColor();
		}
		
		public virtual float spacing()
		{
			return spacing_Renamed_Field;
		}
		
		public virtual RGBColor otherDiffuseColor()
		{
			return otherDiffuseColor_Renamed_Field;
		}
	}
}