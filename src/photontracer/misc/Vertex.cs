using System;
using Vector3D = photontracer.math.Vector3D;
using Point = System.Drawing.Point;

/**
 * Represents a surface point, defined by a point, normal and texture mapping coordinates.
 */
namespace photontracer.misc
{
	public class Vertex 
	{
		/**
		 * Surface point.
		 */
		public Vector3D p;

		/**
		 * Surface normal.
		 */
		public Vector3D n;

		/**
		 * Texture mapping coordinates.
		 */
		public Point tex;

		/**
		 * Creates a new vertex, allocates storage for all fields.
		 */
		public Vertex() 
		{
			p = new Vector3D();
			n = new Vector3D();
			tex = new Point();
		}

		/**
		 * Copy the specified vertex into this one.
		 *
		 * @param v vertex to be copied.
		 */
		public void set(Vertex v) 
		{
			p.set(v.p);
			n.set(v.n);
			tex.X = v.tex.X;
			tex.Y = v.tex.Y;
		}
	}
}