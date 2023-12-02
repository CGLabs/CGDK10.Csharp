//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                           Server Event Classes                            *
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
using System.Threading;
using System.Diagnostics;
using CGDK;

// ----------------------------------------------------------------------------
//
// class CGDK.Server.log_file
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK.Server
{
	public abstract class NLogTargetable : 
		NObjectStateable,
		ILogTargetable
	{
	// contructor) 
		public NLogTargetable()
		{
			// 5) 
			m_count_log_total = 0;
			m_count_log = new int[(int)eLOG_TYPE.MAX+1];
			for(int i=0;i<m_count_log.Length; ++i)
			{
				this.m_count_log[i]	 = 0;
			}
		}

	// public) 
		public void						Trace(LOG_RECORD _log_record) { this.ProcessLog(_log_record); }

		public ILogFilter				Filter { get { return this.m_filter_log; } set { this.m_filter_log = value; } }
		public ulong					LogCount { get { return this.m_count_log_total; } set { this.m_count_log_total = value; } }

		public abstract void			ProcessLog(LOG_RECORD _log_record);

	// implementation) 
		protected ulong					m_count_log_total = 0;
		protected int[]					m_count_log;
		protected object				m_cs_count_log = new();
		private	ILogFilter				m_filter_log = null;
	}
}