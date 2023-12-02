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

namespace CGDK.Server.TimeEvent
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
		public	string			Name;
		public	eTYPE			Type;			//  = eTYPE.NONE;

		public	DateTime		timeExecute;
		public	TimeSpan		timeInterval;
		public	int				countTimes;	//  = 0;
		
		public	sEVENT_SETTING(string _name, eTYPE _type, DateTime _time_execute, TimeSpan _time_interval, int _count_times = 0)
		{
			this.Name = _name;
			this.Type = _type;
			this.timeExecute = _time_execute;
			this.timeInterval = _time_interval;
			this.countTimes = _count_times;
		}
	};

	public struct sEVENT_STATUS
	{
		public	UInt64			Id;		// 0;
		public	eSTATE			State;	// eSTATE.DONE;

		public	DateTime		timeLastExecuted;
		public	DateTime		timeNext;

		public	Int64			countRemained;


		public	void			Reset()
		{
			Id = 0;
			State = eSTATE.NONE;
			timeLastExecuted = default(DateTime);
			timeNext = default(DateTime);
			countRemained = 0;
		}
	};

	public struct sENTITY_SETTING
	{
		public string				Name;
		public Int64				Type; //  = 0;

		public eSETTER				Setter;	//  = eSETTER.NONE;
		public int					Level; // = 0;
	}

	public struct sENTITY_STATUS
	{
		public UInt64				Id;	// = 0;
		public eSTATE				State; // = eSTATE.NONE;

		public DateTime				timeSetup; // = DateTime.MinValue;
		public DateTime				timeLastTry; // = DateTime.MinValue;
		public DateTime				timeLastSucceeded; // = DateTime.MinValue;
		public DateTime				timeLastFailed; // = DateTime.MinValue;
		public DateTime				timeNext; // = DateTime.MinValue;

		public Int64				countTry; //  = 0;
		public Int64				countSucceeded; //  = 0;
		public Int64				countFailed; //  = 0;

		public void					Reset()
		{
			Id = 0;
			State = eSTATE.NONE;
			timeSetup = DateTime.MinValue;
			timeLastTry = DateTime.MinValue;
			timeLastSucceeded = DateTime.MinValue;
			timeLastFailed = DateTime.MinValue;
			timeNext = DateTime.MinValue;
			countTry = 0;
			countSucceeded = 0;
			countFailed = 0;
		}
		public void					ResetNextTime() { timeLastFailed = new DateTime(0); }

		public void					StatisticsTry() { Interlocked.Increment(ref countTry); timeLastTry = DateTime.UtcNow;}
		public void					StatisticsSucceeded() { Interlocked.Increment(ref countSucceeded); timeLastSucceeded = DateTime.UtcNow; }
		public void					StatisticsFailed() { Interlocked.Increment(ref countFailed); timeLastFailed = DateTime.UtcNow; }
	}


}
