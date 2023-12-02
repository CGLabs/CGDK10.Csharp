//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                         Socket Classes Sample (1)                         *
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
using System.Collections.Generic;
using System.Net;
using CGDK;
using CGDK.net;

// ----------------------------------------------------------------------------
//
// test_tcp_echo_client
//
// ----------------------------------------------------------------------------
class test_tcp_echo_client
{
	// 1) Random
	private	static Random		m_random					 = new Random();

	// 2) Connect Text 설정 정보
	public static	bool		enable_connection_test		 = false;
	public static	int			connection_test_min_connect	 = 500;
	public static	int			connection_test_max_connect	 = 800;
	public static	int			connection_test_interval	 = 200;
	public static	IPEndPoint	bind_ip_connect				 = new IPEndPoint(0, 0);
	public static	HashSet<socket_tcp>	m_set_socket		 = new HashSet<socket_tcp>();
	private static	CGDK.factory.auto<socket_tcp> m_pool_socket = new CGDK.factory.auto<socket_tcp>("socket pool");

	// 3) Send Test용 설정 정부
	public static bool			enable_send_test			 = false;
	public	static	int			send_test_times				 = 100; 
	public	static	int			send_test_message_size		 = 0;
	private static CGDK.buffer[,] m_buffer_message			 = new CGDK.buffer[7, 16];
	private	static	int[,]		m_count_message				 = new int[7,16];

	// 4) Threads for Tests
	private	static	Thread		m_thread_connection_test;
	private	static	Thread		m_thread_send_test;
	private static	bool		m_is_work					 = true;
	
	public static void init_test()
	{
		// 1) Initialization the Buffers for sending test
		init_message_buffers();

		// 2) Create Thread object for testing (Connection/Sending)
		m_thread_connection_test = new Thread(process_connection_test);
		m_thread_send_test = new Thread(process_send_test);

		// 3) Start teh Threads
		m_thread_connection_test.Start();
		m_thread_send_test.Start();
	}

