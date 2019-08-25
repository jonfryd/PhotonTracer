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
				this.position_ = value;
			}
			
		}
		virtual public float Intensity
		{
			set
			{
				this.intensity_ = value;
				
				updateFinalColor();
			}
			
		}
		virtual public RGBColor Color
		{
			set
			{
				this.color_ = value;
				
				updateFinalColor();
			}
			
		}
		virtual public int Photons
		{
			set
			{
				photons_ = value;
			}
			
		}
		private Vector3D position_;
		private RGBColor color_;
		private RGBColor finalColor;
		private float intensity_;
		private int photons_;
		
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
			return position_;
		}
		
		public virtual float intensity()
		{
			return intensity_;
		}
		
		public virtual RGBColor color()
		{
			return color_;
		}
		
		public virtual int photons()
		{
			return photons_;
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
			finalColor = color().scaleNew(intensity_);
		}
		
		public virtual void  sample(Vector3D pos, Vector3D ori, RGBColor power)
		{
			double x, y, z;
			
			// diffuse point light rejection sampling
			do 
			{
				x = 2.0 * ThreadLocalRandom.NextDouble() - 1.0;
				y = 2.0 * ThreadLocalRandom.NextDouble() - 1.0;
				//y = 2.0 * ThreadLocalRandom.NextDouble() - 2.0;
				z = 2.0 * ThreadLocalRandom.NextDouble() - 1.0;
				//z = 1.0 * ThreadLocalRandom.NextDouble(); - 1.0;
			}
			while ((x * x + y * y + z * z) > 1.0);
			
			pos.set(position_);
			ori.set(x, y, z);
			power.set(finalColor);
		}
	}
}