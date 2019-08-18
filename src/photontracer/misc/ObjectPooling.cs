using System;
using System.Collections;
using System.Reflection;
using System.Threading;

namespace photontracer.misc
{
	public class ObjectPool
	{
		private Type	m_oType;
		private int		m_iTimeout;
		private Stack	m_oPool;

		public ObjectPool (System.Type Type)
		{
			m_oType    = Type;
			m_iTimeout = 60000;
			m_oPool    = new Stack ();
		}

		public ObjectPool (System.Type Type, int Timeout)
		{
			m_oType    = Type;
			m_iTimeout = Timeout;
			m_oPool    = new Stack ();
		} 

		public Type Type
		{
			get 
			{ 
				return m_oType; 
			}
		}

		public ObjectContext GetObjectContext ()
		{
			return GetObjectContext (null);
		}
	    
		public ObjectContext GetObjectContext (object[] Args)
		{
			ObjectContext	oObject = null;

			while (m_oPool.Count != 0)
			{
				oObject = (ObjectContext) m_oPool.Pop ();

				if (!(oObject.TimedOut))
				{
					oObject.StopIdleTimer ();
					break;
				}
				else
				{
					oObject = null;
				}
			}

			if (oObject == null)
			{
				oObject		   = new ObjectContext (m_oPool, m_iTimeout);
				oObject.Object = Activator.CreateInstance (m_oType, Args);
			}

			return oObject;
		}

		public void Dispose ()
		{
			while (m_oPool.Count != 0)
			{
				ObjectContext oItem = (ObjectContext) m_oPool.Pop ();
				oItem.Dispose ();
			}
		}
	}
  
	public class ObjectContext
	{
		private int		m_iTimeout;
		private object	m_oObject;
		private Stack	m_oStack;
		private bool	m_bTimedOut;
		private Timer	m_oIdleTimer;
		  
		public ObjectContext (Stack oStack, int Timeout)
		{
			m_iTimeout   = Timeout;
			m_oObject    = null;
			m_oStack     = oStack;
			m_bTimedOut  = false;
			m_oIdleTimer = null;
		}

		public Object Object
		{
			set 
			{ 
				m_oObject = value; 
			}
		}

		public bool TimedOut
		{
			get 
			{ 
				return m_bTimedOut; 
			}
		}

		private void IdleTimeoutCallback (object State)
		{
			StopIdleTimer (); 
			m_bTimedOut = true;
		}

		internal void StopIdleTimer ()
		{
			m_oIdleTimer.Dispose ();
		}

		public void Dispose ()
		{
			StopIdleTimer ();

			m_bTimedOut = true;
			m_oStack    = null;
			m_oObject   = null;
		}
	    
		public void Pool ()
		{
			m_oIdleTimer = new Timer (new TimerCallback (IdleTimeoutCallback), null, m_iTimeout, 0);
			m_oStack.Push (this);
		}
	}
}