	private static void init_message_buffers()
	{
		// 1) 8Byte Message
		{
			var temp_buffer = CGDK.factory.memory.alloc_buffer(8);
			temp_buffer.append<int>(8);
			temp_buffer.set_front<int>(temp_buffer.size + 4);
			temp_buffer.append_CRC();
						
			for(int i=0, count=1; i<16; ++i, count*=2)
			{
				m_count_message[0,i] = count;
				m_buffer_message[0, i] = CGDK.factory.memory.alloc_buffer(8 * count);

				for(int j = 0; j < count; ++j)
				{
					m_buffer_message[0,i].append(temp_buffer.Array, temp_buffer.Offset, temp_buffer.Count);
				}
			}

			CGDK.factory.memory.free(temp_buffer.data);
		}

		// 2) 64Byte Message
		{
			var temp_buffer = CGDK.factory.memory.alloc_buffer(64);
			temp_buffer.append<int>(64);
			for(int k=0;k<56;k+=sizeof(int))
				temp_buffer.append<int>(k);
			temp_buffer.set_front<int>(temp_buffer.size + CGDK.buffer.SIZE_OF_CRC);
			temp_buffer.append_CRC();

			for(int i=0, count=1; i<13; ++i, count*=2)
			{
				m_count_message[1,i] = count;
				m_buffer_message[1, i] = CGDK.factory.memory.alloc_buffer(64 * count);

				for(int j=0; j<count; ++j)
				{
					m_buffer_message[1,i].append(temp_buffer.Array, temp_buffer.Offset, temp_buffer.Count);
				}
			}

			CGDK.factory.memory.free(temp_buffer.data);
		}

		// 3) 256Byte Message
		{
			var temp_buffer = CGDK.factory.memory.alloc_buffer(256);
			temp_buffer.append<int>(256);
			for(int k=0;k<248;k+=sizeof(int))
				temp_buffer.append<int>(k);
			temp_buffer.set_front<int>(temp_buffer.size + 4);
			temp_buffer.append_CRC();

			for(int i=0, count=1; i<11; ++i, count*=2)
			{
				m_count_message[2,i] = count;
				m_buffer_message[2, i] = CGDK.factory.memory.alloc_buffer(256 * count);

				for(int j=0; j<count; ++j)
				{
					m_buffer_message[2,i].append(temp_buffer.Array, temp_buffer.Offset, temp_buffer.Count);
				}

			}
			CGDK.factory.memory.free(temp_buffer.data);
		}


		// 4) 1KByte Message
		{
			var temp_buffer = CGDK.factory.memory.alloc_buffer(1024);
			temp_buffer.append<int>(1024);
			for(int k=0;k<1016;k+=sizeof(int))
				temp_buffer.append<int>(k);
			temp_buffer.set_front<int>(temp_buffer.size + 4);
			temp_buffer.append_CRC();

			for(int i=0, count=1; i<9; ++i, count*=2)
			{
				m_count_message[3,i] = count;
				m_buffer_message[3, i] = CGDK.factory.memory.alloc_buffer(1024 * count);

				for(int j=0; j<count; ++j)
				{
					m_buffer_message[3,i].append(temp_buffer.Array, temp_buffer.Offset, temp_buffer.Count);
				}
			}

			CGDK.factory.memory.free(temp_buffer.data);
		}


		// 5) 4KByte Message
		{
			var temp_buffer = CGDK.factory.memory.alloc_buffer(4096);
			temp_buffer.append<int>(4096);
			for(int k = 0; k < 4088; k += sizeof(int))
				temp_buffer.append<int>(k);
			temp_buffer.set_front<int>(temp_buffer.size + 4);
			temp_buffer.append_CRC();

			for(int i=0, count=1; i<7; ++i, count*=2)
			{
				m_count_message[4,i] = count;
				m_buffer_message[4, i] = CGDK.factory.memory.alloc_buffer(4096 * count);

				for(int j = 0; j < count; ++j)
				{
					m_buffer_message[4,i].append(temp_buffer.Array, temp_buffer.Offset, temp_buffer.Count);
				}
			}

			CGDK.factory.memory.free(temp_buffer.data);
		}

		// 6) 16KByte Message
		{
			var temp_buffer = CGDK.factory.memory.alloc_buffer(16384);
			temp_buffer.append<int>(16384);
			for(int k = 0; k < 16376; k += sizeof(int))
				temp_buffer.append<int>(k);
			temp_buffer.set_front<int>(temp_buffer.size + 4);
			temp_buffer.append_CRC();

			for(int i=0, count=1; i<5; ++i, count*=2)
			{
				m_count_message[5,i] = count;
				m_buffer_message[5, i] = CGDK.factory.memory.alloc_buffer(16384 * count);

				for(int j=0; j<count; ++j)
				{
					m_buffer_message[5,i].append(temp_buffer.Array, temp_buffer.Offset, temp_buffer.Count);
				}
			}

			CGDK.factory.memory.free(temp_buffer.data);
		}

		// 7) 64KByte Message
		{
			CGDK.buffer temp_buffer = CGDK.factory.memory.alloc_buffer(65536);
			temp_buffer.append<int>(65536);
			for(int k = 0; k < 65528; k += sizeof(int))
				temp_buffer.append<int>(k);
			temp_buffer.set_front<int>(temp_buffer.size + 4);
			temp_buffer.append_CRC();

			for(int i = 0, count = 1; i < 3; ++i, count *= 2)
			{
				m_count_message[6,i] = count;
				m_buffer_message[6, i] = CGDK.factory.memory.alloc_buffer(65536 * count);

				for(int j = 0; j < count; ++j)
				{
					m_buffer_message[6,i].append(temp_buffer.Array, temp_buffer.Offset, temp_buffer.Count);
				}
			}

			CGDK.factory.memory.free(temp_buffer.data);
		}
	}

	public static void close_test()
	{
		// 1) Test를 모두 제거한다.
		request_start_connection_test();
		request_start_send_test();

		// 2) Thread를 종료시킨다.
		m_is_work = false;
	}

	public static void reset_statistics()
	{
		// 1) Reset
		CGDK.net.io.statistics.Nconnective.total.reset();
		CGDK.net.io.statistics.Ntraffic.total.reset();
	}

	public static void request_start_connection_test()
	{
		enable_connection_test = true;
	}

	public static void request_stop_connection_test()
	{
		enable_connection_test = false;
	}

	public static void request_start_send_test()
	{
		enable_send_test = true;
	}

	public static void request_stop_send_test()
	{
		enable_send_test = false;
	}

	public static void request_connect(int _count_connect)
	{
		lock(m_set_socket)
		{
			for(int i=0; i<_count_connect; i++)
			{
				var socket = m_pool_socket.alloc();

				socket.start(bind_ip_connect);
			}
		}
	}
	public static void request_disconnect_single()
	{
		lock(m_set_socket)
		{
			// 1) 단 하나의 소켓만 접속종료한다.
			foreach(socket_tcp socket in m_set_socket)
			{
				socket.closesocket();
				break;
			}
		}
	}
	public static void request_disconnect_all()
	{
		lock(m_set_socket)
		{
			// 1) 모두 접속종료한다.
			foreach(socket_tcp socket in m_set_socket)
			{
				socket.closesocket();
			}
		}
	}

