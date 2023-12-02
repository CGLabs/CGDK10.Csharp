//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                               Pool Classes                                *
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


//----------------------------------------------------------------------------
//
//  <<interface>> CGDK.Ifactory
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.factory
{
	public enum eFACTORY_TYPE
	{
		UNDEFINED	 = 0,	// 정해지지 않음
		MEMORYBLOCK	 = 1,	// 메모리 블럭 풀
		SYSTEM		 = 2,	// 시스템에서 내부 동작을 위해 사용된 풀
		USER		 = 3,	// 사용자가 필요에 따라 생성한 풀
		MAX
	};

	public enum eFACTORY_SUSTAIN_TYPE
	{
		NORMAL		 = 0,
		HEAVY		 = 1,
		EXCESS_ALL	 = 2,
		STACKED_ALL	 = 3
	}

	namespace statistics 
	{
	public struct factory
	{
		public int					factory_id;			// Pool의 ID
		public eFACTORY_TYPE		factory_type;		// Pool의 형태

		public long					tickCapture;		// 저장된 시간(tick으로..)

		public long					alloc_stacked;	    // 할당시 Stack에 저장된 객체를 재활용해서 할당해 준 횟수 (비율이 높을 수록 좋다.)
		public long					alloc_create;		// 할당시 새로 생성해 할당해 준 횟수(적을 수록 좋다.)
		public long					free_stacked;		// 할당해제 시 Stack에 저장된 횟수. (비율이 높을 수록 좋다.)
		public long					free_delete;		// 할당해제 시 즉시 지워진 횟수.(적을 수록 좋다.)
		public long					existing_limits;	// 할당되는 최대 객체의 제한 ( 이제 한 값 이상의 객체가 생산되어 있을 경우 free시 바로 delete를 하게 된다.)
		public long					existing;			// 현재 생성되어 있는 총 갯수 (할당된 갯수+스택된 갯수)
		public long					in_using;			// 현재 할당 된 총 객체의 갯수
	}
	}

	public abstract class Ifactory : 
		Inameable
	{
	// Constructors) 
		public Ifactory()
		{
			m_statistics_factory.factory_id = 0;
			m_statistics_factory.factory_type = eFACTORY_TYPE.UNDEFINED;
			m_statistics_factory.tickCapture = 0;

			reset_factory_statistics();
		}

	// public) 
		// 1) Name
		public string				name
		{
			get { return m_name;}
			set { m_name = value;}
		}

		// 2) 통계관련 함수.
		public int					pool_id								{ get { return m_statistics_factory.factory_id; } }
		public eFACTORY_TYPE		pool_type							{ get { return m_statistics_factory.factory_type; } }
		public long					pool_statistics_allocated_create	{ get { return m_statistics_factory.alloc_create; } }
		public long					pool_statistics_allocated_stack		{ get { return m_statistics_factory.alloc_stacked; } }
		public long					pool_statistics_allocated			{ get { return (m_statistics_factory.alloc_stacked + m_statistics_factory.alloc_create); } }
		public long					pool_statistics_freed_stack			{ get { return m_statistics_factory.free_stacked; } }
		public long					pool_statistics_freed_delete		{ get { return m_statistics_factory.free_delete; } }
		public long					pool_statistics_freed				{ get { return (m_statistics_factory.free_stacked + m_statistics_factory.free_delete); } }
		public long					pool_statistics_existing_limits		{ get { return m_statistics_factory.existing_limits; } }
		public long					pool_statistics_existing			{ get { return m_statistics_factory.existing; } }
		public long					pool_statistics_in_using			{ get { return m_statistics_factory.in_using; } }
																		  
		public statistics.factory	pool_statistics_					{ get { return m_statistics_factory; } }
		public void					reset_factory_statistics()
		{
			// 1) Pool을 Reset한다.
			m_statistics_factory.alloc_stacked   = 0;
			m_statistics_factory.alloc_create	 = 0;
			m_statistics_factory.free_stacked	 = 0;
			m_statistics_factory.free_delete	 = 0;
			m_statistics_factory.existing_limits = long.MaxValue;
			m_statistics_factory.existing		 = 0;
			m_statistics_factory.in_using		 = 0;
		}

	// implementations) 
		private	string				m_name;
		protected statistics.factory m_statistics_factory;
	}
}
