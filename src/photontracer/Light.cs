// Basic light class - inherit and override 'color(Vector3D)' for something more interesting
using System;
using photontracer.misc;
using photontracer.math;
namespace photontracer
{
	
	public class Light
	{
		virtual public Vector3D Position
		{
			set
			{
				this.position_Field = value;
			}
			
		}
		virtual public float Intensity
		{
			set
			{
				this.intensity_Field = value;
				
				updateFinalColor();
			}
			
		}
		virtual public RGBColor Color
		{
			set
			{
				this.color_Field = value;
				
				updateFinalColor();
			}
			
		}
		virtual public int Photons
		{
			set
			{
				photons_Field = value;
			}
			
		}
		private Vector3D position_Field;
		private RGBColor color_Field;
		private RGBColor finalColor;
		private float intensity_Field;
		private int photons_Field;
		
		public Light()
		{
		}
		
		public Light(Vector3D position, RGBColor color, float intensity, int noPhotons)
		{
			Position = position;
			Color = color;
			Intensity = intensity;
			Photons = noPhotons;
		}
		
		virtual public int u_samples ()
		{
			return 1;
		}
		
		virtual public int v_samples ()
		{
			return 1;
		}

		public virtual Vector3D position(int u, int v)
		{
			return position_Field;
		}
		
		public virtual float intensity()
		{
			return intensity_Field;
		}
		
		public virtual RGBColor color()
		{
			return color_Field;
		}
		
		public virtual int photons()
		{
			return photons_Field;
		}
		
		public virtual RGBColor color(Vector3D point)
		{
			return finalColor;
		}

		protected virtual RGBColor getFinalColor ()
		{
			return finalColor;
		}
		
		private void  updateFinalColor()
		{
			finalColor = color().scaleNew(intensity_Field);
		}
		
		public virtual void  sample(Vector3D pos, Vector3D ori, RGBColor power)
		{
			double x, y, z;
			
			// diffuse point light rejection sampling
			do 
			{
				x = 2.0 * SupportClass.Random.NextDouble() - 1.0;
				y = 2.0 * SupportClass.Random.NextDouble() - 1.0;
				//y = 2.0 * SupportClass.Random.NextDouble() - 2.0;
				z = 2.0 * SupportClass.Random.NextDouble() - 1.0;
				//z = 1.0 * SupportClass.Random.NextDouble(); - 1.0;
			}
			while ((x * x + y * y + z * z) > 1.0);
			
			pos.set(position_Field);
			ori.set(x, y, z);
			power.set(finalColor);
		}
	}
}