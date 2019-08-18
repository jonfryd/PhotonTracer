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
				this.ambientColor_Renamed_Field = value;
			}
			
		}
		virtual public RGBColor DiffuseColor
		{
			set
			{
				this.diffuseColor_Renamed_Field = value;

				absorbationCoef_Renamed_Field = value.average();
				isAbsorbing_Renamed_Field = (absorbationCoef_Renamed_Field > 0)?true:false;
			}
			
		}
		virtual public RGBColor SpecularColor
		{
			set
			{
				this.specularColor_Renamed_Field = value;
				
				reflectionCoef_Renamed_Field = value.average();
				isReflective_Renamed_Field = (reflectionCoef_Renamed_Field > 0)?true:false;
			}
			
		}
		virtual public RGBColor TransmissionColor
		{
			set
			{
				this.transmissionColor_Renamed_Field = value;
				
				transmissionCoef_Renamed_Field = value.average ();
				isTransparent_Renamed_Field = (transmissionCoef_Renamed_Field > 0)?true:false;
			}
			
		}
		virtual public float PhongExponent
		{
			set
			{
				this.phongExponent_Renamed_Field = value;
			}
			
		}
		virtual public Bump Bump
		{
			set
			{
				this.bump_Renamed_Field = value;
			}
			
		}
		private RGBColor ambientColor_Renamed_Field;
		private RGBColor diffuseColor_Renamed_Field;
		private RGBColor specularColor_Renamed_Field;
		private RGBColor transmissionColor_Renamed_Field;
		private float absorbationCoef_Renamed_Field;
		private float reflectionCoef_Renamed_Field;
		private float transmissionCoef_Renamed_Field;
		private bool isAbsorbing_Renamed_Field;
		private bool isReflective_Renamed_Field;
		private bool isTransparent_Renamed_Field;
		private float phongExponent_Renamed_Field;
		private float IOR_Renamed_Field;
		private Bump bump_Renamed_Field;
		
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
			return ambientColor_Renamed_Field;
		}
		
		public virtual RGBColor ambientColor(Vector3D localPoint)
		{
			return ambientColor_Renamed_Field;
		}
		
		public virtual RGBColor diffuseColor()
		{
			return diffuseColor_Renamed_Field;
		}
		
		public virtual RGBColor diffuseColor(Vector3D localPoint)
		{
			return diffuseColor_Renamed_Field;
		}
		
		public virtual RGBColor specularColor()
		{
			return specularColor_Renamed_Field;
		}
		
		public virtual RGBColor specularColor(Vector3D localPoint)
		{
			return specularColor_Renamed_Field;
		}
		
		public virtual RGBColor transmissionColor()
		{
			return transmissionColor_Renamed_Field;
		}
		
		public virtual RGBColor transmissionColor(Vector3D localPoint)
		{
			return transmissionColor_Renamed_Field;
		}
		
		public virtual float absorbationCoef()
		{
			return absorbationCoef_Renamed_Field;
		}
		
		public virtual float absorbationCoef(Vector3D localPoint)
		{
			return diffuseColor(localPoint).average();
		}
		
		public virtual float reflectionCoef()
		{
			return reflectionCoef_Renamed_Field;
		}
		
		public virtual float reflectionCoef(Vector3D localPoint)
		{
			return specularColor(localPoint).average();
		}

		public virtual float transmissionCoef()
		{
			return transmissionCoef_Renamed_Field;
		}
		
		public virtual float transmissionCoef(Vector3D localPoint)
		{
			return transmissionColor(localPoint).average();
		}
		
		public virtual bool isReflective()
		{
			return isReflective_Renamed_Field;
		}
		
		public virtual bool isReflective(Vector3D localPoint)
		{
			return (reflectionCoef(localPoint) > 0)?true:false;
		}
		
		public virtual bool isAbsorbing()
		{
			return isAbsorbing_Renamed_Field;
		}
		
		public virtual bool isAbsorbing(Vector3D localPoint)
		{
			return (absorbationCoef(localPoint) > 0)?true:false;
		}

		public virtual bool isTransparent()
		{
			return isTransparent_Renamed_Field;
		}
		
		public virtual bool isTransparent(Vector3D localPoint)
		{
			return (transmissionCoef(localPoint) > 0) ? true : false;
		}

		public virtual float phongExponent()
		{
			return phongExponent_Renamed_Field;
		}
		
		public virtual float phongExponent(Vector3D localPoint)
		{
			return phongExponent_Renamed_Field;
		}
		
		public virtual float IOR()
		{
			return IOR_Renamed_Field;
		}
		
		public virtual float IOR(Vector3D localPoint)
		{
			return IOR_Renamed_Field;
		}
		
		public virtual void  setIOR(float IOR)
		{
			this.IOR_Renamed_Field = IOR;
		}
		
		public virtual Bump bump()
		{
			return bump_Renamed_Field;
		}
		
		public virtual Vector3D perturbNormal(Vector3D normal, Vector3D localPoint, Primitive object_Renamed)
		{
			if (bump_Renamed_Field != null)
			{
				return bump_Renamed_Field.perturbNormal(normal, localPoint, object_Renamed);
			}
			else
			{
				return normal;
			}
		}
	}
}