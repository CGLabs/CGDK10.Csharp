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
//*  This Program is programmed by Cho sanghyun. sangducks@gmail.com          *
//*  Best for Game Developement and Optimized for Game Developement.          *
//*                                                                           *
//*                (c) 2008 Cho sanghyun. All right reserved.                 *
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
//        제거한다. 이미 할당된 것은 다시 Free되어 돌아왔을 때 stack을 하지 
//        않는다.
//
//
// 2. CGD::seat의 표준 준수 사항
//    1) create_seat
//       - 새로 seat를 생성한다.(일반적으로 처음 한번만 생성한다.)
//       - 한번 생성한 이후에 새로 생성할 경우그 차이만큼의 좌석을 조정한다.
//    2) reset_seat
//       - seat를 Clear하고 다시 설정한다. 이따 이미 할당되어간 seat가 다시
//         돌아올 경우 문제가 있을 수 있으므로 중의해야한다.
//    3) AllocSeat
//       - 새로운 seat를 할당해준다. stack에서 가장 작은 seat 번호를 할당해준다.
//    4) FreeSeat
//       - 할당된seat를 되돌려 받는다.
//       - 되돌려 받은후 sort를 수행한다.
//
// 3. 추가설명 or 사용예제
//
//
//-----------------------------------------------------------------------------

namespace CGDK
{

	public class Seat 
	{
	// Constructors)
		public	Seat()
		{
		}
		public	Seat(int _maxcount, int _begin=0, int _step=1)
		{
			this.Create(_maxcount, _begin, _step);
		}

	// public) 
		public void						Create(int _max_seat_count, int _begin=0, int _step=1)
		{
			// check) p_iMaxSeatCount이 0인지를 확인한다.
			if(_max_seat_count==0)
				return;

			// check) p_iMaxSeatCount이 0인지를 확인한다.
			if(_step<1)
				return;

			// check) p_iMaxSeatCount이 0인지를 확인한다.
			if(_max_seat_count<_step)
				return;

			// 1) 값을 설정한다.
			this.m_vectorSeat = new List<int>(_max_seat_count+1);

			// 2) Step을 설정한다.
			this.m_begin = _begin;
			this.m_step = _step;

			// 3) 모두 지운다.
			this.Reset();
		}
		public void						Reset()
		{
			// 1) 일단 Clear한다.
			this.m_vectorSeat.Clear();

			// 2) 첫번째는 Dummy로 넣는다.
			this.m_vectorSeat.Add(0);

			// 3) 설정한다.
			int	i = this.m_begin-m_step;
			int	end = this.MaxSeat;

			// 4) Push back~~
			for(;i<end;i+=m_step)
			{
				this.m_vectorSeat.Add(i);
			}
		}

		public int						AllocSeat()
		{
			// 1) 사용 가능한 Seat가 0개이면 -1로 한다.
			if(this.IsEmpty)
				return	-1;

			// Declare) 돌려줄값
			int	seat_return = this.m_vectorSeat[1];

			// 2) Down Heap
			int	value = this.m_vectorSeat[this.m_vectorSeat.Count-1];

			int	parent = 1;
			int	child = 2;
			int	end = this.m_vectorSeat.Count-2;

			while(child <= end)
			{
				if(child<end && this.m_vectorSeat[child] > this.m_vectorSeat[child+1])
					++child;

				if(value <= this.m_vectorSeat[child])	
					break;

				this.m_vectorSeat[parent] = m_vectorSeat[child];

				parent = child;
				child <<= 1;
			}
			this.m_vectorSeat[parent] = value;
			this.m_vectorSeat.Remove(m_vectorSeat.Count-1);

			// Return) 
			return seat_return;
		}
		public void						FreeSeat(int _seat)
		{
			// 1) child...
			int	child = this.m_vectorSeat.Count;

			// 2) 마지막에 Push한다.
			this.m_vectorSeat.Add(_seat);

			// 3) Up Heap
			int	parent	 = (child>>1);
			while(parent!=0 && this.m_vectorSeat[parent]>_seat)
			{
				this.m_vectorSeat[child] = this.m_vectorSeat[parent];

				child = parent;
				parent>>=1;
			}
			this.m_vectorSeat[child] = _seat;
		}

		public	int						RemainedSeat {  get { return m_vectorSeat.Count - 1; } }
		public	int						MaxSeat { get { return m_vectorSeat.Capacity - 1; } }
		public	bool					IsEmpty { get { return m_vectorSeat.Count == 1; } }


	// Implementations) 
		private	List<int>				m_vectorSeat;
		private	int						m_begin;
		private	int						m_step;
	}


}