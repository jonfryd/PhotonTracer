// Primitive class (very basic!)
using System;
using Vector3D = photontracer.math.Vector3D;
using Material = photontracer.material.Material;
using Ray = photontracer.misc.Ray;
using Intersection = photontracer.misc.Intersection;
using SceneConstants = photontracer.SceneConstants;

namespace photontracer.objects
{
	
	public class Primitive : PrimitiveInterface
	{
		virtual public Material Material
		{
			set
			{
				this.material_ = value;
			}
			
		}
		virtual public Vector3D Position
		{
			set
			{
				this.position_ = value;

				boundingbox.getCenter().set (value);
			}
			
		}
		private Material material_;
		private Vector3D position_;
		private BoundingBox boundingbox;
		
		public Primitive()
		{
			boundingbox = new BoundingBox ();

			Position = new Vector3D();
			
			material_ = new Material();
		}
		
		public Primitive(Vector3D position)
		{
			boundingbox = new BoundingBox ();

			Position = position;
			
			material_ = new Material();
		}
		
		public virtual Material material()
		{
			return material_;
		}
		
		public virtual Vector3D position()
		{
			return position_;
		}
		
		public virtual bool intersect(Ray ray, Intersection intersection)
		{
			return false;
		}
		
		public virtual Vector3D normal(Vector3D intersectionPoint, Vector3D viewPoint)
		{
			return new Vector3D();
		}

		public virtual BoundingBox getBoundingBox()
		{
			boundingbox.recalcMinMax();

			return boundingbox;			
		}
		
		public virtual Vector3D mapTextureCoordinate(Vector3D localPoint)
		{
			return new Vector3D();
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Primitive] -\n  Position: " + position()).ToString();
		}
	}
}