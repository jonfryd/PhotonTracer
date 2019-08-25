// Triangle primitive class
using System;
using SceneConstants = photontracer.SceneConstants;
using Vector3D = photontracer.math.Vector3D;
using Material = photontracer.material.Material;
using Ray = photontracer.misc.Ray;
using Intersection = photontracer.misc.Intersection;
using Vertex = photontracer.misc.Vertex;

namespace photontracer.objects
{
	public class Triangle:Primitive
	{
		protected Vertex v0;
		protected Vertex v1;
		protected Vertex v2;
		protected Vector3D ng;
		private double d;
		private int dropAxis;
		private double edge1x;
		private double edge1y;
		private double edge2x;
		private double edge2y;

		public Triangle(Vertex v0, Vertex v1, Vertex v2):base()
		{
			this.v0 = v0;
			this.v1 = v1;
			this.v2 = v2;
				
			ng = v1.p.subNew (v0.p).cross (v2.p.subNew (v0.p));
			ng.normalize ();

			d = -((ng.x() * v0.p.x()) + (ng.y() * v0.p.y()) + (ng.z() * v0.p.z()));
			if (Math.Abs(ng.y()) > Math.Abs(ng.x()))
				dropAxis = (Math.Abs(ng.z()) > Math.Abs(ng.y())) ? 2 : 1;
			else
				dropAxis = (Math.Abs(ng.z()) > Math.Abs(ng.x())) ? 2 : 0;
			switch (dropAxis) 
			{
				case 0:
					edge1x = v0.p.y() - v1.p.y();
					edge2x = v0.p.y() - v2.p.y();
					edge1y = v0.p.z() - v1.p.z();
					edge2y = v0.p.z() - v2.p.z();
					break;
				case 1:
					edge1x = v0.p.x() - v1.p.x();
					edge2x = v0.p.x() - v2.p.x();
					edge1y = v0.p.z() - v1.p.z();
					edge2y = v0.p.z() - v2.p.z();
					break;
				default:
					edge1x = v0.p.x() - v1.p.x();
					edge2x = v0.p.x() - v2.p.x();
					edge1y = v0.p.y() - v1.p.y();
					edge2y = v0.p.y() - v2.p.y();
					break;
			}
			double s = 1.0 / ((edge1x * edge2y) - (edge2x * edge1y));
			edge1x *= s;
			edge1y *= s;
			edge2x *= s;
			edge2y *= s;
		}
		
		public Vertex vertex0 ()
		{
			return v0;
		}
		
		public Vertex vertex1 ()
		{
			return v1;
		}
		
		public Vertex vertex2 ()
		{
			return v2;
		}

		public override bool intersect(Ray ray, Intersection intersection)
		{
			double vd;
			double vx;
			double vy;
			Vector3D orig = ray.Location.subNew(position());;
			Vector3D dir = ray.direction();

			/* Check if the ray lies parallel to the plane */
			vd = ng.dot (dir);
			if ((vd > -photontracer.SceneConstants_Fields.EPSILON) && (vd < photontracer.SceneConstants_Fields.EPSILON))
				return false;
			/* Check if ray intersects plane */
			double t = -((ng.x() * orig.x()) + (ng.y() * orig.y()) + (ng.z() * orig.z()) + d) / vd;
			if (t < photontracer.SceneConstants_Fields.EPSILON)
				return false;
			/* Check if intersection is inside the triangle */
			switch (dropAxis) 
			{
				case 0:
					vx = (orig.y() + (dir.y() * t)) - v0.p.y();
					vy = (orig.z() + (dir.z() * t)) - v0.p.z();
					break;
				case 1:
					vx = (orig.x() + (dir.x() * t)) - v0.p.x();
					vy = (orig.z() + (dir.z() * t)) - v0.p.z();
					break;
				default:
					vx = (orig.x() + (dir.x() * t)) - v0.p.x();
					vy = (orig.y() + (dir.y() * t)) - v0.p.y();
					break;
			}
			double u = (edge2x * vy) - (edge2y * vx);
			if ((u < 0.0) || (u > 1.0))
				return false;
			double v = (edge1y * vx) - (edge1x * vy);
			if ((v < 0.0) || ((u + v) > 1.0))
				return false;

			Vector3D intersectionPoint;
			intersectionPoint = ray.direction().scaleNew(t);
			intersectionPoint.add(ray.Location);
				
			intersection.IntersectionPoint = intersectionPoint;
			intersection.IntersectedObject = this;
			intersection.Lambda = t;
			
			return true;
		}
		
		public override Vector3D normal(Vector3D intersectionPoint, Vector3D viewPoint)
		{
			Vector3D normal = new Vector3D (ng);
			
			if (viewPoint.subNew (intersectionPoint).dot (normal) < 0.0)
			{
				normal.neg ();
			}

			return normal;
		}
		
		public override Vector3D mapTextureCoordinate(Vector3D localPoint)
		{
			/*
			Vertex dest = state.getVertex();
			double u = state.getU();
			double v = state.getV();
			state.getRay().getPoint(state.getT(), dest.p);
			if (v0.n != null) {
				dest.n.x = v0.n.x + (u * (v1.n.x - v0.n.x)) + (v * (v2.n.x - v0.n.x));
				dest.n.y = v0.n.y + (u * (v1.n.y - v0.n.y)) + (v * (v2.n.y - v0.n.y));
				dest.n.z = v0.n.z + (u * (v1.n.z - v0.n.z)) + (v * (v2.n.z - v0.n.z));
				dest.n.normalize();
			} else
				dest.n.set(ng);
			dest.tex.x = v0.tex.x + (u * (v1.tex.x - v0.tex.x)) + (v * (v2.tex.x - v0.tex.x));
			dest.tex.y = v0.tex.y + (u * (v1.tex.y - v0.tex.y)) + (v * (v2.tex.y - v0.tex.y));
			state.getGeoNormal().set(ng);
			*/			 
			 
			return new Vector3D(0, 0, 0);
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Triangle] -\n  Position: " + position()).ToString();
		}
	}
}