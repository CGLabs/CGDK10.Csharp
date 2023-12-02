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
//  CGDK.executable_delegate
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class executable_delegate : Iexecutable
	{
	// constructor)
		public executable_delegate()
		{
		}
		public executable_delegate(common.delegate_execute _d_execute, object _param = null)
		{
			m_d_execute = _d_execute;
			m_param	= _param;
		}

	// public)
		public common.delegate_execute	execute_function
		{
			get
			{
				return m_d_execute;
			}
			set
			{
				m_d_execute=value;
			}
		}

	// implementations) 
		public long 					process_execute(ulong _return, ulong _param)
		{
			// check)
			if(m_d_execute == null)
				return 0;

			// 1) OnExecute함수를 호출한다.
			m_d_execute(this, m_param);

			// return)
			return 0;
		}

		private common.delegate_execute m_d_execute = null;
		private object					m_param = null;
	}
}