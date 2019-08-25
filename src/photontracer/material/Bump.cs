// Bump mapping class based on calculate derivatives at pixel neighborhoods
using System;
using SceneConstants = photontracer.SceneConstants;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;
using Primitive = photontracer.objects.Primitive;

namespace photontracer.material
{
	public class Bump
	{
		virtual public Material Source
		{
			set
			{
				this.source_ = value;
			}
			
		}
		virtual public Vector3D GradDisp
		{
			set
			{
				this.gradDisp_ = value;
			}
			
		}
		virtual public float BumpFactor
		{
			set
			{
				this.bumpFactor_ = value;
			}
			
		}
		virtual public Vector3D Samples
		{
			set
			{
				this.samples_ = value;
			}
			
		}
		private Material source_;
		private Vector3D gradDisp_;
		private float bumpFactor_;
		private Vector3D samples_;
		
		public Bump()
		{
			Source = null;
			GradDisp = new Vector3D(0.1, 0.1, 0);
			BumpFactor = 5.0f;
			Samples = new Vector3D(2, 2, 0);
		}
		
		public Bump(Material source, Vector3D gradDisp, float bumpFactor, Vector3D samples)
		{
			Source = source;
			GradDisp = new Vector3D(gradDisp);
			BumpFactor = bumpFactor;
			Samples = new Vector3D(samples);
		}
		
		public Bump(Bump other)
		{
			Source = other.source();
			GradDisp = new Vector3D(other.gradDisp());
			BumpFactor = other.bumpFactor();
			Samples = new Vector3D(other.samples());
		}
		
		public virtual Vector3D perturbNormal(Vector3D normal, Vector3D localPoint, Primitive primitive)
		{
			RGBColor color;
			Vector3D newNormal;
			Vector3D gradientU;
			Vector3D gradientV;
			Vector3D r, s, t;
			Vector3D temp;
			double height, heightDiff;
			double dx, dy;
			double x, y;
			double rDamping, tDamping;
			double rDampingAbs, tDampingAbs;
			double rDampingTotal, tDampingTotal;
			double rTotal, tTotal;
			
			s = normal;
			
			r = s.cross(new Vector3D(0, 1, 0));
			
			if (r.norm() < photontracer.SceneConstants.EPSILON)
			{
				r = new Vector3D(0, 1, 0);
				
				if (System.Math.Abs(s.y() - 1) < photontracer.SceneConstants.EPSILON)
				{
					s = new Vector3D(0, 1, 0);
				}
				else
				{
					s = new Vector3D(0, - 1, 0);
				}
			}
			
			r.normalize();
			t = r.cross(s);
			t.normalize();
			
			gradientU = r.scaleNew(gradDisp().x());
			gradientV = t.scaleNew(gradDisp().y());
			
			color = source().diffuseColor(primitive.mapTextureCoordinate(localPoint));
			height = color.average();
			
			dx = (samples().x() > 1.0)?(2.0 / (samples().x() - 1.0)):0;
			dy = (samples().y() > 1.0)?(2.0 / (samples().y() - 1.0)):0;
			
			rTotal = 0;
			tTotal = 0;
			
			rDampingTotal = 0;
			tDampingTotal = 0;
			
			y = - 1.0;
			
			for (int iy = 0; iy < (int) samples().y(); iy++)
			{
				tDampingAbs = (1.0 - (y * y) * 0.8);
				
				if (tDampingAbs > (1.0 - photontracer.SceneConstants.EPSILON))
				{
					tDampingAbs = 0;
				}
				
				tDamping = (y > 0)?tDampingAbs:- tDampingAbs;
				
				x = - 1.0;
				
				for (int ix = 0; ix < (int) samples().x(); ix++)
				{
					temp = gradientU.scaleNew(x);
					temp.add(gradientV.scaleNew(y));
					temp.add(localPoint);
					
					color = source().diffuseColor(primitive.mapTextureCoordinate(temp));
					
					heightDiff = height - color.average();
					
					rDampingAbs = (1.0 - (x * x) * 0.8);
					
					if (rDampingAbs > (1.0 - photontracer.SceneConstants.EPSILON))
					{
						rDampingAbs = 0;
					}
					
					rDamping = (x > 0)?rDampingAbs:- rDampingAbs;
					
					rDampingTotal += rDampingAbs;
					tDampingTotal += tDampingAbs;
					
					rTotal += heightDiff * rDamping;
					tTotal += heightDiff * tDamping;
					
					x += dx;
				}
				
				y += dy;
			}
			
			r.scale(rTotal / rDampingTotal);
			t.scale(tTotal / tDampingTotal);
			
			newNormal = r.addNew(t);
			newNormal.scale(bumpFactor());
			newNormal.add(s);
			newNormal.normalize();
			
			return newNormal;
		}
		
		public virtual Material source()
		{
			return source_;
		}
		
		public virtual Vector3D gradDisp()
		{
			return gradDisp_;
		}
		
		public virtual float bumpFactor()
		{
			return bumpFactor_;
		}
		
		public virtual Vector3D samples()
		{
			return samples_;
		}
	}
}