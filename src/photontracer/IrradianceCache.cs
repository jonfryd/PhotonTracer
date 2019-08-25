using System;
using SceneConstants = photontracer.SceneConstants;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;

namespace photontracer
{
	public class IrradianceCache 
	{
		private double tolerance;
		static private double invTolerance;
		private double minSpacing;
		private double maxSpacing;
		private Node root;

		public IrradianceCache(double tolerance, double minSpacing, BoundingBox sceneBounds) {
			this.tolerance = tolerance;
			invTolerance = 1.0 / tolerance;
			this.minSpacing = minSpacing;
			maxSpacing = 100.0 * minSpacing;
			Vector3D ext = sceneBounds.getExtents();
			root = new Node(sceneBounds.getCenter(), 1.0001 * Math.Max (ext.x(), Math.Max (ext.y(), ext.z())));
		}

		public void insert(Vector3D p, Vector3D n, double r0, RGBColor irr, RGBColor[] rotGradient, RGBColor[] transGradient, int depth) {
			Node node = root;

			//r0 = MathUtils.clamp(r0 * tolerance, minSpacing, maxSpacing) * invTolerance;
			r0 = r0 * tolerance;

			if (r0 > minSpacing)
			{
			//	System.Diagnostics.Trace.WriteLine ("jatak");
			}

			r0 = (r0 < minSpacing) ? minSpacing : r0;
			r0 = (r0 > maxSpacing) ? maxSpacing : r0;

			r0 *= invTolerance;

			if (root.isInside(p)) {
				while (node.sideLength >= (4.0 * r0 * tolerance)) {
					int k = 0;
					k |= (p.x() > node.center.x()) ? 1 : 0;
					k |= (p.y() > node.center.y()) ? 2 : 0;
					k |= (p.z() > node.center.z()) ? 4 : 0;
					if (node.children[k] == null) {
						Vector3D c = new Vector3D(node.center);
						c.X = c.x() + (((k & 1) == 0) ? -node.quadSideLength : node.quadSideLength);
						c.Y = c.y() + (((k & 2) == 0) ? -node.quadSideLength : node.quadSideLength);
						c.Z = c.z() + (((k & 4) == 0) ? -node.quadSideLength : node.quadSideLength);
						//c.X += 
						//c.Y += ((k & 2) == 0) ? -node.quadSideLength : node.quadSideLength;
						//c.Z += ((k & 4) == 0) ? -node.quadSideLength : node.quadSideLength;
						node.children[k] = new Node(c, node.halfSideLength);
					}
					node = node.children[k];
				}
			}
			Sample s = new Sample(p, n, r0, irr, rotGradient, transGradient, depth);

			s.next = node.first;
			node.first = s;
		}

		public RGBColor getIrradiance(Vector3D p, Vector3D n, int depth) {
			Sample x = new Sample(p, n, depth);
			double w = root.find(x);

			//if (first != null)
			{
			//	System.Diagnostics.Trace.WriteLine ("weight: " + w);
			}
			
			//System.Console.Out.WriteLine (x.irr + " " + w);

			if (x.irr == null)
			{
				return null;
			}
			else
			{
				RGBColor a = new RGBColor(x.irr);

				a.scale (1.0f / (float) w);
				//x.irr.scale(1.0f / (float) w);

				return a;
			}

			//return () ? null : x.irr.scaleNew(1.0f / (float) w);
		}

		internal class Node {
			internal Node[] children;
			internal Sample first;
			internal Vector3D center;
			internal double sideLength;
			internal double halfSideLength;
			internal double quadSideLength;

			internal Node(Vector3D center, double sideLength) {
				children = new Node[8];
				for (int i = 0; i < 8; i++)
					children[i] = null;
				this.center = new Vector3D(center);
				this.sideLength = sideLength;
				halfSideLength = 0.5 * sideLength;
				quadSideLength = 0.5 * halfSideLength;
				first = null;
			}

			internal bool isInside(Vector3D p) {
				return (Math.Abs (p.x() - center.x()) < halfSideLength) && 
					   (Math.Abs (p.y() - center.y()) < halfSideLength) && 
					   (Math.Abs (p.z() - center.z()) < halfSideLength);
			}

