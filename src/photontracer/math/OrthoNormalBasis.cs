using System;
using Vector3D = photontracer.math.Vector3D;

namespace photontracer.math
{
	public class OrthoNormalBasis 
	{
		private Vector3D u;
		private Vector3D v;
		private Vector3D w;

		private OrthoNormalBasis() 
		{
			u = new Vector3D();
			v = new Vector3D();
			w = new Vector3D();
		}

		public Vector3D transform(Vector3D a, Vector3D dest) 
		{
			dest.X = (a.x() * u.x()) + (a.y() * v.x()) + (a.z() * w.x());
			dest.Y = (a.x() * u.y()) + (a.y() * v.y()) + (a.z() * w.y());
			dest.Z = (a.x() * u.z()) + (a.y() * v.z()) + (a.z() * w.z());
			return dest;
		}

		public Vector3D transform(Vector3D a) 
		{
			double x = (a.x() * u.x()) + (a.y() * v.x()) + (a.z() * w.x());
			double y = (a.x() * u.y()) + (a.y() * v.y()) + (a.z() * w.y());
			double z = (a.x() * u.z()) + (a.y() * v.z()) + (a.z() * w.z());

			a.set (x, y, z);

			return new Vector3D (a);
		}

		public Vector3D untransform(Vector3D a, Vector3D dest) 
		{
			dest.X = a.dot(u);
			dest.Y = a.dot(v);
			dest.Z = a.dot(w);
			return dest;
		}

		public static OrthoNormalBasis makeFromW(Vector3D w) 
		{
			OrthoNormalBasis onb = new OrthoNormalBasis();
			onb.w.set (w);
			onb.w.normalize ();
			//w.normalize(onb.w);

			if ((Math.Abs(onb.w.x()) < Math.Abs(onb.w.y())) && (Math.Abs(onb.w.x()) < Math.Abs(onb.w.z()))) 
			{
				onb.v.X = 0;
				onb.v.Y = onb.w.z();
				onb.v.Z = -onb.w.y();
			} 
			else if (Math.Abs(onb.w.y()) < Math.Abs(onb.w.z())) 
			{
				onb.v.X = onb.w.z();
				onb.v.Y = 0;
				onb.v.Z = -onb.w.x();
			} 
			else 
			{
				onb.v.X = onb.w.y();
				onb.v.Y = -onb.w.x();
				onb.v.Z = 0;
			}

			onb.v.normalize();

			onb.u = onb.v.cross (onb.w);
			//Vector3.cross(onb.v.normalize(), onb.w, onb.u);
			return onb;
		}

		public static OrthoNormalBasis makeFromWV(Vector3D w, Vector3D v) 
		{
			OrthoNormalBasis onb = new OrthoNormalBasis();
			onb.w.set (w);
			onb.w.normalize ();
			//w.normalize(onb.w);

			onb.u = v.cross(onb.w);
			onb.u.normalize();
			//Vector3.cross(v, onb.w, onb.u).normalize();
			
			onb.v = onb.w.cross (onb.v);
			//Vector3.cross(onb.w, onb.u, onb.v);

			return onb;
		}
	}
}