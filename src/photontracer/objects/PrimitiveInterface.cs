// Primitive object interface
using System;
using Vector3D = photontracer.math.Vector3D;
using Material = photontracer.material.Material;
using Ray = photontracer.misc.Ray;
using Intersection = photontracer.misc.Intersection;
using BoundingBox = photontracer.BoundingBox;

namespace photontracer.objects
{
	
	internal interface PrimitiveInterface
	{
		Material Material
		{
			set;
			
		}
		Vector3D Position
		{
			set;
			
		}
		Material material();
		
		Vector3D position();
		
		bool intersect(Ray ray, Intersection intersection);
		Vector3D normal(Vector3D intersectionPoint, Vector3D viewPoint);

		BoundingBox getBoundingBox();
		
		Vector3D mapTextureCoordinate(Vector3D localPoint);
		
		System.String ToString();
	}
}