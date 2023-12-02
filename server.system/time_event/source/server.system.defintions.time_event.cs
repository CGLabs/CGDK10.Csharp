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

//----------------------------------------------------------------------------
//
//
//  Definitions for Event Classes
//
//
//----------------------------------------------------------------------------
using System;
using System.Threading;

namespace CGDK.server.time_event
{
	public enum eSETTER : Int32
	{
		NONE		 = 0,

		SYSTEM		 = 1,
		LOCAL		 = 2,	
		ADMIN		 = 3,

		MAX
	}

	public enum eSTATE : Int32
	{
		NONE		 = 0,

		WAIT		 = 1,
		RUN			 = 2,
		DONE		 = 3,

		MAX
	}

	public enum eTYPE : Int32
	{
		NONE			 = 0,

		ONCE			 = 1,	// 1
		ITERATION,				// 2
		START_END,				// 3
		START_ITERATION_END,	// 4
		SCHEDULE,				// 5
		SCHEDULE_FUNCTION,		// 6
		CUSTOM,					// 7

		MAX
	}

	public struct sEVENT_SETTING
	{
		public	string			name;
		public	eTYPE			type;			//  = eTYPE.NONE;

		public	DateTime		time_execute;
		public	TimeSpan		time_interval;
		public	int				count_times;	//  = 0;
		
		public	sEVENT_SETTING(string _name, eTYPE _type, DateTime _time_execute, TimeSpan _time_interval, int _count_times = 0)
		{
			name = _name;
			type = _type;
			time_execute = _time_execute;
			time_interval = _time_interval;
			count_times = _count_times;
		}
	};

	public struct sEVENT_STATUS
	{
		public	UInt64			id;		// 0;
		public	eSTATE			state;	// eSTATE.DONE;

		public	DateTime		time_last_executed;
		public	DateTime		time_next;

		public	Int64			count_remained;


		public	void			reset()
		{
			id = 0;
			state = eSTATE.NONE;
			time_last_executed = default(DateTime);
			time_next = default(DateTime);
			count_remained = 0;
		}
	};

	public struct sENTITY_SETTING
	{
		public string				name;
		public Int64				type; //  = 0;

		public eSETTER				setter;	//  = eSETTER.NONE;
		public int					level; // = 0;
	}

	public struct sENTITY_STATUS
	{
		public UInt64				id;	// = 0;
		public eSTATE				state; // = eSTATE.NONE;

		public DateTime				time_setup; // = DateTime.MinValue;
		public DateTime				time_last_try; // = DateTime.MinValue;
		public DateTime				time_last_succeeded; // = DateTime.MinValue;
		public DateTime				time_last_failed; // = DateTime.MinValue;
		public DateTime				time_next; // = DateTime.MinValue;

		public Int64				count_try; //  = 0;
		public Int64				count_succeeded; //  = 0;
		public Int64				count_failed; //  = 0;

		public void					reset()
		{
			id = 0;
			state = eSTATE.NONE;
			time_setup = DateTime.MinValue;
			time_last_try = DateTime.MinValue;
			time_last_succeeded = DateTime.MinValue;
			time_last_failed = DateTime.MinValue;
			time_next = DateTime.MinValue;
			count_try = 0;
			count_succeeded = 0;
			count_failed = 0;
		}
		public void					reset_next_time() { time_last_failed = new DateTime(0); }

		public void					statistics_try() { Interlocked.Increment(ref count_try); time_last_try = DateTime.UtcNow;}
		public void					statistics_succeeded() { Interlocked.Increment(ref count_succeeded); time_last_succeeded = DateTime.UtcNow; }
		public void					statistics_failed() { Interlocked.Increment(ref count_failed); time_last_failed = DateTime.UtcNow; }
	}


}
