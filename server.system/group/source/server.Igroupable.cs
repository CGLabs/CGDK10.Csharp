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

namespace CGDK.server
{
	public class Igroupable<TMEMBER> 
	{
	// constructor/destructor) 
		public Igroupable(TMEMBER _groupable)								{ m_groupable = _groupable;}

	// public) 
		//! @brief 현재 그룹을 얻는다. @return 현재 그룹
		public Igroup<TMEMBER>		group									{ get { return m_group; } }
		//! @brief 특정 그룹의 멤버인가를 확인한다. @param _pGroup 그룹 @return true 그룹의 멤버이다. @return false 그룹의 멤버가 아니다.
		public bool					is_member_of(Igroup<TMEMBER> _group)	{ return m_group == _group; }
		//! @brief 현재 그룹에서 나온다.
		public void					leave_group()
		{
			lock (m_cs_group)
			{
				// check)
				if(m_group == null)
					return;

				// 1) 일단 Group에서 Leave를 먼저 한다.
				m_group.leave_member(this);
			}
		}

		//! @brief 멤버의 Serial을 얻는다.
		public int					member_serial							{ get { return m_member_serial; } }
		//! @brief 현재 그룹을 얻는다. @return 현재 그룹
		public TMEMBER				member									{ get { return m_groupable; } }

	// implementation)  
		public void					_set_member_serial(int _serial)			{ m_member_serial = _serial;}
		public void					_reset_member_serial()					{ m_member_serial = -1;}
		public void					_set_group(Igroup<TMEMBER> _group)		{ m_group = _group;}
		public void					_reset_group()							{ m_group = null;}
	
		private TMEMBER				m_groupable;
		public	object				m_cs_group = new object();
		private Igroup<TMEMBER>		m_group;
		private int					m_member_serial;
	}
}