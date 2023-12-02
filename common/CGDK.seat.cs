//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                       Ver 6.1 / Release 2012.05.28                        *
//*                                                                           *
//*                           Data Template Classes                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*  This Program is programmed by Cho SangHyun. sangducks@gmail.com          *
//*  Best for Game Developement and Optimized for Game Developement.          *
//*                                                                           *
//*                   (C) 2008 CGLabs All right reserved.                     *
//*                           http://www.CGDK.co.kr                           *
//*                                                                           *
//*****************************************************************************

using System.Collections.Generic;

//-----------------------------------------------------------------------------
//
// CGD::seat
//
// 1. CGD::seat란!
//     게임 Room과 같은 것을 제작할 때 사용자의 관리를 위해 좌석번호의 할당이
//    필요한 경우가 상당히 많다. 
//     예를 들면 전체 8자리가 있는 방의 경우 사용자가 들락날락하게 되면 방들이
//    순차적으로 차거나 비는 것이 아니라 중간 중간 좌석이 비거나 차거나 한다.
//    이때 입실을 할때마다 빈자리를 찾도록 하는 과정이 필요로 한다. 이때
//    seat는 남은 방번호를 stack형태로 보관하고 있다가 비어있는 자리를 즉시
//    제공해 준다.
//
//    - 방번호의 할당은 작은 번호부터 높은 번호로 할당을 해준다.
//    - 할당된 번호를 다시 되돌려 받을 때 Sort가 일어난다.
//    - 좌석번호의 증가나 축소는 가능하다.
//      * 증가를 할때는 이미 할당된 번호 이후의 값을 stack을 하며
//      * 감소를 할때는 stack중 감소된 좌석번호 이후 값들을 stack에서 찾아
//        제거한다. 이미 할당된 것은 다시 free되어 돌아왔을 때 stack을 하지 
//        않는다.
//
//
// 2. CGD::seat의 표준 준수 사항
//    1) create_seat
//       - 새로 seat를 생성한다.(일반적으로 처음 한번만 생성한다.)
//       - 한번 생성한 이후에 새로 생성할 경우그 차이만큼의 좌석을 조정한다.
//    2) reset_seat
//       - seat를 clear하고 다시 설정한다. 이따 이미 할당되어간 seat가 다시
//         돌아올 경우 문제가 있을 수 있으므로 중의해야한다.
//    3) alloc_seat
//       - 새로운 seat를 할당해준다. stack에서 가장 작은 seat 번호를 할당해준다.
//    4) free_seat
//       - 할당된seat를 되돌려 받는다.
//       - 되돌려 받은후 sort를 수행한다.
//
// 3. 추가설명 or 사용예제
//
//
//-----------------------------------------------------------------------------

namespace CGD
{

	public class seat 
	{
	// Constructors)
		public	seat()
		{
		}
		public	seat(int _maxcount, int _begin=0, int _step=1)
		{
			create(_maxcount, _begin, _step);
		}

	// Publics) 
		public void						create(int _max_seat_count, int _begin=0, int _step=1)
		{
			// Check) p_iMaxSeatCount이 0인지를 확인한다.
			if(_max_seat_count==0)
			{
				return;
			}

			// Check) p_iMaxSeatCount이 0인지를 확인한다.
			if(_step<1)
			{
				return;
			}

			// Check) p_iMaxSeatCount이 0인지를 확인한다.
			if(_max_seat_count<_step)
			{
				return;
			}

			// 1) 값을 설정한다.
			m_vectorSeat = new List<int>(_max_seat_count+1);

			// 2) Step을 설정한다.
			m_begin		 = _begin;
			m_step		 = _step;

			// 3) 모두 지운다.
			reset();
		}
		public void						reset()
		{
			// 1) 일단 clear한다.
			m_vectorSeat.Clear();

			// 2) 첫번째는 Dummy로 넣는다.
			m_vectorSeat.Add(0);

			// 3) 설정한다.
			int	i	 = m_begin-m_step;
			int	end	 = max_seat();

			// 4) Push back~~
			for(;i<end;i+=m_step)
			{
				m_vectorSeat.Add(i);
			}
		}

		public int						alloc_seat()
		{
			// 1) 사용 가능한 Seat가 0개이면 -1로 한다.
			if(empty())
			{
				return	-1;
			}

			// Declare) 돌려줄값
			int	seat_return	 = m_vectorSeat[1];

			// 2) Down Heap
			int	value	 = m_vectorSeat[m_vectorSeat.Count-1];

			int	parent	 = 1;
			int	child	 = 2;
			int	end		 = m_vectorSeat.Count-2;

			while(child<=end)
			{
				if(child<end && m_vectorSeat[child]>m_vectorSeat[child+1])
					++child;

				if(value<=m_vectorSeat[child])	
					break;

				m_vectorSeat[parent]	 = m_vectorSeat[child];

				parent	 = child;
				child	<<= 1;
			}
			m_vectorSeat[parent]	 = value;
			m_vectorSeat.Remove(m_vectorSeat.Count-1);

			// Return) 
			return	seat_return;
		}
		public void						free_seat(int _seat)
		{
			// 1) child...
			int	child	 = m_vectorSeat.Count;

			// 2) 마지막에 Push한다.
			m_vectorSeat.Add(_seat);

			// 3) Up Heap
			int	parent	 = (child>>1);
			while(parent!=0 && m_vectorSeat[parent]>_seat)
			{
				m_vectorSeat[child]	 = m_vectorSeat[parent];

				child = parent;
				parent>>=1;
			}
			m_vectorSeat[child]	 = _seat;
		}

		public	int						remained_seat()				{	return m_vectorSeat.Count-1;}
		public	int						max_seat()					{	return m_vectorSeat.Capacity-1;}
		public	bool					empty()						{	return m_vectorSeat.Count==1;}


	// Implementations) 
		private	List<int>				m_vectorSeat;
		private	int						m_begin;
		private	int						m_step;
	}


}