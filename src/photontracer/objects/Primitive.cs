// Primitive class (very basic!)
using System;
using Vector3D = photontracer.math.Vector3D;
using Material = photontracer.material.Material;
using Ray = photontracer.misc.Ray;
using Intersection = photontracer.misc.Intersection;
using SceneConstants = photontracer.SceneConstants;

namespace photontracer.objects
{
	
	public class Primitive : PrimitiveInterface, SceneConstants
	{
		virtual public Material Material
		{
			set
			{
				this.material_Field = value;
			}
			
		}
		virtual public Vector3D Position
		{
			set
			{
				this.position_Field = value;

				boundingbox.getCenter().set (value);
			}
			
		}
		private Material material_Field;
		private Vector3D position_Field;
		private BoundingBox boundingbox;
		
		public Primitive()
		{
			boundingbox = new BoundingBox ();

			Position = new Vector3D();
			
			material_Field = new Material();
		}
		
		public Primitive(Vector3D position)
		{
			boundingbox = new BoundingBox ();

			Position = position;
			
			material_Field = new Material();
		}
		
		public virtual Material material()
		{
			return material_Field;
		}
		
		public virtual Vector3D position()
		{
			return position_Field;
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