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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using CGDK;
using CGDK.Factory;
using CGDK.Net.Io;

//----------------------------------------------------------------------------
//
//  class CGDK.Net.Socket.ITcp
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net.Socket
{
	public class ITcp :
        Nreferenceable,
        Io.IConnectable,
        IMessageable, 
		Io.ISenderStream,
        IDisposable
	{
	// public)
		public bool						Send(CGDK.buffer _buffer)
		{
			return	ProcessRequestSend(_buffer, 1);
		}
		public	IConnective				Connective
		{
			get { return this.m_pconnective; }
			set { this.m_pconnective=value;}
		}
		public Io.Statistics.Ntraffic	Statistics
		{
			get { return this.m_statistics_traffic; }
		}
		public bool						CloseSocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// check) disconnect_reason_MASK에 해당하는 부분의 flag를 유저가 설정해서는 안됀다.
			Debug.Assert((_disconnect_reason & DISCONNECT_REASON.MASK) == 0);

			// 1) add active Disconnect flag
			_disconnect_reason &= ~DISCONNECT_REASON.MASK;
			_disconnect_reason |= DISCONNECT_REASON.ACTIVE;
			_disconnect_reason |= DISCONNECT_REASON.ABORTIVE;

			// 2) CloseSocket
			return this.ProcessCloseSocket(_disconnect_reason);
		}
		public virtual bool				Disconnect(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// 1) set Socket send_disconnect
			var pre_socket_status = this.m_socket.SetSocketStateIf(eSOCKET_STATE.ESTABLISHED, eSOCKET_STATE.SEND_DISCONNECTED);

			// check)
			if(pre_socket_status != eSOCKET_STATE.ESTABLISHED)
				return false;

			// check) disconnect_reason_MASK에 해당하는 부분의 flag를 유저가 설정해서는 안됀다.
			Debug.Assert((_disconnect_reason & DISCONNECT_REASON.MASK) == 0);

			// 2) add active Disconnect flag
			_disconnect_reason &= ~DISCONNECT_REASON.MASK;
			_disconnect_reason |= DISCONNECT_REASON.ACTIVE;

			// 3) set Disconnect reason flag
			this.m_disconnect_reason |= _disconnect_reason;

			// 4) Shutdown if not sending
			lock (this.m_cs_send_queue)
			{
				if(this.m_is_sending != true)
				{
					// - set linger option as graceful close
					this.m_socket.SetLingerOptionGraceful();

					// - Shutdown
					return this.m_socket.Shutdown();
				}
			}

			// return)
			return true;
		}
		public Io.NSocket				Socket
		{
			get { return this.m_socket;}
		}
		public ulong					DisconnectReason 
		{
			get { return this.m_disconnect_reason; }
			set { this.m_disconnect_reason = value; }
		}

		public int						MaximumReceiveBufferSize			{ get { return this.m_maximum_receive_buffer_size; } set { this.m_maximum_receive_buffer_size = value; } }
		public int						MaximumSendBufferSize				{ get { return this.m_maximum_send_buffer_size; } set { this.m_maximum_send_buffer_size = value; } }
		public int						MinimumMesageBufferSize				{ get { return this.m_minimum_mesage_buffer_size; } set { this.m_minimum_mesage_buffer_size = value; } }
		public int						MaximumMesageBufferSize				{ get { return this.m_maximum_mesage_buffer_size; } set { this.m_maximum_mesage_buffer_size = value; } }
		public int						MaxDepthOfSendingQueue				{ get { return this.m_maximum_depth_of_sending_queue; } set { this.m_maximum_depth_of_sending_queue = value; } }

		public static int				DefaultMaximumReceiveBufferSize		{ get { return m_default_maximum_receive_buffer_size; } set { m_default_maximum_receive_buffer_size = value; } }
		public static int				DefaultMaximumSendBufferSize		{ get { return m_default_maximum_send_buffer_size; } set { m_default_maximum_send_buffer_size = value; } }
		public static int				DefaultMinimumMesageBufferSize		{ get { return m_default_minimum_mesage_buffer_size; }	set { m_default_minimum_mesage_buffer_size = value; } }
		public static int				DefaultMaximumMesageBufferSize		{ get { return m_default_maximum_mesage_buffer_size; }	set { m_default_maximum_mesage_buffer_size = value; } }
		public static int				DefaultMaximumDepthOfSendingQueue	{ get { return m_default_maximum_depth_of_sending_queue; } set { m_default_maximum_depth_of_sending_queue = value; } }

		// framework)
		protected virtual void			OnConnect() {}
		protected virtual void			OnFailConnect(ulong _disconnect_reason) {}
		protected virtual void			OnDisconnect(ulong _disconnect_reason) {}
		protected virtual void			OnReceive(CGDK.buffer _buffer_received, SocketAsyncEventArgs _args) {}
		protected virtual int			OnMessage(object _source, sMESSAGE _msg) { return 0;}
		protected override void			OnFinalRelease()
		{
			lock (this.m_cs_send_queue)
			{
				// check) Reference Count는 0이어야 한다.
				Debug.Assert(ReferenceCount == 0);

				// check) 현재 전송중이면 안됀다.
				Debug.Assert(this.m_is_sending == false);

				// check) m_handle_socket은 무조건 null인 상태로 Release되어야 한다.
				Debug.Assert(this.m_socket.SocketHandle == null);

				// 1) RECEIVE- Buffer를 할당해제한다.
				if(!this.m_buffer_receiving.IsEmpty())
				{
					Memory.Free(this.m_buffer_receiving.Clear());
				}

				// 2) RECEIVE-Receive용 Message에 설정해 놓은 Buffer를 제거.
				this.m_buffer_message.Array = null;

				// 3) SEND-Queued된 것 할당해제
				if(this.m_buffer_queued != null)
				{
					this.m_buffer_queued.Clear();

					m_factroy_async_args_send.Free(this.m_buffer_queued);

					this.m_buffer_queued = null;
				}

				// 4) Send Buffer를 할당해제한다.
				if(this.m_buf_send.IsEmpty() == false)
				{
					Memory.Free(this.m_buf_send.Clear());
				}
			}
		}

	// implementation)
		protected void					ProcessPrepareSocket(AddressFamily _address_family)
		{
			// 1) Socket을 생성한다.
			this.m_socket.CreateSocketHandle(_address_family, SocketType.Stream, ProtocolType.Tcp);
		}
		protected bool					ProcessCloseSocket(ulong _disconnect_reason = DISCONNECT_REASON.NONE)
		{
			// 1) set Disconnect reasone flag
			m_disconnect_reason	|= _disconnect_reason;

			// 2) set linger option
			if((_disconnect_reason & (DISCONNECT_REASON.FAIL | DISCONNECT_REASON.ABORTIVE)) != DISCONNECT_REASON.NONE)
				m_socket.SetLingerOptionAbortive();
			else
				m_socket.SetLingerOptionGraceful();

			// 3) close Socket handle
			return m_socket.CloseSocketHandle();
		}

		public virtual bool				ProcessCompleteConnect(SocketAsyncEventArgs _args)
		{
			// check) 접속 성공여부를 확인한다.
			if (_args.SocketError != SocketError.Success)
			{
				// - NSocket Status를 Disconnect로
				this.m_socket.SocketState = eSOCKET_STATE.CLOSED;

				// - OnFailConnect함수를 호출한다.
				try
				{
					this.OnFailConnect(0);
				}
				catch(System.Exception)
				{
				}

				// - Socket을 닫는다.
				this.ProcessCloseSocket(DISCONNECT_REASON.ACTIVE | DISCONNECT_REASON.ABORTIVE);

				// return) 끝!
				return false;
			}

			// 1) NSocket 상태를 eSOCKET_STATE.ESTABLISHED로 바꾼다.
			this.m_socket.SetSocketStateIf(eSOCKET_STATE.SYN, eSOCKET_STATE.ESTABLISHED);

			// Statistics) 
			this.Statistics.statistics_on_connect();

			// reference counting)
			this.AddReference();

			try
			{
				lock(m_cs_send_queue)
				{
					// 1) 임시로 복사한다.
					this.m_socket.SetReceiveBufferSize(MaximumReceiveBufferSize);
					this.m_socket.SetSendBufferSize(MaximumSendBufferSize);

					// 2) prepare for receive
					this.m_buffer_receiving = new CGDK.buffer(Memory.AllocMemory(this.m_minimum_mesage_buffer_size), 0, this.m_minimum_mesage_buffer_size);
					this.m_buffer_message = new CGDK.buffer(this.m_buffer_receiving.Array, 0, 0);

					// 3) prepare sending
					Debug.Assert(this.m_is_sending == false);
					this.m_buffer_queued = m_factroy_async_args_send.Alloc();
					this.m_buffer_queued.SocketFlags = SocketFlags.None;
					this.m_buffer_queued.SocketError = SocketError.Success;
					//this.m_buffer_queued.SendPacketsFlags = TransmitFileOptions.UseDefaultWorkerThread;
					this.m_buffer_queued.UserToken = this;

					// 4) Alloc buffer for sending
					this.m_buf_send = Memory.AllocBuffer(this.m_maximum_send_buffer_size);

					// check) 
					Debug.Assert(this.m_buffer_queued.list_buffers.Count == 0);
					Debug.Assert(this.m_buffer_queued.list_async_send.Count == 0);
					Debug.Assert(this.m_buffer_queued.count_message == 0);
					Debug.Assert(this.m_buffer_queued.Buffer == null);
					Debug.Assert(this.m_buffer_queued.BufferList == null);
				}

				// 5) OnConnect함수를 호출한다. 
				//    * 주의: Receive거는 것보다 NotifyOnConnect를 먼저해야 한다!
				//            만약 Receive를 먼저 걸게 될 경우 다른 Trhead로 CompeteReceive가 수행될 수 있기 때문이다.
				this.OnConnect();

				// 6) call 'PrepareReceive'
				if(this.PrepareReceive() == false)
				{
					// - ProcessDisconnect한다.
					this.ProcessCompleteDisconnect();
				}
			}
			catch(System.Exception)
			{
				// Statistics)
				this.Statistics.statistics_set_error_disconnect();

				// - Socket을 닫는다.
				this.ProcessCloseSocket(DISCONNECT_REASON.ACTIVE | DISCONNECT_REASON.ABORTIVE);

				// - Connective에...
				this.ProcessDisconnect();

				// Reference Counting) 
				this.Release();
			}

			// return) 성공!!!
			return true;
		}
		public void						ProcessCompleteDisconnect()
		{
			// 1) Closed상태로 NSocket State를 바꾸고 이전 State를 얻어온다.
			var	temp_socket_status = this.m_socket.ExchangeSocketState(eSOCKET_STATE.CLOSED);

			// check) Socket이 이미 Closed상태라면 돌려보낸다.
			if(temp_socket_status == eSOCKET_STATE.CLOSED)
			{
				return;
			}

			// declare) 
			var socket_disconnect_reason = this.m_disconnect_reason;

			// 2) ProcessDisconnect를 부른다.
			try
			{
				this.OnDisconnect(socket_disconnect_reason);
			}
			catch(System.Exception /*e*/)
			{
			}

			// Statistics) Statistics.OnDisconnect
			this.Statistics.StatisticsOnDisconnect();

			// 3) 강제 종료를 진행한다.
			this.ProcessCloseSocket();

			// 4) 바로 Disconnect Completion을 처리해 준다.
			this.ProcessDisconnect();

			// Reference Counting)
			this.Release();
		}
		protected virtual void			ProcessDisconnect()
		{
			// 1) Connective의 ProcessConnectiveDisconnect를 호출한다.
			this.m_pconnective?.ProcessConnectiveDisconnect(this);

			// Statistics) 
			Io.Statistics.Nconnective.total.StatisticsOnDisconnect();
		}

		private bool					ProcessSend(SocketAsyncEventArgs_send _args)
		{
			try
			{
				// check) Socket이 Established 상태가 아니면 false를 리턴한다.
				if(this.m_socket.SocketState != eSOCKET_STATE.ESTABLISHED)
					return false;

				// 1) 비동기 전송한다.
				if(this.m_socket.SendAsync(_args) == false)
				{
					// return) 
					return false;
				}

				// Statistics) 
				this.Statistics.statistics_on_send_async();
			}
			catch(System.Exception )
			{
				// return) 
				return false;
			}

			// return) 
			return true;
		}
		public bool						ProcessRequestSend(CGDK.buffer _buffer, int _count_message)
		{
			// check) Buffer 검사!
			Debug.Assert(_buffer.Array != null);
			Debug.Assert((_buffer.Offset + _buffer.Count) <= _buffer.Capacity);
			Debug.Assert(_buffer.Count > 0);

			// check) 전송량이 0이면 애초부터 Return해버린다.
			if(_buffer.Count <= 0)
				return false;

			// Reference Count) SEND
			this.AddReference();

			// declare) 
			var buf_pre = new CGDK.buffer(null, 0, 0);
			CGDK.buffer buf_send;

			lock (this.m_cs_send_queue)
			{
				// check) Socket이 Established상태가 아니라면 전송은 안됀다.
				if(this.m_socket.SocketState < eSOCKET_STATE.ESTABLISHED)
				{
					// Reference Count) SEND
					this.Release();

					// return) 
					return false;
				}

				// check) sending queue의 최대 Depth까지 차있으면 falsee를 return한다.
				if(this.m_buffer_queued.list_buffers.Count >= this.m_maximum_depth_of_sending_queue)
				{
					// reference count) SEND
					this.Release();

					// return) 
					return false;
				}

				// 2) 남은 버퍼의 크기가 모자라면 새로 버퍼를  할당한다.
				if(this.m_buf_send.RemainedSize < _buffer.Count)
				{
					// - 기존 Buffer를 저장해 놓음
					if(this.m_buf_send.Count != 0)
					{
						buf_pre = this.m_buf_send;
					}

					// - Buffer를 새로 할당받음.
					this.m_buf_send = Memory.AllocBuffer(Math.Max(_buffer.Count * 2, m_maximum_send_buffer_size));
				}

				// 3) Buffer를 새로 할당받는다.(C#은 Reference Counting이 불가능하기 때문에 어쩔수 없이 전송시 마다 복사를 할 수 밖에 없다.)
				this.m_buf_send.Append(_buffer.Array, _buffer.Offset, _buffer.Count);

				// 4) 전송 중이면 Queuing만 하고 그냥 끝낸다.
				if(this.m_is_sending == true)
				{
					// - list에 Buffer를 추가한다.(기본 Buffer에 데이터가 없으면 Queuing하지 않는다.)
					if(!buf_pre.IsEmpty())
					{
						// check) buf_pre.Array는 null이어서는 안됀다.
						Debug.Assert(buf_pre.Count != 0);

						// - 추가한다.
						this.m_buffer_queued.list_buffers.Add(buf_pre);
					}

					// check)
					Debug.Assert(this.m_buf_send.Count != 0);

					// - Message의 수를 더한다.
					this.m_buffer_queued.count_message += _count_message;

					// Reference Count) SEND
					Release();

					// return) 그냥 끝낸다.
					return true;
				}

				//Check) 전송중이 buf_pre의 i_count는 반드시 0이어야 한다.
				Debug.Assert(buf_pre.IsEmpty() || buf_pre.Count == 0);

				// 5) Buffer를 옮겨놓는다.
				buf_send = this.m_buf_send;

				// 6) Count만큼 Extract한다.
				this.m_buf_send.Extract(this.m_buf_send.Count);

				// 7) Sending을 true로...
				this.m_is_sending = true;

				// check) 
				Debug.Assert(this.m_buffer_queued.list_buffers.Count == 0);
				Debug.Assert(this.m_buffer_queued.list_async_send.Count == 0);
				Debug.Assert(this.m_buffer_queued.count_message == 0);
			}

			// 8) Sending Buffer는 할당해제한다.
			if(!buf_pre.IsEmpty())
			{
				// check) Prebuffer는 반드시 크기가 0이어야 한다.
				Debug.Assert(buf_pre.Count==0);

				// - 할당해제한다.
				Memory.Free(buf_pre.Clear());
			}
		
			// 9) Send용 AsyncArgs를 할당받는다.
			SocketAsyncEventArgs_send args_send = m_factroy_async_args_send.Alloc();

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
			var result = ProcessSend(args_send);

			// check) 
			if(result)
				return true;

			// 11) get result
			result = (args_send.SocketError == SocketError.Success);

			// 12) ...
			ProcessCompleteSend(args_send);

			// 13) Release
			Release();

			// return) 
			return result;
		}
		private static void				CompleteProcessSend(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_send = _args as SocketAsyncEventArgs_send;
			var socket_tcp = _args.UserToken as ITcp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_tcp != null);

			// 1) ProcessCompleteSend를 호출한다.
			try
			{
				socket_tcp.ProcessCompleteSend(args_send);
			}
			catch(System.Exception)
			{
			}

			// Reference Counting) 
			socket_tcp.Release();
		}
		protected virtual void			ProcessCompleteSend(SocketAsyncEventArgs_send _args)
		{
			// check) 실패했으면 끝낸다...
			if(_args.SocketError != SocketError.Success)
			{
				// 1) Sending Buffer를 Clear한다.
				lock(this.m_cs_send_queue)
				{
					// - 전송 상태를 false로..
					this.m_is_sending = false;

					// - Queued를...
					m_buffer_queued.Clear();
				}

				// 2) args 초기화 및 할당해제
				_args.Clear();
				m_factroy_async_args_send.Free(_args);

				// return) 
				return;
			}

			// Statistics)
			this.Statistics.statistics_on_send_bytes(_args.BytesTransferred);
	
			// Statistics)
			this.Statistics.statistics_on_send_message(_args.count_message);

			// 4) Clear (여기서 BufferList의 제일 마지막은 사용중인 것이므로 FREE해서는 안됀다.)
			_args.Clear();

			// declare)
			var args_pre = _args;

			while(true)
			{
				// declare) 
				SocketAsyncEventArgs_send args_send = null;
				CGDK.buffer buf_temp;

				lock (this.m_cs_send_queue)
				{
					// check) 
					Debug.Assert(this.m_is_sending == true);

					// 2) Buffer에 Queuing된 것이 없으면 m_is_sending을 false로만 하고 끝낸다.
					if (this.m_buf_send.Count == 0)
					{
						// check) 이 Buffer는 반드시 0이여야 한다.
						Debug.Assert(this.m_buffer_queued.list_buffers.Count == 0);

						// - 전송 상태를 false로..
						this.m_is_sending = false;

						// - Shutdown if send_disconnected
						if (this.m_socket.SocketState == eSOCKET_STATE.SEND_DISCONNECTED)
						{
							// - set linger Option for graceful Disconnect
							this.m_socket.SetLingerOptionGraceful();

							// - Send Shutdown
							this.m_socket.Shutdown();
						}

						// return) 
						return;
					}

					// 3) 기존 Buffer를 복사해놓는다.
					buf_temp = this.m_buf_send;

					// 4) 기존 Buffer를 Extract한다.
					this.m_buf_send.Extract(this.m_buf_send.Count);

					// 5) 기존 Args를 저장해 놓는다.
					args_send = this.m_buffer_queued;

					// 6) 기존에 쓰든 Arg를 그대로 쓴다.
					this.m_buffer_queued = args_pre;

					// check) bufferQueue의 값이 제대로 Reset되어 있는지 확인한다.
					Debug.Assert(this.m_buffer_queued.list_buffers.Count == 0);
					Debug.Assert(this.m_buffer_queued.list_async_send.Count == 0);
					Debug.Assert(this.m_buffer_queued.count_message == 0);
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
				this.AddReference();

				// 9) 전송을 수행한다.
				var is_raise_event = this.ProcessSend(args_send);

				// check) 
				if(is_raise_event)
				{
					break;
				}

				// check) failed
				if(args_send.SocketError != SocketError.Success)
				{
  					// Statistics) 
					this.Statistics.statistics_on_error_send();

					lock(this.m_cs_send_queue)
					{
						// - 전송 상태를 false로..
						this.m_is_sending = false;

						// - Queued를...
						this.m_buffer_queued.Clear();
						this.m_buf_send.Count = 0;

						// - Shutdown if send_disconnected
						if (this.m_socket.SocketState == eSOCKET_STATE.SEND_DISCONNECTED)
						{
							// - set linger Option for graceful Disconnect
							this.m_socket.SetLingerOptionGraceful();

							// - Send Shutdown
							this.m_socket.Shutdown();
						}
					}

					// - Clear args_send
					args_send.Clear();

					// - Free args_send
					m_factroy_async_args_send.Free(args_send);

					// Reference Count) 
					this.Release();

					// - 
					break;
				}

				// Statistics)
				this.Statistics.statistics_on_send_bytes(args_send.BytesTransferred);
	
				// Statistics)
				this.Statistics.statistics_on_send_message(args_send.count_message);

				// 10) Clear args_send
				args_send.Clear();

				// 11) store to pre
				args_pre = args_send;

				// Reference Count) 
				this.Release();
			}
		}

		private bool					PrepareReceive()
		{
			// check)
			if(this.m_socket.SocketState != eSOCKET_STATE.ESTABLISHED)
			{
				// return) 
				return false;
			}

			// check) Receiving Buffer가 0보다 작으면 끝낸다.
			if(this.m_buffer_receiving.Count <= 0)
			{
				// return) 
				return false;
			}

			// 1) 할당
			var args_receive = m_factory_async_args_receive.Alloc();

			// referece counting)
			this.AddReference();

			// 3) Receive를 다시 건다.
			while(true)
			{
				// check) 
				Debug.Assert(this.m_buffer_receiving.Count > 0);
				Debug.Assert((this.m_buffer_receiving.Offset + this.m_buffer_receiving.Count) == this.m_buffer_receiving.Capacity);

				// 2) 먼저 Receive를 건다.
				args_receive.UserToken = this;
				args_receive.BufferList = null;
				args_receive.SocketFlags = SocketFlags.None;
				args_receive.SocketError = SocketError.Success;
				args_receive.SetBuffer(this.m_buffer_receiving.Array, this.m_buffer_receiving.Offset, this.m_buffer_receiving.Count);   // Buffer, Offset, Count

				// 1) ReceiveAsync 함수를 호출한다.
				var result = this.m_socket.ReceiveAsync(args_receive);

				// check) 
				if(result == true)
					break;

				// check) 
				if(args_receive.SocketError != SocketError.Success || args_receive.BytesTransferred == 0)
				{
					// Statistics)
					this.Statistics.statistics_set_error_disconnect();

					// - Clear and deallocate
					args_receive.Clear();
					m_factory_async_args_receive.Free(args_receive);

					// referece counting)
					this.Release();

					// return)
					return false;
				}
				
				try
				{
					// Statistics)
					this.Statistics.statistics_on_receive_bytes(args_receive.BytesTransferred);

					// - process receive
					this.ProcessReceive(args_receive);
				}
				catch(System.Exception)
				{
					// Statistics)
					this.Statistics.statistics_set_error_disconnect();

					// - Clear and deallocate
					args_receive.Clear();
					m_factory_async_args_receive.Free(args_receive);

					// referece counting)
					this.Release();

					// return)
					return false;
				}
			}

			Statistics.statistics_on_receive_async();

			// return) 
			return true;
		}
		private static void				CompleteProcessReceive(object _source, SocketAsyncEventArgs _args)
		{
			// declare) 
			var	args_receive = _args as SocketAsyncEventArgs_recv;
			var socket_tcp = _args.UserToken as ITcp;

			// check) 
			Debug.Assert(_args.UserToken != null);
			Debug.Assert(socket_tcp != null);

			// 1) ProcessCompleteSend를 호출한다.
			try
			{
				socket_tcp.ProcessCompleteReceive(args_receive);
			}
			catch(System.Exception)
			{
			}

			// reference counting) 
			socket_tcp.Release();
		}
		protected void					ProcessReceive(SocketAsyncEventArgs _args)
		{
			// 1) 전송받은 크기를 더하고 처리되지 않은 Message의 크기를 구한다.
			this.m_buffer_receiving.Offset += _args.BytesTransferred;
			this.m_buffer_receiving.Count -= _args.BytesTransferred;
			this.m_buffer_message.Count = this.m_buffer_receiving.Offset - this.m_buffer_message.Offset;

			// 2) Hook 함수 호출
			OnReceive(m_buffer_receiving, _args);

			// 3) ProcessPacket을 호출한다.
			var required_bytes = this.ProcessPacket(ref this.m_buffer_message);

			// check) 요구한 Message의 크기가 최대 Message buffer 크기보다 작아야 한다.
			Debug.Assert(required_bytes <= this.m_maximum_mesage_buffer_size);

			// check) 요구한 Message의 크기가 [m_maximum_mesage_buffer_size]보다 크면 Exception을 던진다.
			if(required_bytes > this.m_maximum_mesage_buffer_size)
				throw new System.Exception("message Size is too big!");

			// 5) 받고자하는 Message의 크기보다 Message의 미수신 크기가 많으므로...(메모리를 새로 할당받는다.)
			if(required_bytes >= (this.m_buffer_receiving.Capacity - this.m_buffer_message.Offset))
			{
				// - [필요_버퍼크기]를 구한다.(receive받은 크기 + 이미 받아 놓은 데이터 크기)
				var size_compare = this.m_socket.GetAvailable() + this.m_buffer_message.Count;

				// - [준비할_버퍼_크기]의 [필요_버퍼크기]의 최소 8의 승수로  만든다.(8,16,128, 1024, 8192, 64k, 256k, ...)
				//   (이렇게 8의 배수로 하는 이유는... 너무 단계를 많이 하게 될 경우 오히리 사용하는 메모리 버퍼 풀의 종류가 많아져
				//   각 풀에 재고가 너무 많이 남아 있어 메모리 사용량을 늘릴 수 있으므로 단계를 줄이기 위해서 8의 승수로
				//   단계를 줄여 사용하는 풀의 종류를 줄인다.)
				var size_prepare = this.m_minimum_mesage_buffer_size; while (size_compare > size_prepare) size_prepare *= 8;

				// - [최소_TCP_수신_버퍼_크기]보다는 크게 한다.
				if(size_prepare > this.m_maximum_mesage_buffer_size)
					size_prepare = this.m_maximum_mesage_buffer_size;

				// - 최소 [요구_버퍼_크기]+[이미_수신한_데이터_크기]보다는 크야 한다.
				size_prepare = Math.Max(size_prepare, required_bytes + this.m_buffer_message.Count);

				// - 새로 계산한 메모리 크기만큼 Buffer를 할당받는다.
				CGDK.buffer	buffer_new = Memory.AllocBuffer(size_prepare);

				// - 남아 있는 Byte가 있으면 복사한다.
				if(this.m_buffer_message.Count != 0)
				{
					// check) 복사하려는 길이가 할당받은 Buffer의 크기 보다 작아야 한다.
					Debug.Assert(buffer_new.Capacity>this.m_buffer_message.Count);

					// check) 복사하려는 길이가 할당받은 Buffer의 크기 보다 작아야 한다.
					Debug.Assert(this.m_buffer_message.Capacity>=(this.m_buffer_message.Offset + this.m_buffer_message.Count));

					// - 복사
					Buffer.BlockCopy(this.m_buffer_message.Array, this.m_buffer_message.Offset, buffer_new.Array, 0, this.m_buffer_message.Count);
				}
				buffer_new.Count = buffer_new.Capacity - this.m_buffer_message.Count;
				buffer_new.Offset = this.m_buffer_message.Count;

				// - 기존 Buffer를 Pool로 되돌린다.
				Memory.Free(this.m_buffer_receiving.Clear());

				// - 새로 받은 Buffer를 설정한다.
				this.m_buffer_receiving = buffer_new;

				this.m_buffer_message.Array = buffer_new.Array;
				this.m_buffer_message.Offset = 0;

				// check) 
				Debug.Assert(this.m_buffer_receiving.Count>0);
			}
		}

		protected virtual void			ProcessCompleteReceive(SocketAsyncEventArgs _args)
		{
			// check) NSocket State는 반드시 SOCKET_STATE_SYN이여야지 한다.
			Debug.Assert(m_socket.SocketState != eSOCKET_STATE.SYN);

			// 1) casting
			var args_receive = _args as SocketAsyncEventArgs_recv;

			// check) 전송 받은 Byte의 수가 0 Byte면 접속 종료처리를 한다.
			if (_args.BytesTransferred == 0)
			{
				// - 접속 종료처리를 수행한다.
				this.ProcessCompleteDisconnect();

				// - args 초기화 및 할당해제
				args_receive.Clear();
				m_factory_async_args_receive.Free(args_receive);

				// return) 
				return;
			}

			// Statistics) Socket의 Receivestatistics.
			this.Statistics.statistics_on_receive_bytes(_args.BytesTransferred);

			// check) 전송결과 실패면 접속 종료처리를 한다.
			if(_args.SocketError != SocketError.Success)
			{
				// - 비정상 접속종료일 경우 REASON_FAIL을 설정한다.
				this.m_disconnect_reason |= DISCONNECT_REASON.FAIL;

				// - 접속 종료처리를 수행한다.
				this.ProcessCompleteDisconnect();

				// - args 초기화 및 할당해제
				args_receive.Clear();
				m_factory_async_args_receive.Free(args_receive);

				// return) 
				return;
			}

			try
			{
				this.ProcessReceive(_args);
			}
			catch(System.Exception)
			{
				// Statistics) Error로 인한 Disconnect임을 표시한다.
				this.Statistics.statistics_set_error_disconnect();

				// - Disconnect를 수행한다.
				this.ProcessCompleteDisconnect();

				// - args 초기화 및 할당해제
				args_receive.Clear();
				m_factory_async_args_receive.Free(args_receive);

				// return)
				return;
			}
        
			// 6) receive
			if (this.PrepareReceive() == false)
			{
				// - Disconnect 처리를 한다.
				this.ProcessCompleteDisconnect();
			}

			// 7) args 초기화 및 할당해제
			args_receive.Clear();
			m_factory_async_args_receive.Free(args_receive);
		}
		protected virtual int			ProcessPacket(ref CGDK.buffer _buffer)
		{
			// declare) 
			var count_message = 0; // 처리한 패킷 수의 처리르 위해...

			// 1) 받은 Size
			var i_offset = this.m_buffer_message.Offset;
			var i_count = this.m_buffer_message.Count;
			int iMessageLength;

			try
			{
				for(;;)
				{
					// 2) 먼저 Position을 넣는다.
					this.m_buffer_message.Offset = i_offset;

					// check) 남은 Byte수가 Message의 Head 크기보다 작으면 끝낸다.
					if(i_count < sizeof(uint))
					{
						// - MessageLength를 sizeof(uint)로...
						iMessageLength = sizeof(uint);

						// Break) 
						break;
					}

					// 3) Message 크기를 얻는다.
					iMessageLength = (int)this.m_buffer_message.GetFront<uint>(0);

					// check) 접속이 종료되었으면 바로 Exception을 던진다.
					if(this.m_socket.SocketState!=eSOCKET_STATE.ESTABLISHED)
						throw new OperationCanceledException("Excp) Message Parsing canceled by NSocket disconnection ["+m_socket.SocketState+"]");

					// check) Message Head 크기보다 Message의 크기가 작으면 Exception을 던진다.
					if (iMessageLength<sizeof(uint))
						throw new DecoderFallbackException("Excp) Message Size is less than Message Head Size (Size:"+sizeof(uint)+")");

					// check) Message 크기보다 받아 데이터 크기가 적으면 잃단 끝낸다.
					if (iMessageLength>i_count)
						break;

					// 4) Message 크기를 써넣는다.
					this.m_buffer_message.Count = iMessageLength;

					// declare)
					sMESSAGE msg = new(eMESSAGE.SYSTEM.MESSAGE, this, m_buffer_message);

					// 5) Message를 처리한다.
					this.ProcessMessage(this, msg);

					// 6) Message의 수를 증가싴니다.
					++count_message;

					// 7) Message의 길이를 줄인다.                                        
					i_offset += iMessageLength;
					i_count -= iMessageLength;
				}
			}
			catch(System.Exception)
			{
				// statistiscs) 이때까지 처리된 Packet의 Statistics처리를 한다.
				Statistics.statistics_on_receive_message(count_message);

				// reraise) 
				throw;
			}

			// Statistics) 
			Statistics.statistics_on_receive_message(count_message);

			// 8) Buffer의 남은 길이를 설정한다.
			this.m_buffer_message.Offset = i_offset;
			this.m_buffer_message.Count = i_count;

			// return) 
			return iMessageLength;
		}
		public int						ProcessMessage(object _source, sMESSAGE _msg)
		{
			return this.OnMessage(_source, _msg);
		}

		public void						Dispose()
		{
			// 1) Dispose
			this.Dispose(true);

			// 2) GC
			GC.SuppressFinalize(this);
		}
		protected virtual void			Dispose(bool _is_dispose)
		{
			// check) 이미 Disposed되어 있으면 그냥 끝낸다.
			if (this.m_is_disposed)
				return;

			// 1) 내부 객체를 Dispose한다.
			if (_is_dispose) 
			{
				// - Reference Count는 반드시 0이어야 한다.
				Debug.Assert(ReferenceCount==0);

				// - Socket을 Dispose한다.
				this.m_socket.Dispose();
			}

			// 2) dispose를 true로...
			this.m_is_disposed = true;
		}

		protected class SocketAsyncEventArgs_send : SocketAsyncEventArgs
		{
			public	SocketAsyncEventArgs_send()
			{
				Completed += new EventHandler<SocketAsyncEventArgs>(Tcp.CompleteProcessSend);
			}

			public void Clear()
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
					Memory.Free(iter.Clear());
				}
				list_buffers.Clear();
				list_async_send.Clear();

				// 3) UserToken을 null로 초기화.
				UserToken = null;

				// check) 
				Debug.Assert(AcceptSocket == null);
			}
			public List<CGDK.buffer>		list_buffers = new(ITcp.DefaultMaximumDepthOfSendingQueue);
			public List<ArraySegment<byte>>	list_async_send = new(ITcp.DefaultMaximumDepthOfSendingQueue);
			public int						count_message;
		}
		protected class SocketAsyncEventArgs_recv : SocketAsyncEventArgs
		{
			public	SocketAsyncEventArgs_recv()
			{
				Completed += new EventHandler<SocketAsyncEventArgs>(Tcp.CompleteProcessReceive);
			}

			public void Clear()
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
		// 1) Send receive shared_buffer (TCP Socket의 내부 Send/receive shared_buffer 크기)
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

		// 2) Socket status
		private Io.NSocket				m_socket								 = new();
		private ulong					m_disconnect_reason						 = 0;
		private	bool					m_is_sending							 = false;
		private	Object					m_cs_send_queue							 = new();

		// 3) buffers
		private int						m_maximum_receive_buffer_size			 = DefaultMaximumReceiveBufferSize;
		private int						m_maximum_send_buffer_size				 = DefaultMaximumSendBufferSize;
		private int						m_minimum_mesage_buffer_size			 = DefaultMinimumMesageBufferSize;
		private int						m_maximum_mesage_buffer_size			 = DefaultMaximumMesageBufferSize;
		private int						m_maximum_depth_of_sending_queue		 = DefaultMaximumDepthOfSendingQueue;
		private	SocketAsyncEventArgs_send m_buffer_queued;
		private CGDK.buffer				m_buf_send;

		// 4)
		private CGDK.buffer				m_buffer_receiving;
		private CGDK.buffer				m_buffer_message;
		private static Factory.Object<SocketAsyncEventArgs_send> m_factroy_async_args_send  = new("AsyncEventArgsSend");
		private static Factory.Object<SocketAsyncEventArgs_recv> m_factory_async_args_receive = new("AsyncEventArgsReceive");

		// 5) Connective
		private IConnective				m_pconnective				 = null;

		// 6) Statistics
		private Io.Statistics.Ntraffic	m_statistics_traffic		 = new();

		// 7) 
		private	bool					m_is_disposed				 = false;
	}
}