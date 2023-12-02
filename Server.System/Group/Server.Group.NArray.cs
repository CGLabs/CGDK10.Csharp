//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Group Template Classes                           *
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
using System.Diagnostics;

namespace CGDK.Server.Group
{
	public class NArray<TMEMBER> : 
		IGroup<TMEMBER> 
		where TMEMBER : class
	{
	// constructor/destructor) 
		public NArray(int _max_member = 256)
		{
			this.m_enable_enter = false;
			this.m_count_member = 0;
			this.m_container_member = new IGroupable<TMEMBER>[_max_member];
			this.m_seat = new(_max_member);
			this.m_cs_group = new();
		}
		~NArray()
		{
			this.LeaveAllMember();
		}

	// public) 
		//! @brief '멤버 객체'를 입장시킨다. @param _member 입장할 '멤버 객체' @param _buffer 입장할 때 전달할 메시지 @return eRESULT.SUCCESS 성공 @return !eRESULT.SUCCESS 실패
		public eRESULT				EnterMember(IGroupable<TMEMBER> _member, sMESSAGE? _msg = null)
		{
			return this.ProcessEnterMember(_member, _msg);
		}
		//! @brief 모든 '멤버 객체'를 퇴장시킨다.
		public void					LeaveAllMember(sMESSAGE? _msg = null)
		{
			lock (this.m_cs_group)
			{
				foreach(IGroupable<TMEMBER> member in this.m_container_member)
				{
					this.LeaveMember(member, _msg);
				}
			}
		}

		//! @brief '멤버 객체'의 갯수를 얻는다. @return 멤버 객체 갯수
		public int					MemberCount
		{
			get
			{
				lock (this.m_cs_group)
				{
					return this.m_count_member;
				}
			}
		}
		//! @brief 남아 있는 '멤버 객체'의 갯수를 얻는다. @return 남은 멤버 객체 갯수
		public int					MemberRoomCount
		{
			get
			{
				lock (this.m_cs_group)
				{
					return this.m_container_member.Length-m_count_member;
				}
			}
		}
		//! @brief 최대 '멤버 객체'의 갯수를 얻는다. @return 최대 멤버 객체 갯수
		public int					MaxMemberCount
		{
			set
			{
				lock (this.m_cs_group)
				{
					this._SetMaxMemberCount(value);
				}
			}
			get
			{
				lock (this.m_cs_group)
				{
					return this.m_container_member.Length;
				}
			}
		}

		//! @brief 멤버 객체가 꽉 찼는지를 확인한다. @return true 꽉찼다. false 꽉차지 않았다.
		public	bool				IsMemberFull
		{
			get { return this.m_count_member >= this.m_container_member.Length; }
		}							// 꽉차있는가?
		//! @brief 멤버 객체가 하나도 없는지 확인한다. @return true 완전히 비어있다. false 비어있지 않다.
		public	bool				IsMemberEmpty
		{
			get { return this.m_count_member == 0;}
		}						// 완전히 비어 있는가?
		//! @brief 멤버 객체가 들어갈 여유가 있는가를 확인한다. @return true 최대 인원만큼 꽉찼다. false 들어갈 여유가 있다.
		public	bool				IsMemberRoom
		{
			get { return this.m_count_member < this.m_container_member.Length;}
		}							// 더 들어 올 수 있는가?
		//! @brief 멤버 객체가 하나라도 있는가를 확인한다. @return true 비어있지 않다. false 완전히 비어 있다.
		public	bool				IsMemberExist
		{
			get { return this.m_count_member != 0;}
		}                       // 들어 있는 사람이 있는가?

		//! @brief [Member]의 목록
		public override IEnumerable<IGroupable<TMEMBER>> Members
		{
			get { return this.m_container_member as IEnumerable<IGroupable<TMEMBER>>; }
		}
		public override object		GroupLock
		{
			get { return this.m_cs_group; }
		}

	// framework) 
		//! @brief 입장을 허용했을 때 호출되는 함수
		protected virtual void		OnEnableEnter() {}
		//! @brief 입장이 금지했을 때 호출되는 함수
		protected virtual void		OnDisableEnter() {}
		//! @brief 새로운 멤버가 입장하기 직전 호출되는 함수 @param _member 입장할 '멤버 객체' @param _buffer 입장시 전달할 메시지 @return eRESULT.SUCCESS 입장 성공 @return !eRESULT.SUCCESS 입장 실패
		protected virtual eRESULT	OnMemberEntering(TMEMBER _member, sMESSAGE? _msg) { return eRESULT.SUCCESS;}
		//! @brief 새로운 멤버가 입장된 후 호출되는 함수 @param _member 입장한 '멤버 객체' @param _buffer 입장시 전달할 메시지
		protected virtual void		OnMemberEntered(TMEMBER _member, sMESSAGE? _msg) {}
		//! @brief 멤버가 퇴장되기 직전에 호출되는 함수 @param _member 퇴장할 '멤버 객체' @return 전달할 값
		protected virtual ulong		OnMemberLeaving(TMEMBER _member, sMESSAGE? _msg) { return 0;}
		//! @brief 멤버가 퇴장된 후 호출되는 함수 @param _member 퇴장한 '멤버 객체' @param _result OnMemberLeaving()의 리턴 함수
		protected virtual void		OnMemberLeaved(TMEMBER _member, sMESSAGE? _msg, ulong _result) {}

	// implementation)  
		protected override bool		ProcessEnableEnter(bool _enable)
		{
			lock(this.m_container_member)
			{
				// check) 
				if(this.m_enable_enter == _enable)
					return false;

				// 1) 값을 설정
				this.m_enable_enter = _enable;

				// 2) OnOpen을 호출한다.
				if(_enable)
					this.OnEnableEnter();
				else
					this.OnDisableEnter();
			}

			// return) 
			return true;
		}
		protected override bool		ProcessIsEnableEnter()
		{
			lock (this.m_container_member)
			{
				return this.m_enable_enter;
			}
		}
		protected eRESULT			ProcessEnterMember(IGroupable<TMEMBER> _member, sMESSAGE? _msg)
		{
			// check) p_Member가 nullptr인가?
			if(_member == null)
				return eRESULT.INVALID_ARGUMENT;

			// check)
			if (_member.Member == null)
				throw new System.Exception();

			lock (_member.m_cs_group)
			lock(this.m_container_member)
			{
				// 1) Member의 Group이 똑같지 않은가?
				if(_member.IsMemberOf(this) == true)
					return eRESULT.INVALID_MEMBER;

				// check) Enter가능한 상태인가?
				if(this.m_enable_enter == false)
					return eRESULT.FAIL_DISABLED;

				// 2) 방이 꽉 찼는가?
				if(this.IsMemberFull)
					return eRESULT.MEMBER_FULL;

				// 1) OnMemberEntering()함수를 호출한다.
				var	result = this.OnMemberEntering(_member.Member, _msg);

				// check)
				if(result != eRESULT.SUCCESS)
					return result;
		
				// 2) 이전 Group에서 떼낸다.
				_member.LeaveGroup();

				lock(this.m_cs_group)
				{
					// 3) attach한다.
					this._AttachMember(_member);

					// 4) OnMemberEntered()함수를 호출한다. 
					try
					{
						this._ProcessMemberEntered(_member, _msg);
					}
					// Exception) Rollback한다.
					catch(System.Exception /*e*/)
					{
						// - Member를 떼낸다.
						this._DetachMember(_member);

						// reraise) 
						throw;
					}
				}
			}

			return eRESULT.SUCCESS;
		}
		protected override void		ProcessLeaveMember(IGroupable<TMEMBER> _member, sMESSAGE? _msg)
		{
			// check) _member가 null인가?
			Debug.Assert(_member != null);

			// check)
			if (_member == null)
				return;

			// check)
			if (_member.Member == null)
				throw new System.Exception();

			lock (_member.m_cs_group)
			{
				// declare) 
				ulong result = 0;

				lock(this.m_cs_group)
				{
					// check) _member의 Group이 똑같지 않은가?
					if(_member.IsMemberOf(this) == false)
						return;

					// 1) List에서 떨어지기 전에 처리해야할 사항들을 처리한다.
					try
					{
						result = this._ProcessMemberLeaving(_member, _msg);
					}
					// Exception) 
					catch(System.Exception)
					{
					}

					// 2) detach한다.
					this._DetachMember(_member);
				}

				try
				{
					// 1) Member가 detach되고 난 다음에 해야할 것을 처리한다.
					this.OnMemberLeaved(_member.Member, _msg, result);
				}
				// Exception) 
				catch(System.Exception)
				{
				}
			}
		}

		private	void				_AttachMember(IGroupable<TMEMBER> _member)
		{
			// 1) 숫자를 하나 늘린다.(이걸 제일 먼저 한다.)
			++this.m_count_member;

			// 2) Seat를 얻는다.
			var seat_index = this.m_seat.AllocSeat();

			// check) Seat가 -1이면 Seat가 남아 있지 않은 것이므로 그냥 되돌린다.
			Debug.Assert(seat_index >= 0);

			// 3) 값을 읽어온다.
			this.m_container_member[seat_index] = _member;

			// 4) Seat Info를 Push한다.
			_member._SetMemberSerial(seat_index);

			// 5) Object을 설정!
			_member._SetGroup(this);
		}
		private void				_DetachMember(IGroupable<TMEMBER> _member)
		{
			// 1) Seat Serial을 잠시 저장해 놓는다.
			var seat_index = _member.MemberSerial;

			// 2) 값을 읽어온다.
			IGroupable<TMEMBER>	rSeat = this.m_container_member[seat_index];

			// check) _member와 Seat에 있는 Object가 같은 것이어야 한다.
			Debug.Assert(rSeat == _member);

			// 3) Seat Info를 Pop한다.
			_member._ResetMemberSerial();

			// 4) _member의 Group을 해제함.(이걸 제일 먼저 한다.)
			_member._ResetGroup();

			// 5) Seat번호를 돌려준다.
			this.m_seat.FreeSeat(seat_index);

			// 6) Socket 숫자를 하나 줄인다.(이걸 제일 마지막에 한다.)
			--this.m_count_member;
		}
		private void				_ProcessMemberEntered(IGroupable<TMEMBER> _member, sMESSAGE? _msg)
		{
			// 1) OnMemberEntering함수를 호출한다.
			this.OnMemberEntered(_member.Member, _msg);
		}
		private ulong				_ProcessMemberLeaving(IGroupable<TMEMBER> _member, sMESSAGE? _msg)
		{
			// 1) OnMemberLeaving함수를 호출한다.
			return this.OnMemberLeaving(_member.Member, _msg);
		}
		private void				_SetMaxMemberCount(int _max_count)
		{
			var	array_new = new IGroupable<TMEMBER>[_max_count];

			if(this.m_container_member != null)
			{
				int	i = 0;
				int	count = Math.Min(_max_count, this.m_container_member.Length);

				for(; i < count; ++i)
				{
					array_new[i] = this.m_container_member[i];
					this.m_container_member[i] = null;
				}

				for(; i<this.m_container_member.Length; ++i)
				{
					this.m_container_member[i] = null;
				}
			}

			this.m_container_member = array_new;
			this.m_seat = new(_max_count);
		}
		protected IGroupable<TMEMBER> _get_member(int _index) { return this.m_container_member[_index];}

	// implementation)  
		protected Seat				m_seat;
		protected bool				m_enable_enter;
		protected object			m_cs_group;

		protected IGroupable<TMEMBER>[] m_container_member;
		protected int				m_count_member;
	}
}
