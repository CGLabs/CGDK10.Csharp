//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network Socket Classes                           *
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

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Io.Statistics.CONNECTIVE
//	class CGDK.Net.Io.Statistics.Nconnective
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Io.Statistics
{
	// 1) Connective Unit
	public struct CONNECTIVE
	{
		public long		tick_begin;
		public long		tick_end;

		public long		count_try;					// 접속시도 회수.
		public long		count_success_connect;		// 접속성공 회수.
		public long		count_fail_connect;			// 접속성공 회수.
		public long		count_disconnect;			// 접속종료 회수.
		public long		count_error_disconnect;		// 오류로 인한 접속 종료 회수.
		public long		count_keep;					// 현 접속수.

		public long		count_async;		    	// Overlapped I/O횟수

		public void		Reset()
		{
			tick_begin = 0;
			tick_end = 0;

			count_try = 0;
			count_success_connect = 0;
			count_fail_connect	 = 0;
			count_disconnect = 0;
			count_error_disconnect = 0;
			count_keep = 0;
		
			count_async = 0;
		}
	};

	// 2) Connective Info
	public struct CONNECTIVE_INFO
	{
		public CONNECTIVE_INFO(int _size)
		{
			m_period = new CONNECTIVE[_size];
			m_total = new CONNECTIVE();

			Reset();
		}

		public	void		Reset()
		{
			for(int i=0; i<m_period.Length; ++i)
			{
				m_period[i].Reset();
			}

			m_total.Reset();
		}

		public	long		try_total						{ get { return m_total.count_try+m_period[statisticsTime].count_try;}}
		public	long		try_now							{ get {return m_period[statisticsTime].count_try;}}
		public	long		try_(long _time)				{ return m_period[_time].count_try;}
													  
		public	long		success_connect_total			{ get {return m_total.count_success_connect+m_period[statisticsTime].count_success_connect;}}
		public	long		success_connect_now				{ get {return m_period[statisticsTime].count_success_connect;}}
		public	long		success_connect(long _time)		{ return m_period[_time].count_success_connect; }
													  
		public	long		fail_connect_total				{ get {return m_total.count_fail_connect+m_period[statisticsTime].count_fail_connect;}}
		public	long		fail_connect_now				{ get {return m_period[statisticsTime].count_fail_connect;}}
		public	long		fail_connect(long _time)		{ return m_period[_time].count_fail_connect; }
													  
		public	long		disconnect_total				{ get {return m_total.count_disconnect+m_period[statisticsTime].count_disconnect;}}
		public	long		disconnect_now					{ get {return m_period[statisticsTime].count_disconnect;}}
		public	long		Disconnect(long _time)			{ return m_period[_time].count_disconnect; }
													  
		public	long		error_disconnect_total			{ get {return m_total.count_error_disconnect+m_period[statisticsTime].count_error_disconnect;}}
		public	long		error_disconnect_now			{ get {return m_period[statisticsTime].count_error_disconnect;}}
		public	long		error_disconnect(long _time)    { return m_period[_time].count_error_disconnect; }
													  
		public	long		keep_now						{ get {return m_period[statisticsTime].count_keep;}}
		public	long		keep(long _time)				{ return m_period[_time].count_keep; }
														  
		public	void		statistics_on_try()				{ Interlocked.Increment(ref m_period[statisticsTime].count_try);}
		public	void		statistics_on_success_connect()	{ Interlocked.Increment(ref m_period[statisticsTime].count_success_connect); Interlocked.Increment(ref m_period[statisticsTime].count_keep);}
		public	void		StatisticsOnFailConnect()	{ Interlocked.Increment(ref m_period[statisticsTime].count_fail_connect);}
		public	void		StatisticsOnDisconnect()		{ Interlocked.Increment(ref m_period[statisticsTime].count_disconnect); Interlocked.Decrement(ref m_period[statisticsTime].count_keep);}
		public	void		StatisticsOnErrorDisconnect(){ Interlocked.Increment(ref m_period[statisticsTime].count_error_disconnect);}
		public	void		StatisticsOnErrorDisconnect(long value) { Interlocked.Add(ref m_period[statisticsTime].count_error_disconnect, value); }
		public	void		statistics_on_keep(long _keep)	{ m_period[statisticsTime].count_keep = _keep; }
														  
		public	CONNECTIVE	get_connective_info()			{ return m_period[statisticsTime];}
		public	CONNECTIVE	get_connective_info(int _index)	{ Debug.Assert(_index<m_period.Length); return m_period[_index];}

		private	CONNECTIVE[] m_period;
		private	CONNECTIVE	m_total;

		public static int	statisticsTime = 0;
	}

	public class Nconnective
	{
		public void					reset_statistics()
		{
			// 1) Traffic Info를 Reset한다.
			m_statistics_connection.Reset();

			// 2) Reset
			//m_timeCreate;
			//m_timestart;
			//m_timeStop;
			m_count_async	 = 0;

			// 3) Connective ID
			m_iConnectiveID	 = 0;
		}

		public	int					connective_id					{ get { return m_iConnectiveID; } }
																			  
		public	CONNECTIVE_INFO		connection_info					{ get {return m_statistics_connection;} }
																			  
		public	long				statistics_try					{ get { return m_statistics_connection.try_total; } }
		public	long				statistics_success_connect		{ get { return m_statistics_connection.success_connect_total;} }
		public	long				statistics_fail_connect			{ get { return m_statistics_connection.fail_connect_total;} }
		public	long				statistics_disconnect			{ get { return m_statistics_connection.disconnect_total;} }
		public	long				statistics_error_disconnect		{ get { return m_statistics_connection.error_disconnect_total;} }
		public	long				statistics_keep					{ get { return m_statistics_connection.keep_now; } }
		public	long				statistics_now_async_io			{ get { return m_count_async;} }
																			  
		public	DateTime			statistics_create_time			{ get { return m_time_create;}}
		public	DateTime			statistics_start_time			{ get {return m_time_start;}}
		public	DateTime			statistics_stop_time			{ get {return m_time_stop;}}
																			  
		public	void				statistics_on_try()				{ m_statistics_connection.statistics_on_try(); m_Statistics_connection_total.statistics_on_try();}
		public	void				statistics_on_success_connect() { m_statistics_connection.statistics_on_success_connect();}
		public	void				StatisticsOnFailConnect()	{ m_statistics_connection.StatisticsOnFailConnect(); m_Statistics_connection_total.StatisticsOnFailConnect();}
		public	void				StatisticsOnDisconnect()		{ m_statistics_connection.StatisticsOnDisconnect();}
		public	void				StatisticsOnErrorDisconnect(long _value) { m_statistics_connection.StatisticsOnErrorDisconnect(_value);}
																			  
		public	long				statistics_on_increse_async()   { return Interlocked.Increment(ref m_count_async);}
		public	long				StatisticsOnDecreseAsync()   { return Interlocked.Decrement(ref m_count_async);}

		public	void				set_create_time()				{ m_time_create=DateTime.Now;}
		public	void				set_start_time()				{ m_time_start=DateTime.Now;}
		public	void				set_stop_time()					{ m_time_stop=DateTime.Now;}
		public	void				Reset()							{ m_statistics_connection.Reset();}

		public static CONNECTIVE_INFO total							{ get { return m_Statistics_connection_total; } }

		//public const int	TOTAL_INFO	 = 1440*7;
		public const int	TOTAL_INFO	 = 32;

		// 1) 전송량 Statistics
		public	CONNECTIVE_INFO			m_statistics_connection = new CONNECTIVE_INFO(TOTAL_INFO);
		public static CONNECTIVE_INFO	m_Statistics_connection_total = new CONNECTIVE_INFO(TOTAL_INFO);

		// 2) Connective ID
				int					m_iConnectiveID;

		// 3) 시간
		public	DateTime			m_time_create;
		public	DateTime			m_time_start;
		public	DateTime			m_time_stop;

		public	long				m_count_async;
	}
}