			internal double find(Sample x) {
				int j = 0;
				double weight = 0.0;
				for (Sample s = first; s != null; s = s.next) {
					double wi = Math.Min (1e10, s.weight(x));

					if ((wi > invTolerance) && (x.depth <= s.depth)) {
						j++;
						if (x.irr != null)
						{
							//x.irr.madd(wi, s.getIrradiance(x));
							x.irr.addLerp ((float) wi, s.getIrradiance(x));
						}
						else
						{
							//if (s.getIrradiance(x) == null)
							//{
							//	System.Console.Out.WriteLine ("null!!!");
							//}

							x.irr = s.getIrradiance(x).scaleNew ((float) wi);
							//x.irr.set (s.getIrradiance(x).scaleNew ((float) wi));
						}
						weight += wi;
					}
				}

				//if (first != null)
				//{
				//	System.Diagnostics.Trace.WriteLine (j);
				//}

				for (int i = 0; i < 8; i++)
					if ((children[i] != null) && 
						(Math.Abs(children[i].center.x() - x.pi.x()) <= halfSideLength) && 
						(Math.Abs(children[i].center.y() - x.pi.y()) <= halfSideLength) && 
						(Math.Abs(children[i].center.z() - x.pi.z()) <= halfSideLength))
						weight += children[i].find(x);

				return weight;
			}
		}

		internal class Sample {
			internal Vector3D pi;
			internal Vector3D ni;
			internal double invR0;
			internal RGBColor irr;
			internal RGBColor[] rotGradient;
			internal RGBColor[] transGradient;
			internal Sample next;
			internal int depth;

			internal Sample(Vector3D p, Vector3D n, int depth) 
			{
				pi = new Vector3D(p);
				ni = new Vector3D(n);
				ni.normalize();
				irr = null;
				next = null;
				this.depth = depth;
			}

			internal Sample(Vector3D p, Vector3D n, double r0, RGBColor irr, RGBColor[] rotGradient, RGBColor[] transGradient, int depth) {
				pi = new Vector3D(p);
				ni = new Vector3D(n);
				ni.normalize();
				invR0 = 1.0 / r0;
				this.irr = new RGBColor(irr);
				this.rotGradient = rotGradient;
				this.transGradient = transGradient;
				this.depth = depth;
				next = null;

				//if (Double.IsNaN (transGradient[0].red ()))
				//{
				//	invR0 = 0.0;
				//}
			}

			internal double weight(Sample x) {
				//return 1.0 / ((x.pi.distance(pi) * invR0) + Math.Sqrt(Math.Min(1.0, 1.0 - x.ni.dot(ni))));
				//if (x.pi.distance(pi) < photontracer.SceneConstants_Fields.EPSILON)
				//if (x.pi.distance(pi) < 1e-3)
				{
					//System.Diagnostics.Trace.WriteLine ("small!");
					//return 1.0 / (Math.Sqrt(Math.Min(1.0, 1.0 - x.ni.dot(ni))));
					//return photontracer.SceneConstants_Fields.HUGEVALUE;
					//return 1e10;
				}

				//if (Math.Abs (x.ni.dot(ni)) < 0.1)
				{
				//	return 1e10;
				}

				return 1.0 / ((x.pi.distance(pi) * invR0) + Math.Sqrt(Math.Min(1.0, 1.0 - x.ni.dot(ni))));
 				//return 1.0 / ((x.pi.distance(pi) * invR0));
				//return 0.1;
			}

			internal RGBColor getIrradiance(Sample x) {
				RGBColor temp = new RGBColor(irr);
				//RGBColor temp = new RGBColor();

				//System.Diagnostics.Trace.WriteLine (transGradient[0].scaleNew ((float) (x.pi.x() - pi.x())));
				//System.Diagnostics.Trace.Flush ();

				temp.addLerp ((float) (x.pi.x() - pi.x()), transGradient[0]);
				temp.addLerp ((float) (x.pi.y() - pi.y()), transGradient[1]);
				temp.addLerp ((float) (x.pi.z() - pi.z()), transGradient[2]);
				//temp.addLerp ((float) (pi.x() - x.pi.x()), transGradient[0]);
				//temp.addLerp ((float) (pi.y() - x.pi.y()), transGradient[1]);
				//temp.addLerp ((float) (pi.z() - x.pi.z()), transGradient[2]);
				//temp.madd(x.pi.x() - pi.x(), transGradient[0]);
				//temp.madd(x.pi.y() - pi.y(), transGradient[1]);
				//temp.madd(x.pi.z() - pi.z(), transGradient[2]);
				Vector3D cross = ni.cross (x.ni);
				temp.addLerp ((float) cross.x(), rotGradient[0]);
				temp.addLerp ((float) cross.y(), rotGradient[1]);
				temp.addLerp ((float) cross.z(), rotGradient[2]);
				//temp.madd(cross.x(), rotGradient[0]);
				//temp.madd(cross.y(), rotGradient[1]);
				//temp.madd(cross.z(), rotGradient[2]);
				//if (Double.IsNaN (temp.red ()))
				//{
				//	temp = null;
				//}

				return temp;
			}
		}
	}
}