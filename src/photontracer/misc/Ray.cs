// Ray class (transmission and reflection code should go here...)
using System;
using Vector3D = photontracer.math.Vector3D;
namespace photontracer.misc
{
	
	public class Ray
	{
		virtual public Vector3D Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
			
		}
		virtual public Vector3D Direction
		{
			set
			{
				this.direction_Field = value;
			}
			
		}
		virtual public Vector3D Destination
		{
			set
			{
				Direction = value.subNew(location);
			}
			
		}
		private Vector3D location, direction_Field;
		
		public Ray()
		{
			Location = new Vector3D(0);
			Direction = new Vector3D(0);
		}
		
		public Ray(Vector3D location, Vector3D direction)
		{
			Location = location;
			Direction = direction;
		}
		
		public Ray(Ray other)
		{
			Location = new Vector3D(other.location);
			Direction = new Vector3D(other.direction());
		}
		
		//public virtual Vector3D location()
		//{
		//	return location_Field;
		//}
		
		public virtual Vector3D direction()
		{
			return direction_Field;
		}
		
		public virtual Vector3D destination()
		{
			return location.addNew(direction_Field);
		}
		
		public virtual void  normalize()
		{
			direction_Field.normalize();
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Ray] -\n  Location:  " + location + "\n  Direction: " + direction()).ToString();
		}
	}
}