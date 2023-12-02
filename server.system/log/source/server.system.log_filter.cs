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
using CGDK;

// ----------------------------------------------------------------------------
//
// class CGDK..server.log_filter
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK.server
{
	public class log_filter : CGDK.Ilog_filter
	{
		public log_filter() 
		{
			foreach (var iter in m_filter_info)
				iter.reset();
		}
	// definitions) 
		public struct FILTER_INFO
		{
			public bool				is_enable;

			public bool				is_limit_level;
			public int				range_level_min;
			public int				range_level_max;

			public bool				is_limit_source;
			public int				range_source_min;
			public int				range_source_max;

			public bool				is_limit_destination;
			public int				range_destination_min;
			public int				range_destination_max;

			public bool				is_limit_location;
			public int				range_location_min;
			public int				range_location_max;
		
			public void reset()
			{
				is_enable = false;

				is_limit_level  = false;
				range_level_min = int.MinValue;
				range_level_max = int.MaxValue;

				is_limit_source  = false;
				range_source_min = int.MinValue;
				range_source_max = int.MaxValue;

				is_limit_destination  = false;
				range_destination_min = int.MinValue;
				range_destination_max = int.MaxValue;

				is_limit_location  = false;
				range_location_min = int.MinValue;
				range_location_max = int.MaxValue;
			}
		};

		public void						set_filter_info				(eLOG_TYPE _log_type, FILTER_INFO _filter_info)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");
					
			m_filter_info[(int)_log_type] = _filter_info;
		}
		public FILTER_INFO				get_filter_info					(eLOG_TYPE _log_type)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");

			return m_filter_info[(int)_log_type];
		}

		public void						enable_log					(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");

			m_filter_info[(int)_log_type].is_enable = _enable;
		}
		public void						disable_log					(eLOG_TYPE _log_type) { enable_log(_log_type, false); }

		public void						enable_level_range			(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");
					
			m_filter_info[(int)_log_type].is_limit_level = _enable;
		}
		public void						disable_level_range			(eLOG_TYPE _log_type) { enable_level_range(_log_type, false); }
		public void						set_level_range				(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");

			m_filter_info[(int)_log_type].range_level_min = _min;
			m_filter_info[(int)_log_type].range_level_max = _max;
		}

		public void						enable_source_range			(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");
					
			m_filter_info[(int)_log_type].is_limit_source = _enable;
		}
		public void						disable_source_range		(eLOG_TYPE _log_type) { enable_source_range(_log_type, false); }
		public void						set_source_range			(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");

			m_filter_info[(int)_log_type].range_source_min = _min;
			m_filter_info[(int)_log_type].range_source_max = _max;
		}

		public void						enable_destination_range	(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");
					
			m_filter_info[(int)_log_type] .is_limit_destination= _enable;
		}
		public void						disable_destination_range	(eLOG_TYPE _log_type)
		{
			enable_destination_range(_log_type, false);
		}
		public void						set_destination_range		(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");

			m_filter_info[(int)_log_type].range_destination_min = _min;
			m_filter_info[(int)_log_type].range_destination_max = _max;
		}

		public void						enable_location_range		(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");
					
			m_filter_info[(int)_log_type].is_limit_location = _enable;
		}
		public void						disable_location_range		(eLOG_TYPE _log_type)
		{
			enable_location_range(_log_type, false);
		}
		public void						set_location_range			(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log type is invalid ");
					
			m_filter_info[(int)_log_type].range_location_min = _min;
			m_filter_info[(int)_log_type].range_location_max = _max;
		}

	// framework) 
		public bool						process_filtering(LOG_RECORD _plog_record)
		{
			// 1) Log type을 얻는다.
			eLOG_TYPE log_type = (eLOG_TYPE)(((int)_plog_record.type) & 0xffff);

			// check) type이 범위안의 값인지 검사한다.
			if(log_type<eLOG_TYPE.INFO || log_type >= eLOG_TYPE.MAX)
				return false;

			FILTER_INFO filter_info = m_filter_info[(int)log_type];

			// 2) 허가된 type인지 검사한다.
			if(filter_info.is_enable == false)
				return false;

			// 3) Level검사를 한다.
			if( filter_info.is_limit_level && (_plog_record.level < filter_info.range_level_min || _plog_record.level > filter_info.range_level_max))
				return false;

			// return) 
			return true;
		}

	// implementation) 
		protected FILTER_INFO[]			m_filter_info = new FILTER_INFO[(int)eLOG_TYPE.MAX];
	}
}