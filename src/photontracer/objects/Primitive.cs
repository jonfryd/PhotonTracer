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
				this.material_Renamed_Field = value;
			}
			
		}
		virtual public Vector3D Position
		{
			set
			{
				this.position_Renamed_Field = value;

				boundingbox.getCenter().set_Renamed (value);
			}
			
		}
		private Material material_Renamed_Field;
		private Vector3D position_Renamed_Field;
		private BoundingBox boundingbox;
		
		public Primitive()
		{
			boundingbox = new BoundingBox ();

			Position = new Vector3D();
			
			material_Renamed_Field = new Material();
		}
		
		public Primitive(Vector3D position)
		{
			boundingbox = new BoundingBox ();

			Position = position;
			
			material_Renamed_Field = new Material();
		}
		
		public virtual Material material()
		{
			return material_Renamed_Field;
		}
		
		public virtual Vector3D position()
		{
			return position_Renamed_Field;
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
		
		public virtual System.String ToString()
		{
			//UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
			return new System.Text.StringBuilder("[Primitive] -\n  Position: " + position()).ToString();
		}
	}
}