// Intersection container class
using System;
using Vector3D = photontracer.math.Vector3D;
using Primitive = photontracer.objects.Primitive;
namespace photontracer.misc
{
	
	public class Intersection
	{
		virtual public Vector3D IntersectionPoint
		{
			set
			{
				this.intersectionPoint_Renamed_Field = value;
			}
			
		}
		virtual public Primitive IntersectedObject
		{
			set
			{
				this.intersectedObject_Renamed_Field = value;
			}
			
		}
		virtual public double Lambda
		{
			set
			{
				this.lambda_Renamed_Field = value;
			}
			
		}
		private Vector3D intersectionPoint_Renamed_Field;
		private Primitive intersectedObject_Renamed_Field;
		private double lambda_Renamed_Field;
		
		public Intersection()
		{
			set_Renamed(null, null, 0);
		}
		
		public Intersection(Vector3D intersectionPoint, Primitive intersectedObject, double lambda)
		{
			set_Renamed(intersectionPoint, intersectedObject, lambda);
		}
		
		public Intersection(Intersection other)
		{
			set_Renamed(other);
		}
		
		public virtual Vector3D intersectionPoint()
		{
			return intersectionPoint_Renamed_Field;
		}
		
		public virtual Primitive intersectedObject()
		{
			return intersectedObject_Renamed_Field;
		}
		
		public virtual double lambda()
		{
			return lambda_Renamed_Field;
		}
		
		public virtual void  set_Renamed(Vector3D intersectionPoint, Primitive intersectedObject, double lambda)
		{
			IntersectionPoint = intersectionPoint;
			IntersectedObject = intersectedObject;
			Lambda = lambda;
		}
		
		public virtual void  set_Renamed(Intersection other)
		{
			set_Renamed(other.intersectionPoint(), other.intersectedObject(), other.lambda());
		}
	}
}