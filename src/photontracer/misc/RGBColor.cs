// RGB color class
using System;
using Vector3D = photontracer.math.Vector3D;
namespace photontracer.misc
{
	
	public class RGBColor
	{
		virtual public float Red
		{
			set
			{
				this.red_ = value;
			}
			
		}
		virtual public float Green
		{
			set
			{
				this.green_ = value;
			}
			
		}
		virtual public float Blue
		{
			set
			{
				this.blue_ = value;
			}
			
		}
		private static RGBColor RGBblack_;
		private static RGBColor RGBwhite_;
		private static RGBColor RGBred_;
		private static RGBColor RGBgreen_;
		private static RGBColor RGBblue_;
		private static RGBColor RGByellow_;
		private static RGBColor RGBpurple_;
		private static RGBColor RGBcyan_;
		
		private float red_, green_, blue_;
		
		public RGBColor()
		{
			set(0);
		}
		
		public RGBColor(float intensity)
		{
			set(intensity);
		}
		
		public RGBColor(float red, float green, float blue)
		{
			set(red, green, blue);
		}
		
		public RGBColor(RGBColor source)
		{
			set(source);
		}
		
		public RGBColor(Vector3D source)
		{
			set((float) source.x(), (float) source.y(), (float) source.z());
		}
		
		public virtual float red()
		{
			return red_;
		}
		
		public virtual float green()
		{
			return green_;
		}
		
		public virtual float blue()
		{
			return blue_;
		}
		
		public virtual void  set(float value)
		{
			set(value, value, value);
		}
		
		public virtual RGBColor convertTo24Bits()
		{
			return scaleNew(255.0f);
		}
		
		public virtual RGBColor convertFrom24Bits()
		{
			return scaleNew(1.0f / 255.0f);
		}
		
		public virtual RGBColor convertToGrey()
		{
			return new RGBColor(intensity());
		}
		
		public virtual float average()
		{
			return (red() + green() + blue()) * (1.0f / 3.0f);
		}
		
		public virtual float intensity()
		{
			return (red() * 0.2125f + green() * 0.7154f + blue() * 0.0721f);
		}
		
		public virtual void  clamp()
		{
			if (red_ > 1)
				red_ = 1;
			if (red_ < 0)
				red_ = 0;

			if (green_ > 1)
				green_ = 1;
			if (green_ < 0)
				green_ = 0;
			
			if (blue_ > 1)
				blue_ = 1;
			if (blue_ < 0)
				blue_ = 0;
		}
		
		public virtual void  set(float red, float green, float blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}
		
		public virtual void  set(RGBColor other)
		{
			set(other.red(), other.green(), other.blue());
		}
		
		public virtual void  add(RGBColor other)
		{
			set(red() + other.red(), green() + other.green(), blue() + other.blue());
		}
		
		public virtual RGBColor addNew(RGBColor other)
		{
			RGBColor result = new RGBColor(red() + other.red(), green() + other.green(), blue() + other.blue());
			
			return result;
		}
		
		public virtual void  sub(RGBColor other)
		{
			set(red() - other.red(), green() - other.green(), blue() - other.blue());
		}
		
		public virtual RGBColor subNew(RGBColor other)
		{
			RGBColor result = new RGBColor(red() - other.red(), green() - other.green(), blue() - other.blue());
			
			return result;
		}
		
		public virtual void  scale(float factor)
		{
			set(red() * factor, green() * factor, blue() * factor);
		}
		
		public virtual RGBColor scaleNew(float factor)
		{
			RGBColor result = new RGBColor(red() * factor, green() * factor, blue() * factor);
			
			return result;
		}
		
		public virtual void  scale(float factorR, float factorG, float factorB)
		{
			set(red() * factorR, green() * factorG, blue() * factorB);
		}
		
		public virtual RGBColor scaleNew(float factorR, float factorG, float factorB)
		{
			RGBColor result = new RGBColor(red() * factorR, green() * factorG, blue() * factorB);
			
			return result;
		}
		
		public virtual void  scale(RGBColor other)
		{
			set(red() * other.red(), green() * other.green(), blue() * other.blue());
		}
		
		public virtual RGBColor scaleNew(RGBColor other)
		{
			RGBColor result = new RGBColor(red() * other.red(), green() * other.green(), blue() * other.blue());
			
			return result;
		}
		
		public virtual void  addLerp(float factor, RGBColor other)
		{
			float r1 = red() + factor * other.red();
			float g1 = green() + factor * other.green();
			float b1 = blue() + factor * other.blue();

			//if (r1 < 0.0f) r1 = 0.0f;
			//if (g1 < 0.0f) g1 = 0.0f;
			//if (b1 < 0.0f) b1 = 0.0f;

			set(r1, g1, b1);  
			//set(red() + factor * other.red(), green() + factor * other.green(), blue() + factor * other.blue());   
		}

		public static RGBColor RGBblack()
		{
			return RGBblack_;
		}
		public static RGBColor RGBwhite()
		{
			return RGBwhite_;
		}
		public static RGBColor RGBred()
		{
			return RGBred_;
		}
		public static RGBColor RGBgreen()
		{
			return RGBgreen_;
		}
		public static RGBColor RGBblue()
		{
			return RGBblue_;
		}
		public static RGBColor RGByellow()
		{
			return RGByellow_;
		}
		public static RGBColor RGBpurple()
		{
			return RGBpurple_;
		}
		public static RGBColor RGBcyan()
		{
			return RGBcyan_;
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[RGBColor] - <" + red() + ", " + green() + ", " + blue() + ">").ToString();
		}
		static RGBColor()
		{
			RGBblack_ = new RGBColor(0, 0, 0);
			RGBwhite_ = new RGBColor(1, 1, 1);
			RGBred_ = new RGBColor(1, 0, 0);
			RGBgreen_ = new RGBColor(0, 1, 0);
			RGBblue_ = new RGBColor(0, 0, 1);
			RGByellow_ = new RGBColor(1, 1, 0);
			RGBpurple_ = new RGBColor(1, 0, 1);
			RGBcyan_ = new RGBColor(0, 1, 1);
		}
	}
}