// Material class - extend and override 'RGBColor foo (Vector3D)' methods for interesting patterns/textures
using System;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;
using Primitive = photontracer.objects.Primitive;
namespace photontracer.material
{
	
	public class Material
	{
		virtual public RGBColor AmbientColor
		{
			set
			{
				this.ambientColor_ = value;
			}
			
		}
		virtual public RGBColor DiffuseColor
		{
			set
			{
				this.diffuseColor_ = value;

				absorbationCoef_ = value.average();
				isAbsorbing_ = (absorbationCoef_ > 0)?true:false;
			}
			
		}
		virtual public RGBColor SpecularColor
		{
			set
			{
				this.specularColor_ = value;
				
				reflectionCoef_ = value.average();
				isReflective_ = (reflectionCoef_ > 0)?true:false;
			}
			
		}
		virtual public RGBColor TransmissionColor
		{
			set
			{
				this.transmissionColor_ = value;
				
				transmissionCoef_ = value.average ();
				isTransparent_ = (transmissionCoef_ > 0)?true:false;
			}
			
		}
		virtual public float PhongExponent
		{
			set
			{
				this.phongExponent_ = value;
			}
			
		}
		virtual public Bump Bump
		{
			set
			{
				this.bump_ = value;
			}
			
		}
		private RGBColor ambientColor_;
		private RGBColor diffuseColor_;
		private RGBColor specularColor_;
		private RGBColor transmissionColor_;
		private float absorbationCoef_;
		private float reflectionCoef_;
		private float transmissionCoef_;
		private bool isAbsorbing_;
		private bool isReflective_;
		private bool isTransparent_;
		private float phongExponent_;
		private float IOR_;
		private Bump bump_;
		
		public Material()
		{
			AmbientColor = RGBColor.RGBblack();
			DiffuseColor = RGBColor.RGBblack();
			SpecularColor = RGBColor.RGBwhite();
			TransmissionColor = RGBColor.RGBwhite();
			
			PhongExponent = 8;
			setIOR(1);
			
			Bump = null;
		}
		
		public Material(RGBColor ambientColor, RGBColor diffuseColor, RGBColor specularColor, RGBColor transmissionColor, float phongExponent, float IOR)
		{
			AmbientColor = ambientColor;
			DiffuseColor = diffuseColor;
			SpecularColor = specularColor;
			TransmissionColor = transmissionColor;
			
			PhongExponent = phongExponent;
			setIOR(IOR);
			
			Bump = null;
		}
		
		public Material(Material other)
		{
			AmbientColor = new RGBColor(other.ambientColor());
			DiffuseColor = new RGBColor(other.diffuseColor());
			SpecularColor = new RGBColor(other.specularColor());
			TransmissionColor = new RGBColor(other.transmissionColor());
			
			PhongExponent = other.phongExponent();
			setIOR(other.IOR());
			
			if (other.bump() != null)
			{
				Bump = new Bump(other.bump());
			}
			else
			{
				Bump = null;
			}
		}
		
		public virtual RGBColor ambientColor()
		{
			return ambientColor_;
		}
		
		public virtual RGBColor ambientColor(Vector3D localPoint)
		{
			return ambientColor_;
		}
		
		public virtual RGBColor diffuseColor()
		{
			return diffuseColor_;
		}
		
		public virtual RGBColor diffuseColor(Vector3D localPoint)
		{
			return diffuseColor_;
		}
		
		public virtual RGBColor specularColor()
		{
			return specularColor_;
		}
		
		public virtual RGBColor specularColor(Vector3D localPoint)
		{
			return specularColor_;
		}
		
		public virtual RGBColor transmissionColor()
		{
			return transmissionColor_;
		}
		
		public virtual RGBColor transmissionColor(Vector3D localPoint)
		{
			return transmissionColor_;
		}
		
		public virtual float absorbationCoef()
		{
			return absorbationCoef_;
		}
		
		public virtual float absorbationCoef(Vector3D localPoint)
		{
			return diffuseColor(localPoint).average();
		}
		
		public virtual float reflectionCoef()
		{
			return reflectionCoef_;
		}
		
		public virtual float reflectionCoef(Vector3D localPoint)
		{
			return specularColor(localPoint).average();
		}

		public virtual float transmissionCoef()
		{
			return transmissionCoef_;
		}
		
		public virtual float transmissionCoef(Vector3D localPoint)
		{
			return transmissionColor(localPoint).average();
		}
		
		public virtual bool isReflective()
		{
			return isReflective_;
		}
		
		public virtual bool isReflective(Vector3D localPoint)
		{
			return (reflectionCoef(localPoint) > 0)?true:false;
		}
		
		public virtual bool isAbsorbing()
		{
			return isAbsorbing_;
		}
		
		public virtual bool isAbsorbing(Vector3D localPoint)
		{
			return (absorbationCoef(localPoint) > 0)?true:false;
		}

		public virtual bool isTransparent()
		{
			return isTransparent_;
		}
		
		public virtual bool isTransparent(Vector3D localPoint)
		{
			return (transmissionCoef(localPoint) > 0) ? true : false;
		}

		public virtual float phongExponent()
		{
			return phongExponent_;
		}
		
		public virtual float phongExponent(Vector3D localPoint)
		{
			return phongExponent_;
		}
		
		public virtual float IOR()
		{
			return IOR_;
		}
		
		public virtual float IOR(Vector3D localPoint)
		{
			return IOR_;
		}
		
		public virtual void  setIOR(float IOR)
		{
			this.IOR_ = IOR;
		}
		
		public virtual Bump bump()
		{
			return bump_;
		}
		
		public virtual Vector3D perturbNormal(Vector3D normal, Vector3D localPoint, Primitive primitive)
		{
			if (bump_ != null)
			{
				return bump_.perturbNormal(normal, localPoint, primitive);
			}
			else
			{
				return normal;
			}
		}
	}
}