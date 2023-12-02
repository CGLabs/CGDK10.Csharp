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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CGDK;

//----------------------------------------------------------------------------
//
//  class CGDK.server.service
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.server
{
	public class service :	
		Nservice
	{
	// constructor) 
		public service()
		{
			m_component_state.notify_on_initializing = _process_notify_initializing;
			m_component_state.notify_on_initialize	 = _process_notify_initialize;
			m_component_state.notify_on_destroying	 = _process_notify_destroying;
			m_component_state.notify_on_destroy		 = _process_notify_destroy;

			m_component_state.notify_on_starting	 = _process_notify_starting;
			m_component_state.notify_on_start		 = _process_notify_start;
			m_component_state.notify_on_stopping	 = _process_notify_stopping;
			m_component_state.notify_on_stop		 = _process_notify_stop;
		}
		public service(string _name) : this()
		{
			name = _name;
		}

	// frameworks) 
		protected virtual void			on_service_setting(context _context) {}
		protected virtual void			on_initializing(context _context) {}
		protected virtual void			on_initialize(context _context) {}
		protected virtual void			on_starting(context _context) {}
		protected virtual void			on_start(context _contextS) {}
		protected virtual void			on_stopping() {}
		protected virtual void			on_stop() {}
		protected virtual void			on_destroying() {}
		protected virtual void			on_destroy() {}

	// implementations) 
		protected void					_process_notify_initializing(object _object, context _context)
		{
			on_service_setting(_context);
			on_initializing(_context);
		}
		protected void					_process_notify_initialize(object _object, context _context)
		{
			on_initialize(_context);
		}
		protected void					_process_notify_starting(object _object, context _context)
		{
			on_service_setting(_context);
			on_starting(_context);
		}
		protected void					_process_notify_start(object _object, context _context)
		{
			on_start(_context);
		}
		protected void					_process_notify_stopping(object _object)
		{
			on_stopping();
		}
		protected void					_process_notify_stop(object _object)
		{
			on_stop();
		}
		protected void					_process_notify_destroying(object _object)
		{
			on_destroying();
		}
		protected void					_process_notify_destroy(object _object)
		{
			on_destroy();
		}
	}
}