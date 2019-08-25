// Photon class
using System;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;
namespace photontracer.photonmap
{
	
	public class Photon
	{
		public Vector3D position;
		public short plane;
		public float theta, phi;
		public float surfaceTheta, surfacePhi;
		public RGBColor power;
		public RGBColor accumPower;
		public int number;
		public bool precomputedIrradiance;
		
		// direction->spherical
		public virtual void  toSpherical(Vector3D direction)
		{
			theta = (float) System.Math.Acos(direction.z());
			phi = (float) System.Math.Atan2(direction.y(), direction.x());
		}
		
		public virtual void  surfaceToSpherical(Vector3D direction)
		{
			surfaceTheta = (float) System.Math.Acos(direction.z());
			surfacePhi = (float) System.Math.Atan2(direction.y(), direction.x());
		}
		
		// spherical->cartesian
		public virtual void  toCartesian(Vector3D direction)
		{
			direction.X = System.Math.Sin(theta) * System.Math.Cos(phi);
			direction.Y = System.Math.Sin(theta) * System.Math.Sin(phi);
			direction.Z = System.Math.Cos(theta);
		}
		
		public virtual void  surfaceToCartesian(Vector3D direction)
		{
			direction.X = System.Math.Sin(surfaceTheta) * System.Math.Cos(surfacePhi);
			direction.Y = System.Math.Sin(surfaceTheta) * System.Math.Sin(surfacePhi);
			direction.Z = System.Math.Cos(surfaceTheta);
		}
	}
}