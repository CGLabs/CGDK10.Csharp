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
using System.Diagnostics;
using System.Collections.Generic;

// ----------------------------------------------------------------------------
//
// class CGDK.Server.LoggerHub
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK.Server
{
	public class LoggerHub : NLogTargetable
	{
		public bool					RegisterLogTargetable(ILogTargetable _plog_targetable) { return this.ProcessRegisterLogTargetable(_plog_targetable);}
		public bool					UnregisterLogTargetable(ILogTargetable _plog_targetable) { return	this.ProcessUnregisterLogTargetable(_plog_targetable);}

		public override void		ProcessLog(LOG_RECORD _log_record)
		{
			// check)
			Debug.Assert(_log_record != null);

			// check) 
			if(_log_record == null)
				return;

			// 1) Log Type을 얻는다.
			int	Type = ((int)_log_record.Type) & 0x00ff;

			// 2) filtering
			if(Filter != null)
			{
				// - filtering result
				var filter_result = Filter.ProcessFiltering(_log_record);

				// check) 
				if(filter_result == false)
					return;
			}

			// Declare) 
			ILogTargetable[] array_log_targetable;

			// 1) List를 복사
			lock(m_list_log_targetable)
			{
				// - 배열로 복사한다.
				array_log_targetable = new ILogTargetable[this.m_list_log_targetable.Count];

				// - list 내용을 복사한다.
				this.m_list_log_targetable.CopyTo(array_log_targetable);
			}

			// 2) Message를 호출
			foreach(var iter in array_log_targetable)
			{
				iter.Trace(_log_record);
			}

			// statistics)
			++m_count_log_total;
		}
		protected bool				ProcessRegisterLogTargetable(ILogTargetable _plog_targetable)
		{
			lock(this.m_list_log_targetable)
			{
				// 1) 이미 존재하고 있으면 false를 리턴한다.
				if(this.m_list_log_targetable.Exists(x => x== _plog_targetable))
				{
					return	false;
				}

				// 2) 추가한다.
				this.m_list_log_targetable.Add(_plog_targetable);
			}

			// return) 
			return	true;
		}
		protected bool				ProcessUnregisterLogTargetable(ILogTargetable _plog_targetable)
		{
			// 1) 제거한다.
			lock(this.m_list_log_targetable)
			{
				this.m_list_log_targetable.Remove(_plog_targetable);
			}

			// return) 
			return	true;
		}

		protected	List<ILogTargetable>	m_list_log_targetable = new();
	}
}
