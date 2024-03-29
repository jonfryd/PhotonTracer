// Cylinder primitive class
using System;
using SceneConstants = photontracer.SceneConstants;
using Vector3D = photontracer.math.Vector3D;
using Material = photontracer.material.Material;
using Ray = photontracer.misc.Ray;
using Intersection = photontracer.misc.Intersection;

namespace photontracer.objects
{
	
	public class Cylinder:Primitive
	{
		private void init()
		{
			invPI = 1.0 / System.Math.PI;
		}
		virtual public double Radius
		{
			set
			{
				this.radius_ = value;
				getBoundingBox().getExtents ().set (value, Double.PositiveInfinity, value);
				
				try
				{
					invRadius_ = 1.0 / value;
				}
				catch (System.ArithmeticException e)
				{
					System.Console.Out.WriteLine(e);
				}
			}
			
		}
		private double radius_, invRadius_, invPI;
		
		public Cylinder():base()
		{
			init();
			
			Radius = 1;
		}
		
		public Cylinder(Vector3D position, double radius):base(position)
		{
			init();
			
			Radius = radius;
		}
		
		public virtual double invRadius()
		{
			return invRadius_;
		}
		
		public virtual double radius()
		{
			return radius_;
		}
		
		public override bool intersect(Ray ray, Intersection intersection)
		{
			Vector3D location, direction;
			double a, b, c, d, dx, dz, lx, lz;
			
			location = ray.Location.subNew(position());
			direction = ray.direction().scaleNew(invRadius());
			location.scale(invRadius());
			
			dx = direction.x();
			dz = direction.z();
			lx = location.x();
			lz = location.z();
			
			a = dx * dx + dz * dz;
			b = dx * lx + dz * lz;
			c = lx * lx + lz * lz - 1;
			
			d = (b * b) - a * c;
			
			if (d < 0)
			{
				return false;
			}
			
			try
			{
				Vector3D intersectionPoint;
				double invA, l1, l2, lambda;
				
				invA = 1.0 / a;
				
				l1 = (- b + System.Math.Sqrt(d)) * invA;
				l2 = (- b - System.Math.Sqrt(d)) * invA;
				
				if (l1 >= photontracer.SceneConstants.EPSILON && l2 >= photontracer.SceneConstants.EPSILON)
				{
					lambda = (l1 < l2)?l1:l2;
				}
				else if (l1 >= photontracer.SceneConstants.EPSILON)
				{
					lambda = l1;
				}
				else if (l2 >= photontracer.SceneConstants.EPSILON)
				{
					lambda = l2;
				}
				else
				{
					return false;
				}
				
				intersectionPoint = ray.direction().scaleNew(lambda);
				intersectionPoint.add(ray.Location);
				
				intersection.IntersectionPoint = intersectionPoint;
				intersection.IntersectedObject = this;
				intersection.Lambda = lambda;
				
				return true;
			}
			catch (System.ArithmeticException e)
			{
				System.Console.Out.WriteLine(e);
				
				return false;
			}
		}
		
		public override Vector3D normal(Vector3D intersectionPoint, Vector3D viewPoint)
		{
			Vector3D normal = intersectionPoint.subNew(position());
			normal.Y = 0;
			normal.scale(invRadius());

			if (viewPoint.subNew (intersectionPoint).dot (normal) < 0.0)
			{
				normal.neg ();
			}
			
			return normal;
		}
		
		public override Vector3D mapTextureCoordinate(Vector3D localPoint)
		{
			double ang = System.Math.Atan2(localPoint.z(), localPoint.x()) * invPI;
			
			return new Vector3D(ang, localPoint.y() * invPI * invRadius(), 0);
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Cylinder] -\n  Position: " + position() + "\n  Radius  : " + radius()).ToString();
		}
	}
}