using System;
using Vector3D = photontracer.math.Vector3D;

/**
 * 3D axis-aligned bounding box. Stores only the minimum and maximum corner points.
 */
namespace photontracer
{
	public class BoundingBox 
	{
		private Vector3D minimum;
		private Vector3D maximum;
		private Vector3D center;
		private Vector3D extents;

		/**
		* Creates an empty box. The minimum point will have all components set to positive infinity, and the maximum will
		* have all components set to negative infinity.
		*/
		public BoundingBox() 
		{
			minimum = new Vector3D(Double.PositiveInfinity, Double.PositiveInfinity, Double.PositiveInfinity);
			maximum = new Vector3D(Double.NegativeInfinity, Double.NegativeInfinity, Double.NegativeInfinity);
			center = new Vector3D();
			extents = new Vector3D();
		}

		/**
		* Gets the minimum corner of the box. That is the corner of smallest coordinates on each axis. Note that the
		* returned reference is not cloned for efficiency purposes so care must be taken not to change the coordinates of
		* the point.
		*
		* @return a reference to the minimum corner
		*/
		public Vector3D getMinimum() 
		{
			return minimum;
		}

		/**
		* Gets the maximum corner of the box. That is the corner of largest coordinates on each axis. Note that the
		* returned reference is not cloned for efficiency purposes so care must be taken not to change the coordinates of
		* the point.
		*
		* @return a reference to the maximum corner
		*/
		public Vector3D getMaximum() 
		{
			return maximum;
		}

		/**
		* Gets the center of the box, computed as (min + max) / 2.
		*
		* @return a reference to the center of the box
		*/
		public Vector3D getCenter() 
		{
			center.set_Renamed (minimum);
			center.add (maximum);
			center.scale (1.0 / 2.0);

			return center;
		}

		/**
		* Gets the extents vector for the box. This vector is computed as (max - min). Its coordinates are always positive
		* and represent the dimensions of the box along the three axes.
		*
		* @return a refreence to the extent vector
		* @see org.sunflow.math.Vector3#length()
		*/
		public Vector3D getExtents() 
		{
			extents.set_Renamed (maximum);
			extents.sub (minimum);

			return extents;
		}

		/**
		* Scales the box up or down in all directions by a the given percentage. The routine only uses the magnitude of
		* the scale factor to avoid inversion of the minimum and maximum.
		*
		* @param s scale factor
		*/
		public void scale(double s) 
		{
			//Vector3D.mid(minimum, maximum, center);
			//Vector3D.sub(maximum, minimum, extents);
			getCenter ();
			getExtents ();
			
			s *= ((s > 0.0) ? 0.5 : (-0.5));

			minimum.X = center.x() - (extents.x() * s);
			minimum.Y = center.y() - (extents.y() * s);
			minimum.Z = center.z() - (extents.z() * s);
			maximum.X = center.x() + (extents.x() * s);
			maximum.Y = center.y() + (extents.y() * s);
			maximum.Z = center.z() + (extents.z() * s);
		}

		/**
		* Returns <code>true</code> when the box has just been initialized, and is still empty. This method might also
		* return true if the state of the box becomes inconsistent and some component of the minimum corner is larger than
		* the corresponding coordinate of the maximum corner.
		*
		* @return <code>true</code> if the box is empty, <code>false</code> otherwise
		*/
		public bool isEmpty() 
		{
			return (maximum.x() < minimum.x()) || (maximum.y() < minimum.y()) || (maximum.z() < minimum.z());
		}

		/**
		* Returns <code>true</code> if the specified bounding box intersects this one. The boxes are treated as volumes,
		* so a box inside another will return true. Returns <code>false</code> if the parameter is <code>null</code>.
		*
		* @param b box to be tested for intersection
		* @return <code>true</code> if the boxes overlap, <code>false</code> otherwise
		*/
		public bool intersects(BoundingBox b) 
		{
			return ((b != null) && (minimum.x() <= b.maximum.x()) && (maximum.x() >= b.minimum.x()) && (minimum.y() <= b.maximum.y()) && (maximum.y() >= b.minimum.y()) && (minimum.z() <= b.maximum.z()) && (maximum.z() >= b.minimum.z()));
		}

		/**
		* Checks to see if the specified {@link org.sunflow.math.Vector3D point} is inside the volume defined by this box. Returns <code>false</code> if the
		* parameter is <code>null</code>.
		*
		* @param p point to be tested for containment
		* @return <code>true</code> if the point is inside the box, <code>false</code> otherwise
		*/
		public bool contains(Vector3D p) 
		{
			return ((p != null) && (p.x() >= minimum.x()) && (p.x() <= maximum.x()) && (p.y() >= minimum.y()) && (p.y() <= maximum.y()) && (p.z() >= minimum.z()) && (p.z() <= maximum.z()));
		}

		/**
		* Changes the extents of the box as needed to include the given {@link org.sunflow.math.Vector3D point} into this box.
		* Does nothing if the parameter is <code>null</code>.
		*
		* @param p point to be included
		*/
		public void include(Vector3D p) 
		{
			if (p != null) 
			{
				if (p.x() < minimum.x())
					minimum.X = p.x();
				if (p.x() > maximum.x())
					maximum.X = p.x();
				if (p.y() < minimum.y())
					minimum.Y = p.y();
				if (p.y() > maximum.y())
					maximum.Y = p.y();
				if (p.z() < minimum.z())
					minimum.Z = p.z();
				if (p.z() > maximum.z())
					maximum.Z = p.z();
			}
		}

		/**
		* Changes the extents of the box as needed to include the given box into this box.
		* Does nothing if the parameter is <code>null</code>.
		*
		* @param b box to be included
		*/
		public void include(BoundingBox b) 
		{
			if (b != null) 
			{
				if (b.minimum.x() < minimum.x())
					minimum.X = b.minimum.x();
				if (b.maximum.x() > maximum.x())
					maximum.X = b.maximum.x();
				if (b.minimum.y() < minimum.y())
					minimum.Y = b.minimum.y();
				if (b.maximum.y() > maximum.y())
					maximum.Y = b.maximum.y();
				if (b.minimum.z() < minimum.z())
					minimum.Z = b.minimum.z();
				if (b.maximum.z() > maximum.z())
					maximum.Z = b.maximum.z();
			}
		}

		public void recalcMinMax()
		{
			Vector3D halfExtents = extents.scaleNew (1.0 / 2.0);

			minimum.set_Renamed (center);
			minimum.sub (halfExtents);

			maximum.set_Renamed (minimum);
			maximum.add (extents);
		}
	}
}