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
using System.Diagnostics;

namespace CGDK.Server.Group
{
	public class NArraySender<TMEMBER> : 
		NArray<TMEMBER>,
		Net.Io.ISenderStream
		where TMEMBER : class, Net.Io.ISenderStream
	{
	// constructor/destructor) 
		public NArraySender(int _max_member = 256) : base(_max_member){}

	// public) 
		public bool					Send(CGDK.buffer _buffer)
		{
			// check)
			if(_buffer.Data == null)
				return	false;

			// check)
			if(_buffer.Count == 0)
				return	false;

			lock(m_cs_group)
			{
				int temp_count = MemberCount;

				for(int i=0; i<m_container_member.Length; ++ i)
				{
					// check)
					if(temp_count == 0)
						break;

					// check) Member가 비어 있으면 넘어간다.
					if(m_container_member[i] == null)
						continue;

					// - 전송한다.
					m_container_member[i].Member.Send(_buffer);

					--temp_count;
				}
			}

			// return) 성공...
			return	true;
		}
		public bool					SendTo(int _index, CGDK.buffer _buffer)
		{
			// check)
			if(_buffer.Data == null)
				return false;

			// check)
			if(_buffer.Count == 0)
				return false;

			lock(m_cs_group)
			{
				// check) _index가 정당한 범위 내에 있는가?
				if(_index<0 || _index >= MaxMemberCount)
					return false;

				// 1) 해당 Member를 얻는다.
				IGroupable<TMEMBER>	member = _get_member(_index);

				// check)
				if(member == null)
					return false;

				// 2) 해당 Member에게 전송한다.
				member.Member.Send(_buffer);
			}

			// return)
			return	true;
		}
		public bool					SendExcept(TMEMBER _except, CGDK.buffer _buffer)
		{
			lock(m_cs_group)
			{
				int count = MemberCount;
				int i = 0;

				// 1) 제외시킬 객체를 찾기전...(객체를 비교함.)
				for(; i<m_container_member.Length; ++i)
				{
					// check)
					if(count == 0)
						break;

					// check) Member가 비어 있으면 넘어간다.
					if(m_container_member[i] == null)
						continue;

					// check) _except일 경우 다음...
					if(m_container_member[i].Member == _except)
					{
						++i;
						break;
					}

					// - 전송한다.
					m_container_member[i].Member.Send(_buffer);

					// - 갯수를 줄인다.
					--count;
				}

				// 2) 제외시킬 객체를 찾은 후...(객체 비교없이 바로 전송)
				for(; count > 0; ++i)
				{
					// check)
					Debug.Assert(i!=m_container_member.Length);

					// check) 
					if(i == m_container_member.Length)
						return false;

					// check) nullptr이면 다음...
					if(m_container_member[i] == null)
						continue;

					// - 갯수를 줄인다.
					--count;

					// - Send한다.
					m_container_member[i].Member.Send(_buffer);
				}
			}

			// return) 성공...
			return	true;
		}
		public bool					SendConditional(CGDK.buffer _buffer, d_pred _f_pred)
		{
			lock(m_cs_group)
			{
				int count = MemberCount;

				for(int i=0; i<m_container_member.Length; ++i)
				{
					// check)
					if(count == 0)
						break;

					// check) Member가 비어 있으면 넘어간다.
					if(m_container_member[i] == null)
						continue;

					// check) 조건에 맞지 않으면 즉! fPred함수의 결과가 false이면 그냥 넘어간다.
					if(_f_pred(m_container_member[i].Member) == false)
						continue;

					// - 전송한다.
					m_container_member[i].Member.Send(_buffer);

					--count;
				}
			}

			// return) 성공...
			return	true;
		}
	}
}