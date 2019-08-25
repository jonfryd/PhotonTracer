// Various global constants
using System;

namespace photontracer
{
	public struct SceneConstants 
	{
		public readonly static double EPSILON = 1e-6;
		public readonly static double HUGEVALUE = 1e10;

		public readonly static double SMALLDISTANCE = 1e0; // threshold for choosing the accurate solution in favor of the approximate
		
		public readonly static int MINPHOTONS = 10;
		public readonly static int MAXPHOTONS = 500;
		public readonly static int MAXBOUNCES = 5;
		
		public readonly static float MAXPHOTDIST = 4.0f;
		public readonly static float MAXPHOTDISTPREC = 1.0f;
		public readonly static float INITIALPHOTDIST = 0.5f;

		//public readonly static float IRRADIANCECACHETOLERANCE = 0.1f;
		//public readonly static float IRRADIANCECACHESPACING = 0.01f;
		public readonly static float IRRADIANCECACHETOLERANCE = 0.15f;
		public readonly static float IRRADIANCECACHESPACING = 0.3f;
		//public readonly static float IRRADIANCECACHESPACING = 0.1f;
		public readonly static float IRRADIANCESAMPLES = 600;

		public readonly static int MAXDEPTH = 5;
		public readonly static int MAXAADEPTH = 2;
		public readonly static float MAXCOLORDIFF = 0.02f;
	}
}