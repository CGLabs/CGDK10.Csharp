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
//  CGDK.ExecutorList
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class ExecutorList : NObjectStateable, IExecutor, INameable
	{
	// Constructor)
		public ExecutorList()
		{
		}
		public ExecutorList(string _name)
		{
			this.m_name = _name;	
		}

	// public) 
		public virtual bool				Post(IExecutable _pexecutable, ulong _para = 0)
		{
			return this.ProcessPostExecute(_pexecutable, _para);
		}
		public bool						Post(Common.DelegateExecute _d_execute, object _param = null)
		{
			return this.Post(new ExecutableDelegate(_d_execute, _param));
		}
		private static void				FunctionProcessExecute(object _object)
		{
			// check) 
			if (_object is not QUEUE_EXECUTABLE pexecutable)
				return;

			// 1) process executable
			pexecutable.executable.ProcessExecute(0, pexecutable.param);
		}
		public virtual bool				ProcessPostExecute(IExecutable _pexecutable, ulong _para = 0)
		{
			return ThreadPool.QueueUserWorkItem(FunctionProcessExecute, new QUEUE_EXECUTABLE(_pexecutable, _para));
		}

		public string					Name
		{
			get { return this.m_name; }
			set { this.m_name=value; }
		}

	// implementations) 
		protected override void			_ProcessNotifyStarting(object _object, Context _context)
		{
			base._ProcessNotifyStarting(_object, _context);
		}
		protected override void			_ProcessNotifyStart(object _object, Context _context)
		{
			// 1) 
			base._ProcessNotifyStarting(_object, _context);

			// 2) ...
		}
		protected override void			_ProcessNotifyStopping(object _object)
		{
			base._ProcessNotifyStopping(_object);
		}
		protected override void			_ProcessNotifyStop(object _object)
		{
			base._ProcessNotifyStop(_object);
		}

		private ConcurrentBlcokedQueue<QUEUE_EXECUTABLE> m_queue_executable = new();

	// Declare)
		private class QUEUE_EXECUTABLE
		{
			public  QUEUE_EXECUTABLE(IExecutable _pexecutable, ulong _param)
			{
				this.executable = _pexecutable;
				this.param = _param;
			}

			public IExecutable	executable;
			public ulong		param;
		}

		private string					m_name;
	}
}
