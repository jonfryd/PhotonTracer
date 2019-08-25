// Photon map class
using System;
using System.Threading;
using SceneConstants = photontracer.SceneConstants;
using Vector3D = photontracer.math.Vector3D;
using RGBColor = photontracer.misc.RGBColor;

namespace photontracer.photonmap
{
	
	public class PhotonMap : SceneConstants
	{
		private System.Collections.ArrayList photons;
		
		private int storedPhotons_Field;
		private int halfStoredPhotons;
		private int prevScale;
		
		private Vector3D bboxMin;
		private Vector3D bboxMax;
		
		public PhotonMap()
		{
			storedPhotons_Field = 0;
			prevScale = 1;
			
			photons = new System.Collections.ArrayList(10);
			
			bboxMin = new Vector3D(photontracer.SceneConstants_Fields.HUGEVALUE);
			bboxMax = new Vector3D(- photontracer.SceneConstants_Fields.HUGEVALUE);
			
			photons.Add(new Photon());
		}
		
		public virtual int storedPhotons()
		{
			return storedPhotons_Field;
		}
		
		public virtual Photon getPhoton(int i)
		{
			return (Photon) photons[i];
		}
		
		public virtual Photon store(RGBColor power, Vector3D position, Vector3D direction, Vector3D surfaceNormal)
		{
			storedPhotons_Field++;
			
			Photon photon = new Photon();
			
			photon.precomputedIrradiance = false;
			
			photon.power = new RGBColor(power);
			photon.position = new Vector3D(position);
			
			photon.number = storedPhotons_Field;
			
			photon.toSpherical(direction);
			photon.surfaceToSpherical(surfaceNormal);
			
			position.updateMinMax(bboxMin, bboxMax);
			
			photons.Add(photon);
			
			return photon;
		}
		
		public virtual void  scalePhotonPower(float scale)
		{
			for (int i = prevScale; i <= storedPhotons_Field; i++)
			{
				Photon p = getPhoton(i);
				
				p.power.scale(scale);
			}
			
			if (storedPhotons_Field >= prevScale)
			{
				prevScale = storedPhotons_Field + 1;
			}
		}
		
		public virtual void  balance()
		{
			if (storedPhotons_Field > 1)
			{
				Photon[] pa1;
				Photon[] pa2;
				
				pa1 = new Photon[storedPhotons_Field + 1];
				pa2 = new Photon[storedPhotons_Field + 1];
				
				for (int i = 0; i <= storedPhotons_Field; i++)
				{
					pa2[i] = getPhoton(i);
				}
				
				balanceSegment(pa1, pa2, 1, 1, storedPhotons_Field);
				
				// reorganise balance kd-tree (make a heap)
				int foo = 1;
				int j = 1;
				int d;
				
				Photon fooPhoton = getPhoton(j);
				
				for (int i = 1; i <= storedPhotons_Field; i++)
				{
					d = pa1[j].number;
					pa1[j] = null;
					
					if (d != foo)
					{
						photons[j] = getPhoton(d);
						getPhoton(d);
					}
					else
					{
						photons[j] = fooPhoton;
						System.Object generatedAux = fooPhoton;
						
						if (i < storedPhotons_Field)
						{
							for (; foo <= storedPhotons_Field; foo++)
							{
								if (pa1[foo] != null)
								{
									break;
								}
							}
							
							fooPhoton = getPhoton(foo);
							
							j = foo;
						}
						
						continue;
					}
					
					j = d;
				}
			}
			
			halfStoredPhotons = (storedPhotons_Field / 2) - 1;
		}
		
		private void  medianSplit(Photon[] p, int left, int right, int median, int axis)
		{
			Photon p2;
			
			while (right > left)
			{
				double v = p[right].position.get(axis);
				
				int i = left - 1;
				int j = right;
				
				for (; ; )
				{
					while (p[++i].position.get(axis) < v)
						;
					while (p[--j].position.get(axis) > v && j > left)
						;
					
					if (i >= j)
					{
						break;
					}
					
					p2 = p[i];
					p[i] = p[j];
					p[j] = p2;
				}
				
				p2 = p[i];
				p[i] = p[right];
				p[right] = p2;
				
				if (i >= median)
				{
					right = i - 1;
				}
				if (i <= median)
				{
					left = i + 1;
				}
			}
		}
		
