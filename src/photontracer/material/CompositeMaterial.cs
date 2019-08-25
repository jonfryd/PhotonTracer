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
				amount1_ = value;
			}
			
		}
		virtual public float Amount2
		{
			set
			{
				amount2_ = value;
			}
			
		}
		virtual public Material Material1
		{
			set
			{
				material1_ = value;
			}
			
		}
		virtual public Material Material2
		{
			set
			{
				material2_ = value;
			}
			
		}
		private Material material1_, material2_;
		private float amount1_, amount2_;
		
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
			RGBColor color = material1_.diffuseColor(localPoint).scaleNew(amount1_);
			color.add(material2_.diffuseColor(localPoint).scaleNew(amount2_));
			
			return color;
		}
		
		public virtual float amount1()
		{
			return amount1_;
		}
		
		public virtual float amount2()
		{
			return amount2_;
		}
		
		public virtual Material material1()
		{
			return material1_;
		}
		
		public virtual Material material2()
		{
			return material2_;
		}
	}
}