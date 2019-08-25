// PhotonTracer: A photon map renderer implemented in Java2
using System;
using System.Diagnostics;
using System.Threading;
using photontracer;
using photontracer.material;
using photontracer.math;
using photontracer.misc;
using photontracer.objects;
using photontracer.photonmap;

class Photontracer
{
	private Scene scene;
	private Camera camera;
	private Image image, imagePhotons;
	private PhotonMap causticsmap;
	private PhotonMap photonmap;
	private IrradianceCache irradianceCache;
	private System.Collections.ArrayList primitiveList;
	private System.Collections.ArrayList lightList;
	private System.Windows.Forms.Form  window, windowPhotons;

	private int noShadowRays, noIrradianceRays, noReflectionRays, noTransmissionRays, noPrimaryRayHits, noPrimaryRays;
	
	internal Photontracer()
	{
		new Photontracer(512, 384, "photontraced.jpg");
	}
	
	internal Photontracer(int width, int height, System.String outputFilename)
	{
		Sphere sph1, sph2, sph3, sph4, sph5, sph6, sph7, sph8, sph9;
		Cylinder cyl1, cyl2;
		Ring ring1;
		//Light light1, light2, light3;
		TriangleAreaLight arealight1, arealight2;
		Vertex roofv1, roofv2, roofv3, roofv4;
		Vertex v1, v2, v3, v4;
		Vertex []cube1 = null;
		Vertex []cube2 = null;
		Triangle tri1, tri2;
		Triangle rooftri1, rooftri2;
		Triangle []cubetri1 = null;
		Triangle []cubetri2 = null;
		//Material redReflectivePlastic, bluePlastic, blueGlass, ground, mirror;
		Material redWall, blueWall, whiteWall, yellowWall, cyanWall;
		Material metal, glass;
		Material lightmat;
		CheckerMaterial checker, mirrorChecker;
		TextureMaterial textureGrass;
		//Bump bump;
		System.DateTime before, after;
		
		scene = new Scene();
		
		scene.Background = (new RGBColor (0.05f, 0.01f, 0.15f));
/*
		sph1 = new Sphere (new Vector3D (   0,        0,    11),    3);
		sph2 = new Sphere (new Vector3D (   2,       -2,     6),  1.0);
		sph3 = new Sphere (new Vector3D (   0, -5e2 - 3,     0),  5e2);
		//sph3 = new Sphere (new Vector3D ( 0, 0,     10), 2);
		sph4 = new Sphere (new Vector3D (-1.8,    -2.25,     6), 0.75);

		cyl1 = new Cylinder (new Vector3D (-7,  0, 13), 1);
		cyl2 = new Cylinder (new Vector3D ( 7,  0, 16), 3);

		redReflectivePlastic = new Material (new RGBColor (0.0f, 0.0f, 0.0f),
			new RGBColor (0.6f, 0.3f, 0.4f),
			new RGBColor (0.4f, 0.5f, 0.5f), 0, 20.0f, 1.0f);

		bluePlastic = new Material (new RGBColor (0.0f, 0.0f, 0.0f),
			new RGBColor (0.2f, 1.0f, 0.1f),
			new RGBColor (0.0f, 0.0f, 0.0f), 0, 10.0f, 1.0f);

		blueGlass = new Material (new RGBColor (0.0f, 0.0f, 0.05f),
			new RGBColor (0.1f, 0.3f, 0.8f),
			new RGBColor (0.0f, 0.0f, 0.0f), 1.0f, 40.0f, 1.52f);

		ground = new Material (new RGBColor (0.0f, 0.00f, 0.00f),
			new RGBColor (1.0f, 1.00f, 1.0f),
			new RGBColor (0.0f, 0.0f, 0.0f), 0.0f, 6.0f, 1.0f);

		mirror = new Material (new RGBColor (0.1f, 0.1f, 0.1f),
			new RGBColor (0.1f, 0.1f, 0.1f),
			new RGBColor (0.6f, 0.6f, 0.6f), 0, 24.0f, 1.0f);

		checker       = new CheckerMaterial (redReflectivePlastic, new RGBColor (0.4f, 0.1f, 0.2f), 0.2f);
		mirrorChecker = new CheckerMaterial (mirror, new RGBColor (0.4f, 0.4f, 0.4f), 0.3f);

		textureGrass = new TextureMaterial (ground, new RGBColor (0.4f, 0.2f, 0.3f), new Vector3D (1.5*1, 1.5*60, 0),
			new Vector3D (1.34, 1.6, 0), false, true, "data/dots.jpg");

		bump = new Bump ();
		bump.Source = (textureGrass);
		bump.Samples = (new Vector3D (3, 3, 0));
		bump.GradDisp = (new Vector3D (0.02, 0.02, 0));
		bump.BumpFactor = (40.0f);

		textureGrass.Bump = (bump);

		sph1.Material = checker;
		sph2.Material = blueGlass;
		//sph3.setMaterial (textureGrass);
		sph3.Material = (redReflectivePlastic);
		sph4.Material = (bluePlastic);

		cyl1.Material = (mirror);
		cyl2.Material = (mirrorChecker);

		scene.addPrimitive (sph1);
		scene.addPrimitive (sph2);
		scene.addPrimitive (sph3);
		scene.addPrimitive (sph4);
		scene.addPrimitive (cyl1);
		scene.addPrimitive (cyl2);
*/
	
		scene.Background = new RGBColor(0.0f, 0.0f, 0.0f);

		//
		//
		// Material creation
		//
		//

		blueWall = new Material(new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.02f, 0.55f, 0.85f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), 10.0f, 1.0f);
		redWall = new Material(new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.8f, 0.34f, 0.25f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), 10.0f, 1.0f);
		whiteWall = new Material(new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.95f, 0.95f, 0.95f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), 10.0f, 1.0f);
		cyanWall = new Material(new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.85f, 0.72f, 0.15f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), 10.0f, 1.0f);
		yellowWall = new Material(new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.1f, 0.77f, 0.85f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), 10.0f, 1.0f);

		lightmat = new Material(new RGBColor(1.0f, 1.0f, 1.0f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), 0.0f, 0.0f);

		metal = new Material(new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.9f, 0.85f, 0.95f), new RGBColor(0.0f, 0.0f, 0.0f), 10.0f, 1.0f);
		glass = new Material(new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(0.0f, 0.0f, 0.0f), new RGBColor(1.0f, 0.8f, 0.85f), 10.0f, 1.5f);
		
		textureGrass = new TextureMaterial (whiteWall, new RGBColor (0.4f, 0.2f, 0.3f), new Vector3D (1.5*60, 1.5*3, 0),
			new Vector3D (1.34, 1.6, 0), true, true, "data/green052.jpg");

		checker = new CheckerMaterial (whiteWall, new RGBColor (0.7f, 0.3f, 0.4f), 0.05f);
		mirrorChecker = new CheckerMaterial (glass, new RGBColor (0.4f, 0.4f, 0.4f), 0.3f);

		//
		//
		// primitive creation
		//
		//

		sph1 = new Sphere(new Vector3D(0, 0 - 5e4 - 9, 0), 5e4);
		sph2 = new Sphere(new Vector3D(0 - 5e4 - 12, 0, 0), 5e4);
		//sph2 = new Sphere(new Vector3D(0 - 5e4 - 3, 0, 3), 5e4);
		sph3 = new Sphere(new Vector3D(0, 0 + 5e4 + 9, 0), 5e4);
		sph4 = new Sphere(new Vector3D(0 + 5e4 + 12, 0, 0), 5e4);
		//sph4 = new Sphere(new Vector3D(0 + 5e4 + 3, 0, -1), 5e4);
		sph5 = new Sphere(new Vector3D(0, 0, 5e6 + 30), 5e6);
		sph8 = new Sphere(new Vector3D(0, 0, 0 - 5e3), 5e3);

		cyl1 = new Cylinder (new Vector3D (-8,  0, 25), 1.5);
		cyl2 = new Cylinder (new Vector3D ( 8,  0, 25), 1.5);

		//ring1 = new Ring(new Vector3D(0, -9, 26), 9, 2.5);
		ring1 = new Ring(new Vector3D(0, -9.2, 27), 10, 3.7);
		
		v1 = new Vertex ();
		v2 = new Vertex ();
		v3 = new Vertex ();
		v4 = new Vertex ();
	
		roofv1 = new Vertex ();
		roofv2 = new Vertex ();
		roofv3 = new Vertex ();
		roofv4 = new Vertex ();
	
		v1.p = new Vector3D (-3.0, 8.999999, 23);
		v2.p = new Vector3D (3.0, 8.999999, 23);
		v3.p = new Vector3D (3.0, 8.999999, 18);
		v4.p = new Vector3D (-3.0, 8.999999, 18);

		roofv1.p = new Vector3D (-20.0, 9, 40);
		roofv2.p = new Vector3D (20.0, 9, 40);
		roofv3.p = new Vector3D (20.0, 9, 0);
		roofv4.p = new Vector3D (-20.0, 9, 0);

		tri1 = new Triangle (v1, v2, v3);
		tri2 = new Triangle (v1, v3, v4);

		rooftri1 = new Triangle (roofv1, roofv2, roofv3);
		rooftri2 = new Triangle (roofv1, roofv3, roofv4);

		GenerateCube (ref cube1, ref cubetri1, new Vector3D(-3, -3, 24), new Vector3D (7, 12, 6), 35.0, whiteWall);
		GenerateCube (ref cube2, ref cubetri2, new Vector3D(6, -7, 21), new Vector3D (5, 4, 5), -25.0, whiteWall);

		//
		//
		// Material assignment
		//
		//

		sph1.Material = whiteWall;
		//sph3.Material = whiteWall;
		sph5.Material = whiteWall;
		sph8.Material = whiteWall;

		rooftri1.Material = whiteWall;
		rooftri2.Material = whiteWall;

		sph2.Material = redWall;
		sph4.Material = blueWall;

		cyl1.Material = yellowWall;
		cyl2.Material = cyanWall;

		tri1.Material = lightmat;
		tri2.Material = lightmat;

		sph6 = new Sphere(new Vector3D(-7, -4.5, 25.5), 3.5);
		sph7 = new Sphere(new Vector3D(2.5, -5.0, 23.5), 3.5);
		sph9 = new Sphere(new Vector3D(0.0, -5.0, 22.5), 4.0);

		sph6.Material = metal;
		sph7.Material = glass;
		sph9.Material = metal;

		ring1.Material = metal;

		//
		//
		// Scene setup (add primitives)
		//
		//

		scene.addPrimitive(sph1);
		scene.addPrimitive(sph2);
		//scene.addPrimitive(sph3);
		scene.addPrimitive(rooftri1);
		scene.addPrimitive(rooftri2);
		scene.addPrimitive(sph4);
		scene.addPrimitive(sph5);
		scene.addPrimitive(sph8);
		//scene.addPrimitive(sph6);
		//scene.addPrimitive(sph7);
		//scene.addPrimitive(sph9);
		scene.addPrimitive(tri1);
		scene.addPrimitive(tri2);
		//scene.addPrimitive(cyl1);
		//scene.addPrimitive(cyl2);
		
		//
		//
		// Light setup
		//
		//

		//light1 = new Light (new Vector3D (-1.8, -2.2,   5), RGBColor.RGByellow(),  700.0f, 120000);
		//light2 = new Light (new Vector3D (  0,    0,   8), RGBColor.RGBwhite() , 50.6f, 100000);
