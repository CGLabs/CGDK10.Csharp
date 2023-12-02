//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                               Pool Classes                                *
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
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;


//----------------------------------------------------------------------------
//
//  CGDK.executor_list
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class executor_list : Nobject_stateable, Iexecutor, Inameable
	{
	// Constructor)
		public executor_list()
		{
		}
		public executor_list(string _name)
		{
			m_name = _name;	
		}

	// public) 
		public virtual bool				post(Iexecutable _pexecutable, ulong _para = 0)
		{
			return	process_post_execute(_pexecutable, _para);
		}
		public bool						post(common.delegate_execute _d_execute, object _param = null)
		{
			return post(new executable_delegate(_d_execute, _param));
		}
		protected void f_process_execute(object _object)
		{
			// 1) get executable
			QUEUE_EXECUTABLE pexecutable = _object as QUEUE_EXECUTABLE;

			// check) 
			if(pexecutable == null)
				return;

			// 2) process executable
			pexecutable.executable.process_execute(0, pexecutable.param);
		}

		public virtual bool				process_post_execute(Iexecutable _pexecutable, ulong _para = 0)
		{
			return ThreadPool.QueueUserWorkItem(f_process_execute, new QUEUE_EXECUTABLE(_pexecutable, _para));
		}

		public string					name
		{
			get { return m_name; }
			set { m_name=value; }
		}

	// implementations) 
		protected override void			_process_notify_starting(object _object, context _context)
		{
			base._process_notify_starting(_object, _context);
		}
		protected override void			_process_notify_start(object _object, context _context)
		{
			// 1) 
			base._process_notify_starting(_object, _context);

			// 2) ...
		}
		protected override void			_process_notify_stopping(object _object)
		{
			base._process_notify_stopping(_object);
		}
		protected override void			_process_notify_stop(object _object)
		{
			base._process_notify_stop(_object);
		}

		private ConcurrentBlcokedQueue<QUEUE_EXECUTABLE> m_queue_executable	 = new ConcurrentBlcokedQueue<QUEUE_EXECUTABLE>();

	// Declare)
		private class QUEUE_EXECUTABLE
		{
			public  QUEUE_EXECUTABLE(Iexecutable _pexecutable, ulong _param)
			{
				executable = _pexecutable;
				param = _param;
			}

			public Iexecutable	executable;
			public ulong		param;
		}

		private string					m_name;
	}
}
