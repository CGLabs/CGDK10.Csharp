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
// class CGDK.server.log_hub
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK.server
{
	public class log_hub : Nlog_targetable
	{
		public bool					register_log_targetable(Ilog_targetable _plog_targetable) { return process_register_log_targetable(_plog_targetable);}
		public bool					unregister_log_targetable(Ilog_targetable _plog_targetable) { return	process_unregister_log_targetable(_plog_targetable);}

		public override void		process_log(LOG_RECORD _log_record)
		{
			// check)
			Debug.Assert(_log_record != null);

			// check) 
			if(_log_record == null)
				return;

			// 1) Log Type을 얻는다.
			int	type = ((int)_log_record.type) & 0x00ff;

			// 2) filtering
			if(filter != null)
			{
				// - filtering result
				var filter_result = filter.process_filtering(_log_record);

				// check) 
				if(filter_result == false)
					return;
			}

			// Declare) 
			Ilog_targetable[] array_log_targetable;

			// 1) List를 복사
			lock(m_list_log_targetable)
			{
				// - 배열로 복사한다.
				array_log_targetable = new Ilog_targetable[m_list_log_targetable.Count];

				// - list 내용을 복사한다.
				m_list_log_targetable.CopyTo(array_log_targetable);
			}

			// 2) Message를 호출
			foreach(var iter in array_log_targetable)
			{
				iter.trace(_log_record);
			}

			// statistics)
			++m_count_log_total;
		}
		protected bool				process_register_log_targetable(Ilog_targetable _plog_targetable)
		{
			lock(m_list_log_targetable)
			{
				// 1) 이미 존재하고 있으면 false를 리턴한다.
				if(m_list_log_targetable.Exists(x => x== _plog_targetable))
				{
					return	false;
				}

				// 2) 추가한다.
				m_list_log_targetable.Add(_plog_targetable);
			}

			// return) 
			return	true;
		}
		protected bool				process_unregister_log_targetable(Ilog_targetable _plog_targetable)
		{
			// 1) 제거한다.
			lock(m_list_log_targetable)
			{
				m_list_log_targetable.Remove(_plog_targetable);
			}

			// return) 
			return	true;
		}

		protected	List<Ilog_targetable>	m_list_log_targetable = new List<Ilog_targetable>();
	}
}
