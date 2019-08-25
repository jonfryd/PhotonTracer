// Various global constants
using System;

namespace photontracer
{
	public struct SceneConstants 
	{
		public readonly static double EPSILON = 1e-6;
		public readonly static double HUGE_VALUE = 1e10;

		public readonly static double SMALL_DISTANCE = 1e0; // threshold for choosing the accurate solution in favor of the approximate
		
		public readonly static int MIN_PHOTONS = 10;
		public readonly static int MAX_PHOTONS = 500;
		public readonly static int MAX_BOUNCES = 5;
		
		public readonly static float MAX_PHOTON_DIST = 4.0f;
		public readonly static float MAX_PHOTON_DIST_PREC = 1.0f;
		public readonly static float INITIAL_PHOTON_DIST = 0.5f;

		//public readonly static float IRRADIANCECACHETOLERANCE = 0.1f;
		//public readonly static float IRRADIANCECACHESPACING = 0.01f;
		public readonly static float IRRADIANCE_CACHE_TOLERANCE = 0.15f;
		public readonly static float IRRADIANCE_CACHE_SPACING = 0.3f;
		//public readonly static float IRRADIANCECACHESPACING = 0.1f;
		public readonly static float IRRADIANCE_SAMPLES = 600;

		public readonly static int MAX_DEPTH = 5;
		public readonly static int MAX_AA_DEPTH = 2;
		public readonly static float MAX_COLOR_DIFF = 0.02f;
	}
}