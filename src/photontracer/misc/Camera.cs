// Basic camera class
using System;
using Vector3D = photontracer.math.Vector3D;
namespace photontracer.misc
{
	
	public class Camera
	{
		virtual public double FocalLength
		{
			set
			{
				this.focalLength_Field = value;
			}
			
		}
		private double focalLength_Field;
		
		public Camera()
		{
			FocalLength = 1;
		}
		
		public Camera(double focalLength)
		{
			FocalLength = focalLength;
		}
		
		public virtual double focalLength()
		{
			return focalLength_Field;
		}
		
		public virtual Ray getRay(double u, double v)
		{
			Vector3D direction = new Vector3D(u, v, focalLength_Field);
			direction.normalize();
			
			return new Ray(new Vector3D(u, v, 0), direction);
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Camera] -\n  Focal length: " + focalLength_Field).ToString();
		}
	}
}