// Basic light class - inherit and override 'color(Vector3D)' for something more interesting
using System;
using photontracer.misc;
using photontracer.math;
using Vertex = photontracer.misc.Vertex;
using Triangle = photontracer.objects.Triangle;

namespace photontracer
{
	public class TriangleAreaLight : Light
	{
		protected Triangle tri;
		protected Vertex v0;
		protected Vertex v1;
		protected Vertex v2;
		protected Vector3D edge1, edge2;
		private float area;
		
		public TriangleAreaLight()
		{
		}
		
		public TriangleAreaLight(Triangle tri, RGBColor color, float intensity, int noPhotons) 
		{
			this.tri = tri;

			this.v0 = tri.vertex0 ();
			this.v1 = tri.vertex1 ();
			this.v2 = tri.vertex2 ();

			edge1 = v1.p.subNew (v0.p);
			edge2 = v2.p.subNew (v0.p);

			area = 0.5f * (float) edge1.cross (edge2).norm ();

			Color = color;
			Intensity = intensity * area * (float) Math.PI;
			Photons = noPhotons;
			Position = v0.p.addNew (v1.p.addNew (v2.p)).scaleNew (1.0 / 3.0);
		}

		public Triangle triangle ()
		{
			return tri;
		}
		
		public override int u_samples ()
		{
			return 14;
		}
		
		public override int v_samples ()
		{
			return 14;
		}
	
		public override Vector3D position(int u, int v)
		{ 
			double	fu = (((double) u) + ThreadLocalRandom.NextDouble()) / (double) (u_samples ());
			double	fv = (((double) v) + ThreadLocalRandom.NextDouble()) / (double) (v_samples ());
			//double	fu = (((double) u)) / (double) (u_samples () - 1);
			//double	fv = (((double) v)) / (double) (v_samples () - 1);

			if ((fu + fv) > 1.0)
			{
				return null;
			}

			return v0.p.addNew (edge1.scaleNew (fu).addNew (edge2.scaleNew (fv)));
		}

		public override void  sample(Vector3D pos, Vector3D ori, RGBColor power)
		{
			double	dx, dy, dz;
			double	u, v;
	
			// triangle position rejection sampling
			do 
			{
				u = ThreadLocalRandom.NextDouble();
				v = ThreadLocalRandom.NextDouble();
			}
			while ((u + v) > 1.0);
			
			// diffuse direction light rejection sampling
			do 
			{
				dx = 2.0 * ThreadLocalRandom.NextDouble() - 1.0;
				dy = 2.0 * ThreadLocalRandom.NextDouble() - 1.0;
				dz = 2.0 * ThreadLocalRandom.NextDouble() - 1.0;
			}
			while ((dx * dx + dy * dy + dz * dz) > 1.0);
			
			pos.set(v0.p.addNew (edge1.scaleNew (u).addNew (edge2.scaleNew (v))));
			ori.set(dx, dy, dz);
			power.set(getFinalColor ());
		}
	}
}