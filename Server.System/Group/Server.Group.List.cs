//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Group Template Classes                           *
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
using CGDK.Server.Group;

namespace CGDK.Server.Group
{
	public class List<TMEMBER> :
		NList<TMEMBER>,
		IMessageable,
		IObjectStateable,
		IInitializable,
		IStartable
		where TMEMBER : class
	{
	// constructor) 
		public List(int _max_member = int.MaxValue) : 
			base(_max_member)
		{
			// - dispatchable
			this.m_component_MessageTransmitter = new MessageTransmitter();

			// - State
			this.m_component_state = new ObjectState
			{
				Target = this,
				notifyOnInitializing = new(_ProcessNotifyInitializing),
				notifyOnInitialize = new(_ProcessNotifyInitialize),
				notifyOnDestroying = new(_ProcessNotifyDestroying),
				notifyOnDestroy = new(_ProcessNotifyDestroy),
				notifyOnStarting = new(_ProcessNotifyStarting),
				notifyOnStart = new(_ProcessNotifyStart),
				notifyOnStopping = new(_ProcessNotifyStopping),
				notifyOnStop = new(_ProcessNotifyStop)
			};
		}

	// publics) 
		public eOBJECT_STATE		Now
		{
			get
			{
				return this.m_component_state.Now;
			}
			set
			{
				this.m_component_state.Now = value;
			}
		}
		public MessageTransmitter	MeessageMediator
		{
			get
			{
				return this.m_component_MessageTransmitter;
			}
			set
			{
				this.m_component_MessageTransmitter = value;
			}
		}

		public bool					Initialize (Context _Context)
		{
			return this.m_component_state.Initialize(_Context);
		}
		public bool					Destroy ()
		{
			return this.m_component_state.Destroy();
		}
		public bool					Start (Context _Context)
		{
			return this.m_component_state.Start(_Context);
		}
		public bool					Stop ()
		{
			return this.m_component_state.Stop();
		}
		public bool					Attach (IObjectStateable _child)
		{
			return this.m_component_state.Attach(_child);
		}
		public int					Detach (IObjectStateable _child)
		{
			return this.m_component_state.Detach (_child);
		}
		public bool					SetObjectStateIf (eOBJECT_STATE _state_compare, eOBJECT_STATE _new_states)
		{
			return this.m_component_state.SetObjectStateIf(_state_compare, _new_states);
		}
		public IEnumerator			GetEnumerator ()
		{
			return this.m_component_state.GetEnumerator();
		}

	// frameworks)
		protected virtual void		OnInitializing (Context _Context) 
        {
        }
        protected virtual void		OnInitialize (Context _Context) 
        {
        }
        protected virtual void		OnDestroying () 
        {
		}
        protected virtual void		OnDestroy () 
        {
        }
        protected virtual void		OnStarting (Context _Context) 
        {
        }
        protected virtual void		OnStart (Context _Context) 
        {
			this.EnableEnter = false;
        }
        protected virtual void		OnStopping () 
        {
		}
        protected virtual void		OnStop () 
        {
			this.EnableEnter = false;
        }
        protected virtual int		OnMessage (object _source, sMESSAGE _msg) 
        {
			return 0;
        }

	// implementations)
		protected void				_ProcessNotifyInitializing (object _object, Context _Context) { this.OnInitializing(_Context);}
		protected void				_ProcessNotifyInitialize (object _object, Context _Context) { this.OnInitialize(_Context);}
		protected void				_ProcessNotifyDestroying (object _object) { this.OnDestroying();}
		protected void				_ProcessNotifyDestroy (object _object) { this.OnDestroy();}

		protected void				_ProcessNotifyStarting (object _object, Context _Context) { this.OnStarting(_Context);}
		protected void				_ProcessNotifyStart (object _object, Context _Context) { this.OnStart(_Context);}
		protected void				_ProcessNotifyStopping (object _object) { this.OnStopping();}
		protected void				_ProcessNotifyStop (object _object) { this.OnStop();}

		public int					ProcessMessage (object _source, sMESSAGE _msg)
		{
			// - transmit message to message mediators
			if (this.m_component_MessageTransmitter != null)
			{
				var result = this.m_component_MessageTransmitter.TransmitMessage(_source, _msg);

				if (result != 0)
					return result;
			}

			// - OnMessage
			return this.OnMessage(_source, _msg);
		}

		// - Compoments
		private readonly ObjectState m_component_state;
		private MessageTransmitter	m_component_MessageTransmitter;
	}
}
