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
				this.intersectionPoint_Field = value;
			}
			
		}
		virtual public Primitive IntersectedObject
		{
			set
			{
				this.intersectedObject_Field = value;
			}
			
		}
		virtual public double Lambda
		{
			set
			{
				this.lambda_Field = value;
			}
			
		}
		private Vector3D intersectionPoint_Field;
		private Primitive intersectedObject_Field;
		private double lambda_Field;
		
		public Intersection()
		{
			set(null, null, 0);
		}
		
		public Intersection(Vector3D intersectionPoint, Primitive intersectedObject, double lambda)
		{
			set(intersectionPoint, intersectedObject, lambda);
		}
		
		public Intersection(Intersection other)
		{
			set(other);
		}
		
		public virtual Vector3D intersectionPoint()
		{
			return intersectionPoint_Field;
		}
		
		public virtual Primitive intersectedObject()
		{
			return intersectedObject_Field;
		}
		
		public virtual double lambda()
		{
			return lambda_Field;
		}
		
		public virtual void  set(Vector3D intersectionPoint, Primitive intersectedObject, double lambda)
		{
			IntersectionPoint = intersectionPoint;
			IntersectedObject = intersectedObject;
			Lambda = lambda;
		}
		
		public virtual void  set(Intersection other)
		{
			set(other.intersectionPoint(), other.intersectedObject(), other.lambda());
		}
	}
}