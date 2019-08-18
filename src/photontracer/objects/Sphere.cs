// Sphere primitive class
using System;
using SceneConstants = photontracer.SceneConstants;
using Vector3D = photontracer.math.Vector3D;
using Material = photontracer.material.Material;
using Ray = photontracer.misc.Ray;
using Intersection = photontracer.misc.Intersection;
namespace photontracer.objects
{
	
	public class Sphere:Primitive
	{
		private void  InitBlock()
		{
			invPI = 1.0 / System.Math.PI;
		}
		virtual public double Radius
		{
			set
			{
				this.radius_Renamed_Field = value;
				getBoundingBox().getExtents ().set_Renamed (value);
				
				try
				{
					invRadius_Renamed_Field = 1.0 / value;
				}
				catch (System.ArithmeticException e)
				{
					//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.WriteLine' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
					System.Console.Out.WriteLine(e);
				}
			}
			
		}
		//UPGRADE_NOTE: The initialization of  'invPI' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private double radius_Renamed_Field, invRadius_Renamed_Field, invPI;
		
		public Sphere():base()
		{
			InitBlock();
			
			Radius = 1;
		}
		
		public Sphere(Vector3D position, double radius):base(position)
		{
			InitBlock();
			
			Radius = radius;
		}
		
		public virtual double invRadius()
		{
			return invRadius_Renamed_Field;
		}
		
		public virtual double radius()
		{
			return radius_Renamed_Field;
		}
		
		public override bool intersect(Ray ray, Intersection intersection)
		{
			Vector3D location, direction;
			double a, b, c, d;
			
			location = ray.Location.subNew(position());
			direction = ray.direction().scaleNew(invRadius());
			location.scale(invRadius());
			
			a = direction.sqrNorm();
			b = direction.dot(location);
			c = location.sqrNorm() - 1;
			
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
				
				if (l1 >= photontracer.SceneConstants_Fields.EPSILON && l2 >= photontracer.SceneConstants_Fields.EPSILON)
				{
					lambda = (l1 < l2)?l1:l2;
				}
				else if (l1 >= photontracer.SceneConstants_Fields.EPSILON)
				{
					lambda = l1;
				}
				else if (l2 >= photontracer.SceneConstants_Fields.EPSILON)
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
				//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.WriteLine' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				System.Console.Out.WriteLine(e);
				
				return false;
			}
		}
		
		public override Vector3D normal(Vector3D intersectionPoint, Vector3D viewPoint)
		{
			Vector3D normal = intersectionPoint.subNew(position());
			normal.scale(invRadius());

			if (viewPoint.subNew (intersectionPoint).dot (normal) < 0.0)
			{
				normal.neg ();
			}
			
			return normal;
		}
		
		public override Vector3D mapTextureCoordinate(Vector3D localPoint)
		{
			double x = localPoint.x();
			double y = localPoint.y();
			double z = localPoint.z();
			
			double elev = System.Math.Atan2(y, System.Math.Sqrt(x * x + z * z)) * invPI;
			double ang = System.Math.Atan2(z, x) * invPI;
			
			return new Vector3D(ang, elev, 0);
		}
		
		public override System.String ToString()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			return new System.Text.StringBuilder("[Sphere] -\n  Position: " + position() + "\n  Radius  : " + radius()).ToString();
		}
	}
}