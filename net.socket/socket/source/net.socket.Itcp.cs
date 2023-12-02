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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using CGDK;
using CGDK.factory;
using CGDK.net.io;

//----------------------------------------------------------------------------
//
//  class CGDK.net.socket.Itcp
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.net.socket
{
	public class Itcp :
        Nreferenceable,
        io.Iconnectable,
        CGDK.Imessageable, 
		io.Isender_stream,
        IDisposable
	{
	// public)
		public bool						send(CGDK.buffer _buffer)
		{
			return	process_request_send(_buffer, 1);
		}
		public	Iconnective				connective
		{
			get { return m_pconnective; }
			set { m_pconnective=value;}
		}
		public io.statistics.Ntraffic	statistics
		{
			get { return m_statistics_traffic; }
		}
		public bool						closesocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// check) disconnect_reason_MASK에 해당하는 부분의 flag를 유저가 설정해서는 안됀다.
			Debug.Assert((_disconnect_reason & DISCONNECT_REASON.MASK) == 0);

			// 1) add active disconnect flag
			_disconnect_reason &= ~DISCONNECT_REASON.MASK;
			_disconnect_reason |= DISCONNECT_REASON.ACTIVE;
			_disconnect_reason |= DISCONNECT_REASON.ABORTIVE;

			// 2) closesocket
			return process_closesocket(_disconnect_reason);
		}
		public virtual bool				disconnect(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// 1) set socket send_disconnect
			var pre_socket_status = m_socket.set_socket_state_if(eSOCKET_STATE.ESTABLISHED, eSOCKET_STATE.SEND_DISCONNECTED);

			// check)
			if(pre_socket_status != eSOCKET_STATE.ESTABLISHED)
				return false;

			// check) disconnect_reason_MASK에 해당하는 부분의 flag를 유저가 설정해서는 안됀다.
			Debug.Assert((_disconnect_reason & DISCONNECT_REASON.MASK) == 0);

			// 2) add active disconnect flag
			_disconnect_reason &= ~DISCONNECT_REASON.MASK;
			_disconnect_reason |= DISCONNECT_REASON.ACTIVE;

			// 3) set disconnect reason flag
			m_disconnect_reason |= _disconnect_reason;

			// 4) shutdown if not sending
			lock (m_cs_send_queue)
			{
				if(m_is_sending != true)
				{
					// - set linger option as graceful close
					m_socket.set_linger_option_graceful();

					// - shutdown
					return m_socket.shutdown();
				}
			}

			// return)
			return true;
		}
		public io.NSocket				socket
		{
			get { return m_socket;}
		}
		public ulong					disconnect_reason 
		{
			get { return m_disconnect_reason; }
			set { m_disconnect_reason = value; }
		}

		public int						maximum_receive_buffer_size				{ get { return m_maximum_receive_buffer_size; } set { m_maximum_receive_buffer_size = value; } }
		public int						maximum_send_buffer_size				{ get { return m_maximum_send_buffer_size; } set { m_maximum_send_buffer_size = value; } }
		public int						minimum_mesage_buffer_size				{ get { return m_minimum_mesage_buffer_size; } set { m_minimum_mesage_buffer_size = value; } }
		public int						maximum_mesage_buffer_size				{ get { return m_maximum_mesage_buffer_size; } set { m_maximum_mesage_buffer_size = value; } }
		public int						max_depth_of_sending_queue				{ get { return m_maximum_depth_of_sending_queue; } set { m_maximum_depth_of_sending_queue = value; } }

		public static int				default_maximum_receive_buffer_size		{ get { return m_default_maximum_receive_buffer_size; } set { m_default_maximum_receive_buffer_size = value; } }
		public static int				default_maximum_send_buffer_size		{ get { return m_default_maximum_send_buffer_size; } set { m_default_maximum_send_buffer_size = value; } }
		public static int				default_minimum_mesage_buffer_size		{ get { return m_default_minimum_mesage_buffer_size; }	set { m_default_minimum_mesage_buffer_size = value; } }
		public static int				default_maximum_mesage_buffer_size		{ get { return m_default_maximum_mesage_buffer_size; }	set { m_default_maximum_mesage_buffer_size = value; } }
		public static int				default_maximum_depth_of_sending_queue	{ get { return m_default_maximum_depth_of_sending_queue; } set { m_default_maximum_depth_of_sending_queue = value; } }

		// framework)
		protected virtual void			on_connect() {}
		protected virtual void			on_fail_connect(ulong _disconnect_reason) {}
		protected virtual void			on_disconnect(ulong _disconnect_reason) {}
		protected virtual void			on_receive(CGDK.buffer _buffer_received, SocketAsyncEventArgs _args) {}
		protected virtual int			on_message(object _source, sMESSAGE _msg) { return 0;}
		protected override void			on_final_release()
		{
			lock (m_cs_send_queue)
			{
				// check) Reference Count는 0이어야 한다.
				Debug.Assert(reference_count == 0);

				// check) 현재 전송중이면 안됀다.
				Debug.Assert(m_is_sending == false);

				// check) m_handle_socket은 무조건 null인 상태로 Release되어야 한다.
				Debug.Assert(m_socket.socket_handle == null);

				// 1) RECEIVE- Buffer를 할당해제한다.
				if(!m_buffer_receiving.empty())
				{
					memory.free(m_buffer_receiving.clear());
				}

				// 2) RECEIVE-Receive용 Message에 설정해 놓은 Buffer를 제거.
				m_buffer_message.Array = null;

				// 3) SEND-Queued된 것 할당해제
				if(m_buffer_queued != null)
				{
					m_buffer_queued.clear();

					m_factroy_async_args_send.free(m_buffer_queued);

					m_buffer_queued = null;
				}

				// 4) Send Buffer를 할당해제한다.
				if(m_buf_send.empty() == false)
				{
					memory.free(m_buf_send.clear());
				}
			}
		}

	// implementation)
		protected void					process_prepare_socket(AddressFamily _address_family)
		{
			// 1) socket을 생성한다.
			m_socket.create_socket_handle(_address_family, SocketType.Stream, ProtocolType.Tcp);
		}
		protected bool					process_closesocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// 1) set disconnect reasone flag
			m_disconnect_reason	|= _disconnect_reason;

			// 2) set linger option
			if((_disconnect_reason & (DISCONNECT_REASON.FAIL | DISCONNECT_REASON.ABORTIVE)) != DISCONNECT_REASON.NONE)
				m_socket.set_linger_option_abortive();
			else
				m_socket.set_linger_option_graceful();

			// 3) close socket handle
			return m_socket.close_socket_handle();
		}

		public virtual bool				process_complete_connect(SocketAsyncEventArgs _args)
		{
			// check) 접속 성공여부를 확인한다.
			if (_args.SocketError != SocketError.Success)
			{
				// - NSocket Status를 Disconnect로
				m_socket.socket_state = eSOCKET_STATE.CLOSED;

				// - on_fail_connect함수를 호출한다.
				try
				{
					on_fail_connect(0);
				}
				catch(System.Exception)
				{
				}

				// - socket을 닫는다.
				process_closesocket(DISCONNECT_REASON.ACTIVE | DISCONNECT_REASON.ABORTIVE);

				// return) 끝!
				return false;
			}

			// 1) NSocket 상태를 eSOCKET_STATE.ESTABLISHED로 바꾼다.
			m_socket.set_socket_state_if(eSOCKET_STATE.SYN, eSOCKET_STATE.ESTABLISHED);

			// statistics) 
			statistics.statistics_on_connect();

			// reference counting)
			add_ref();

			try
			{
				lock(m_cs_send_queue)
				{
					// 1) 임시로 복사한다.
					m_socket.set_receive_buffer_size(maximum_receive_buffer_size);
					m_socket.set_send_buffer_size(maximum_send_buffer_size);

					// 2) prepare for receive
					m_buffer_receiving = new CGDK.buffer(memory.alloc_memory(m_minimum_mesage_buffer_size), 0, m_minimum_mesage_buffer_size);
					m_buffer_message = new CGDK.buffer(m_buffer_receiving.Array, 0, 0);

					// 3) prepare sending
					Debug.Assert(m_is_sending == false);
					m_buffer_queued = m_factroy_async_args_send.alloc();
					m_buffer_queued.SocketFlags = SocketFlags.None;
					m_buffer_queued.SocketError = SocketError.Success;
					//m_buffer_queued.SendPacketsFlags = TransmitFileOptions.UseDefaultWorkerThread;
					m_buffer_queued.UserToken = this;

					// 4) alloc buffer for sending
					m_buf_send = memory.alloc_buffer(m_maximum_send_buffer_size);

					// check) 
					Debug.Assert(m_buffer_queued.list_buffers.Count == 0);
					Debug.Assert(m_buffer_queued.list_async_send.Count == 0);
					Debug.Assert(m_buffer_queued.count_message == 0);
					Debug.Assert(m_buffer_queued.Buffer == null);
					Debug.Assert(m_buffer_queued.BufferList == null);
				}

				// 5) on_connect함수를 호출한다. 
				//    * 주의: Receive거는 것보다 notify_on_connect를 먼저해야 한다!
				//            만약 Receive를 먼저 걸게 될 경우 다른 Trhead로 CompeteReceive가 수행될 수 있기 때문이다.
				on_connect();

				// 6) call 'prepare_receive'
				if(prepare_receive() == false)
				{
					// - process_disconnect한다.
					process_complete_disconnect();
				}
			}
			catch(System.Exception)
			{
				// statistics)
				statistics.statistics_set_error_disconnect();

				// - socket을 닫는다.
				process_closesocket(DISCONNECT_REASON.ACTIVE | DISCONNECT_REASON.ABORTIVE);

				// - connective에...
				process_disconnect();

				// Reference Counting) 
				release();
			}

			// return) 성공!!!
			return true;
		}
		public void						process_complete_disconnect()
		{
			// 1) Closed상태로 NSocket State를 바꾸고 이전 State를 얻어온다.
			var	temp_socket_status = m_socket.exchange_socket_state(eSOCKET_STATE.CLOSED);

			// check) socket이 이미 Closed상태라면 돌려보낸다.
			if(temp_socket_status == eSOCKET_STATE.CLOSED)
			{
				return;
			}

			// declare) 
			var socket_disconnect_reason = m_disconnect_reason;

			// 2) process_disconnect를 부른다.
			try
			{
				on_disconnect(socket_disconnect_reason);
			}
			catch(System.Exception /*e*/)
			{
			}

			// statistics) statistics.on_disconnect
			statistics.statistics_on_disconnect();

			// 3) 강제 종료를 진행한다.
			process_closesocket();

			// 4) 바로 Disconnect Completion을 처리해 준다.
			process_disconnect();

			// Reference Counting)
			release();
		}
		protected virtual void			process_disconnect()
		{
			// 1) connective의 process_connective_disconnect를 호출한다.
			if(m_pconnective != null)
			{
				m_pconnective.process_connective_disconnect(this);
			}

			// statistics) 
			io.statistics.Nconnective.total.statistics_on_disconnect();
		}

		private bool					process_send(SocketAsyncEventArgs_send _args)
		{
			try
			{
				// check) socket이 Established 상태가 아니면 false를 리턴한다.
				if(m_socket.socket_state != eSOCKET_STATE.ESTABLISHED)
					return false;

				// 1) 비동기 전송한다.
				if(m_socket.send_async(_args) == false)
				{
					// return) 
					return false;
				}

				// statistics) 
				statistics.statistics_on_send_async();
			}
			catch(System.Exception )
			{
				// return) 
				return false;
			}

			// return) 
			return true;
		}
		public bool						process_request_send(CGDK.buffer _buffer, int _count_message)
		{
			// check) Buffer 검사!
			Debug.Assert(_buffer.Array != null);
			Debug.Assert((_buffer.Offset + _buffer.Count) <= _buffer.capacity);
			Debug.Assert(_buffer.Count > 0);

			// check) 전송량이 0이면 애초부터 Return해버린다.
			if(_buffer.Count <= 0)
			{
				return false;
			}

			// Reference Count) SEND
			add_ref();

			// declare) 
			CGDK.buffer buf_pre = new CGDK.buffer(null, 0, 0);
			CGDK.buffer buf_send;

			lock (m_cs_send_queue)
			{
				// check) socket이 Established상태가 아니라면 전송은 안됀다.
				if(m_socket.socket_state < eSOCKET_STATE.ESTABLISHED)
				{
					// Reference Count) SEND
					release();

					// return) 
					return false;
				}

				// check) sending queue의 최대 Depth까지 차있으면 falsee를 return한다.
				if(m_buffer_queued.list_buffers.Count >= m_maximum_depth_of_sending_queue)
				{
					// reference count) SEND
					release();

					// return) 
					return false;
				}

				// 2) 남은 버퍼의 크기가 모자라면 새로 버퍼를  할당한다.
				if(m_buf_send.get_remained() < _buffer.Count)
				{
					// - 기존 Buffer를 저장해 놓음
					if(m_buf_send.Count != 0)
					{
						buf_pre = m_buf_send;
					}

					// - Buffer를 새로 할당받음.
					m_buf_send = memory.alloc_buffer(Math.Max(_buffer.Count * 2, m_maximum_send_buffer_size));
				}

				// 3) Buffer를 새로 할당받는다.(C#은 Reference Counting이 불가능하기 때문에 어쩔수 없이 전송시 마다 복사를 할 수 밖에 없다.)
				m_buf_send.append(_buffer.Array, _buffer.Offset, _buffer.Count);

				// 4) 전송 중이면 Queuing만 하고 그냥 끝낸다.
				if(m_is_sending == true)
				{
					// - list에 Buffer를 추가한다.(기본 Buffer에 데이터가 없으면 Queuing하지 않는다.)
					if(!buf_pre.empty())
					{
						// check) buf_pre.Array는 null이어서는 안됀다.
						Debug.Assert(buf_pre.Count != 0);

						// - 추가한다.
						m_buffer_queued.list_buffers.Add(buf_pre);
					}

					// check)
					Debug.Assert(m_buf_send.Count != 0);

					// - Message의 수를 더한다.
					m_buffer_queued.count_message += _count_message;

					// Reference Count) SEND
					release();

					// return) 그냥 끝낸다.
					return true;
				}

				//Check) 전송중이 buf_pre의 i_count는 반드시 0이어야 한다.
				Debug.Assert(buf_pre.empty() || buf_pre.Count == 0);

				// 5) Buffer를 옮겨놓는다.
				buf_send = m_buf_send;

				// 6) Count만큼 Extract한다.
				m_buf_send.extract_skip(m_buf_send.Count);

				// 7) Sending을 true로...
				m_is_sending = true;

				// check) 
				Debug.Assert(m_buffer_queued.list_buffers.Count == 0);
				Debug.Assert(m_buffer_queued.list_async_send.Count == 0);
				Debug.Assert(m_buffer_queued.count_message == 0);
			}

			// 8) Sending Buffer는 할당해제한다.
			if(!buf_pre.empty())
			{
				// check) Prebuffer는 반드시 크기가 0이어야 한다.
				Debug.Assert(buf_pre.Count==0);

				// - 할당해제한다.
				memory.free(buf_pre.clear());
			}
		
			// 9) Send용 AsyncArgs를 할당받는다.
			SocketAsyncEventArgs_send args_send = m_factroy_async_args_send.alloc();

			// check)
			Debug.Assert(args_send.list_buffers.Count == 0);
			Debug.Assert(args_send.list_async_send.Count == 0);
			Debug.Assert(args_send.count_message == 0);

			args_send.count_message = _count_message;
			args_send.BufferList = null;
			args_send.SetBuffer(buf_send.Array, buf_send.Offset, buf_send.Count);
			args_send.SocketFlags = SocketFlags.None;
			args_send.SocketError = SocketError.SocketError;
			//args_send.SendPacketsFlags = TransmitFileOptions.UseDefaultWorkerThread;
			args_send.UserToken = this;

			// 10) 전송을 수행한다.
			var result = process_send(args_send);

			// check) 
			if(result)
			{
				return true;
			}

			// 11) get result
			result = (args_send.SocketError == SocketError.Success);

			// 12) ...
			process_complete_send(args_send);

			// 13) release
			release();

			// return) 
			return result;
		}
		private static void				complete_process_send(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_send = _args as SocketAsyncEventArgs_send;
			var socket_tcp = _args.UserToken as Itcp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_tcp != null);

			// 1) process_complete_send를 호출한다.
			try
			{
				socket_tcp.process_complete_send(args_send);
			}
			catch(System.Exception)
			{
			}

			// Reference Counting) 
			socket_tcp.release();
		}
		protected virtual void			process_complete_send(SocketAsyncEventArgs_send _args)
		{
			// check) 실패했으면 끝낸다...
			if(_args.SocketError != SocketError.Success)
			{
				// 1) Sending Buffer를 Clear한다.
				lock(m_cs_send_queue)
				{
					// - 전송 상태를 false로..
					m_is_sending = false;

					// - Queued를...
					m_buffer_queued.clear();
				}

				// 2) args 초기화 및 할당해제
				_args.clear();
				m_factroy_async_args_send.free(_args);

				// return) 
				return;
			}

			// statistics)
			statistics.statistics_on_send_bytes(_args.BytesTransferred);
	
			// statistics)
			statistics.statistics_on_send_message(_args.count_message);

			// 4) Clear (여기서 BufferList의 제일 마지막은 사용중인 것이므로 FREE해서는 안됀다.)
			_args.clear();

			// declare)
			var args_pre = _args;

			while(true)
			{
				// declare) 
				SocketAsyncEventArgs_send args_send = null;
				CGDK.buffer buf_temp;

				lock (m_cs_send_queue)
				{
					// check) 
					Debug.Assert(m_is_sending == true);

					// 2) Buffer에 Queuing된 것이 없으면 m_is_sending을 false로만 하고 끝낸다.
					if (m_buf_send.Count == 0)
					{
						// check) 이 Buffer는 반드시 0이여야 한다.
						Debug.Assert(m_buffer_queued.list_buffers.Count == 0);

						// - 전송 상태를 false로..
						m_is_sending = false;

						// - shutdown if send_disconnected
						if (m_socket.socket_state == eSOCKET_STATE.SEND_DISCONNECTED)
						{
							// - set linger Option for graceful disconnect
							m_socket.set_linger_option_graceful();

							// - send shutdown
							m_socket.shutdown();
						}

						// return) 
						return;
					}

					// 3) 기존 Buffer를 복사해놓는다.
					buf_temp = m_buf_send;

					// 4) 기존 Buffer를 Extract한다.
					m_buf_send.extract_skip(m_buf_send.Count);

					// 5) 기존 Args를 저장해 놓는다.
					args_send = m_buffer_queued;

					// 6) 기존에 쓰든 Arg를 그대로 쓴다.
					m_buffer_queued = args_pre;

					// check) bufferQueue의 값이 제대로 Reset되어 있는지 확인한다.
					Debug.Assert(m_buffer_queued.list_buffers.Count == 0);
					Debug.Assert(m_buffer_queued.list_async_send.Count == 0);
					Debug.Assert(m_buffer_queued.count_message == 0);
				}

				// check) buf_temp.Count는 애초부터 0이 아님을 전제하고 진행했으므로 절대 0일 수는 없다.
				Debug.Assert(buf_temp.Count != 0);

				// 7) 전체를 돌며 Sending을 설정 (하나만 보내는 경우 SetBuffer를 사용한다.)
				if(args_send.list_buffers.Count == 0)
				{
					// - BufferList를 null로 설정한다.
					args_send.BufferList = null;

					// - Buffer를 설정한다.
					args_send.SetBuffer(buf_temp.Array, buf_temp.Offset, buf_temp.Count);
				}
				else
				{
					// check)
					Debug.Assert(args_send.list_async_send.Count == 0);

					// - Buffer는 null로 설정
					args_send.SetBuffer(null, 0, 0);

					// - Queuing된 Buffer를 추가.
					for (int i=0; i<args_send.list_buffers.Count; ++i)
					{
						// - Buffer를 얻는다.
						CGDK.buffer	bufferIter = args_send.list_buffers.ElementAt(i);

						// check) 
						Debug.Assert(bufferIter.Count != 0);

						// - Buffer를 추가한다.
						args_send.list_async_send.Add(new ArraySegment<byte>(bufferIter.Array, bufferIter.Offset, bufferIter.Count));
					}

					// - 마지막 m_buf_send의 내용 추가 (이것은 사용 중인 것이므로 추후 FREE를 해서는 안돼는 버퍼다.)
					args_send.list_async_send.Add(new ArraySegment<byte>(buf_temp.Array, buf_temp.Offset, buf_temp.Count));
					args_send.BufferList = args_send.list_async_send;
				}
				args_send.UserToken = this;
				args_send.SocketFlags = SocketFlags.None;
				args_send.SocketError = SocketError.SocketError;
				//args_send.SendPacketsFlags = TransmitFileOptions.UseDefaultWorkerThread;
		
				// Reference Count) 
				add_ref();

				// 9) 전송을 수행한다.
				var is_raise_event = process_send(args_send);

				// check) 
				if(is_raise_event)
				{
					break;
				}

				// check) failed
				if(args_send.SocketError != SocketError.Success)
				{
  					// statistics) 
					statistics.statistics_on_error_send();

					lock(m_cs_send_queue)
					{
						// - 전송 상태를 false로..
						m_is_sending = false;

						// - Queued를...
						m_buffer_queued.clear();
						m_buf_send.Count = 0;

						// - shutdown if send_disconnected
						if (m_socket.socket_state == eSOCKET_STATE.SEND_DISCONNECTED)
						{
							// - set linger Option for graceful disconnect
							m_socket.set_linger_option_graceful();

							// - send shutdown
							m_socket.shutdown();
						}
					}

					// - clear args_send
					args_send.clear();

					// - free args_send
					m_factroy_async_args_send.free(args_send);

					// Reference Count) 
					release();

					// - 
					break;
				}

				// statistics)
				statistics.statistics_on_send_bytes(args_send.BytesTransferred);
	
				// statistics)
				statistics.statistics_on_send_message(args_send.count_message);

				// 10) clear args_send
				args_send.clear();

				// 11) store to pre
				args_pre = args_send;

				// Reference Count) 
				release();
			}
		}

		private bool					prepare_receive()
		{
			// check)
			if(m_socket.socket_state != eSOCKET_STATE.ESTABLISHED)
			{
				// return) 
				return false;
			}

			// check) Receiving Buffer가 0보다 작으면 끝낸다.
			if(m_buffer_receiving.Count <= 0)
			{
				// return) 
				return false;
			}

			// 1) 할당
			SocketAsyncEventArgs_recv args_receive = m_factory_async_args_receive.alloc();

			// referece counting)
			add_ref();

			// 3) Receive를 다시 건다.
			while(true)
			{
				// check) 
				Debug.Assert(m_buffer_receiving.Count > 0);
				Debug.Assert((m_buffer_receiving.Offset + m_buffer_receiving.Count) == m_buffer_receiving.capacity);

				// 2) 먼저 Receive를 건다.
				args_receive.UserToken = this;
				args_receive.BufferList = null;
				args_receive.SocketFlags = SocketFlags.None;
				args_receive.SocketError = SocketError.Success;
				args_receive.SetBuffer(m_buffer_receiving.Array, m_buffer_receiving.Offset, m_buffer_receiving.Count);   // Buffer, Offset, Count

				// 1) ReceiveAsync 함수를 호출한다.
				var result = m_socket.receive_async(args_receive);

				// check) 
				if(result == true)
				{
					break;
				}

				// check) 
				if(args_receive.SocketError != SocketError.Success || args_receive.BytesTransferred == 0)
				{
					// statistics)
					statistics.statistics_set_error_disconnect();

					// - clear and deallocate
					args_receive.clear();
					m_factory_async_args_receive.free(args_receive);

					// referece counting)
					release();

					// return)
					return false;
				}
				
				try
				{
					// statistics)
					statistics.statistics_on_receive_bytes(args_receive.BytesTransferred);

					// - process receive
					process_receive(args_receive);
				}
				catch(System.Exception)
				{
					// statistics)
					statistics.statistics_set_error_disconnect();

					// - clear and deallocate
					args_receive.clear();
					m_factory_async_args_receive.free(args_receive);

					// referece counting)
					release();

					// return)
					return false;
				}
			}

			statistics.statistics_on_receive_async();

			// return) 
			return true;
		}
		private static void				complete_process_receive(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_receive = _args as SocketAsyncEventArgs_recv;
			var socket_tcp = _args.UserToken as Itcp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_tcp != null);

			// 1) process_complete_send를 호출한다.
			try
			{
				socket_tcp.process_complete_receive(args_receive);
			}
			catch(System.Exception)
			{
			}

			// reference counting) 
			socket_tcp.release();
		}
		protected void					process_receive(SocketAsyncEventArgs _args)
		{
			// 1) 전송받은 크기를 더하고 처리되지 않은 Message의 크기를 구한다.
			m_buffer_receiving.Offset += _args.BytesTransferred;
			m_buffer_receiving.Count -= _args.BytesTransferred;
			m_buffer_message.Count = m_buffer_receiving.Offset - m_buffer_message.Offset;

			// 2) Hook 함수 호출
			on_receive(m_buffer_receiving, _args);

			// 3) process_packet을 호출한다.
			var required_bytes = process_packet(ref m_buffer_message);

			// check) 요구한 Message의 크기가 최대 Message buffer 크기보다 작아야 한다.
			Debug.Assert(required_bytes <= m_maximum_mesage_buffer_size);

			// check) 요구한 Message의 크기가 [m_maximum_mesage_buffer_size]보다 크면 Exception을 던진다.
			if(required_bytes > m_maximum_mesage_buffer_size)
				throw new System.Exception("message size is too big!");

			// 5) 받고자하는 Message의 크기보다 Message의 미수신 크기가 많으므로...(메모리를 새로 할당받는다.)
			if(required_bytes >= (m_buffer_receiving.capacity - m_buffer_message.Offset))
			{
				// - [필요_버퍼크기]를 구한다.(receive받은 크기 + 이미 받아 놓은 데이터 크기)
				var size_compare = m_socket.get_available() + m_buffer_message.size;

				// - [준비할_버퍼_크기]의 [필요_버퍼크기]의 최소 8의 승수로  만든다.(8,16,128, 1024, 8192, 64k, 256k, ...)
				//   (이렇게 8의 배수로 하는 이유는... 너무 단계를 많이 하게 될 경우 오히리 사용하는 메모리 버퍼 풀의 종류가 많아져
				//   각 풀에 재고가 너무 많이 남아 있어 메모리 사용량을 늘릴 수 있으므로 단계를 줄이기 위해서 8의 승수로
				//   단계를 줄여 사용하는 풀의 종류를 줄인다.)
				var size_prepare = m_minimum_mesage_buffer_size; while (size_compare > size_prepare) size_prepare *= 8;

				// - [최소_TCP_수신_버퍼_크기]보다는 크게 한다.
				if(size_prepare > m_maximum_mesage_buffer_size)
					size_prepare = m_maximum_mesage_buffer_size;

				// - 최소 [요구_버퍼_크기]+[이미_수신한_데이터_크기]보다는 크야 한다.
				size_prepare = Math.Max(size_prepare, required_bytes + m_buffer_message.size);

				// - 새로 계산한 메모리 크기만큼 Buffer를 할당받는다.
				CGDK.buffer	buffer_new = memory.alloc_buffer(size_prepare);

				// - 남아 있는 Byte가 있으면 복사한다.
				if(m_buffer_message.Count != 0)
				{
					// check) 복사하려는 길이가 할당받은 Buffer의 크기 보다 작아야 한다.
					Debug.Assert(buffer_new.capacity>m_buffer_message.Count);

					// check) 복사하려는 길이가 할당받은 Buffer의 크기 보다 작아야 한다.
					Debug.Assert(m_buffer_message.capacity>=(m_buffer_message.Offset+m_buffer_message.Count));

					// - 복사
					Buffer.BlockCopy(m_buffer_message.Array, m_buffer_message.Offset, buffer_new.Array, 0, m_buffer_message.Count);
				}
				buffer_new.Count = buffer_new.capacity - m_buffer_message.Count;
				buffer_new.Offset = m_buffer_message.Count;

				// - 기존 Buffer를 Pool로 되돌린다.
				memory.free(m_buffer_receiving.clear());

				// - 새로 받은 Buffer를 설정한다.
				m_buffer_receiving = buffer_new;

				m_buffer_message.Array = buffer_new.Array;
				m_buffer_message.Offset = 0;

				// check) 
				Debug.Assert(m_buffer_receiving.Count>0);
			}
		}

		protected virtual void			process_complete_receive(SocketAsyncEventArgs _args)
		{
			// check) NSocket State는 반드시 SOCKET_STATE_SYN이여야지 한다.
			Debug.Assert(m_socket.socket_state != eSOCKET_STATE.SYN);

			// 1) casting
			var args_receive = _args as SocketAsyncEventArgs_recv;

			// check) 전송 받은 Byte의 수가 0 Byte면 접속 종료처리를 한다.
			if (_args.BytesTransferred == 0)
			{
				// - 접속 종료처리를 수행한다.
				process_complete_disconnect();

				// - args 초기화 및 할당해제
				args_receive.clear();
				m_factory_async_args_receive.free(args_receive);

				// return) 
				return;
			}

			// statistics) socket의 Receivestatistics.
			statistics.statistics_on_receive_bytes(_args.BytesTransferred);

			// check) 전송결과 실패면 접속 종료처리를 한다.
			if(_args.SocketError != SocketError.Success)
			{
				// - 비정상 접속종료일 경우 REASON_FAIL을 설정한다.
				m_disconnect_reason	|= DISCONNECT_REASON.FAIL;

				// - 접속 종료처리를 수행한다.
				process_complete_disconnect();

				// - args 초기화 및 할당해제
				args_receive.clear();
				m_factory_async_args_receive.free(args_receive);

				// return) 
				return;
			}

			try
			{
				process_receive(_args);
			}
			catch(System.Exception)
			{
				// statistics) Error로 인한 Disconnect임을 표시한다.
				statistics.statistics_set_error_disconnect();

				// - Disconnect를 수행한다.
				process_complete_disconnect();

				// - args 초기화 및 할당해제
				args_receive.clear();
				m_factory_async_args_receive.free(args_receive);

				// return)
				return;
			}
        
			// 6) receive
			if (prepare_receive() == false)
			{
				// - Disconnect 처리를 한다.
				process_complete_disconnect();
			}

			// 7) args 초기화 및 할당해제
			args_receive.clear();
			m_factory_async_args_receive.free(args_receive);
		}
		protected virtual int			process_packet(ref CGDK.buffer _buffer)
		{
			// declare) 
			var count_message = 0;		// 처리한 패킷 수의 처리르 위해...

			// 1) 받은 Size
			var i_offset = m_buffer_message.Offset;
			var i_count = m_buffer_message.Count;
			var iMessageLength = 0;

			try
			{
				for(;;)
				{
					// 2) 먼저 Position을 넣는다.
					m_buffer_message.Offset = i_offset;

					// check) 남은 Byte수가 Message의 Head 크기보다 작으면 끝낸다.
					if(i_count < sizeof(uint))
					{
						// - MessageLength를 sizeof(uint)로...
						iMessageLength = sizeof(uint);

						// Break) 
						break;
					}

					// 3) Message 크기를 얻는다.
					iMessageLength = (int)m_buffer_message.get_front<uint>(0);

					// check) 접속이 종료되었으면 바로 Exception을 던진다.
					if(m_socket.socket_state!=eSOCKET_STATE.ESTABLISHED)
					{
						throw new OperationCanceledException("Excp) Message Parsing canceled by NSocket disconnection ["+m_socket.socket_state+"]");
					}

					// check) Message Head 크기보다 Message의 크기가 작으면 Exception을 던진다.
					if (iMessageLength<sizeof(uint))
					{
						// Throw) Exception을 던진다.
						throw new DecoderFallbackException("Excp) Message Size is less than Message Head Size (Size:"+sizeof(uint)+")");
					}

					// check) Message 크기보다 받아 데이터 크기가 적으면 잃단 끝낸다.
					if (iMessageLength>i_count)
					{
						// Break) 
						break;
					}

					// 4) Message 크기를 써넣는다.
					m_buffer_message.Count = iMessageLength;

					// declare)
					sMESSAGE msg = new sMESSAGE(eMESSAGE.SYSTEM.MESSAGE, this, m_buffer_message);

					// 5) Message를 처리한다.
					process_message(this, msg);

					// 6) Message의 수를 증가싴니다.
					++count_message;

					// 7) Message의 길이를 줄인다.                                        
					i_offset += iMessageLength;
					i_count -= iMessageLength;
				}
			}
			catch(System.Exception)
			{
				// statistiscs) 이때까지 처리된 Packet의 statistics처리를 한다.
				statistics.statistics_on_receive_message(count_message);

				// reraise) 
				throw;
			}

			// statistics) 
			statistics.statistics_on_receive_message(count_message);

			// 8) Buffer의 남은 길이를 설정한다.
			m_buffer_message.Offset = i_offset;
			m_buffer_message.Count = i_count;

			// return) 
			return iMessageLength;
		}
		public int						process_message(object _source, sMESSAGE _msg)
		{
			return on_message(_source, _msg);
		}

		public void						Dispose()
		{
			// 1) Dispose
			Dispose(true);

			// 2) GC
			GC.SuppressFinalize(this);
		}
		protected virtual void			Dispose(bool _is_dispose)
		{
			// check) 이미 Disposed되어 있으면 그냥 끝낸다.
			if (m_is_disposed)
				return;

			// 1) 내부 객체를 Dispose한다.
			if (_is_dispose) 
			{
				// - Reference Count는 반드시 0이어야 한다.
				Debug.Assert(reference_count==0);

				// - socket을 Dispose한다.
				m_socket.Dispose();
			}

			// 2) dispose를 true로...
			m_is_disposed = true;
		}

		protected class SocketAsyncEventArgs_send : SocketAsyncEventArgs
		{
			public	SocketAsyncEventArgs_send()
			{
				Completed += new EventHandler<SocketAsyncEventArgs>(tcp.complete_process_send);
			}

			public void clear()
			{
				// 1) Buffer 설정 초기화.
				count_message = 0;
				BufferList = null;
				SetBuffer(null, 0, 0);

				// 2) 모든 Buffer 할당해제
				foreach(var iter in list_buffers)
				{
					// check) 
					Debug.Assert(iter.Count != 0);

					// - Free한다.
					memory.free(iter.clear());
				}
				list_buffers.Clear();
				list_async_send.Clear();

				// 3) UserToken을 null로 초기화.
				UserToken = null;

				// check) 
				Debug.Assert(AcceptSocket == null);
			}
			public List<CGDK.buffer>		list_buffers = new List<CGDK.buffer>(Itcp.default_maximum_depth_of_sending_queue);
			public List<ArraySegment<byte>>	list_async_send = new List<ArraySegment<byte>>(Itcp.default_maximum_depth_of_sending_queue);
			public int						count_message;
		}
		protected class SocketAsyncEventArgs_recv : SocketAsyncEventArgs
		{
			public	SocketAsyncEventArgs_recv()
			{
				Completed += new EventHandler<SocketAsyncEventArgs>(tcp.complete_process_receive);
			}

			public void clear()
			{
				// 1) 모두 null로 초기화.
				SetBuffer(null, 0, 0);
				UserToken = null;

				// check) 
				Debug.Assert(BufferList == null);
				Debug.Assert(AcceptSocket == null);
			}
		}


	//  settings)
		// 1) send receive shared_buffer (TCP socket의 내부 send/receive shared_buffer 크기)
		public const int				MIN_TCP_SEND_buffer_SIZE				 = (              1024);
		public const int				MAX_TCP_SEND_buffer_SIZE				 = (   8 * 1024 * 1024);
		public const int				MIN_TCP_RECEIVE_buffer_SIZE				 = (          4 * 1024);
		public const int				MAX_TCP_RECEIVE_buffer_SIZE				 = (   8 * 1024 * 1024);
		public const int				MIN_TCP_MESSAGE_SIZE					 = (              1024);
		public const int				MAX_TCP_MESSAGE_SIZE					 = ( 512 * 1024 * 1024);

		// 2) for tcp receiveable
		public const int				DEFAULT_TCP_MESSAGE_buffer_SIZE			 = (          1 * 1024);
		public const int				DEFAULT_TCP_MESSAGE_buffer_SIZE_MAX		 = (         64 * 1024);
		public const int				DEFAULT_MAX_BYTES_OF_MESSAGE_RECEIVE_QUEUE=(  16 * 1024 * 1024);

		// 3) for tcp sender with gathering
		public const int				DEFAULT_MAX_DEPTH_OF_MESSAGE_SEND_QUEUE	 = (               64); // - 이 숫자는 반드시 2의 n승(2,4,8,16,32,64,128,256 ...)이여야 한다.
		public const int				DEFAULT_MAX_BYTES_OF_MESSAGE_SEND_QUEUE	 = (  16 * 1024 * 1024); // - 16MByte

		// 4) for udp
		public const int				DEFAULT_UDP_SEND_BUFFER_SIZE			 = (   8 * 1024 * 1024);
		public const int				DEFAULT_UDP_RECEIVE_BUFFER_SIZE			 = (   8 * 1024 * 1024);
		public const int				DEFAULT_UDP_COUNT_RECEIVE_BUFFER		 = 32;
		public const int				DEFAULT_UDP_MESSAGE_BUFFER_SIZE			 = (         64 * 1024);
		public const int				MIN_UDP_MESSAGE_SIZE					 = 4;
		public const int				MAX_UDP_MESSAGE_SIZE					 = (         64 * 1024);

	// implementations)
		// 1) tunning parameters
		public static int				m_default_maximum_receive_buffer_size	 = MAX_TCP_RECEIVE_buffer_SIZE;
		public static int				m_default_maximum_send_buffer_size		 = MAX_TCP_SEND_buffer_SIZE;
		public static int				m_default_minimum_mesage_buffer_size	 = DEFAULT_TCP_MESSAGE_buffer_SIZE;
		public static int				m_default_maximum_mesage_buffer_size	 = DEFAULT_UDP_MESSAGE_BUFFER_SIZE;
		public static int				m_default_maximum_depth_of_sending_queue = DEFAULT_MAX_DEPTH_OF_MESSAGE_SEND_QUEUE;

		// 2) socket status
		private io.NSocket				m_socket								 = new io.NSocket();
		private ulong					m_disconnect_reason						 = 0;
		private	bool					m_is_sending							 = false;
		private	Object					m_cs_send_queue							 = new Object();

		// 3) buffers
		private int						m_maximum_receive_buffer_size			 = default_maximum_receive_buffer_size;
		private int						m_maximum_send_buffer_size				 = default_maximum_send_buffer_size;
		private int						m_minimum_mesage_buffer_size			 = default_minimum_mesage_buffer_size;
		private int						m_maximum_mesage_buffer_size			 = default_maximum_mesage_buffer_size;
		private int						m_maximum_depth_of_sending_queue		 = default_maximum_depth_of_sending_queue;
		private	SocketAsyncEventArgs_send m_buffer_queued;
		private CGDK.buffer				m_buf_send;

		// 4)
		private CGDK.buffer				m_buffer_receiving;
		private CGDK.buffer				m_buffer_message;
		private static factory.Cobject<SocketAsyncEventArgs_send>   m_factroy_async_args_send	 = new factory.Cobject<SocketAsyncEventArgs_send>("AsyncEventArgsSend");
		private static factory.Cobject<SocketAsyncEventArgs_recv>	m_factory_async_args_receive = new factory.Cobject<SocketAsyncEventArgs_recv>("AsyncEventArgsReceive");

		// 5) connective
		private Iconnective				m_pconnective				 = null;

		// 6) statistics
		private io.statistics.Ntraffic	m_statistics_traffic		 = new io.statistics.Ntraffic();

		// 7) 
		private	bool					m_is_disposed				 = false;
	}
}