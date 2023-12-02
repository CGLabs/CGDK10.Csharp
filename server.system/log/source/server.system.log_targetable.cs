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
// class CGDK.server.log_file
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK.server
{
	public abstract class Nlog_targetable : 
		Nobject_stateable,
		Ilog_targetable
	{
	// contructor) 
		public Nlog_targetable()
		{
			// 5) 
			m_count_log_total = 0;
			m_count_log = new int[(int)eLOG_TYPE.MAX+1];
			for(int i=0;i<m_count_log.Length; ++i)
			{
				m_count_log[i]	 = 0;
			}
		}

	// public) 
		public void						trace(LOG_RECORD _log_record) { process_log(_log_record); }

		public Ilog_filter				filter { get { return m_filter_log; } set { m_filter_log = value; } }
		public ulong					log_count { get { return m_count_log_total; } set { m_count_log_total = value; } }

		public abstract void			process_log(LOG_RECORD _log_record);

	// implementation) 
		protected ulong					m_count_log_total = 0;
		protected int[]					m_count_log;
		protected object				m_cs_count_log = new object();
		private	Ilog_filter				m_filter_log = null;
	}
}