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
using System.Collections.Generic;
using System.Linq;
using System.Text;

//----------------------------------------------------------------------------
//
//  CGDK.ExecutableDelegate
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class ExecutableDelegate : IExecutable
	{
	// constructor)
		public ExecutableDelegate()
		{
		}
		public ExecutableDelegate(Common.DelegateExecute _d_execute, object _param = null)
		{
			this.m_d_execute = _d_execute;
			this.m_param = _param;
		}

	// public)
		public Common.DelegateExecute	execute_function
		{
			get => this.m_d_execute;
			set => this.m_d_execute = value;
		}

	// implementations) 
		public long 					ProcessExecute(ulong _return, ulong _param)
		{
			// check)
			if(this.m_d_execute == null)
				return 0;

			// 1) OnExecute함수를 호출한다.
			this.m_d_execute(this, this.m_param);

			// return)
			return 0;
		}

		private Common.DelegateExecute m_d_execute = null;
		private object					m_param = null;
	}
}