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
namespace CGDK.Server
{
	public class LogFilter : CGDK.ILogFilter
	{
		public LogFilter() 
		{
			foreach (var iter in m_filter_info)
				iter.Reset();
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
		
			public void Reset()
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

		public void						SetFilterInfo				(eLOG_TYPE _log_type, FILTER_INFO _filter_info)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");
					
			m_filter_info[(int)_log_type] = _filter_info;
		}
		public FILTER_INFO				SetFilterInfo					(eLOG_TYPE _log_type)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");

			return this.m_filter_info[(int)_log_type];
		}

		public void						EnableLog					(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");

			this.m_filter_info[(int)_log_type].is_enable = _enable;
		}
		public void						DisableLog					(eLOG_TYPE _log_type) { this.EnableLog(_log_type, false); }

		public void						EnableLevelRange			(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");
					
			this.m_filter_info[(int)_log_type].is_limit_level = _enable;
		}
		public void						DisableLevelRange			(eLOG_TYPE _log_type) { this.EnableLevelRange(_log_type, false); }
		public void						SetLevelRange				(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");

			this.m_filter_info[(int)_log_type].range_level_min = _min;
			this.m_filter_info[(int)_log_type].range_level_max = _max;
		}

		public void						EnableSourceRange			(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");
					
			this.m_filter_info[(int)_log_type].is_limit_source = _enable;
		}
		public void						DisableSourceRange			(eLOG_TYPE _log_type) { this.EnableSourceRange(_log_type, false); }
		public void						SetSourceRange				(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");

			this.m_filter_info[(int)_log_type].range_source_min = _min;
			this.m_filter_info[(int)_log_type].range_source_max = _max;
		}

		public void						EnableDestinationRange		(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");
					
			this.m_filter_info[(int)_log_type] .is_limit_destination= _enable;
		}
		public void						DisableDestinationRange		(eLOG_TYPE _log_type)
		{
			this.EnableDestinationRange(_log_type, false);
		}
		public void						SetDestinationRange			(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");

			this.m_filter_info[(int)_log_type].range_destination_min = _min;
			this.m_filter_info[(int)_log_type].range_destination_max = _max;
		}

		public void						EnableLocationRange			(eLOG_TYPE _log_type, bool _enable = true)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");
					
			this.m_filter_info[(int)_log_type].is_limit_location = _enable;
		}
		public void						DisableLocationRange		(eLOG_TYPE _log_type)
		{
			this.EnableLocationRange(_log_type, false);
		}
		public void						SetLocationRange			(eLOG_TYPE _log_type, int _min, int _max)
		{
			// check) 
			if(_log_type < eLOG_TYPE.INFO || _log_type >= eLOG_TYPE.MAX)
				throw new System.Exception("log Type is invalid ");
					
			this.m_filter_info[(int)_log_type].range_location_min = _min;
			this.m_filter_info[(int)_log_type].range_location_max = _max;
		}

	// framework) 
		public bool						ProcessFiltering(LOG_RECORD _plog_record)
		{
			// 1) Log Type을 얻는다.
			eLOG_TYPE log_type = (eLOG_TYPE)(((int)_plog_record.Type) & 0xffff);

			// check) Type이 범위안의 값인지 검사한다.
			if(log_type<eLOG_TYPE.INFO || log_type >= eLOG_TYPE.MAX)
				return false;

			FILTER_INFO filter_info = this.m_filter_info[(int)log_type];

			// 2) 허가된 Type인지 검사한다.
			if(filter_info.is_enable == false)
				return false;

			// 3) Level검사를 한다.
			if( filter_info.is_limit_level && (_plog_record.Level < filter_info.range_level_min || _plog_record.Level > filter_info.range_level_max))
				return false;

			// return) 
			return true;
		}

	// implementation) 
		protected FILTER_INFO[]			m_filter_info = new FILTER_INFO[(int)eLOG_TYPE.MAX];
	}
}