//		light2 = new Light(new Vector3D(0, 1.7, 6), RGBColor.RGBwhite(), 150.0f, 50000);
		//light2 = new Light(new Vector3D(6.5, 4, 20.5), RGBColor.RGBwhite(), 3000.0f, 1700);
		arealight1 = new TriangleAreaLight (tri1, RGBColor.RGBwhite(), 15.0f, 150000);
		arealight2 = new TriangleAreaLight (tri2, RGBColor.RGBwhite(), 15.0f, 150000);
		//light3 = new Light (new Vector3D ( -10,    8,   2), RGBColor.RGBcyan() , 750.0f, 120000);
		//scene.addLight (light1);
		//scene.addLight(light2);
		scene.addLight(arealight1);
		scene.addLight(arealight2);
		//scene.addLight (light3);
		
		//
		//
		// Render image...
		//
		//
		
		causticsmap = new PhotonMap();
		photonmap = new PhotonMap();
		
		image = new Image(width, height);
		imagePhotons = new Image(width, height);
	
		window = new System.Windows.Forms.Form ();
		window.Text = "Rendering";
		windowPhotons = new System.Windows.Forms.Form ();
		windowPhotons.Text = "Photon Positions";

		window.Closing += new System.ComponentModel.CancelEventHandler(windowClosing);
		windowPhotons.Closing += new System.ComponentModel.CancelEventHandler(windowClosing);
		
		//window.getContentPane().Controls.Add(image);
		//window.TopLevel = true;
		//window.Controls.Add(image);
		//window.Owner = image;
		window.Controls.Add (image);
		//window.Size = image.Size;
		window.ClientSize = image.Size;
		//image.Visible = true;
		//image.setDimensions (100,100);
		//window = image;

		//windowPhotons.Controls.Add(imagePhotons);
		//windowPhotons.Controls.Add (imagePhotons);
		windowPhotons.Controls.Add (imagePhotons);
		windowPhotons.ClientSize = imagePhotons.Size;
		
		//window.pack();
		window.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		window.Visible = true;
		
		//windowPhotons.pack();
		windowPhotons.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		windowPhotons.Visible = true;
		
		image.initializeFramebuffer();
		imagePhotons.initializeFramebuffer();
		
		Trace.WriteLine(image);

		//window.Refresh ();
		//window.
		
		primitiveList = scene.primitiveList();
		lightList = scene.lightList();
	
		// sample and trace photons
		sampleLights();
		
		Trace.WriteLine(causticsmap.storedPhotons() + " photons in caustics map.");
		Trace.WriteLine(photonmap.storedPhotons() + " photons in global map.");
		
		// balance photon map
		Trace.WriteLine("Balancing kd-tree.");
		
		causticsmap.balance();
		photonmap.balance();
		
		// precompute irradiance
		Trace.WriteLine("Precomputing irradiance.");
		
		//causticsmap.precomputeRadiance(photontracer.SceneConstants.MAXPHOTDIST, photontracer.SceneConstants.MAXPHOTONS);
		before = System.DateTime.Now;
		photonmap.precomputeRadiance(photontracer.SceneConstants.MAX_PHOTON_DIST, photontracer.SceneConstants.MAX_PHOTONS);
		after = System.DateTime.Now;

		System.TimeSpan timeDiff = after - before;

		// init irradiance cache
		irradianceCache = new IrradianceCache (photontracer.SceneConstants.IRRADIANCE_CACHE_TOLERANCE, photontracer.SceneConstants.IRRADIANCE_CACHE_SPACING, scene.getBoundingBox ());

		before = System.DateTime.Now;
		traceImage(false);
		traceImage(true);
		after = System.DateTime.Now;

		// the time difference
		timeDiff = after - before;
		Trace.WriteLine("\nElapsed: " + timeDiff.TotalMilliseconds / 1000.0 + " secs.\n");
		
		Trace.WriteLine(this + "\n");

		image.saveJPEG(outputFilename);

		System.Windows.Forms.Application.Run (window);
	}
	
    internal void windowClosing(System.Object event_sender, System.ComponentModel.CancelEventArgs evt)
    {
        Debug.WriteLine("\nBye!");
        Trace.Flush();
        
        System.Environment.Exit(0);
    }

	internal void GenerateCube (ref Vertex []vertices, ref Triangle []tris, Vector3D pos, Vector3D dimensions, double yRot, Material mat)
	{
		double	dx, dy, dz;

		vertices = new Vertex[8];
		tris = new Triangle[12];
		
		for (int i = 0; i < 8; i++)
		{	
			vertices[i] = new Vertex();
		}

		dx = dimensions.x () * 0.5;
		dy = dimensions.y () * 0.5;
		dz = dimensions.z () * 0.5;

		vertices[0].p = new Vector3D ( dx,  dy,  dz);
		vertices[1].p = new Vector3D ( dx,  dy, -dz);
		vertices[2].p = new Vector3D ( dx, -dy,  dz);
		vertices[3].p = new Vector3D ( dx, -dy, -dz);
		vertices[4].p = new Vector3D (-dx,  dy,  dz);
		vertices[5].p = new Vector3D (-dx,  dy, -dz);
		vertices[6].p = new Vector3D (-dx, -dy,  dz);
		vertices[7].p = new Vector3D (-dx, -dy, -dz);
		
		double yRotRad = yRot / 180.0 * Math.PI;

		for (int i = 0; i < 8; i++)
		{
			double	tx, tz;

			tx = vertices[i].p.x();
			tz = vertices[i].p.z();

			tx = Math.Cos (yRotRad) * tx + Math.Sin (yRotRad) * tz;
			tz = Math.Cos (yRotRad) * tz - Math.Sin (yRotRad) * tx;

			vertices[i].p.X = tx;
			vertices[i].p.Z = tz;

			vertices[i].p.add (pos);
		}

		// up
		tris[0] = new Triangle (vertices[0], vertices[1], vertices[4]);
		tris[1] = new Triangle (vertices[1], vertices[4], vertices[5]);

		// down
		tris[2] = new Triangle (vertices[2], vertices[3], vertices[6]);
		tris[3] = new Triangle (vertices[3], vertices[6], vertices[7]);
		
		// left
		tris[4] = new Triangle (vertices[4], vertices[5], vertices[6]);
		tris[5] = new Triangle (vertices[5], vertices[6], vertices[7]);

		// right
		tris[6] = new Triangle (vertices[0], vertices[1], vertices[2]);
		tris[7] = new Triangle (vertices[1], vertices[2], vertices[3]);

		// back
		tris[8] = new Triangle (vertices[0], vertices[2], vertices[4]);
		tris[9] = new Triangle (vertices[2], vertices[4], vertices[6]);

		// front
		tris[10] = new Triangle (vertices[1], vertices[3], vertices[5]);
		tris[11] = new Triangle (vertices[3], vertices[5], vertices[7]);
		
		// set material and add triangle to scene
		for (int i = 0; i < 12; i++)
		{
			tris[i].Material = mat;
					
			scene.addPrimitive(tris[i]);
		}
	}
	
	internal virtual void  traceImage(bool doAntialiasing)
	{
		int y, height;
		double v, stepV;
		//double halfStepV;
		
		noShadowRays = 0;
		noIrradianceRays = 0;
		noPrimaryRays = 0;
		noPrimaryRayHits = 0;
		noReflectionRays = 0;
		noTransmissionRays = 0;
		
		camera = scene.camera();
		
		height = image.height();
		stepV = 1.0 / (double) (height - 1);
		
		window.Refresh();
		System.Windows.Forms.Application.DoEvents ();
		
		Trace.Write("\nRendering.");
		
		//
		ScanlineWorkerThread[]	Threads = new ScanlineWorkerThread[4];
	
		for (int i = 0; i < Threads.Length; i++)
		{
			Threads[i] = null;
		}

		//
		y = 0;
		v = 0.5;

		do
		{
			bool	activity = false;

			for (int i = 0; i < Threads.Length; i++)
			{
				if ((Threads[i] == null) || (Threads[i].isDone () == true))
				{
					if ((Threads[i] != null) && (Threads[i].isDone () == true))
					{
						for (int x = 0; x < image.width(); x++)
						{
							image.setPixel (x, Threads[i].lineNo(), Threads[i].scanline()[x]);
						}
			
						window.Refresh ();
					}

					if (y < height)
					{
						Threads[i] = new ScanlineWorkerThread(y, v, doAntialiasing, image, this);

						Thread t = new Thread(new ThreadStart(Threads[i].ThreadProc));
						t.Start ();

						//
						y++;
						v -= stepV;

						if ((y % 25) == 0)
						{
							Trace.Write(".");
						}

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


			//System.Threading.Thread.Sleep (10);
			System.Windows.Forms.Application.DoEvents ();
		}
		while (true);
		
		Trace.WriteLine(" done!");
	}

	internal class ScanlineWorkerThread
	{
		private int				line;
		private double			v;
		private bool			antiAlias;
		private Image			image;
		private Photontracer	mainInstance;
		private bool			done;
		private RGBColor[]		scanlineRGB;

		public ScanlineWorkerThread (int y, double v, bool doAntialiasing, Image finalImage, Photontracer main)
		{
			line		 = y;
			this.v		 = v;
			antiAlias    = doAntialiasing;
			image		 = finalImage;
			mainInstance = main;
			done         = false;
		}
		
		public bool isDone () 
		{ 
			return  done;
		}

		public int lineNo ()
		{
			return line;
		}

		public RGBColor[] scanline ()
		{
			return scanlineRGB;
		}

		public void ThreadProc ()
		{
			int x, width, height;
			double u, stepU, stepV, uvCorrection;
			double halfStepU, halfStepV;
			RGBColor[] tracedColors;
			RGBColor finalColor;
		
			tracedColors = new RGBColor[4];
		
			width  = image.width();
			height = image.height();

			scanlineRGB = new RGBColor[width];
		
			uvCorrection = ((double) width / height) * 0.9375;

			stepU = uvCorrection / (double) (width - 1);
			stepV = 1.0 / (double) (height - 1);
		
			halfStepU = stepU * 0.5;
			halfStepV = stepV * 0.5;

			u = (- 0.5) * uvCorrection;

			for (x = 0; x < width; x++, u += stepU)
			{
				if (antiAlias)
				{
					tracedColors[0] = mainInstance.shadeRay(u - halfStepU, v + halfStepV);
					tracedColors[1] = mainInstance.shadeRay(u + halfStepU, v + halfStepV);
					tracedColors[2] = mainInstance.shadeRay(u - halfStepU, v - halfStepV);
					tracedColors[3] = mainInstance.shadeRay(u + halfStepU, v - halfStepV);
					
					finalColor = mainInstance.adaptiveSampling(u, stepU, v, stepV, tracedColors, 0);
				}
				else
				{
					finalColor = mainInstance.shadeRay(u, v);
				}
				
				scanlineRGB[x] = new RGBColor(finalColor);
			}

			// signal!
			done = true;
		}
	}
	
	internal virtual RGBColor adaptiveSampling(double u, double stepU, double v, double stepV, RGBColor[] colors, int depth)
	{
		RGBColor radiance;
		RGBColor[] newColors;
		RGBColor totalRadiance, tempColor;
		double halfStepU = stepU * 0.5;
		double halfStepV = stepV * 0.5;
		
		newColors = new RGBColor[4];
		totalRadiance = new RGBColor();
		
		radiance = shadeRay(u, v);
		
		// Upper-left
		
		if (((System.Math.Abs(radiance.red() - colors[0].red()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.green() - colors[0].green()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.blue() - colors[0].blue()) > photontracer.SceneConstants.MAX_COLOR_DIFF)) && (depth < photontracer.SceneConstants.MAX_AA_DEPTH))
		{
			newColors[0] = colors[0];
			newColors[1] = shadeRay(u, v + stepV);
			newColors[2] = shadeRay(u - stepU, v);
			newColors[3] = radiance;
			
			totalRadiance.add(adaptiveSampling(u - halfStepU, halfStepU, v + halfStepV, halfStepV, newColors, depth + 1));
		}
		else
		{
			tempColor = radiance.addNew(colors[0]);
			tempColor.scale(0.5f);
			
			totalRadiance.add(tempColor);
		}
		
		// Upper-right
		
		if (((System.Math.Abs(radiance.red() - colors[1].red()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.green() - colors[1].green()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.blue() - colors[1].blue()) > photontracer.SceneConstants.MAX_COLOR_DIFF)) && (depth < photontracer.SceneConstants.MAX_AA_DEPTH))
		{
			newColors[0] = shadeRay(u, v + stepV);
			newColors[1] = colors[1];
			newColors[2] = radiance;
			newColors[3] = shadeRay(u + stepU, v);
			
			totalRadiance.add(adaptiveSampling(u + halfStepU, halfStepU, v + halfStepV, halfStepV, newColors, depth + 1));
		}
		else
		{
			tempColor = radiance.addNew(colors[1]);
			tempColor.scale(0.5f);
			
			totalRadiance.add(tempColor);
		}
		
		// Lower-left
		
		if (((System.Math.Abs(radiance.red() - colors[2].red()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.green() - colors[2].green()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.blue() - colors[2].blue()) > photontracer.SceneConstants.MAX_COLOR_DIFF)) && (depth < photontracer.SceneConstants.MAX_AA_DEPTH))
		{
			newColors[0] = shadeRay(u - stepU, v);
			newColors[1] = radiance;
			newColors[2] = colors[2];
			newColors[3] = shadeRay(u, v - stepV);
			
			totalRadiance.add(adaptiveSampling(u - halfStepU, halfStepU, v - halfStepV, halfStepV, newColors, depth + 1));
		}
		else
		{
			tempColor = radiance.addNew(colors[2]);
			tempColor.scale(0.5f);
			
			totalRadiance.add(tempColor);
		}
		
		// Lower-right
		
		if (((System.Math.Abs(radiance.red() - colors[3].red()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.green() - colors[3].green()) > photontracer.SceneConstants.MAX_COLOR_DIFF) || (System.Math.Abs(radiance.blue() - colors[3].blue()) > photontracer.SceneConstants.MAX_COLOR_DIFF)) && (depth < photontracer.SceneConstants.MAX_AA_DEPTH))
		{
			newColors[0] = radiance;
			newColors[1] = shadeRay(u + stepU, v);
			newColors[2] = shadeRay(u, v - stepV);
			newColors[3] = colors[3];
			
			totalRadiance.add(adaptiveSampling(u + halfStepU, halfStepU, v - halfStepV, halfStepV, newColors, depth + 1));
		}
		else
		{
			tempColor = radiance.addNew(colors[3]);
			tempColor.scale(0.5f);
			
			totalRadiance.add(tempColor);
		}
		
		
		totalRadiance.scale(0.25f);
		
		return totalRadiance;
	}
	
	internal virtual RGBColor shadeRay(double u, double v)
	{
		Ray primaryRay;
		
		primaryRay = camera.getRay(u, v);
		
		return traceRay(primaryRay, photontracer.SceneConstants.MAX_DEPTH, false);
	}

	RGBColor getIrradiance(Vector3D P, Vector3D N, int depth) 
	{
		RGBColor irr;

		int irrM = (int) Math.Max(1, Math.Sqrt(photontracer.SceneConstants.IRRADIANCE_SAMPLES / Math.PI));
		int irrN = (int) Math.Max(1, (irrM * Math.PI));

		//if (irradianceCache == null || state.getDiffuseDepth() > 0)
		//	return globalPhotonMap.getIrradiance(state.getVertex().p, state.getVertex().n);
		
		lock (irradianceCache)
		{
			irr = irradianceCache.getIrradiance(P, N, depth);
		}
		
		if (irr == null) 
		{
			double halfWidth, halfHeight;
			double uvCorrection;
		
			halfWidth = imagePhotons.width() / 2.0;
			halfHeight = imagePhotons.height() / 2.0;
		
			uvCorrection = (halfWidth / halfHeight) * 0.9375;

			double denom = P.z() + camera.focalLength();
						
			double u = P.x() / denom;
			double v = P.y() / denom;
						
			//Vector3D normal = new Vector3D(u, v, camera.focalLength());
			//Vector3D dir    = new Vector3D();
			//p.toCartesian(dir);
						
			Intersection closest = findClosestIntersection(camera.getRay(u, v));

			if ((closest != null) && (closest.intersectionPoint().distanceSqr(P) < photontracer.SceneConstants.EPSILON))
			{
				//Trace.WriteLine (u + " " + v);

				//imagePhotons.setPixel((int) (halfWidth + (2.0 * halfWidth * u / uvCorrection)), (int) (halfHeight - (2.0 * halfHeight * v)), RGBColor.RGBwhite());
				//imagePhotons.setPixel((int) (halfWidth + (2.0 * halfWidth * u / uvCorrection)), (int) (halfHeight - (2.0 * halfHeight * v)), RGBColor.RGByellow());

				//imagePhotons.setPixel(10, 10, RGBColor.RGBwhite());
				//imagePhotons.Refresh();
				//System.Windows.Forms.Application.DoEvents ();
			}

			// compute new sample
			irr = new RGBColor();
			OrthoNormalBasis onb = OrthoNormalBasis.makeFromW(N);

//			Trace.WriteLine (N);
//			Trace.WriteLine (onb.transform (new Vector3D(0.0,0.0,1.0)));

			int hits = 0;
			double invR = 0.0;
			Vector3D w = new Vector3D();

			// irradiance gradients
			RGBColor[] rotGradient = new RGBColor[3];
			RGBColor[] transGradient1 = new RGBColor[3];
			RGBColor[] transGradient2 = new RGBColor[3];
			for (int i = 0; i < 3; i++) 
			{
				rotGradient[i] = new RGBColor();
				transGradient1[i] = new RGBColor();
				transGradient2[i] = new RGBColor();
			}

			// irradiance gradients temp variables
			Vector3D vi = new Vector3D();
			RGBColor rotGradientTemp = new RGBColor();
			Vector3D ui = new Vector3D();
			Vector3D vim = new Vector3D();
			RGBColor transGradient1Temp = new RGBColor();
			RGBColor transGradient2Temp = new RGBColor();
			RGBColor lijm = new RGBColor(); // L_i,j-1
			RGBColor[] lim = new RGBColor[irrM]; // L_i-1,j
			RGBColor[] l0 = new RGBColor[irrM]; // L_0,j
			for (int i = 0; i < irrM; i++) 
			{
				lim[i] = new RGBColor();
				l0[i] = new RGBColor();
			}
			double rijm = 0; // R_i,j-1
			double[] rim = new double[irrM]; // R_i-1,j
			double[] r0 = new double[irrM]; // R_0, j
			bool bOK = true;
			for (int i = 0; i < irrN; i++) 
			{
				double xi = (i + SupportClass.Random.NextDouble()) / irrN;
				double phi = 2 * Math.PI * xi;
				double cosPhi = Math.Cos(phi);
				double sinPhi = Math.Sin(phi);
				vi.X = -sinPhi; //Math.cos(phi + Math.PI * 0.5);
				vi.Y = cosPhi; //Math.sin(phi + Math.PI * 0.5);
				vi.Z = 0.0;
				onb.transform(vi);
				rotGradientTemp.set (RGBColor.RGBblack());
				ui.X = cosPhi;
				ui.Y = sinPhi;
				ui.Z = 0.0;
				onb.transform(ui);
				double phim = (2.0 * Math.PI * i) / irrN;
				vim.X = Math.Cos(phim + (Math.PI * 0.5));
				vim.Y = Math.Sin(phim + (Math.PI * 0.5));
				vim.Z = 0.0;
				onb.transform(vim);
				transGradient1Temp.set (RGBColor.RGBblack());
				transGradient2Temp.set (RGBColor.RGBblack());
				for (int j = 0; j < (irrM / 1); j++) 
				{
					//System.Console.Out.WriteLine (SupportClass.Random.NextDouble());

					double xj = (j + SupportClass.Random.NextDouble()) / irrM;
					double sinTheta = Math.Sqrt(xj);
					double cosTheta = Math.Sqrt(1.0 - xj);
					double rij = Double.PositiveInfinity;
					w.X = cosPhi * sinTheta;
					w.Y = sinPhi * sinTheta;
					w.Z = cosTheta;
					onb.transform(w);
					RGBColor lij = new RGBColor ();

					Ray sampleRay = new Ray(new Vector3D (P), new Vector3D (w));
					//sampleRay.Location.add (N.scaleNew (0.05).addNew (new Vector3D (0.0, -0.05, 0.0)));

					Intersection closestIntersection = findClosestIntersection(sampleRay);

					if (closestIntersection != null) 
					{
						rij = closestIntersection.lambda ();

						if (rij < 0.5) bOK = false;

						//Trace.WriteLine ("Lamb: " + i + " " + j + " " + k + "   "+lij.subNew (lijm).scaleNew((float) k) + "   " + w);

						//if (rij < 0.1)
						{
						//	rij = 1.0;
						}
						{
							invR += (1.0 / Math.Sqrt (rij));
							hits++;
							System.Threading.Interlocked.Increment (ref noIrradianceRays);
							lij.set (traceRay (sampleRay, depth - 1, true));

							//double		ang = ((double) i / (double)irrN) * Math.PI * 2.0;
							//double		rad = (j + 1) * 5.0;
							//int			xp = (int) (Math.Cos(ang) * rad) + 140;
							//int			yp = (int) (Math.Sin(ang) * rad) + 140;

							//Vector3D v1 = sampleRay.direction();
							//Vector3D v1 = N;
							//Vector3D v2 = closestIntersection.intersectedprimitive().normal (closestIntersection.intersectionPoint());
							//double CosAng = -v1.dot(v2);
							//if (CosAng < 0) CosAng = 0;
							//lij.scale ((float) -sampleRay.direction().dot(closestIntersection.intersectedprimitive().normal (closestIntersection.intersectionPoint())));
							//lij.scale ((float) CosAng);

							irr.add(lij);
							//lij.scale(1.0f / (float)rij);
/*
							if (depth == 3)
							{
								for(int s = 0; s < 4; s++)
								{
										for(int t = 0; t < 4; t++)
									{
										imagePhotons.setPixel ((xp + t), (yp + s), lij.scaleNew (1.0f));
									}
								}
							}
*/
							//if (depth == 3) Trace.WriteLine ("L: " + i + " " + j + " " + rij + " " + lij + " " + irr);

							// increment rotational gradient
							//Trace.WriteLine (-sinTheta / cosTheta);
							rotGradientTemp.addLerp ((float) (-sinTheta / cosTheta), lij);
							//}
						}
						//else
						{
							//lij.set (0);
							//rij = Double.PositiveInfinity;
						}
					}

					//rij = (rij < 1.0) ? 1.0 : rij;

					// increment translational gradient
					//double rij = temp.getT();
					double sinThetam = Math.Sqrt((double) j / irrM);
					double cosThetam2 = Math.Cos (Math.Asin (sinThetam));
					cosThetam2 *= cosThetam2;
					if (j > 0) 
					{
						//double k = (sinThetam * (1.0 - ((double) j / irrM))) / Math.Min(rij, rijm);
						double k = (sinThetam * cosThetam2) / Math.Min(rij, rijm);
						//k = (k < 0.1) ? k : 0.1;
						//k *= 0.01;

						//if (depth == 3) Trace.WriteLine ("G1: " + i + " " + j + " " + k + "   "+lij.subNew (lijm).scaleNew((float) k) + "   " + w);
						//Trace.Flush ();
						//if (Math.Min(rij, rijm) > 5e-1)
						transGradient1Temp.add (lij.subNew (lijm).scaleNew((float) k));
					}
					if (i > 0) 
					{
						double sinThetap = Math.Sqrt((double) (j + 1) / irrM);
						double k = (sinThetap - sinThetam) / Math.Min(rij, rim[j]);
						//k *= 0.01;
						//k = (k < 0.1) ? k : 0.1;

						//if (depth == 3) Trace.WriteLine ("G2: " + i + " " + j + " " + k + "   "+lij.subNew (lim[j]).scaleNew((float) k) + "   " + w);

						//if (rijm < 1e5)
						//if (Math.Min(rij, rim[j]) > 5e-1)
							transGradient2Temp.add (lij.subNew (lim[j]).scaleNew((float) k));
					} 
					else 
					{
						r0[j] = rij;
						l0[j].set (lij);
					}

					// set previous
					rijm = rij;
					lijm.set (lij);
					rim[j] = rij;
					lim[j].set (lij);
				}

				// increment rotational gradient vector
				rotGradient[0].addLerp ((float) vi.x(), rotGradientTemp);
				rotGradient[1].addLerp ((float) vi.y(), rotGradientTemp);
				rotGradient[2].addLerp ((float) vi.z(), rotGradientTemp);
				// increment translational gradient vectors
				transGradient1[0].addLerp ((float) ui.x(), transGradient1Temp);
				transGradient1[1].addLerp ((float) ui.y(), transGradient1Temp);
				transGradient1[2].addLerp ((float) ui.z(), transGradient1Temp);
				transGradient2[0].addLerp ((float) vim.x(), transGradient2Temp);
				transGradient2[1].addLerp ((float) vim.y(), transGradient2Temp);
				transGradient2[2].addLerp ((float) vim.z(), transGradient2Temp);

				float fac = (float) (2.0 * Math.PI) / irrN;
				//if (depth == 3) Trace.WriteLine ("sum TG1: " + ui + "   " + transGradient1[0].scaleNew (fac) + " " + transGradient1[1].scaleNew (fac) + " " + transGradient1[2].scaleNew (fac));
				//if (depth == 3) Trace.WriteLine ("sum TG2: " + vim + "   " + transGradient2[0] + " " + transGradient2[1] + " " + transGradient2[2]);
			}

			// finish computing second part of the translational gradient
			vim.X = 0.0;
			vim.Y = 1.0;
			vim.Z = 0.0;
			onb.transform(vim);
			//Trace.WriteLine (vim.dot (N));
			transGradient2Temp.set (RGBColor.RGBblack());
			for (int j = 0; j < (irrM / 1); j++) 
			{
				double sinThetam = Math.Sqrt((double) j / irrM);
				double sinThetap = Math.Sqrt((double) (j + 1) / irrM);
				double k = (sinThetap - sinThetam) / Math.Min(r0[j], rim[j]);
				//transGradient2Temp.add(Color.sub(l0[j], lim[j]).mul(k));
				//Console.Out.WriteLine (k);
				//Trace.WriteLine (k);
				//k = (k < 0.5f) ? k : 0.5f;
				transGradient2Temp.add (l0[j].subNew (lim[j]).scaleNew((float) k));
			}
			transGradient2[0].addLerp ((float) vim.x(), transGradient2Temp);
			transGradient2[1].addLerp ((float) vim.y(), transGradient2Temp);
			transGradient2[2].addLerp ((float) vim.z(), transGradient2Temp);
			// scale first part of translational gradient
			double scale = (2.0 * Math.PI) / irrN;
			//double scale = (0.1) / irrN;
			transGradient1[0].scale((float) scale);
			transGradient1[1].scale((float) scale);
			transGradient1[2].scale((float) scale);
			// sum two pieces of translational gradient
			transGradient1[0].add(transGradient2[0]);
			transGradient1[1].add(transGradient2[1]);
			transGradient1[2].add(transGradient2[2]);
			scale = Math.PI / (irrM * irrN);
//			scale = 1.0 / (irrM * irrN);
			irr.scale((float) scale / (float) Math.PI);
			//irr.set (new RGBColor((float) SupportClass.Random.NextDouble(),(float) SupportClass.Random.NextDouble(),(float) SupportClass.Random.NextDouble()));
			rotGradient[0].scale((float) scale);
			rotGradient[1].scale((float) scale);
			rotGradient[2].scale((float) scale);
			invR = (hits > 0) ? (hits / invR) : 0.0;
//			Console.Out.WriteLine (hits);
			//invR = 0.5;

			//Trace.WriteLine (irr + "  " + invR);
			//Trace.WriteLine (P + "  " + N);
			//Trace.Flush ();

			//transGradient1[0].scale(0.3f);
			//transGradient1[1].scale(0.3f);
			//transGradient1[2].scale(0.3f);
			/*

			double minArad = 10.0 * 1e-3;
			double maxArad = 64.0 * minArad;

		if (pg != NULL) {		// reduce radius if gradient large 
			d = DOT(pg,pg);
			if (d*arad*arad > 1.0)
				arad = 1.0/sqrt(d);
		}
		if (arad < minarad) 
		{
			arad = minarad;
			if (pg != NULL && d*arad*arad > 1.0) 
			{	// cap gradient 
				d = 1.0/arad/sqrt(d);
				for (i = 0; i < 3; i++)
					pg[i] *= d;
			}
		}
		if ((arad /= sqrt(wt)) > maxarad)
			arad = maxarad;
*/
/*
			double dMax = 0.0;

			if (invR < 1e-3)
			{
				invR = maxArad;
			}

			float bright = irr.average ();

			if (bright > 1e-3)
			{
				bright = 1.0f / bright;

				for (int t = 0; t < 3; t++)
				{
				//	transGradient1[t].scale (bright);
				//	rotGradient[t].scale (bright);
				}
			}

			for (int t = 0; t < 3; t++)
			{
				double d = (transGradient1[t].red () * transGradient1[t].red ())+
						   (transGradient1[t].green () * transGradient1[t].green ())+
						   (transGradient1[t].blue () * transGradient1[t].blue ());

				dMax = (d > dMax) ? d : dMax;

				Trace.WriteLine (t + " " + d + " " + d*invR*invR);
			}

			if ((dMax * invR * invR) > 1.0)
			{
				invR = 1.0 / Math.Sqrt (dMax);
			}

			if (invR < minArad)
			{
				invR = minArad;

				if ((dMax * invR * invR) > 1.0)
				{
					dMax = 1.0 / invR / Math.Sqrt (dMax);

					for (int t = 0; t < 3; t++)
					{
						transGradient1[t].scale ((float) dMax);
					}
				}
			}

			invR /= Math.Sqrt (SceneConstants.IRRADIANCECACHETOLERANCE); //!!!

			if (invR > maxArad)
			{
				invR = maxArad;
			}
*/

			if (bOK == false) 
			{
				transGradient1[0].set (0);
				transGradient1[1].set (0);
				transGradient1[2].set (0);
				rotGradient[0].set (0);
				rotGradient[1].set (0);
				rotGradient[2].set (0);
			}

			//if (depth == 2) 
			
			//Trace.WriteLine (hits + " " + u + " " + v + " " + P + " " + N + " " + transGradient1[0] + " " + transGradient1[1] + " " + transGradient1[2]);


			//imagePhotons.Refresh();
			//System.Windows.Forms.Application.DoEvents ();

			lock (irradianceCache)
			{
				irradianceCache.insert(P, N, invR, irr, rotGradient, transGradient1, depth);
			}
			//return irr;
		}
		//irr.scale (0.2f);
		return irr;
	}

	internal RGBColor causticsRadianceEstimate(Vector3D P, Vector3D N)
	{
		RGBColor	radiance = new RGBColor();

		if (causticsmap.storedPhotons () > 0)
		{
			causticsmap.radianceEstimate(radiance, P, N, photontracer.SceneConstants.MAX_PHOTON_DIST, photontracer.SceneConstants.MAX_PHOTONS);
			//causticsmap.radianceEstimatePrecomputed(radiance, P, N, photontracer.SceneConstants.INITIALPHOTDIST, photontracer.SceneConstants.MAXPHOTDISTPREC);
		}

		return radiance;
	}

	internal RGBColor globalRadianceEstimate(Vector3D P, Vector3D N)
	{
		RGBColor	radiance = new RGBColor();

		if (photonmap.storedPhotons () > 0)
		{
			//photonmap.radianceEstimate(radiance, P, N, photontracer.SceneConstants.MAXPHOTDIST, photontracer.SceneConstants.MAXPHOTONS);
			photonmap.radianceEstimatePrecomputed(radiance, P, N, photontracer.SceneConstants.INITIAL_PHOTON_DIST, photontracer.SceneConstants.MAX_PHOTON_DIST_PREC);
		}

		return radiance;
	}

	internal RGBColor accurateSolution(Vector3D P, Vector3D N, Vector3D V, Vector3D localPoint, int depth, Material material)
	{
		RGBColor	directIllum, caustics, diffuse;
		RGBColor	totalRadiance;
		RGBColor	directContribution;
		RGBColor	directSum;
		Vector3D	L, LightPos;
		Ray			shadowRay;
		bool		inSight;
		
		directIllum = new RGBColor();
		caustics = new RGBColor();
		diffuse = new RGBColor();
		totalRadiance = new RGBColor();

		shadowRay = new Ray(P, null);

		directContribution = new RGBColor ();
		directSum = new RGBColor ();

		// Direct illumination
		for (System.Collections.IEnumerator elem = lightList.GetEnumerator(); elem.MoveNext(); )
		{
			Primitive	LightObj;
			int			acceptedLightRays;

			Light light = (Light) elem.Current;
	
			directSum.set (0);
			acceptedLightRays = 0;
	
			for (int i = 0 ; i < light.v_samples (); i++)
			{
				for (int j = 0 ; j < light.u_samples (); j++)
				{
					LightPos = light.position(i, j);

					if (LightPos != null)
					{
						acceptedLightRays++;

						L = LightPos.subNew(P);
						L.normalize();
						shadowRay.Direction = L;

						if (Object.Equals (light.GetType (), typeof (TriangleAreaLight)) == true)
						{
							LightObj = ((TriangleAreaLight) light).triangle ();
						}
						else
						{
							LightObj = null;
						}

						inSight = isInSight (shadowRay, LightPos.distance(P), LightObj);
		
						if (inSight)
						{
							directContribution.set (light.color(P));
							directContribution.scale ((float) Math.Max(0.0, N.dot (L)));

							// scale for point light source
							directContribution.scale ((float) (1.0f / (4.0 * System.Math.PI * P.distanceSqr(LightPos))));

							directSum.add (directContribution);
						}
					}
				}
			}

			directIllum.addLerp (1.0f / ((float) acceptedLightRays), directSum);
		}

		// Caustics
		caustics.add (causticsRadianceEstimate (P, N));

		// using irradiance cache
		diffuse.set (getIrradiance (P, N, depth));

		// Sum contributions
		totalRadiance.add (directIllum);
		totalRadiance.add (caustics);
		totalRadiance.add (diffuse);

		//
		totalRadiance.scale (material.diffuseColor (localPoint));

		return totalRadiance;
	}

	internal RGBColor approximateSolution(Vector3D P, Vector3D N, Vector3D localPoint, Material material)
	{
		RGBColor totalRadiance = new RGBColor();

		// Direct illumination + Caustics + Multiple Diffuse Reflections
		totalRadiance.add (globalRadianceEstimate (P, N));
		totalRadiance.scale(material.diffuseColor(localPoint));

		return totalRadiance;
	}

	internal virtual RGBColor traceRay(Ray ray, int depth, bool hasBeenDiffuselyReflected)
	{
		Ray /* shadowRay,*/ reflectedRay, transmittedRay;
		Primitive primitive;
		Material material;
		Intersection closestIntersection;
		RGBColor localColor, diffSpecColor, transmittedColor;
		Vector3D P, PL, N, V, R, T, localPoint;
		double cosVN, cosSqr, nMedia;
		
		if (depth == 0)
		{
			return scene.background();
		}
		
		if (depth == photontracer.SceneConstants.MAX_DEPTH)
		{
			System.Threading.Interlocked.Increment (ref noPrimaryRays);
		}
		
		closestIntersection = findClosestIntersection(ray);
		
		if (closestIntersection == null)
		{
			return scene.background();
		}
		
		primitive = closestIntersection.intersectedObject();
		material = primitive.material();
		
		P = closestIntersection.intersectionPoint();
		N = primitive.normal(P, ray.Location);
		V = ray.direction();
		
		PL = P.subNew(primitive.position());
		localPoint = primitive.mapTextureCoordinate(PL);
		
		N = material.perturbNormal(N, PL, primitive);
		//N.normalize();
		
		// ambient light
		localColor = new RGBColor(material.ambientColor(localPoint));
		
		diffSpecColor = new RGBColor();
		transmittedColor = new RGBColor();
		
		// diffuse + direct light
		if (material.absorbationCoef (localPoint) > 0) 
		{
			//Console.Out.WriteLine (closestIntersection.lambda ());

			if (((hasBeenDiffuselyReflected == false) && (depth > (SceneConstants.MAX_DEPTH - 2))) || (closestIntersection.lambda () < SceneConstants.SMALL_DISTANCE)) 
			{
				diffSpecColor.add (accurateSolution(P, N, V, localPoint, depth, material));
			}
			else
			{
				diffSpecColor.add (approximateSolution(P, N, localPoint, material));
			}

			localColor.add(diffSpecColor);

			hasBeenDiffuselyReflected = true;
		}

		cosVN = - V.dot(N);
		
		if (material.isReflective(localPoint) && cosVN >= 0)
		{
			R = V.addNew(N.scaleNew(2.0 * cosVN));
			
			reflectedRay = new Ray(P, R);
			
			System.Threading.Interlocked.Increment (ref noReflectionRays);
			
			//localColor.add(traceRay(reflectedRay, depth - 1, hasBeenDiffuselyReflected).scaleNew(material.reflectionCoef(localPoint)));
			localColor.add(traceRay(reflectedRay, depth - 1, hasBeenDiffuselyReflected).scaleNew(material.specularColor (localPoint)));
		}
		
		if (material.isTransparent(localPoint))
		{
			if (cosVN > 0)
			{
				N.neg();
				
				nMedia = 1.0 / material.IOR(localPoint);
			}
			else
			{
				cosVN = - cosVN;
				
				nMedia = material.IOR(localPoint);
			}
			
			cosSqr = 1.0 - (nMedia * nMedia) * (1.0 - cosVN * cosVN);
			
			if (cosSqr >= 0.0)
			{
				T = V.scaleNew(nMedia);
				T.add(N.scaleNew(System.Math.Sqrt(cosSqr) - nMedia * cosVN));
				
				transmittedRay = new Ray(P, T);
				
				System.Threading.Interlocked.Increment (ref noTransmissionRays);
				
				//transmittedColor = material.transmissionColor(localPoint).scaleNew(1.0f - material.transmissionCoef(localPoint));
				//transmittedColor.add(traceRay(transmittedRay, depth - 1, hasBeenDiffuselyReflected).scaleNew(material.transmissionCoef(localPoint)));
				
				//transmittedColor = material.transmissionColor(localPoint).scaleNew();

				localColor.add(traceRay(transmittedRay, depth - 1, hasBeenDiffuselyReflected).scaleNew (material.transmissionColor(localPoint)));
			}
		}
		
		if (depth == photontracer.SceneConstants.MAX_DEPTH)
		{
			System.Threading.Interlocked.Increment (ref noPrimaryRayHits);
		}
		
		return localColor;
	}
	
	internal virtual bool isInSight(Ray ray, double maxDistance, Primitive LightPrimitive)
	{
		Intersection currentIntersection;
		bool didIntersect;
		
		System.Threading.Interlocked.Increment (ref noShadowRays);
		
		currentIntersection = new Intersection();
		
		for (System.Collections.IEnumerator elem = primitiveList.GetEnumerator(); elem.MoveNext(); )
		{
			Primitive primitive = (Primitive) elem.Current;
			
			if ((LightPrimitive == null) || (primitive != LightPrimitive))
			{
				didIntersect = primitive.intersect(ray, currentIntersection);
			
				if (didIntersect)
				{
					if (currentIntersection.lambda() > maxDistance)
					{
						continue;
					}

					return false;
				}
			}
		}
		
		return true;
	}
	
	internal virtual Intersection findClosestIntersection(Ray ray)
	{
		Intersection closestIntersection, currentIntersection;
		Primitive primitive;
		bool didIntersect = false;
		
		closestIntersection = null;
		currentIntersection = new Intersection();
		
		for (System.Collections.IEnumerator elem = primitiveList.GetEnumerator(); elem.MoveNext(); )
		{
			primitive = (Primitive) elem.Current;
			
			didIntersect = primitive.intersect(ray, currentIntersection);
			
			if (didIntersect)
			{
				if (closestIntersection != null)
				{
					if (currentIntersection.lambda() < closestIntersection.lambda())
					{
						closestIntersection.set(currentIntersection);
					}
				}
				else
				{
					closestIntersection = new Intersection(currentIntersection);
				}
			}
		}
		
		return closestIntersection;
	}
	
	internal virtual void  sampleLights()
	{
		Camera camera;
		double halfWidth, halfHeight;
		double uvCorrection;
		
		RGBColor power = new RGBColor();
		Vector3D pos = new Vector3D();
		Vector3D ori = new Vector3D();
		Ray sampleRay = new Ray();
		
		int oldStored = photonmap.storedPhotons();
		
		camera = scene.camera();
		
		halfWidth = imagePhotons.width() / 2.0;
		halfHeight = imagePhotons.height() / 2.0;
		
		uvCorrection = (halfWidth / halfHeight) * 0.9375;
		
		for (System.Collections.IEnumerator elem = lightList.GetEnumerator(); elem.MoveNext(); )
		{
			Light light = (Light) elem.Current;
			
			Trace.WriteLine("Tracing " + light.photons() + " photons.");
			
			for (int i = 0; i < light.photons(); i++)
			{
				light.sample(pos, ori, power);
				
				sampleRay.Location = pos;
				sampleRay.Direction = ori;
				//sampleRay.normalize();
				
				tracePhoton(sampleRay, power, photontracer.SceneConstants.MAX_BOUNCES, false, false);
				
				//if ((i % 1000) == 0)
				if ((i % 100) == 0)
				{
					for (int t = oldStored; t < photonmap.storedPhotons(); t++)
					//for (int t = oldStored; t < causticsmap.storedPhotons(); t++)
					{
						Photon p = photonmap.getPhoton(t);
						//Photon p = causticsmap.getPhoton(t);
						
						if (p.position != null && p.position.z() > 0)
						{
							double denom = p.position.z() + camera.focalLength();
							
							double u = p.position.x() / denom;
							double v = p.position.y() / denom;
							
							//Vector3D normal = new Vector3D(u, v, camera.focalLength());
							//Vector3D dir    = new Vector3D();
							//p.toCartesian(dir);
							
							Intersection closest = findClosestIntersection(camera.getRay(u, v));
							
							if (closest == null)
							{
								continue;
							}
							
							if (closest.intersectionPoint().distanceSqr(p.position) < photontracer.SceneConstants.EPSILON)
							{
								imagePhotons.setPixel((int) (halfWidth + (2.0 * halfWidth * u / uvCorrection)), (int) (halfHeight - (2.0 * halfHeight * v)), RGBColor.RGBwhite());
								//imagePhotons.setPixel((int) (halfWidth + (2.0 * halfWidth * u / uvCorrection)), (int) (halfHeight - (2.0 * halfHeight * v)), p.power);
							}
						}
					}
					
					oldStored = photonmap.storedPhotons();
					//oldStored = causticsmap.storedPhotons();
					
					
					windowPhotons.Refresh();
					System.Windows.Forms.Application.DoEvents ();
					//windowPhotons.Invalidate ();
				}
			}
			
			causticsmap.scalePhotonPower(1.0f / (float) light.photons());
			photonmap.scalePhotonPower(1.0f / (float) light.photons());
		}
	}

	internal Vector3D calculateLambertianReflection(Vector3D N)
	{
		OrthoNormalBasis basis;
		Vector3D reflectedVector;

		double rphi, rtheta;
		double rand1, rand2;

		reflectedVector = new Vector3D ();

		rand1 = SupportClass.Random.NextDouble();
		rand2 = SupportClass.Random.NextDouble();
			
		// calculate lambertian bounce
		rtheta = System.Math.Acos(System.Math.Sqrt(rand1));
		rphi = 2.0 * System.Math.PI * rand2;

		/*	
		Vector3D H1 = new Vector3D(0.0, 1.0, 0.0);
			
		if (H1.distanceSqr(N) < photontracer.SceneConstants.EPSILON)
		{
			H1.set(0.0, 0.0, 1.0);
		}
			
		Vector3D H2 = N.cross(H1);
		Vector3D H3 = H2.cross(N);
		H2.normalize();
		H3.normalize();
		*/			

		basis = OrthoNormalBasis.makeFromW (N);

		return basis.transform (new Vector3D(rtheta, rphi), reflectedVector);
		//return new Vector3D(H2.x() * temp.x() + H3.x() * temp.y() + N.x() * temp.z(), H2.y() * temp.x() + H3.y() * temp.y() + N.y() * temp.z(), H2.z() * temp.x() + H3.z() * temp.y() + N.z() * temp.z());
	}
	
	internal virtual void  tracePhoton(Ray ray, RGBColor power, int depth, bool wasSpecularlyReflected, bool hasBeenCausticsAbsorbed)
	{
		Ray reflectedRay, transmittedRay;
		Primitive primitive;
		Material material;
		Intersection closestIntersection;
		Vector3D P, PL, N, V, R, T, localPoint;
		RGBColor newPower;
		double cosVN, cosSqr, nMedia;
		float diffuseCoef, reflectCoef, transCoef, total, rand;
		
		if (depth == 0)
		{
			return ;
		}
		
		closestIntersection = findClosestIntersection(ray);
		
		if ((closestIntersection == null))// || (closestIntersection.lambda () < 0.05))
		{
			return ;
		}
		
		if (closestIntersection.lambda () < 0.05)
		{
		//	Trace.WriteLine (closestIntersection.lambda ());
			return;
		}

		primitive = closestIntersection.intersectedObject();
		material = primitive.material();
		
		P = closestIntersection.intersectionPoint();
		N = primitive.normal(P, ray.Location);
		V = ray.direction();
		
		PL = P.subNew(primitive.position());
		localPoint = primitive.mapTextureCoordinate(PL);
		
		N = material.perturbNormal(N, PL, primitive);
		//N.normalize();
		
		diffuseCoef = material.absorbationCoef(localPoint);
		reflectCoef = material.reflectionCoef(localPoint);
		transCoef = material.transmissionCoef(localPoint);
		total = diffuseCoef + reflectCoef + transCoef;
	
		rand = (float) SupportClass.Random.NextDouble() * (1 + transCoef);
		
		cosVN = - V.dot(N);
		
		//if ((diffuseCoef > photontracer.SceneConstants.EPSILON) && (depth < photontracer.SceneConstants.MAXBOUNCES))
		if (diffuseCoef > photontracer.SceneConstants.EPSILON)
		{
			if ((wasSpecularlyReflected == true) && (hasBeenCausticsAbsorbed == false))
			{
				// store photon in caustics map
				//!!!! b�r bare v�re power
				causticsmap.store(power, P, V, N);
				//causticsmap.store(power.scaleNew(material.diffuseColor(localPoint)), P, V, N);

				hasBeenCausticsAbsorbed = true;
			}

			// store photon in global map (non-specular surface)
			//if (power.red () == 13500.0f)
			{
			//	Trace.WriteLine ("store: " + power);
			}
			photonmap.store(power, P, V, N);
		}

		if (rand < diffuseCoef)
		{
			// diffuse reflection
			reflectedRay = new Ray(P, calculateLambertianReflection(N));
			
			//matColor = new RGBColor (material.diffuseColor(localPoint));

			newPower = power.scaleNew(material.diffuseColor(localPoint));
			//newPower.scale(diffuseCoef);
			newPower.scale(1.0f / diffuseCoef);
			//newPower.scale(1.0f / diffuseCoef);
			
			//Trace.WriteLine ("diffuse ref.: " + power + " " + material.diffuseColor(localPoint) + " " + newPower + " " + diffuseCoef + " " + total);

			tracePhoton(reflectedRay, newPower, depth - 1, wasSpecularlyReflected, hasBeenCausticsAbsorbed);
		}
		else if (rand < (diffuseCoef + reflectCoef))
		{
			// specular reflection
			if (cosVN >= 0)
			{
				R = V.addNew(N.scaleNew(2.0 * cosVN));
				
				reflectedRay = new Ray(P, R);
				
				newPower = power.scaleNew(material.specularColor(localPoint));
				newPower.scale(1.0f / reflectCoef);
				
				tracePhoton(reflectedRay, newPower, depth - 1, true, hasBeenCausticsAbsorbed);
			}
		}
		else if (rand < (diffuseCoef + reflectCoef + transCoef))
		{
			// transmission
			if (cosVN > 0)
			{
				N.neg();
				
				nMedia = 1.0 / material.IOR(localPoint);
			}
			else
			{
				cosVN = - cosVN;
				
				nMedia = material.IOR(localPoint);
			}
			cosSqr = 1.0 - (nMedia * nMedia) * (1.0 - cosVN * cosVN);
			
			if (cosSqr >= 0.0)
			{
				T = V.scaleNew(nMedia);
				T.add(N.scaleNew(System.Math.Sqrt(cosSqr) - nMedia * cosVN));
				
				transmittedRay = new Ray(P, T);
				
				newPower = power.scaleNew(material.transmissionColor(localPoint));
				newPower.scale(1.0f / transCoef);
				
				tracePhoton(transmittedRay, newPower, depth - 1, true, hasBeenCausticsAbsorbed);
			}
		}
		else
		{
			// absorption
		}
		
		return ;
	}
	
	[STAThread]
	public static void Main(System.String[] args)
	{
		TextWriterTraceListener tr1 = new TextWriterTraceListener(System.IO.File.CreateText("Output.txt"));
		Trace.Listeners.Add (tr1);

		try
		{
			if (args.Length == 3)
			{
				new Photontracer(System.Int32.Parse(args[0]), System.Int32.Parse(args[1]), args[2]);
			}
			else if (args.Length == 2)
			{
				new Photontracer(System.Int32.Parse(args[0]), System.Int32.Parse(args[1]), "photontraced.jpg");
			}
			else
				new Photontracer();
		}
		catch (System.FormatException ne)
		{
            Trace.WriteLine(ne);
			Trace.WriteLine("Warning: parse error! Using default parameters instead.\n");
			
			new Photontracer();
		}
	}
	
	public override System.String ToString()
	{
		int percentage = (int) (noPrimaryRayHits * 100.0 / noPrimaryRays);
		
		return new System.Text.StringBuilder("[Photontracer] -" + "\n  Primary ray hits      : " + noPrimaryRayHits + "/" + noPrimaryRays + " (~" + percentage + "%)" + "\n  Shadow rays shot      : " + noShadowRays  + "\n  Irradiance rays shot  : " + noIrradianceRays + "\n  Reflection rays shot  : " + noReflectionRays + "\n  Transmission rays shot: " + noTransmissionRays).ToString();
	}
}