	public static void request_send(int _times)
	{
		request_send(send_test_message_size, _times);
	}
	public static void request_send(int p_iIndex, int _times)
	{
		switch(p_iIndex)
		{
		case	0:
				request_send_message_0(_times);
				break;
		case	1:
				request_send_message_1(_times);
				break;
		case	2:
				request_send_message_2(_times);
				break;
		case	3:
				request_send_message_3(_times);
				break;
		case	4:
				request_send_message_4(_times);
				break;
		case	5:
				request_send_message_5(_times);
				break;
		case	6:
				request_send_message_6(_times);
				break;
		}
	}
	public static void request_send_error_zero()
	{
		var temp_buffer = CGDK.factory.memory.alloc_buffer(256);

		temp_buffer.append<ushort>(0);
		temp_buffer.append<ushort>(1);

		lock(m_set_socket)
		{
			foreach(socket_tcp socket in m_set_socket)
			{
				socket.send(temp_buffer);
			}
		}
	}

	
	public static void process_connection_test()
	{
		int	tick_pre = Environment.TickCount;

		while(m_is_work)
		{
			// - 현재 Tick을 구한다.
			int	tick_now = Environment.TickCount;

			// - Tick Gap을 구한다.
			int	tick_gap = tick_now-tick_pre;

			// - Tick Gap이 100ms이하면 Sleep한다.
			if(tick_gap<connection_test_interval)
			{
				Thread.Sleep(connection_test_interval-tick_gap);
			}

			// - Tick Pre를 Update한다.
			tick_pre	 = Environment.TickCount;

			// Check) Test가 disable되어 있으면 그냥 여기서 Continue
			if(enable_connection_test==false)
				continue;

			lock(m_set_socket)
			{
				// Declare)
				int	nConnect = 0;
				int	disconnected = 0;

				// 1) Disconnect Test
				int	count_socket = m_set_socket.Count;

				if(count_socket > connection_test_min_connect)
				{
					int	range = count_socket-connection_test_min_connect;
					int	disconnect = (range!=0) ? m_random.Next()%range : 0;

					foreach(socket_tcp socket in m_set_socket)
					{
						// - Disconnect한다.
						if(socket.closesocket())
						{
							if((++disconnected)==disconnect)
								break;
						}
					}

					count_socket -= disconnected;
				}

				// 2) Connect Test
				count_socket = m_set_socket.Count-disconnected;

				if(count_socket < connection_test_min_connect)
				{
					// MinConnect보다 낮을 시 Min Connect만큼 한꺼번에 접속해버린다.
					nConnect = connection_test_min_connect-count_socket;
				}
				else if(count_socket < connection_test_max_connect)
				{
					int	range = connection_test_max_connect-count_socket;

					// 여유최대 Remained/8(12.5%)의 값을 신규로 접속시도한다.
					nConnect = (range!=0) ? m_random.Next()%range : 0;
				}
				else
				{
					nConnect = 0;
				}

				request_connect(nConnect);
			}
		}
	}
	public static void process_send_test()
	{
		int	tick_pre = Environment.TickCount;

		while(m_is_work)
		{
			// - 현재 Tick을 구한다.
			int	tick_now = Environment.TickCount;

			// - Tick Gap을 구한다.
			int	tick_gap = tick_now-tick_pre;

			// - Tick Gap이 100ms이하면 Sleep한다.
			if(tick_gap < 100)
			{
				Thread.Sleep(100-tick_gap);
			}

			// - Tick Pre를 Update한다.
			tick_pre = Environment.TickCount;

			// Check) Test가 disable되어 있으면 그냥 여기서 Continue
			if(enable_send_test == false)
				continue;

			request_send(send_test_message_size, send_test_times);
		}
	}

