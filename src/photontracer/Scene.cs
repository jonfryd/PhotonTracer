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
				this.background_Renamed_Field = new RGBColor(value);
			}
			
		}
		virtual public Camera Camera
		{
			set
			{
				this.camera_Renamed_Field = value;
			}
			
		}

		private System.Collections.ArrayList primitiveList_Renamed_Field;
		private System.Collections.ArrayList lightList_Renamed_Field;
		private RGBColor background_Renamed_Field;
		private Camera camera_Renamed_Field;
		private BoundingBox boundingbox;
		
		public Scene()
		{
			primitiveList_Renamed_Field = new System.Collections.ArrayList(10);
			lightList_Renamed_Field = new System.Collections.ArrayList(10);
			
			Background = RGBColor.RGBblue();
			
			Camera = new Camera();

			boundingbox = new BoundingBox ();
		}
		
		public virtual System.Collections.ArrayList primitiveList()
		{
			return primitiveList_Renamed_Field;
		}
		
		public virtual void  addPrimitive(Primitive object_Renamed)
		{
			primitiveList_Renamed_Field.Add(object_Renamed);

			boundingbox.include (object_Renamed.getBoundingBox ());
		}
		
		public virtual System.Collections.ArrayList lightList()
		{
			return lightList_Renamed_Field;
		}
		
		public virtual void  addLight(Light light)
		{
			lightList_Renamed_Field.Add(light);
		}
		
		public virtual RGBColor background()
		{
			return background_Renamed_Field;
		}
		
		public virtual Camera camera()
		{
			return camera_Renamed_Field;
		}

		public virtual BoundingBox getBoundingBox()
		{
			return boundingbox;
		}
	}
}