		private void  balanceSegment(Photon[] pbal, Photon[] porg, int index, int start, int end)
		{
			// compute new median
			int median = 1;
			
			while ((4 * median) <= (end - start + 1))
			{
				median += median;
			}
			
			if ((3 * median) <= (end - start + 1))
			{
				median += median;
				median += start - 1;
			}
			else
			{
				median = end - median + 1;
			}
			
			// find axis to split along
			short axis = 2;
			
			if ((bboxMax.x() - bboxMin.x()) > (bboxMax.y() - bboxMin.y()) && (bboxMax.x() - bboxMin.x()) > (bboxMax.z() - bboxMin.z()))
			{
				axis = 0;
			}
			else if ((bboxMax.y() - bboxMin.y()) > (bboxMax.z() - bboxMin.z()))
			{
				axis = 1;
			}
			
			// partition photon block around the median
			
			medianSplit(porg, start, end, median, axis);
			
			pbal[index] = porg[median];
			pbal[index].plane = axis;
			
			// recursively balance the left and right block
			
			if (median > start)
			{
				// balance left segment
				if (start < (median - 1))
				{
					double temp = bboxMax.get(axis);
					
					bboxMax.set(axis, pbal[index].position.get(axis));
					balanceSegment(pbal, porg, (2 * index), start, (median - 1));
					bboxMax.set(axis, temp);
				}
				else
				{
					pbal[2 * index] = porg[start];
				}
			}
			
			if (median < end)
			{
				// balance right segment
				if ((median + 1) < end)
				{
					double temp = bboxMin.get(axis);
					
					bboxMin.set(axis, pbal[index].position.get(axis));
					balanceSegment(pbal, porg, (2 * index) + 1, (median + 1), end);
					bboxMin.set(axis, temp);
				}
				else
				{
					pbal[(2 * index) + 1] = porg[end];
				}
			}
		}
		
		public virtual void  precomputeRadiance(float maxDist, int noPhotons)
		{
			const int							interval = 500;
			int									indexBegin, indexEnd;
			precomputeRadianceWorkerThread[]	Threads = new precomputeRadianceWorkerThread[4];

			for (int i = 0; i < Threads.Length; i++)
			{
				Threads[i] = null;
			}

			//
			indexBegin = 1;
			indexEnd   = indexBegin + interval - 1;

			do
			{
				bool	activity = false;

				for (int i = 0; i < Threads.Length; i++)
				{
					if ((Threads[i] == null) || (Threads[i].isDone () == true))
					{
						if ((Threads[i] != null) && (Threads[i].isDone () == true))
						{
							//
						}

						if (indexEnd > storedPhotons())
						{
							indexEnd = storedPhotons();
						}

						if (indexBegin <= storedPhotons())
						{
							Threads[i] = new precomputeRadianceWorkerThread(indexBegin, indexEnd, maxDist, noPhotons, this);

							Thread t = new Thread(new ThreadStart(Threads[i].ThreadProc));
							t.Start ();

							//
							indexBegin += interval;
							indexEnd   += interval;

							//
							activity = true;
						}
					}
					else
					{
						//
						activity = true;
					}
				}

				if (activity == false)
				{
					break;
				}

				System.Threading.Thread.Sleep (10);
			}
			while (true);

		}
		
		internal class precomputeRadianceWorkerThread
		{
			private int				start;
			private int				stop;
			private float			maxDist;
			private int				noPhotons;
			private PhotonMap		mainInstance;
			private volatile bool	done;

			public precomputeRadianceWorkerThread (int iBegin, int iEnd, float maxDist, int noPhotons, PhotonMap main)
			{
				start          = iBegin;
				stop           = iEnd;
				this.maxDist   = maxDist;
				this.noPhotons = noPhotons;
				mainInstance   = main;
				done           = false;
			}
		
			public bool isDone () 
			{ 
                return  done;
			}

			public void ThreadProc ()
			{
				Vector3D surfaceDirection = new Vector3D();
			
				for (int i = start; i <= stop; i++)
				{
					Photon p = mainInstance.getPhoton(i);
				
					p.accumPower = new RGBColor();
					p.surfaceToCartesian(surfaceDirection);
				
					mainInstance.radianceEstimate(p.accumPower, p.position, surfaceDirection, maxDist, noPhotons);
				
					p.precomputedIrradiance = true;
				}

				// signal!
				lock (this)
				{
					done = true;
				}
			}
		}

