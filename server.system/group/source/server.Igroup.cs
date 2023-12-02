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
using System.Collections.Generic;

namespace CGDK.server
{
	public abstract class Igroup<TMEMBER>
	{
	// definitions) 
		public delegate bool			d_pred(TMEMBER _member);

	// public) 
		//! @brief 현재 그룹에서 나온다.
		public eRESULT					leave_member(Igroupable<TMEMBER> _member, sMESSAGE? _msg = null)
		{
			// check)
			if(_member == null)
				return eRESULT.INVALID_ARGUMENT;

			lock (_member.m_cs_group)
			{
				// check)
				if(_member.group != this)
					return	eRESULT.INVALID_ARGUMENT;

				// 1) 일단 Group에서 Leave를 먼저 한다.
				process_leave_member(_member, _msg);
			}
			
			// return) 
			return eRESULT.SUCCESS;
		}
		//! @brief [Member] 입장의 허용 여부를 결정한다. @param true=허용, false=불허
		public bool						enable_enter
		{
			set { process_enable_enter(value); }
			get { return process_is_enable_enter(); }
		}

		//! @brief [Member]의 목록
		public abstract IEnumerable<Igroupable<TMEMBER>> members
		{
			get;
		}
		public abstract object			group_lock
		{
			get;
		}

		// framework) 
		//! @brief 입장 가능 여부를 설정하는 과정을 정의한다. @param _pEnable 입장 가능 여부 @return true 성공 @return false 실패
		protected virtual bool			process_enable_enter(bool _enable) { return true;}
		//! @brief 입장 가능 여부를 설정하는 과정을 정의한다. @param _pEnable 입장 가능 여부 @return true 성공 @return false 실패
		protected virtual bool			process_is_enable_enter() { return true;}
		//! @brief 그룹에서 멤버가 퇴장하는 처리를 정의한다. @param _member 퇴장할 '멤버 객체'
		protected virtual void			process_leave_member(Igroupable<TMEMBER> _member, sMESSAGE? _msg) {}

	}
}