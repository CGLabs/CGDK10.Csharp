//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network socket Classes                           *
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
//  class CGDK.net.io.statistics.TRAFFIC
//	class CGDK.net.io.statistics.Ntraffic
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.io.statistics
{
	// 1) Traffice Unit
	public struct TRAFFIC
	{
		public long		tick_begin;
		public long		tick_end;

		public long		count_received_message;		// 받은 Message 수.
		public long		count_received_bytes;		// 받은 Bytes

		public long		count_sended_message;		// 보낸 Message 수.
		public long		count_sended_bytes;			// 보낸 Bytes

		public long		count_send_io;				// 송신 I/O횟수
		public long		count_receive_io;			// 수신 I/O횟수

		public long		count_error_on_send;		// Send시 Error
		public long		count_error_on_receive;		// Receive시 Error

		public void		reset()
		{
			tick_begin = 0;
			tick_end = 0;

			count_received_message = 0;
			count_received_bytes = 0;

			count_sended_message = 0;
			count_sended_bytes = 0;

			count_send_io = 0;
			count_receive_io = 0;

			count_error_on_send = 0;
			count_error_on_receive = 0;
		}
	};

	// 2) Traffice
	public struct TRAFFIC_INFO
	{
		public TRAFFIC_INFO(int iSize)
		{
			m_period = new TRAFFIC[iSize];
			m_total	 = new TRAFFIC();

			reset();
		}

		public	void		reset()							
		{ 
			for(int i=0; i<m_period.Length; ++i)
			{
				m_period[i].reset();
			}

			m_total.reset();
		}

		public	long		sended_message_total							{ get { return m_total.count_sended_message + m_period[statistics_time].count_sended_message;} }
		public	long		sended_message_now								{ get { return m_period[statistics_time].count_sended_message;} }
		public	long		sended_message(int _time)						{ return m_period[_time].count_sended_message; }
						
		public	long		sended_bytes_total								{ get { return m_total.count_sended_bytes + m_period[statistics_time].count_sended_bytes;} }
		public	long		sended_bytes_now								{ get { return m_period[statistics_time].count_sended_bytes;} }
		public	long		sended_bytes(int _time)							{ return m_period[_time].count_sended_bytes; }
						
		public	long		received_message_total							{ get { return m_total.count_received_message + m_period[statistics_time].count_received_message;} }
		public	long		received_message_now							{ get { return m_period[statistics_time].count_received_message;}}
		public	long		received_message(int _time)						{ return m_period[_time].count_received_message; }

		public	long		received_bytes_total							{ get { return m_total.count_received_bytes + m_period[statistics_time].count_received_bytes;} }
		public	long		received_bytes_now								{ get { return m_period[statistics_time].count_received_bytes;} }
		public	long		received_bytes(int _time)						{ return m_period[_time].count_received_bytes; }

		public	long		error_on_send_total								{ get { return m_total.count_error_on_send + m_period[statistics_time].count_error_on_send;}}
		public	long		error_on_send_now								{ get { return m_period[statistics_time].count_error_on_send;}}
		public	long		error_on_send(int _time)						{ return m_period[_time].count_error_on_send; }

		public	long		error_on_reveive_total							{ get { return m_total.count_error_on_receive + m_period[statistics_time].count_error_on_receive;}}
		public	long		error_on_reveive_now							{ get { return m_period[statistics_time].count_error_on_receive;}}
		public	long		error_on_reveive(int _time)						{ return m_period[_time].count_error_on_receive; }

		public	long		async_send_total								{ get { return m_total.count_send_io + m_period[statistics_time].count_send_io;}}
		public	long		async_send_now				    				{ get { return m_period[statistics_time].count_send_io;}}
		public	long		async_send(int _time)			    			{ return m_period[_time].count_send_io; }

		public	long		async_reveive_total					    		{ get { return m_total.count_receive_io + m_period[statistics_time].count_receive_io;}}
		public	long		async_reveive_now				    			{ get { return m_period[statistics_time].count_receive_io;}}
		public	long		async_reveive(int _time)		    			{ return m_period[_time].count_receive_io; }

		public	void		statistics_on_receive_message()					{ Interlocked.Increment(ref m_period[statistics_time].count_received_message);}
		public	void		statistics_on_receive_message(long _count)		{ Interlocked.Add(ref m_period[statistics_time].count_received_message, _count); }
		public	void		statistics_on_receive_bytes(long _bytes)		{ Interlocked.Add(ref m_period[statistics_time].count_received_bytes, _bytes); }
		public	void		statistics_on_send_message()					{ Interlocked.Increment(ref m_period[statistics_time].count_sended_message);}
		public	void		statistics_on_send_message(long _count)			{ Interlocked.Add(ref m_period[statistics_time].count_sended_message, _count); }
		public	void		statistics_on_send_bytes(long _bytes)			{ Interlocked.Add(ref m_period[statistics_time].count_sended_bytes, _bytes); }
		public	void		statistics_on_error_send()						{ Interlocked.Increment(ref m_period[statistics_time].count_error_on_send);}
		public	void		statistics_on_error_receive()					{ Interlocked.Increment(ref m_period[statistics_time].count_error_on_receive);}
		public	void		statistics_on_async_receive()					{ Interlocked.Increment(ref m_period[statistics_time].count_receive_io);}
		public	void		statistics_on_async_send()						{ Interlocked.Increment(ref m_period[statistics_time].count_send_io);}

		public	TRAFFIC		traffic_info									{ get { return m_period[statistics_time];} }
		public	TRAFFIC		get_traffic_info(int _index)					{ Debug.Assert(_index<m_period.Length); return m_period[_index]; }

		private	TRAFFIC[]	m_period;
		private	static TRAFFIC m_total;

		public static int	statistics_time = 0;
	}

	public class Ntraffic
	{
	// public)
		// 1) Get 함수.
		public	DateTime		connect_time								{ get { return m_time_connect;} }
		public	long			tick_connect								{ get { return m_tick_connect;} }
		public	long			count_connect_try							{ get { return m_count_connect_try;} }
		public	long			tick_disconnect								{ get { return m_tick_disconnect;} }
																		 
		public	long			received_message							{ get { return m_pstatistics_traffic.received_message_total; } }
		public	long			received_bytes								{ get { return m_pstatistics_traffic.received_bytes_total;} }
		public	long			tick_last_send								{ get { return m_tick_last_send;} }
		public	long			sended_message								{ get { return m_pstatistics_traffic.sended_message_total;} }
		public	long			sended_bytes								{ get { return m_pstatistics_traffic.sended_bytes_total;} }
		public	long			tick_last_receive							{ get { return m_tick_last_receive;} }
																		  
		public	long			now_overlapped_io							{ get { return m_count_async_con_dis+m_count_async_sending+m_count_async_receiving;} }
		public	long			overlapped_send								{ get { return m_pstatistics_traffic.async_send_total;} }
		public	long			overlapped_sending							{ get { return m_count_async_sending;} }
		public	long			overlapped_receive							{ get { return m_pstatistics_traffic.async_reveive_total;} }
		public	long			overlapped_receiving						{ get { return m_count_async_receiving;} }
																						  
		public	int				is_error_disconnected()						{ return m_is_error_disconnect;}
																						  
		// 2) socket의 생성후 전체 정보														   
		public	void			reset()										{ m_pstatistics_traffic.reset();}
																						  
		// 3) NSocket 전체 정보																   
		public static	void	reset_total()								{ m_statistics_traffic_total.reset();}
		public static TRAFFIC_INFO total									{ get { return m_statistics_traffic_total;} }

	// implementation) 
		public void				statistics_on_connect_try()					{ Interlocked.Increment(ref m_count_connect_try); Interlocked.Increment(ref m_count_total_connect_try); Nconnective.total.statistics_on_try(); m_tick_disconnect=0;}
		public void				statistics_on_connect()						{ m_time_connect=DateTime.Now; m_tick_last_receive=m_tick_last_send=m_tick_connect=Environment.TickCount; Nconnective.total.statistics_on_success_connect(); Interlocked.Increment(ref m_count_total_connect);}
		public void				statistics_on_disconnect()					{ m_tick_disconnect=Environment.TickCount; m_count_connect_try=0;}
		public void				statistics_set_error_disconnect()			{ Interlocked.Exchange(ref m_is_error_disconnect, 1);}
		public void				statistics_check_error_disconnect()			{ long count_error_disconnect = Interlocked.Exchange(ref m_is_error_disconnect, 0); Nconnective.total.statistics_on_error_disconnect(count_error_disconnect);}
																						  
		public void				statistics_on_receive_message()				{ m_pstatistics_traffic.statistics_on_receive_message();		m_statistics_traffic_total.statistics_on_receive_message();}
		public void				statistics_on_receive_message(long _count)	{ m_pstatistics_traffic.statistics_on_receive_message(_count);m_statistics_traffic_total.statistics_on_receive_message(_count);}
		public void				statistics_on_receive_bytes(long _bytes)	{ m_pstatistics_traffic.statistics_on_receive_bytes(_bytes);	m_statistics_traffic_total.statistics_on_receive_bytes(_bytes); m_tick_last_receive=Environment.TickCount;}
		public void				statistics_on_receive_async()				{ m_pstatistics_traffic.statistics_on_async_receive();	        m_statistics_traffic_total.statistics_on_async_receive();}
		public void				statistics_on_send_message()				{ m_pstatistics_traffic.statistics_on_send_message();			m_statistics_traffic_total.statistics_on_send_message();}
		public void				statistics_on_send_message(long _count)		{ m_pstatistics_traffic.statistics_on_send_message(_count);		m_statistics_traffic_total.statistics_on_send_message(_count);}
		public void				statistics_on_send_bytes(long _bytes)		{ m_pstatistics_traffic.statistics_on_send_bytes(_bytes);	    m_statistics_traffic_total.statistics_on_send_bytes(_bytes);  m_tick_last_send=Environment.TickCount;}
		public void				statistics_on_send_async()					{ m_pstatistics_traffic.statistics_on_async_send();		        m_statistics_traffic_total.statistics_on_async_send();}
		public void				statistics_on_error_send()					{ m_statistics_traffic_total.statistics_on_error_send();}
		public void				statistics_on_error_receive()				{ m_statistics_traffic_total.statistics_on_error_receive();}
		public void				statistics_update_tick_last_send(long _tick){ m_tick_last_send = _tick; }
		public void				statistics_update_tick_last_receive(long _tick){ m_tick_last_receive = _tick; }

		public long				statistics_on_increase_async_con_dis()		{ return Interlocked.Increment(ref m_count_async_con_dis);}
		public long				statistics_on_decrease_async_con_dis()		{ return Interlocked.Decrement(ref m_count_async_con_dis);}
		public long				statistics_on_increase_async_sending()		{ return Interlocked.Increment(ref m_count_async_sending);}
		public long				statistics_on_decrease_async_sending()		{ return Interlocked.Decrement(ref m_count_async_sending);}
		public long				statistics_on_increase_async_receiveing()	{ return Interlocked.Increment(ref m_count_async_receiving);}
		public long				statistics_on_decrease_async_receiveing()	{ return Interlocked.Decrement(ref m_count_async_receiving);}
		public void				reset_statistics()
		{
			// 1) Traffic Info를 Reset한다.
			m_pstatistics_traffic.reset();

			// 2) I/O관련...
			m_count_async_con_dis = 0;
			m_count_async_sending = 0;
			m_count_async_receiving = 0;
			m_is_error_disconnect = 0;

			// 3) 전송시간 관련
			m_tick_last_receive = 0;
			m_tick_last_send = 0;
			m_tick_wait_time = 0;
		}

		//public const int	TOTAL_INFO	 = 1440*7;
		public const int	TOTAL_INFO	 = 32;

		// 1) 전송량 statistics
		public TRAFFIC_INFO		m_pstatistics_traffic			 = new TRAFFIC_INFO(TOTAL_INFO);
		public static TRAFFIC_INFO	m_statistics_traffic_total	 = new TRAFFIC_INFO(TOTAL_INFO);

		// 2) Total관련(이것은 socket이 Disconnect되어도 Reset되지 않는다.)
		public	long			m_count_total_connect_try;	// 총 접속 시도 횟수
		public	long			m_count_total_connect;		// 총 접속 성공 횟수

		// 3) 전송시간 관련
		public	long			m_count_connect_try;		// 접속 시도 횟수
		public	DateTime		m_time_connect;				// 접속한 시간.
		public	long			m_tick_connect;				// 접속한 시간.(Tick)
		public	long			m_tick_last_send;			// 최후에 보낸 시간.(Tick)
		public	long			m_tick_last_receive;		// 최후에 받은 시간.(Tick)
		public	long			m_tick_wait_time;			// 기다리는 시간
		public	long			m_tick_disconnect;			// 접속종료한 시간.(Tick)
		public	int				m_is_error_disconnect;		// Error로 인한 강제접속종료 여부

		public	int				m_count_async_con_dis;	    // 현재 진행중인 Connect/Disconnect Overlapped I/O의 수.
		public	int				m_count_async_sending;	    // 현재 진행중인 Send Overlapped I/O의 수.
		public	int				m_count_async_receiving;	// 현재 진행중인 Receive Overlapped I/O의 수.
	}
}
