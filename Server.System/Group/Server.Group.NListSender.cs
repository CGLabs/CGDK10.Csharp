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
namespace CGDK.Server.Group
{
	public class NListSender<TMEMBER> : 
		NList<TMEMBER>, 
		Net.Io.ISenderStream
		where TMEMBER : class, Net.Io.ISenderStream
	{
			// constructor/destructor) 
		public NListSender(int _max_member = int.MaxValue) : 
			base(_max_member)
		{
		}

	// public) 
		public bool						Send(CGDK.buffer _buffer)
		{
			// check)
			if(_buffer.Data == null)
				return false;

			// check)
			if(_buffer.Count== 0)
				return false;

			lock(m_cs_group)
			{
				// - 모든 Member에게 전송한다.
				foreach(var iter in m_container_member)
				{
					iter.Member.Send(_buffer);
				}
			}

			// return) 성공...
			return true;
		}
		public bool						SendExcept(TMEMBER _except, CGDK.buffer _buffer)
		{
			// check)
			if(_buffer.Data == null)
				return	false;

			// check)
			if(_buffer.Count == 0)
				return	false;

			lock(m_cs_group)
			{
				foreach(var iter in m_container_member)
				{
					// check) _except일 경우 다음...
					if(iter.Member == _except)
					{
						break;
					}

					// - 전송한다.
					iter.Member.Send(_buffer);
				}
			}

			// return) 성공...
			return	true;
		}
		public bool						SendConditional(CGDK.buffer _buffer, d_pred _f_pred)
		{
			// check)
			if(_buffer.Data == null)
				return	false;

			// check)
			if(_buffer.Count == 0)
				return	false;

			lock(m_cs_group)
			{
				foreach(var iter in m_container_member)
				{
					// check) _f_pred가 false면 전송하지 않는다.
					if(_f_pred(iter.Member) ==false)
						continue;

					// - 전송한다.
					iter.Member.Send(_buffer);
				}
			}

			// return)
			return true;
		}

	}
}