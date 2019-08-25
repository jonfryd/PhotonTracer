// NearestPhoton class
using System;
using Vector3D = photontracer.math.Vector3D;

namespace photontracer.photonmap
{
	public class NearestPhotons
	{
		public int max;
		public int found;
		public bool gotHeap;
		public Vector3D position;
		public double[] dist2;
		public int[] indices;
		
		internal NearestPhotons(int nphots, double maxDist, Vector3D pos)
		{
			dist2 = new double[nphots + 1];
			indices = new int[nphots + 1];
			
			position = new Vector3D(pos);
			max = nphots;
			found = 0;
			gotHeap = false;
			dist2[0] = maxDist * maxDist;
		}
	}
}