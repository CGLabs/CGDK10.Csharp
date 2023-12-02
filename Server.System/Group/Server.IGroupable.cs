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

namespace CGDK.Server
{
	public class IGroupable<TMEMBER> 
	{
	// constructor/destructor) 
		public IGroupable(TMEMBER _groupable)								{ this.m_groupable = _groupable;}

	// public) 
		//! @brief 현재 그룹을 얻는다. @return 현재 그룹
		public IGroup<TMEMBER>		Group									{ get { return this.m_group; } }
		//! @brief 특정 그룹의 멤버인가를 확인한다. @param _pGroup 그룹 @return true 그룹의 멤버이다. @return false 그룹의 멤버가 아니다.
		public bool					IsMemberOf(IGroup<TMEMBER> _group)	{ return m_group == _group; }
		//! @brief 현재 그룹에서 나온다.
		public void					LeaveGroup()
		{
			lock (this.m_cs_group)
			{
				// check)
				if(this.m_group == null)
					return;

				// 1) 일단 Group에서 Leave를 먼저 한다.
				this.m_group.LeaveMember(this);
			}
		}

		//! @brief 멤버의 Serial을 얻는다.
		public int					MemberSerial							{ get { return this.m_member_serial; } }
		//! @brief 현재 그룹을 얻는다. @return 현재 그룹
		public TMEMBER				Member									{ get { return this.m_groupable; } }

	// implementation)  
		public void					_SetMemberSerial(int _serial)			{ this.m_member_serial = _serial;}
		public void					_ResetMemberSerial()					{ this.m_member_serial = -1;}
		public void					_SetGroup(IGroup<TMEMBER> _group)		{ this.m_group = _group;}
		public void					_ResetGroup()							{ this.m_group = null;}
	
		private readonly TMEMBER	m_groupable;
		public	object				m_cs_group = new();
		private IGroup<TMEMBER>		m_group;
		private int					m_member_serial;
	}
}