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
				this.red_Renamed_Field = value;
			}
			
		}
		virtual public float Green
		{
			set
			{
				this.green_Renamed_Field = value;
			}
			
		}
		virtual public float Blue
		{
			set
			{
				this.blue_Renamed_Field = value;
			}
			
		}
		//UPGRADE_NOTE: The initialization of  'RGBblack_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGBblack_Renamed_Field;
		//UPGRADE_NOTE: The initialization of  'RGBwhite_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGBwhite_Renamed_Field;
		//UPGRADE_NOTE: The initialization of  'RGBred_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGBred_Renamed_Field;
		//UPGRADE_NOTE: The initialization of  'RGBgreen_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGBgreen_Renamed_Field;
		//UPGRADE_NOTE: The initialization of  'RGBblue_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGBblue_Renamed_Field;
		//UPGRADE_NOTE: The initialization of  'RGByellow_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGByellow_Renamed_Field;
		//UPGRADE_NOTE: The initialization of  'RGBpurple_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGBpurple_Renamed_Field;
		//UPGRADE_NOTE: The initialization of  'RGBcyan_Renamed_Field' was moved to static method 'photontracer.misc.RGBColor'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
		private static RGBColor RGBcyan_Renamed_Field;
		
		private float red_Renamed_Field, green_Renamed_Field, blue_Renamed_Field;
		
		public RGBColor()
		{
			set_Renamed(0);
		}
		
		public RGBColor(float intensity)
		{
			set_Renamed(intensity);
		}
		
		public RGBColor(float red, float green, float blue)
		{
			set_Renamed(red, green, blue);
		}
		
		public RGBColor(RGBColor source)
		{
			set_Renamed(source);
		}
		
		public RGBColor(Vector3D source)
		{
			//UPGRADE_WARNING: Narrowing conversions may produce unexpected results in C#. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1042"'
			set_Renamed((float) source.x(), (float) source.y(), (float) source.z());
		}
		
		public virtual float red()
		{
			return red_Renamed_Field;
		}
		
		public virtual float green()
		{
			return green_Renamed_Field;
		}
		
		public virtual float blue()
		{
			return blue_Renamed_Field;
		}
		
		public virtual void  set_Renamed(float value_Renamed)
		{
			set_Renamed(value_Renamed, value_Renamed, value_Renamed);
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
			if (red_Renamed_Field > 1)
				red_Renamed_Field = 1;
			if (red_Renamed_Field < 0)
				red_Renamed_Field = 0;

			if (green_Renamed_Field > 1)
				green_Renamed_Field = 1;
			if (green_Renamed_Field < 0)
				green_Renamed_Field = 0;
			
			if (blue_Renamed_Field > 1)
				blue_Renamed_Field = 1;
			if (blue_Renamed_Field < 0)
				blue_Renamed_Field = 0;
		}
		
		public virtual void  set_Renamed(float red, float green, float blue)
		{
			Red = red;
			Green = green;
			Blue = blue;
		}
		
		public virtual void  set_Renamed(RGBColor other)
		{
			set_Renamed(other.red(), other.green(), other.blue());
		}
		
		public virtual void  add(RGBColor other)
		{
			set_Renamed(red() + other.red(), green() + other.green(), blue() + other.blue());
		}
		
		public virtual RGBColor addNew(RGBColor other)
		{
			RGBColor result = new RGBColor(red() + other.red(), green() + other.green(), blue() + other.blue());
			
			return result;
		}
		
		public virtual void  sub(RGBColor other)
		{
			set_Renamed(red() - other.red(), green() - other.green(), blue() - other.blue());
		}
		
		public virtual RGBColor subNew(RGBColor other)
		{
			RGBColor result = new RGBColor(red() - other.red(), green() - other.green(), blue() - other.blue());
			
			return result;
		}
		
		public virtual void  scale(float factor)
		{
			set_Renamed(red() * factor, green() * factor, blue() * factor);
		}
		
		public virtual RGBColor scaleNew(float factor)
		{
			RGBColor result = new RGBColor(red() * factor, green() * factor, blue() * factor);
			
			return result;
		}
		
		public virtual void  scale(float factorR, float factorG, float factorB)
		{
			set_Renamed(red() * factorR, green() * factorG, blue() * factorB);
		}
		
		public virtual RGBColor scaleNew(float factorR, float factorG, float factorB)
		{
			RGBColor result = new RGBColor(red() * factorR, green() * factorG, blue() * factorB);
			
			return result;
		}
		
		public virtual void  scale(RGBColor other)
		{
			set_Renamed(red() * other.red(), green() * other.green(), blue() * other.blue());
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

			set_Renamed(r1, g1, b1);  
			//set_Renamed(red() + factor * other.red(), green() + factor * other.green(), blue() + factor * other.blue());   
		}

		public static RGBColor RGBblack()
		{
			return RGBblack_Renamed_Field;
		}
		public static RGBColor RGBwhite()
		{
			return RGBwhite_Renamed_Field;
		}
		public static RGBColor RGBred()
		{
			return RGBred_Renamed_Field;
		}
		public static RGBColor RGBgreen()
		{
			return RGBgreen_Renamed_Field;
		}
		public static RGBColor RGBblue()
		{
			return RGBblue_Renamed_Field;
		}
		public static RGBColor RGByellow()
		{
			return RGByellow_Renamed_Field;
		}
		public static RGBColor RGBpurple()
		{
			return RGBpurple_Renamed_Field;
		}
		public static RGBColor RGBcyan()
		{
			return RGBcyan_Renamed_Field;
		}
		
		public override System.String ToString()
		{
			return new System.Text.StringBuilder("[RGBColor] - <" + red() + ", " + green() + ", " + blue() + ">").ToString();
		}
		static RGBColor()
		{
			RGBblack_Renamed_Field = new RGBColor(0, 0, 0);
			RGBwhite_Renamed_Field = new RGBColor(1, 1, 1);
			RGBred_Renamed_Field = new RGBColor(1, 0, 0);
			RGBgreen_Renamed_Field = new RGBColor(0, 1, 0);
			RGBblue_Renamed_Field = new RGBColor(0, 0, 1);
			RGByellow_Renamed_Field = new RGBColor(1, 1, 0);
			RGBpurple_Renamed_Field = new RGBColor(1, 0, 1);
			RGBcyan_Renamed_Field = new RGBColor(0, 1, 1);
		}
	}
}