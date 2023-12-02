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
	public class NList<TMEMBER> : 
		IGroup<TMEMBER> 
		where TMEMBER : class
	{
	// constructor/destructor) 
		public NList(int _max_member=int.MaxValue)
		{
			m_enable_enter		 = false;
			m_count_max_member	 = _max_member;
			m_container_member	 = new LinkedList<IGroupable<TMEMBER>>();
			m_cs_group			 = new object();
		}
		~NList()
		{
			this.LeaveAllMember();
		}

	// public) 
		//! @brief '멤버 객체'를 입장시킨다. @param _member 입장할 '멤버 객체' @param _buffer 입장할 때 전달할 메시지 @return eRESULT.SUCCESS 성공 @return !eRESULT.SUCCESS 실패
		public eRESULT				EnterMember(IGroupable<TMEMBER> _member, sMESSAGE? _msg = null)
		{
			return ProcessEnterMember(_member, _msg);
		}
		//! @brief 모든 '멤버 객체'를 퇴장시킨다.
		public void					LeaveAllMember(sMESSAGE? _msg = null)
		{
			lock (m_cs_group)
			{
				foreach(IGroupable<TMEMBER> member in m_container_member)
				{
					this.LeaveMember(member, _msg);
				}
			}
		}

		//! @brief '멤버 객체'의 갯수를 얻는다. @return 멤버 객체 갯수
		public int					MemberCount
		{
			get { lock (m_cs_group) { return m_container_member.Count; } }
		}
		//! @brief 남아 있는 '멤버 객체'의 갯수를 얻는다. @return 남은 멤버 객체 갯수
		public int					MemberRoomCount
		{
			get { lock (m_cs_group) { return m_count_max_member - m_container_member.Count; } }
		}
		//! @brief 최대 '멤버 객체'의 갯수를 얻는다. @return 최대 멤버 객체 갯수
		public int					MaxMemberCount
		{
			set { lock(m_cs_group) { m_count_max_member = value;}}
			get { lock(m_cs_group) { return m_count_max_member;}}
		}

        public Object               Lockable
        {
            get { return m_cs_group; }
        }

        //! @brief 멤버 객체가 꽉 찼는지를 확인한다. @return true 꽉찼다. false 꽉차지 않았다.
        public bool					IsMemberFull
		{
			get { return this.m_container_member.Count >= this.m_count_max_member;}
		}
		//! @brief 멤버 객체가 하나도 없는지 확인한다. @return true 완전히 비어있다. false 비어있지 않다.
		public bool					IsMemberEmpty
		{
			get { return this.m_container_member.Count == 0;}
		}
		//! @brief 멤버 객체가 들어갈 여유가 있는가를 확인한다. @return true 최대 인원만큼 꽉찼다. false 들어갈 여유가 있다.
		public bool					IsMemberRoom
		{
			get { return this.m_container_member.Count<this.m_count_max_member;}
		}
		//! @brief 멤버 객체가 하나라도 있는가를 확인한다. @return true 비어있지 않다. false 완전히 비어 있다.
		public bool					IsMemberExist
		{
			get { return this.m_container_member.Count != 0;}
		}

		//! @brief [Member]의 목록
		public override IEnumerable<IGroupable<TMEMBER>> Members
		{
			get { return this.m_container_member as IEnumerable<IGroupable<TMEMBER>>; }
		}
		public override object		GroupLock
		{
			get { return m_cs_group; }
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
			// check)
			if(_member == null)
				return eRESULT.INVALID_ARGUMENT;

			// check)
			if (_member.Member == null)
				throw new System.Exception();

			lock (_member.m_cs_group)
			lock(this.m_container_member)
			{
				// check) is aleady entered
				if(_member.IsMemberOf(this) == true)
					return eRESULT.INVALID_ARGUMENT;

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
						// - member를 떼낸다.
						this._DetachMember(_member);

						// reraise) 
						throw;
					}
				}
			}

			return	eRESULT.SUCCESS;
		}
		protected override void		ProcessLeaveMember(IGroupable<TMEMBER> _member, sMESSAGE? _msg)
		{
			// check)
			Debug.Assert(_member != null);

			// check)
			if (_member.Member == null)
				throw new System.Exception();

			lock (_member.m_cs_group)
			{
				// declare) 
				ulong result = 0;

				lock(m_cs_group)
				{
					// check) is aleady entered
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

		private	void				_AttachMember(IGroupable<TMEMBER> _member)		{ this._AttachMemberToListTail(_member);}
		private void				_AttachMemberToListTail(IGroupable<TMEMBER> _member)
		{
			// 1) attach를 한다.
			this.m_container_member.AddLast(_member);

			// 2) Object을 설정!
			_member._SetGroup(this);
		}
		private void				_DetachMember(IGroupable<TMEMBER> _member)
		{
			// 1) Object을 때낸다.
			this.m_container_member.Remove(_member);

			// 2) ObejctGroup를 nullptr로 설정한다.
			_member._ResetGroup();
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

		protected bool				m_enable_enter;
		protected Object			m_cs_group;

		protected LinkedList<IGroupable<TMEMBER>> m_container_member;
		protected int				m_count_max_member;
	}
}
