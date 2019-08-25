// Scene class
using System;
using photontracer.misc;
using Vector3D = photontracer.math.Vector3D;
using Primitive = photontracer.objects.Primitive;
using BoundingBox = photontracer.BoundingBox;

namespace photontracer
{
	public class Scene
	{
		virtual public RGBColor Background
		{
			set
			{
				this.background_ = new RGBColor(value);
			}
			
		}
		virtual public Camera Camera
		{
			set
			{
				this.camera_ = value;
			}
			
		}

		private System.Collections.ArrayList primitiveList_;
		private System.Collections.ArrayList lightList_;
		private RGBColor background_;
		private Camera camera_;
		private BoundingBox boundingbox;
		
		public Scene()
		{
			primitiveList_ = new System.Collections.ArrayList(10);
			lightList_ = new System.Collections.ArrayList(10);
			
			Background = RGBColor.RGBblue();
			
			Camera = new Camera();

			boundingbox = new BoundingBox ();
		}
		
		public virtual System.Collections.ArrayList primitiveList()
		{
			return primitiveList_;
		}
		
		public virtual void  addPrimitive(Primitive primitive)
		{
			primitiveList_.Add(primitive);

			boundingbox.include (primitive.getBoundingBox ());
		}
		
		public virtual System.Collections.ArrayList lightList()
		{
			return lightList_;
		}
		
		public virtual void  addLight(Light light)
		{
			lightList_.Add(light);
		}
		
		public virtual RGBColor background()
		{
			return background_;
		}
		
		public virtual Camera camera()
		{
			return camera_;
		}

		public virtual BoundingBox getBoundingBox()
		{
			return boundingbox;
		}
	}
}