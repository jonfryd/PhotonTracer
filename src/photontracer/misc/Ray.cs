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
				this.direction_ = value;
			}
			
		}
		virtual public Vector3D Destination
		{
			set
			{
				Direction = value.subNew(location);
			}
			
		}
		private Vector3D location, direction_;
		
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
		//	return location_;
		//}
		
		public virtual Vector3D direction()
		{
			return direction_;
		}
		
		public virtual Vector3D destination()
		{
			return location.addNew(direction_);
		}
		
		public virtual void  normalize()
		{
			direction_.normalize();
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Ray] -\n  Location:  " + location + "\n  Direction: " + direction()).ToString();
		}
	}
}