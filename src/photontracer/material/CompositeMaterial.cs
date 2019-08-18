// Composite material class
using System;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;
using Primitive = photontracer.objects.Primitive;
namespace photontracer.material
{
	
	public class CompositeMaterial:Material
	{
		virtual public float Amount1
		{
			set
			{
				amount1_Renamed_Field = value;
			}
			
		}
		virtual public float Amount2
		{
			set
			{
				amount2_Renamed_Field = value;
			}
			
		}
		virtual public Material Material1
		{
			set
			{
				material1_Renamed_Field = value;
			}
			
		}
		virtual public Material Material2
		{
			set
			{
				material2_Renamed_Field = value;
			}
			
		}
		private Material material1_Renamed_Field, material2_Renamed_Field;
		private float amount1_Renamed_Field, amount2_Renamed_Field;
		
		public CompositeMaterial():base()
		{
			
			Material1 = new Material();
			Material2 = new Material();
			Amount1 = 0.5f;
			Amount2 = 0.5f;
		}
		
		public CompositeMaterial(RGBColor ambientColor, RGBColor diffuseColor, RGBColor specularColor, RGBColor transmissionColor, float phongExponent, float IOR, Material material1, Material material2, float amount1, float amount2):base(ambientColor, diffuseColor, specularColor, transmissionColor, phongExponent, IOR)
		{
			
			Material1 = material1;
			Material2 = material2;
			Amount1 = amount1;
			Amount2 = amount2;
		}
		
		public CompositeMaterial(Material material, Material material1, Material material2, float amount1, float amount2):base(material)
		{
			
			Material1 = material1;
			Material2 = material2;
			Amount1 = amount1;
			Amount2 = amount2;
		}
		
		public override RGBColor diffuseColor(Vector3D localPoint)
		{
			RGBColor color = material1_Renamed_Field.diffuseColor(localPoint).scaleNew(amount1_Renamed_Field);
			color.add(material2_Renamed_Field.diffuseColor(localPoint).scaleNew(amount2_Renamed_Field));
			
			return color;
		}
		
		public virtual float amount1()
		{
			return amount1_Renamed_Field;
		}
		
		public virtual float amount2()
		{
			return amount2_Renamed_Field;
		}
		
		public virtual Material material1()
		{
			return material1_Renamed_Field;
		}
		
		public virtual Material material2()
		{
			return material2_Renamed_Field;
		}
	}
}