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
	public interface IReferenceable
	{
	#if !_REFERENCE_INFO
		int		AddReference();
		void	Release();
	#else
		int		AddReference(string _Pos);
		void	Release(string _Pos);
	#endif
		int		ReferenceCount { get; }
	}

	public class Nreferenceable : IReferenceable
	{
	// public)
	#if !_REFERENCE_INFO
		public int AddReference()
		{
			// check) Result_count는 무조건 0보다는 커야한다.
			Debug.Assert(this.m_count_reference >= 0);

			// 1) RefrenceCount를 증가시킨다.
			return Interlocked.Increment(ref this.m_count_reference);
		}
		public void Release()
		{
			// check) 
			Debug.Assert(this.m_count_reference>0);

			// 1) Reference Count를 줄인다.
			int	Result_count = Interlocked.Decrement(ref this.m_count_reference);

			// check) Result_count는 무조건 0보다는 커야한다.
			Debug.Assert(Result_count >= 0);

			// 2) 만약 Count가 0이면 Dispose한다.
			if(Result_count == 0)
			{
				// - OnFinalRelease
				this.OnFinalRelease();

				// - Free
				this.Free();
			}
		}
		public void Free()
		{
			if(this.ProcessFree == null)
			{
				return;
			}

			this.ProcessFree(this);
		}
	#else
		public int AddReference(string _Pos)
		{
			lock(this.m_list_lock)
			{
				// check) Result_count는 무조건 0보다는 커야한다.
				Debug.Assert(m_count_reference>=0);

				// 1) AddReference한 위치를 추가한다.
				this.m_list_lock.Add(_Pos);

				// 2) RefrenceCount를 증가시킨다.
				return Interlocked.Increment(ref m_count_reference);
			}
		}
		public void Release(string _Pos)
		{
			lock(this.m_list_lock)
			{
				// check) 
				Debug.Assert(this.m_count_reference>0);

				// 1) Reference Count를 줄인다.
				int	Result_count = Interlocked.Decrement(ref this.m_count_reference);

				// check) Result_count는 무조건 0보다는 커야한다.
				Debug.Assert(Result_count >= 0);

				// 2) Release한 위치를 추가한다.
				this.m_list_lock.Add(_Pos);

				// 3) 만약 Count가 0이면 Dispose한다.
				if(Result_count == 0)
				{
					// - OnFinalRelease
					this.OnFinalRelease();

					// - Free
					this.Free(_Pos);
				}
			}
		}
		public void Free(string _Pos)
		{
			// check) m_list_lock의 갯수는 항상 2의 배수여야 한다.(AddReference와 Release가 짝을 이루어야 하기 때문이다.
			Debug.Assert((this.m_list_lock.Count%2)==0);

			// 1) Free된 위치를 저장한다.
			this.m_pos_free = _Pos;

			// 2) 모두 지우기...
			this.m_list_lock.Clear();

			// 3) Free
			if(this.ProcessFree == null)
			{
				return;
			}

			// 4) ProcessFree을 호출하낟.
			this.ProcessFree(this);
		}
	#endif

		public int ReferenceCount
		{
			get	{return this.m_count_reference;}
		}


	// framework)
		protected virtual void	OnFinalRelease() {}

	// implementation)
		// 1) Reference Counting
		private	int				m_count_reference = 0;

	#if _REFERENCE_INFO
		public List<string>		m_list_lock = new List<string>();
		public string			m_pos_free = null;
	#endif

	// declare) 
		public delegate void	f_free(IReferenceable _object);

		// 2) OnFinalRelease
		public 	f_free			ProcessFree;

	}
}
