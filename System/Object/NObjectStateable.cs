//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network Socket Classes                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*  This Program is programmed by Cho sanghyun. sangducks@cgcii.co.kr        *
//*  Best for Game Developement and Optimized for Game Developement.          *
//*                                                                           *
//*                (c) 2008 Cho sanghyun. All right reserved.                 *
//*                          http://www.CGCII.co.kr                           *
//*                                                                           *
//*****************************************************************************

using System.Collections;

//----------------------------------------------------------------------------
//  <<interface>> CGDK.ICGExecutable
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class NObjectStateable : 
		IObjectStateable,
		IInitializable,
		IStartable
	{
	// Constructor) 
		public NObjectStateable()
		{
			this.m_component_state = new ObjectState();
			this.m_component_state.Target = this;
			this.m_component_state.notifyOnInitializing	 = new delegateNotifyContext(_ProcessNotifyInitializing);
			this.m_component_state.notifyOnInitialize = new delegateNotifyContext(_ProcessNotifyInitialize);
			this.m_component_state.notifyOnDestroying = new delegateNotify(_ProcessNotifyDestroying);
			this.m_component_state.notifyOnDestroy = new delegateNotify(_ProcessNotifyDestroy);
			this.m_component_state.notifyOnStarting = new delegateNotifyContext(_ProcessNotifyStarting);
			this.m_component_state.notifyOnStart = new delegateNotifyContext(_ProcessNotifyStart);
			this.m_component_state.notifyOnStopping = new delegateNotify(_ProcessNotifyStopping);
			this.m_component_state.notifyOnStop = new delegateNotify(_ProcessNotifyStop);
		}

	// public) 
		public eOBJECT_STATE			Now
		{
			get { return this.m_component_state.Now;}
			set { this.m_component_state.Now = value;}
		}
		public bool						SetObjectStateIf(eOBJECT_STATE _value, eOBJECT_STATE _compare)
		{
			return this.m_component_state.SetObjectStateIf(_value, _compare);
		}
		public bool Initialize()
		{
			return this.m_component_state.Initialize(new Context());
		}

		public bool						Initialize(Context _context)
		{
			return this.m_component_state.Initialize(_context);
		}
		public bool						Destroy()
		{
			return this.m_component_state.Destroy();
		}
		public bool Start()
		{
			return this.Start(new Context());
		}
		public bool						Start(Context _context)
		{
			return this.m_component_state.Start(_context);
		}
		public bool						Stop()
		{
			return this.m_component_state.Stop();
		}
		public bool						Attach(IObjectStateable _child)
		{
			return this.m_component_state.Attach(_child);
		}
		public int						Detach(IObjectStateable _child)
		{
			return this.m_component_state.Detach(_child);
		}
		public IEnumerator				GetEnumerator()
		{
			return this.m_component_state.GetEnumerator();
		}

	// frameworks)
		protected virtual void			OnInitializing(Context _context) 
        {
        }
        protected virtual void			OnInitialize(Context _context) 
        {
        }
        protected virtual void			OnDestroying() 
        {
		}
        protected virtual void			OnDestroy() 
        {
        }
        protected virtual void			OnStarting(Context _context) 
        {
        }
        protected virtual void			OnStart(Context _context) 
        {
        }
        protected virtual void			OnStopping() 
        {
		}
        protected virtual void			OnStop() 
        {
        }
	
	// implementations) 
		protected virtual void			_ProcessNotifyInitializing(object _object, Context _context)
		{
			this.OnInitializing(_context);
		}
		protected virtual void			_ProcessNotifyInitialize(object _object, Context _context)
		{
			this.OnInitialize(_context);
		}
		protected virtual void			_ProcessNotifyDestroying(object _object)
		{
			this.OnDestroying();
		}
		protected virtual void			_ProcessNotifyDestroy(object _object)
		{
			this.OnDestroy();
		}

		protected virtual void			_ProcessNotifyStarting(object _object, Context _context)
		{
			this.OnStarting(_context);
		}
		protected virtual void			_ProcessNotifyStart(object _object, Context _context)
		{
			this.OnStart(_context);
		}
		protected virtual void			_ProcessNotifyStopping(object _object)
		{
			this.OnStopping();
		}
		protected virtual void			_ProcessNotifyStop(object _object)
		{
			this.OnStop();
		}

		// - Components
		private ObjectState				m_component_state;
	}
}
