﻿//*****************************************************************************
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
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

//----------------------------------------------------------------------------
//
//  CGDK.ExecutableQueue
//
//
//  여러 개의 ICGExecutable 객체를 Queuing하여 모두 실행한다.
//  CGExecutableList와 달리 한번 실행한 Executable은 Queue에서 모두 제거된다.
//  실행된 이후 Enqueue()된 ICGExecutable을 Queuing되었다가 다음 PostExecute()
//  의 호출 때에 모두 실행된 후 Queue에서 제거된다.
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class ExecutableQueue : IExecutable
	{
	// 1) 실행할 Executable을 Enqueue한다.
		public void				Enqueue(IExecutable _executable)
		{
			this.m_queue_io.Enqueue(_executable);
		}

	// 2) Queuin된 모든 Executable을 실행한다.
		public long				ProcessExecute(ulong _return, ulong _param)
		{
			// 1) 모두 다 Dequeuing해온다.
			var list_executable = this.m_queue_io.TryDequeueAll();

			// 2) 모두를 돌며 ProcessExecute를 호출한다.
			foreach (var iter in list_executable)
			{
				try
				{
					iter.ProcessExecute(_return, _param);
				}
				catch (System.Exception)
				{
				}
			}

			// Return)
			return 0;
		}
		public long				Execute()
		{
			return this.ProcessExecute(0, 0);
		}

		private ConcurrentQueue<IExecutable>	m_queue_io = new ConcurrentQueue<IExecutable>();

	// 3) Default Qeueu
		public static ExecutableQueue DefaultQueue
        {
			get { return m_default_queue; }
		}
		private static ExecutableQueue m_default_queue = new ExecutableQueue();
	}
}