	public static void request_send_message_0(int _send_count)
	{
		// 8Byte
		lock(m_set_socket)
		{
			try
			{
				foreach(socket_tcp socket in m_set_socket)
				{
					int nSend = _send_count;

					for(int i=0; i<15 && nSend!=0 ; ++i, nSend/=2)
					{
						if((nSend & 1)!=0)
						{
							socket.process_request_send(m_buffer_message[0,i], m_count_message[0,i]);
						}
					}

					for(; nSend!=0; --nSend)
					{
						socket.process_request_send(m_buffer_message[0,15], m_count_message[0,15]);
					}
				}
			}
			catch(Exception )
			{
			}
		}
	}
	public static void request_send_message_1(int _send_count)
	{
		// 16Byte
		lock(m_set_socket)
		{
			try
			{
				foreach(socket_tcp socket in m_set_socket)
				{
					int nSend = _send_count;

					for(int i = 0; i < 14 && nSend != 0 ; ++i, nSend /= 2)
					{
						if((nSend & 1)!=0)
						{
							socket.process_request_send(m_buffer_message[1,i], m_count_message[1,i]);
						}
					}

					for(; nSend != 0; --nSend)
					{
						socket.process_request_send(m_buffer_message[1,14], m_count_message[1,14]);
					}
				}
			}
			catch(Exception )
			{
			}
		}
	}
	public static void request_send_message_2(int _send_count)
	{
		// 64Byte
		lock(m_set_socket)
		{
			try
			{
				foreach(socket_tcp socket in m_set_socket)
				{
					int nSend = _send_count;

					for(int i=0; i < 12 && nSend != 0 ; ++i, nSend /= 2)
					{
						if((nSend & 1) != 0)
						{
							socket.process_request_send(m_buffer_message[2,i], m_count_message[2,i]);
						}
					}

					for(; nSend!=0; --nSend)
					{
						socket.process_request_send(m_buffer_message[2,12], m_count_message[2,12]);
					}
				}
			}
			catch(Exception )
			{
			}
		}
	}
	public static void request_send_message_3(int _send_count)
	{
		// 256Byte
		lock(m_set_socket)
		{
			try
			{
				foreach(socket_tcp socket in m_set_socket)
				{
					int nSend = _send_count;

					for(int i = 0; i < 10 && nSend != 0 ; ++i, nSend /= 2)
					{
						if((nSend & 1) != 0)
						{
							socket.process_request_send(m_buffer_message[3,i], m_count_message[3,i]);
						}
					}

					for(; nSend != 0; --nSend)
					{
						socket.process_request_send(m_buffer_message[3,10], m_count_message[3,10]);
					}
				}
			}
			catch(Exception )
			{
			}
		}
	}
	public static void request_send_message_4(int _send_count)
	{
		// 1KByte
		lock(m_set_socket)
		{
			try
			{
				foreach(socket_tcp socket in m_set_socket)
				{
					int nSend = _send_count;

					for(int i = 0; i < 8 && nSend != 0 ; ++i, nSend /= 2)
					{
						if((nSend & 1) != 0)
						{
							socket.process_request_send(m_buffer_message[4,i], m_count_message[4,i]);
						}
					}

					for(; nSend != 0; --nSend)
					{
						socket.process_request_send(m_buffer_message[4,8], m_count_message[4,8]);
					}
				}
			}
			catch(Exception )
			{
			}
		}
	}
	public static void request_send_message_5(int _send_count)
	{
		// 4KByte
		lock(m_set_socket)
		{
			try
			{
				foreach(socket_tcp socket in m_set_socket)
				{
					int nSend = _send_count;

					for(int i = 0; i < 6 && nSend != 0 ; ++i, nSend /= 2)
					{
						if((nSend & 1) != 0)
						{
							socket.process_request_send(m_buffer_message[5,i], m_count_message[5,i]);
						}
					}

					for(; nSend!=0; --nSend)
					{
						socket.process_request_send(m_buffer_message[5,6], m_count_message[5,6]);
					}
				}
			}
			catch(Exception )
			{
			}
		}
	}
	public static void request_send_message_6(int _send_count)
	{
		// 16KByte
		lock(m_set_socket)
		{
			try
			{
				foreach(socket_tcp socket in m_set_socket)
				{
					int nSend = _send_count;

					for(int i = 0; i < 4 && nSend != 0 ; ++i, nSend /= 2)
					{
						if((nSend & 1) != 0)
						{
							socket.process_request_send(m_buffer_message[6,i], m_count_message[6,i]);
						}
					}

					for(; nSend != 0; --nSend)
					{
						socket.process_request_send(m_buffer_message[6,4], m_count_message[6,4]);
					}
				}
			}
			catch(Exception )
			{
			}
		}
	}
	
	public static void on_socket_connect(object _source)
	{
		lock(m_set_socket)
		{
			m_set_socket.Add(_source as socket_tcp);
		}
	}
	public static void on_socket_fail_connect(object _source, ulong _disconnect_reason)
	{
	}
	public static void on_socket_disconnect(object _source, ulong _disconnect_reason)
	{
		lock(m_set_socket)
		{
			m_set_socket.Remove(_source as socket_tcp);
		}
	}
	public static int on_socket_message(object _source, sMESSAGE _msg)
	{
		// Return) 
		return 1;
	}
	
}