		// Lambertian! Replace with general BSDF.
		public virtual void  radianceEstimatePrecomputed(RGBColor rad, Vector3D position, Vector3D normal, float initialDist, float maxDist)
		{
			NearestPhotons np;
			
			// locate the nearest photon
			float r = initialDist;
			
			do 
			{
				np = new NearestPhotons(1, r, position);
				
				locatePhotonsPrecomputed(normal, np, 1);
				
				r = (float) (r * 2.0);
			}
			while (np.found == 0 && r < maxDist);
			
			if (np.found > 0)
			{
				Photon p = getPhoton(np.indices[1]);
				
				rad.set(p.accumPower);
			}
			else
			{
				rad.set(0.0f);
			}
		}
		
		public virtual void  irradianceEstimate(RGBColor irrad, Vector3D position, Vector3D normal, float maxDist, int noPhotons)
		{
			float ALPHA = 0.918f;
			float MBETA = - 1.953f;
			float DENOMFACTOR = 1.0f / (1.0f - (float) System.Math.Exp(MBETA));
			
			NearestPhotons np = new NearestPhotons(noPhotons, maxDist, position);
			
			irrad.set(0.0f);
			
			// locate the nearest photons
			locatePhotons(np, 1);
			
			// if less than MINPHOTONS return
			if (np.found < photontracer.SceneConstants_Fields.MINPHOTONS)
			{
				return ;
			}
			
			Vector3D direction = new Vector3D();
			
			float mittrs = MBETA / (2.0f * (float) np.dist2[0]);
			
			// sum irrandiance from all photons
			for (int i = 1; i <= np.found; i++)
			{
				Photon p = getPhoton(np.indices[i]);
				
				// the toCartesian call and following if can be omitted (for speed)
				// if the scene does not have any thin surfaces
				p.toCartesian(direction);
				
				if (direction.dot(normal) < 0.0)
				{
					float gaussWeight = 1.0f - (1.0f - (float) System.Math.Exp((float) p.position.distanceSqr(position) * mittrs)) * DENOMFACTOR;
					
					irrad.add(p.power.scaleNew(gaussWeight));
				}
			}
			
			// estimate of density
			irrad.scale(ALPHA / (float) (System.Math.PI * np.dist2[0]));
		}
		
		// Lambertian! Replace with general BSDF.
		public virtual void  radianceEstimate(RGBColor rad, Vector3D position, Vector3D normal, float maxDist, int noPhotons)
		{
			//float ALPHA = 0.918f;
			float MBETA = - 1.953f;
			float DENOMFACTOR = 1.0f / (1.0f - (float) System.Math.Exp(MBETA));
			
			NearestPhotons np = new NearestPhotons(noPhotons, maxDist, position);
			
			rad.set(0.0f);
			
			// locate the nearest photons
			locatePhotons(np, 1);
			
			// if less than MINPHOTONS return
			if (np.found < photontracer.SceneConstants_Fields.MINPHOTONS)
			{
				return ;
			}
			
			Vector3D direction = new Vector3D();
			
			//float mittrs = MBETA / (2.0f * (float) np.dist2[0]);
			float kdenom = (float) (1.0 / (1.1 * Math.Sqrt (np.dist2[0])));
			
			// sum irrandiance from all photons
			for (int i = 1; i <= np.found; i++)
			{
				float cosNL;
				
				Photon p = getPhoton(np.indices[i]);
				
				// the toCartesian call and following if can be omitted (for speed)
				// if the scene does not have any thin surfaces
				p.toCartesian(direction);
				
				cosNL = (float) direction.dot(normal);
				//cosNL = -1.0f;
				
				if (cosNL < 0.0)
				{
					//float gaussWeight = 1.0f - (1.0f - (float) System.Math.Exp((float) p.position.distanceSqr(position) * mittrs)) * DENOMFACTOR;
					float coneWeight = 1.0f - (float) (p.position.distance (position) * kdenom); 
					
					//rad.add(p.power.scaleNew((- cosNL) * gaussWeight));
					//rad.add(p.power.scaleNew((- cosNL)));
					rad.add(p.power.scaleNew (coneWeight));
				}
			}
			
			// estimate of density
			//rad.scale(ALPHA / (float) (System.Math.PI * np.dist2[0]));
			//rad.scale(1.0f / (float) (System.Math.PI * np.dist2[0]));
			rad.scale(1.0f / (float) ((1.0f - 2.0f / (3.0f * 1.1f)) * System.Math.PI * np.dist2[0]));
		}
		
