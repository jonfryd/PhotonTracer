// 3D vector class
using System;
using RGBColor = photontracer.misc.RGBColor;
namespace photontracer.math
{
	
	public class Vector3D
	{
		virtual public double X
		{
			set
			{
				this.x_Renamed_Field = value;
			}
			
		}
		virtual public double Y
		{
			set
			{
				this.y_Renamed_Field = value;
			}
			
		}
		virtual public double Z
		{
			set
			{
				this.z_Renamed_Field = value;
			}
			
		}
		private double x_Renamed_Field, y_Renamed_Field, z_Renamed_Field;
		
		public Vector3D()
		{
			x_Renamed_Field = y_Renamed_Field = z_Renamed_Field = 0;
		}
		
		public Vector3D(double a)
		{
			set_Renamed(a);
		}
		
		public Vector3D(double x, double y, double z)
		{
			set_Renamed(x, y, z);
		}
		
		
		public Vector3D(double theta, double phi)
		{
			set_Renamed(System.Math.Sin(theta) * System.Math.Cos(phi), System.Math.Sin(theta) * System.Math.Sin(phi), System.Math.Cos(theta));
		}
		
		public Vector3D(Vector3D source)
		{
			set_Renamed(source);
		}
		
		public Vector3D(RGBColor source)
		{
			set_Renamed(source.red(), source.green(), source.blue());
		}
		
		public virtual double x()
		{
			return x_Renamed_Field;
		}
		
		public virtual double y()
		{
			return y_Renamed_Field;
		}
		
		public virtual double z()
		{
			return z_Renamed_Field;
		}
		
		public virtual double get_Renamed(int i)
		{
			switch (i)
			{
				
				case 0: 
					return x();
				
				case 1: 
					return y();
				
				case 2: 
					return z();
				
				
				default: 
					return 0.0;
				
			}
		}
		
		public virtual void  set_Renamed(int i, double value_Renamed)
		{
			switch (i)
			{
				
				case 0: 
					X = value_Renamed;
					break;
				
				case 1: 
					Y = value_Renamed;
					break;
				
				case 2: 
					Z = value_Renamed;
					break;
				
				
				default: 
					return ;
				
			}
		}
		
		public virtual double sqrNorm()
		{
			return (x() * x() + y() * y() + z() * z());
		}
		
		public virtual double norm()
		{
			return System.Math.Sqrt(sqrNorm());
		}
		
		public virtual void  normalize()
		{
			try
			{
				double length = 1.0 / norm();
				
				scale(length);
			}
			catch (System.ArithmeticException e)
			{
				//UPGRADE_TODO: Method 'java.io.PrintStream.println' was converted to 'System.Console.WriteLine' which has a different behavior. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1073"'
				System.Console.Out.WriteLine(e);
			}
		}
		
		public virtual double dot(Vector3D other)
		{
			return (x() * other.x() + y() * other.y() + z() * other.z());
		}
		
		public virtual Vector3D cross(Vector3D other)
		{
			Vector3D result = new Vector3D(y() * other.z() - z() * other.y(), z() * other.x() - x() * other.z(), x() * other.y() - y() * other.x());
			
			return result;
		}
		
		public virtual double distanceSqr(Vector3D other)
		{
			Vector3D diffVector = subNew(other);
			
			return diffVector.sqrNorm();
		}
		
		public virtual double distance(Vector3D other)
		{
			Vector3D diffVector = subNew(other);
			
			return diffVector.norm();
		}
		
		public virtual void  set_Renamed(double value_Renamed)
		{
			set_Renamed(value_Renamed, value_Renamed, value_Renamed);
		}
		
		public virtual void  set_Renamed(double x, double y, double z)
		{
			X = x;
			Y = y;
			Z = z;
		}
		
		public virtual void  set_Renamed(Vector3D other)
		{
			set_Renamed(other.x(), other.y(), other.z());
		}
		
		public virtual void  neg()
		{
			set_Renamed(- x(), - y(), - z());
		}
		
		public virtual Vector3D negNew()
		{
			Vector3D result = new Vector3D(- x(), - y(), - z());
			
			return result;
		}
		
		public virtual void  add(double value_Renamed)
		{
			set_Renamed(x() + value_Renamed, y() + value_Renamed, z() + value_Renamed);
		}
		
		public virtual Vector3D addNew(double value_Renamed)
		{
			Vector3D result = new Vector3D(x() + value_Renamed, y() + value_Renamed, z() + value_Renamed);
			
			return result;
		}
		
		public virtual void  add(Vector3D other)
		{
			set_Renamed(x() + other.x(), y() + other.y(), z() + other.z());
		}
		
		public virtual Vector3D addNew(Vector3D other)
		{
			Vector3D result = new Vector3D(x() + other.x(), y() + other.y(), z() + other.z());
			
			return result;
		}
		
		public virtual void  sub(Vector3D other)
		{
			set_Renamed(x() - other.x(), y() - other.y(), z() - other.z());
		}
		
		public virtual Vector3D subNew(Vector3D other)
		{
			Vector3D result = new Vector3D(x() - other.x(), y() - other.y(), z() - other.z());
			
			return result;
		}
		
		public virtual void  scale(double factor)
		{
			set_Renamed(x() * factor, y() * factor, z() * factor);
		}
		
		public virtual Vector3D scaleNew(double factor)
		{
			Vector3D result = new Vector3D(x() * factor, y() * factor, z() * factor);
			
			return result;
		}
		
		public virtual void  scale(double factorX, double factorY, double factorZ)
		{
			set_Renamed(x() * factorX, y() * factorY, z() * factorZ);
		}
		
		public virtual Vector3D scaleNew(double factorX, double factorY, double factorZ)
		{
			Vector3D result = new Vector3D(x() * factorX, y() * factorY, z() * factorZ);
			
			return result;
		}
		
		public virtual void  scale(Vector3D other)
		{
			set_Renamed(x() * other.x(), y() * other.y(), z() * other.z());
		}
		
		public virtual Vector3D scaleNew(Vector3D other)
		{
			Vector3D result = new Vector3D(x() * other.x(), y() * other.y(), z() * other.z());
			
			return result;
		}
		
		public virtual void  updateMinMax(Vector3D min, Vector3D max)
		{
			if (x_Renamed_Field < min.x())
			{
				min.X = x_Renamed_Field;
			}
			if (x_Renamed_Field > max.x())
			{
				max.X = x_Renamed_Field;
			}
			
			if (y_Renamed_Field < min.y())
			{
				min.Y = y_Renamed_Field;
			}
			if (y_Renamed_Field > max.y())
			{
				max.Y = y_Renamed_Field;
			}
			
			if (z_Renamed_Field < min.z())
			{
				min.Z = z_Renamed_Field;
			}
			if (z_Renamed_Field > max.z())
			{
				max.Z = z_Renamed_Field;
			}
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[Vector3D] - (" + x() + ", " + y() + ", " + z() + ")").ToString();
		}
	}
}