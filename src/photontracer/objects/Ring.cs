// Ring primitive class
using System;
using SceneConstants = photontracer.SceneConstants;
using Vector3D = photontracer.math.Vector3D;
using Material = photontracer.material.Material;
using Ray = photontracer.misc.Ray;
using Intersection = photontracer.misc.Intersection;

namespace photontracer.objects
{
	
	public class Ring:Primitive
	{
		private void init()
		{
			invPI = 1.0 / System.Math.PI;
		}
		virtual public double Radius
		{
			set
			{
				this.radius_Field = value;
				getBoundingBox().getExtents ().X = value;
				getBoundingBox().getExtents ().Z = value;
				
				try
				{
					invRadius_Field = 1.0 / value;
				}
				catch (System.ArithmeticException e)
				{
					System.Console.Out.WriteLine(e);
				}
			}
			
		}
		virtual public double Height
		{
			set
			{
				this.height_Field = value;
				getBoundingBox().getExtents ().Y = value;
			}
		}

		private double radius_Field, invRadius_Field, height_Field, invPI;
		
		public Ring():base()
		{
			init();
			
			Radius = 1;
			Height = 1;
		}
		
		public Ring(Vector3D position, double radius, double height):base(position)
		{
			init();
			
			Radius = radius;
			Height = height;
		}
		
		public virtual double invRadius()
		{
			return invRadius_Field;
		}
		
		public virtual double radius()
		{
			return radius_Field;
		}
		
		public virtual double height()
		{
			return height_Field;
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
				double invA, l1, l2, lambda1, lambda2, f_lambda, no_lambdas;
				
				invA = 1.0 / a;
				
				l1 = (- b + System.Math.Sqrt(d)) * invA;
				l2 = (- b - System.Math.Sqrt(d)) * invA;
				
				lambda1 = -1;
				lambda2 = -1;

				if (l1 >= photontracer.SceneConstants_Fields.EPSILON && l2 >= photontracer.SceneConstants_Fields.EPSILON)
				{
					no_lambdas = 2;
					lambda1 = (l1 < l2)?l1:l2;
					lambda2 = (l1 < l2)?l2:l1;
				}
				else if (l1 >= photontracer.SceneConstants_Fields.EPSILON)
				{
					no_lambdas = 1;
					lambda1 = l1;
				}
				else if (l2 >= photontracer.SceneConstants_Fields.EPSILON)
				{
					no_lambdas = 1;
					lambda1 = l2;
				}
				else
				{
					return false;
				}
	
				intersectionPoint = ray.direction().scaleNew(lambda1);
				intersectionPoint.add(ray.Location);

				double py = intersectionPoint.y () - position().y ();

				if ((py < 0.0) || (py > height ()))
				{
					if (no_lambdas == 2)
					{
						intersectionPoint = ray.direction().scaleNew(lambda2);
						intersectionPoint.add(ray.Location);

						py = intersectionPoint.y () - position().y ();
						
						if ((py < 0.0) || (py > height ()))
						{
							return false;
						}
						else
						{
							f_lambda = lambda2;
						}
					}
					else 
					{
						return false;
					}
				}
				else
				{
					f_lambda = lambda1;
				}
				
				intersection.IntersectionPoint = intersectionPoint;
				intersection.IntersectedObject = this;
				intersection.Lambda = f_lambda;
				
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
			return new System.Text.StringBuilder("[Ring] -\n  Position: " + position() + "\n  Radius  : " + radius() + "\n  Height  : " + height()).ToString();
		}
	}
}