		public virtual void  locatePhotons(NearestPhotons np, int index)
		{
			double dist1, dist2;
			Photon p = getPhoton(index);
			
			if (index < halfStoredPhotons)
			{
				dist1 = np.position.get(p.plane) - p.position.get(p.plane);
				
				if (dist1 > 0.0)
				{
					// if dist1 is positive search right plane
					locatePhotons(np, (2 * index) + 1);
					
					if (dist1 * dist1 < np.dist2[0])
					{
						locatePhotons(np, (2 * index));
					}
				}
				else
				{
					// dist1 is negative search left first
					locatePhotons(np, (2 * index));
					
					if (dist1 * dist1 < np.dist2[0])
					{
						locatePhotons(np, (2 * index) + 1);
					}
				}
			}
			
			// compute squared distance between current photon and np.position
			dist2 = p.position.distanceSqr(np.position);
			
			if (dist2 < np.dist2[0])
			{
				// we found a photon :) Insert it in the candidate list
				
				if (np.found < np.max)
				{
					// heap is not full; use array
					np.found++;
					np.dist2[np.found] = dist2;
					np.indices[np.found] = index;
				}
				else
				{
					int j, parent;
					
					if (!np.gotHeap)
					{
						// build heap
						int halfFound;
						int photIndex;
						double tempDist;
						
						halfFound = np.found >> 1;
						
						for (int k = halfFound; k >= 1; k--)
						{
							parent = k;
							
							photIndex = np.indices[k];
							tempDist = np.dist2[k];
							
							while (parent < halfFound)
							{
								j = parent + parent;
								
								if (j < np.found && np.dist2[j] < np.dist2[j + 1])
								{
									j++;
								}
								if (tempDist >= np.dist2[j])
								{
									break;
								}
								
								np.dist2[parent] = np.dist2[j];
								np.indices[parent] = np.indices[j];
								parent = j;
							}
							
							np.dist2[parent] = tempDist;
							np.indices[parent] = photIndex;
						}
						
						np.gotHeap = true;
					}
					
					// insert new photon into max heap
					// delete largest element, insert new, and reorder the heap
					
					parent = 1;
					j = 2;
					
					while (j <= np.found)
					{
						if (j < np.found && np.dist2[j] < np.dist2[j + 1])
						{
							j++;
						}
						if (dist2 > np.dist2[j])
						{
							break;
						}
						
						np.dist2[parent] = np.dist2[j];
						np.indices[parent] = np.indices[j];
						
						parent = j;
						j += j;
					}

					//!!!
					if (dist2 < np.dist2[parent])
					{
						np.dist2[parent] = dist2;
						np.indices[parent] = index;
					}
					
					np.dist2[0] = np.dist2[1];
				}
			}
		}
		
		public virtual void  locatePhotonsPrecomputed(Vector3D normal, NearestPhotons np, int index)
		{
			double dist1, dist2;
			Photon p = getPhoton(index);
			
			if (index < halfStoredPhotons)
			{
				dist1 = np.position.get(p.plane) - p.position.get(p.plane);
				
				if (dist1 > 0.0)
				{
					// if dist1 is positive search right plane
					locatePhotonsPrecomputed(normal, np, (2 * index) + 1);
					
					if (dist1 * dist1 < np.dist2[0])
					{
						locatePhotonsPrecomputed(normal, np, (2 * index));
					}
				}
				else
				{
					// dist1 is negative search left first
					locatePhotonsPrecomputed(normal, np, (2 * index));
					
					if (dist1 * dist1 < np.dist2[0])
					{
						locatePhotonsPrecomputed(normal, np, (2 * index) + 1);
					}
				}
			}
			
			// compute squared distance between current photon and np.position
			dist2 = p.position.distanceSqr(np.position);
			
			//assert(p.precomputedIrradiance);
			//assert(np.max == 1);
			
			Vector3D surfaceDirection = new Vector3D();
			p.surfaceToCartesian(surfaceDirection);
			
			//System.out.println(normal.dot(surfaceDirection));
			
			if (dist2 < np.dist2[0] && normal.dot(surfaceDirection) > 0.0)
			{
				// we found a photon :) Insert it in the candidate list
				
				if (np.found < 1)
				{
					// heap is not full; use array
					np.found++;
					np.dist2[np.found] = dist2;
					np.indices[np.found] = index;
				}
				else
				{
					// exchange element if necessary
					
					if (np.dist2[0] > dist2)
					{
						np.dist2[1] = dist2;
						np.indices[1] = index;
						
						np.dist2[0] = dist2;
					}
				}
			}
		}
	}
}