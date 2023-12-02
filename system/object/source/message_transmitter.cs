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
using System.Collections;
using System.Collections.Generic;

//----------------------------------------------------------------------------
//  <<interface>> CGDK.ICGDispatchable
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public delegate void d_dipatchable(Imessageable _messageable);

	public class message_transmitter : Imessage_transmitter
	{
	// public) 
		public int				transmit_message(object _source, sMESSAGE _msg)
		{
			// 1) 임시로 List를 생성한다.
			var tempList = new Imessageable[64];
			int iCount = 0;

			// 2) Messageable list를 복사한다.
			lock (m_list_messageable)
			{
				foreach(var iter in m_list_messageable)
				{
					tempList[iCount] = iter;
					++iCount;
				}
			}

			// 3) 모두에게 메시지를 전달한다.
			for (int i = 0; i != iCount; ++i)
			{
				var result = tempList[i].process_message(_source, _msg);

				if(result!=0)
					return	result;
			}

			// Return)
			return 0;
		}
		public bool				register_messageable(Imessageable _messageable)
		{
			// check)
			if(_messageable == null)
				return	false;

			lock (m_list_messageable)
			{
				// 1) 먼저 같은 것이 있는지 찾는다.
				var result = m_list_messageable.Find(x => x == _messageable);

				// check) 이미 존재하는 것이라면 그냥 리턴한다.
				if (result != null) 
					return false;

				// 2) Attaching 훅함수를 호출한다.
				if (notify_on_messageable_attaching != null)
					notify_on_messageable_attaching(_messageable);

				// 3) 추가한다.
				m_list_messageable.Add(_messageable);
			}

			// 4) Attached 훅함수를 호출한다.
			if (notify_on_messageable_attached != null)
				notify_on_messageable_attached(_messageable);

			// Return)
			return true;
		}
		public bool				unregister_messageable(Imessageable _messageable)
		{
			// check)
			if(_messageable == null)
				return	false;

			lock (m_list_messageable)
			{
				// 1) 먼저 같은 것이 있는지 찾는다.
				var result = m_list_messageable.Remove(_messageable);

				// check) 
				if (result == false)
					return false;

				// 2) Detaching 훅함수를 호출한다.
				if (notify_on_messageable_detaching != null)
					notify_on_messageable_detaching(_messageable);
			}

			// 3) Detached 훅함수를 호출한다.
			if (notify_on_messageable_detached != null)
				notify_on_messageable_detached(_messageable);

			// Return)
			return	true;
		}
		
		public d_dipatchable	notify_on_messageable_attaching;	//! @brief 새로운 '대상 객체'가 붙기 직전 호출되는 함수.
		public d_dipatchable	notify_on_messageable_attached;	//! @brief 새로운 '대상 객체'가 붙은 후 호출되는 함수.
		public d_dipatchable	notify_on_messageable_detaching;	//! @brief 등록된 '대상 객체'가 떼기 직전 호출되는 함수.
		public d_dipatchable	notify_on_messageable_detached;	//! @brief 등록된 '대상 객체'가 떼어진 후 호출되는 함수.

	// implementations) 
		private List<Imessageable>	m_list_messageable	 = new List<Imessageable>();

	}
}
