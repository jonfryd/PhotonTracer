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
				this.background_Field = new RGBColor(value);
			}
			
		}
		virtual public Camera Camera
		{
			set
			{
				this.camera_Field = value;
			}
			
		}

		private System.Collections.ArrayList primitiveList_Field;
		private System.Collections.ArrayList lightList_Field;
		private RGBColor background_Field;
		private Camera camera_Field;
		private BoundingBox boundingbox;
		
		public Scene()
		{
			primitiveList_Field = new System.Collections.ArrayList(10);
			lightList_Field = new System.Collections.ArrayList(10);
			
			Background = RGBColor.RGBblue();
			
			Camera = new Camera();

			boundingbox = new BoundingBox ();
		}
		
		public virtual System.Collections.ArrayList primitiveList()
		{
			return primitiveList_Field;
		}
		
		public virtual void  addPrimitive(Primitive primitive)
		{
			primitiveList_Field.Add(primitive);

			boundingbox.include (primitive.getBoundingBox ());
		}
		
		public virtual System.Collections.ArrayList lightList()
		{
			return lightList_Field;
		}
		
		public virtual void  addLight(Light light)
		{
			lightList_Field.Add(light);
		}
		
		public virtual RGBColor background()
		{
			return background_Field;
		}
		
		public virtual Camera camera()
		{
			return camera_Field;
		}

		public virtual BoundingBox getBoundingBox()
		{
			return boundingbox;
		}
	}
}