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
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

//----------------------------------------------------------------------------
//  <<class>> CGDK.ICGMessageable
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface Ireferenceable
	{
	#if !_REFERENCE_INFO
		int		add_ref();
		void	release();
	#else
		int		add_ref(string _Pos);
		void	release(string _Pos);
	#endif
		int		reference_count { get; }
	}

	public class Nreferenceable : Ireferenceable
	{
	// public)
	#if !_REFERENCE_INFO
		public int add_ref()
		{
			// check) result_count는 무조건 0보다는 커야한다.
			Debug.Assert(m_count_reference>=0);

			// 1) RefrenceCount를 증가시킨다.
			return Interlocked.Increment(ref m_count_reference);
		}
		public void release()
		{
			// check) 
			Debug.Assert(m_count_reference>0);

			// 1) Reference Count를 줄인다.
			int	result_count = Interlocked.Decrement(ref m_count_reference);

			// check) result_count는 무조건 0보다는 커야한다.
			Debug.Assert(result_count >= 0);

			// 2) 만약 Count가 0이면 Dispose한다.
			if(result_count==0)
			{
				// - on_final_release
				on_final_release();

				// - free
				free();
			}
		}
		public void free()
		{
			if(process_free == null)
			{
				return;
			}

			process_free(this);
		}
	#else
		public int add_ref(string _Pos)
		{
			lock(m_list_lock)
			{
				// check) result_count는 무조건 0보다는 커야한다.
				Debug.Assert(m_count_reference>=0);

				// 1) add_ref한 위치를 추가한다.
				m_list_lock.Add(_Pos);

				// 2) RefrenceCount를 증가시킨다.
				return Interlocked.Increment(ref m_count_reference);
			}
		}
		public void release(string _Pos)
		{
			lock(m_list_lock)
			{
				// check) 
				Debug.Assert(m_count_reference>0);

				// 1) Reference Count를 줄인다.
				int	result_count = Interlocked.Decrement(ref m_count_reference);

				// check) result_count는 무조건 0보다는 커야한다.
				Debug.Assert(result_count>=0);

				// 2) release한 위치를 추가한다.
				m_list_lock.Add(_Pos);

				// 3) 만약 Count가 0이면 Dispose한다.
				if(result_count == 0)
				{
					// - on_final_release
					on_final_release();

					// - free
					free(_Pos);
				}
			}
		}
		public void free(string _Pos)
		{
			// check) m_list_lock의 갯수는 항상 2의 배수여야 한다.(add_ref와 release가 짝을 이루어야 하기 때문이다.
			Debug.Assert((m_list_lock.Count%2)==0);

			// 1) free된 위치를 저장한다.
			m_pos_free	 = _Pos;

			// 2) 모두 지우기...
			m_list_lock.Clear();

			// 3) free
			if(process_free == null)
			{
				return;
			}

			// 4) process_free을 호출하낟.
			process_free(this);
		}
	#endif

		public int reference_count
		{
			get	{return m_count_reference;}
		}


	// framework)
		protected virtual void	on_final_release() {}

	// implementation)
		// 1) Reference Counting
		private	int				m_count_reference = 0;

	#if _REFERENCE_INFO
		public List<string>		m_list_lock = new List<string>();
		public string			m_pos_free = null;
	#endif

	// declare) 
		public delegate void	f_free(Ireferenceable _object);

		// 2) on_final_release
		public 	f_free			process_free;

	}
}
