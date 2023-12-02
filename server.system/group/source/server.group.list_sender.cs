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

namespace CGDK.server.group
{
	public class list_sender<TMEMBER> :
		Nlist_sender<TMEMBER>,
		Imessageable,
		Iobject_stateable,
		Iinitializable,
		Istartable
		where TMEMBER : class, net.io.Isender_stream
	{
	// constructor) 
		public list_sender(int _max_member = int.MaxValue) : 
			base(_max_member)
		{
			// - dispatchable
			m_component_message_transmitter			 = new message_transmitter();

			// - state
			m_component_state						 = new Cobject_state();
			m_component_state.target				 = this;
			m_component_state.notify_on_initializing = new delegate_notify_context(_process_notify_initializing);
			m_component_state.notify_on_initialize	 = new delegate_notify_context(_process_notify_initialize);
			m_component_state.notify_on_destroying	 = new delegate_notify(_process_notify_destroying);
			m_component_state.notify_on_destroy		 = new delegate_notify(_process_notify_destroy);
			m_component_state.notify_on_starting	 = new delegate_notify_context(_process_notify_starting);
			m_component_state.notify_on_start		 = new delegate_notify_context(_process_notify_start);
			m_component_state.notify_on_stopping	 = new delegate_notify(_process_notify_stopping);
			m_component_state.notify_on_stop		 = new delegate_notify(_process_notify_stop);
		}

	// publics) 
		public eOBJECT_STATE		object_state
		{
			get
			{
				return m_component_state.object_state;
			}
			set
			{
				m_component_state.object_state=value;
			}
		}
		public message_transmitter	meessage_mediator
		{
			get
			{
				return m_component_message_transmitter;
			}
			set
			{
				m_component_message_transmitter = value;
			}
		}

		public bool					initialize			(context _context)
		{
			return m_component_state.initialize(_context);
		}
		public bool					destroy				()
		{
			return m_component_state.destroy();
		}
		public bool					start				(context _context)
		{
			return m_component_state.start(_context);
		}
		public bool					stop				()
		{
			return m_component_state.stop();
		}
		public bool					attach				(Iobject_stateable _child)
		{
			return m_component_state.attach(_child);
		}
		public int					detach				(Iobject_stateable _child)
		{
			return m_component_state.detach(_child);
		}
		public bool					set_object_state_if	(eOBJECT_STATE _state_compare, eOBJECT_STATE _new_states)
		{
			return m_component_state.set_object_state_if(_state_compare, _new_states);
		}
		public IEnumerator			GetEnumerator		()
		{
			return m_component_state.GetEnumerator();
		}

	// frameworks)
		protected virtual void		on_initializing		(context _context) 
        {
        }
        protected virtual void		on_initialize		(context _context) 
        {
        }
        protected virtual void		on_destroying		() 
        {
		}
        protected virtual void		on_destroy			() 
        {
        }
        protected virtual void		on_starting			(context _context) 
        {
        }
        protected virtual void		on_start			(context _context) 
        {
			enable_enter = false;
        }
        protected virtual void		on_stopping			() 
        {
		}
        protected virtual void		on_stop				() 
        {
			enable_enter = false;
        }
        protected virtual int		on_message			(object _source, sMESSAGE _msg) 
        {
			return 0;
        }

	// implementations)
		protected void				_process_notify_initializing(object _object, context _context)	{ on_initializing(_context);}
		protected void				_process_notify_initialize	(object _object, context _context)	{ on_initialize(_context);}
		protected void				_process_notify_destroying	(object _object) 					{ on_destroying();}
		protected void				_process_notify_destroy		(object _object) 					{ on_destroy();}
																				 
		protected void				_process_notify_starting	(object _object, context _context)	{ on_starting(_context);}
		protected void				_process_notify_start		(object _object, context _context)	{ on_start(_context);}
		protected void				_process_notify_stopping	(object _object)					{ on_stopping();}
		protected void				_process_notify_stop		(object _object)					{ on_stop();}

		public int					process_message				(object _source, sMESSAGE _msg)
		{
			// - transmit message to message mediators
			if (m_component_message_transmitter != null)
			{
				var result = m_component_message_transmitter.transmit_message(_source, _msg);

				if (result != 0)
					return result;
			}

			// - on_message
			return on_message(_source, _msg);
		}

		// - Compoments
		private Cobject_state		m_component_state;
		private message_transmitter	m_component_message_transmitter;
	}
}