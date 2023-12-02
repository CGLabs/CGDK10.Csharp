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
using System.Diagnostics;
using System.Reflection;


//-----------------------------------------------------------------------------
//
// eOBJECT_STATE
//
//    1) Initialize	eOBJECT_STATE::NONE    ->eOBJECT_STATE::INITIALIZING_PENDING ->eOBJECT_STATE::STOPPED
//    2) Start		eOBJECT_STATE::STOPPED ->eOBJECT_STATE::START_PENDING        ->eOBJECT_STATE::RUNNING
//    3) Stop		eOBJECT_STATE::RUNNING ->eOBJECT_STATE::STOP_PENDING		   ->eOBJECT_STATE::STOPPED
//    4) Destory	eOBJECT_STATE::STOPPED ->eOBJECT_STATE::DESTROYING_PENDING   ->eOBJECT_STATE::NONE
//    5) Pause		eOBJECT_STATE::RUNNING ->eOBJECT_STATE::PAUSE_PENDING		   ->eOBJECT_STATE::PAUSED
//    6) Continue	eOBJECT_STATE::PAUSED  ->eOBJECT_STATE::CONTINUE_PENDING	   ->eOBJECT_STATE::RUNNING
//
//-----------------------------------------------------------------------------
namespace CGDK
{
	public enum eOBJECT_STATE : uint
	{
		NONE					 = 0,
		STOPPED					 = 1,	// SERVICE_STOPPED
		START_PENDING			 = 2,	// SERVICE_START_PENDING
		STOP_PENDING			 = 3,	// SERVICE_STOP_PENDING
		RUNNING					 = 4,	// SERVICE_RUNNING
		CONTINUE_PENDING		 = 5,	// SERVICE_CONTINUE_PENDING
		PAUSE_PENDING			 = 6,	// SERVICE_PAUSE_PENDING
		PAUSED					 = 7,	// SERVICE_PAUSED
		INITIALIZING_PENDING	 = 8,
		DESTROYING_PENDING		 = 9,
		UPDATE					 = 10,
		COMMIT					 = 11,
		NOTCONNECT				 = 12,
		DEFAULT					 = 0xff
	};

	public enum eRESULT : uint
	{
		SUCCESS									 = 0x00000000,	// 0x0******* Success
		PENDED									 = 0x00000201,	//            Success setting and Execution Pended
		SUCCESS_CREATE							 = 0x00000202,	//            Success on creation new account
		SUCCESS_RESTORE							 = 0x00000203,	//            Success on Restore
		PENDING_								 = 0x00000204,	//            Pended

		_SUCCESS_USER							 = 0x00010000,	// 0x0001**** User Success

		FAIL									 = 0x03000000,	// 0x3******* Fail
		FAIL_DISABLED							 = 0x03000001,
		TIME_OUT								 = 0x03000012,
		ALEADY_EXIST							 = 0x03000003,
		EXPIRED									 = 0x03000014,
		NOT_EXIST								 = 0x03000005,
		NOT_READY								 = 0x03000006,
		BANNED_MEMBER							 = 0x03000017,
		MEMBER_FULL								 = 0x03000018,
		INVALID_MEMBER							 = 0x03000019,
		INVALID_PASSWORD						 = 0x0300001a,
		INVALID_PLATFORM						 = 0x0300001b,
		ALEADY_EXIST_NICKNAME					 = 0x0300001c,
		ALEADY_DONE								 = 0x0300001d,
		API_FAILURE								 = 0x0300001e,
		FAIL_TO_ACCEPT_SOCKET					 = 0x0300001f,
		FAIL_TO_ATTACHING_TO_EXECUTOR			 = 0x03000020,
		CONNECTION_FAILURE						 = 0x03000021,
		FAIL_TO_CREATE							 = 0x03000022,
		FAIL_TO_OPEN							 = 0x03000023,
		FAIL_TO_OPERATION						 = 0x03000024,
		FAIL_TO_CONNECT							 = 0x03000025,
		FAIL_NO_WEBSOCKET_KEY					 = 0x03010026,
		_FAIL_USER								 = 0x030f0000,

		CANCEL									 = 0x03100000,	// 0x2******* Cancel
		_CANCEL_USER							 = 0x031f0000,

		ERROR_									 = 0x03200000,	// 0x3******* Error
		LOGIC_ERROR								 = 0x03200001,
		LENGTH_ERROR							 = 0x03200002,
		DOMAIN_ERROR							 = 0x03200003,
		OUT_OF_RANGE							 = 0x03200004,
		RUNTIME_ERROR							 = 0x03200005,
		OVERFLOW_ERROR							 = 0x03200006,
		UNDERFLOW_ERROR							 = 0x03200007,
		RANGE_ERROR								 = 0x03200008,
		BAD_ALLOC								 = 0x03200009,
		BAD_FUNCTION_CALL						 = 0x0320000a,
		CONCURRENT_LOCK_ERROR					 = 0x0320000b,
		ACCESS_DENIED							 = 0x0320000c,
		NO_INITIALIZED							 = 0x0320000d,
		INVALID_ARGUMENT						 = 0x0320000e,
		NOT_ALLOWED								 = 0x03200010,
		INVALID_PARAMETER_HANDLE				 = 0x03200011,
		INVALID_MESSAGE_LENGTH					 = 0x03200012,
		INVALID_MESSAGE_BUFFER_IS_NULL			 = 0x03200013,
		INVALID_MESSAGE_TOO_SHORT_LENGTH		 = 0x03200014,
		INVALID_MESSAGE_TOO_LONG_LENGTH			 = 0x03200015,
		INVALID_STATE							 = 0x03200016,
		INVALID_HANDLE							 = 0x03200017,
		_ERROR_USER								 = 0x032f0000,

		EXCEPTION								 = 0x03400000,	// 0x4*** Exception
		BAD_EXCEPTION							 = 0x03400001,
		FORCE_CLOSE								 = 0x03400002,
		NOT_SUPPORT								 = 0x03400003,
		INVALID_CALL							 = 0x03400004,
		INVALID_LICENSE							 = 0x03400005,
		ABNORMAL_STATE							 = 0x03400006,
		UNEXPECTED_EXCEPTION					 = 0x034d0000,
		_EXCEPTION_USER							 = 0x034f0000,

		PROCESS									 = 0x03500000,	// 0x5*** Process
		REQUEST									 = 0x03500001,
		COMPLETE								 = 0x03500002,
		RESET									 = 0x03500003,
		RESTORE									 = 0x03500004,
		_PROCESS_USER							 = 0x035f0000,

		_USER									 = 0x03600000,

		CGCII_EXPIRED							 = 0x7e000000,
		NOT_DEFINED								 = 0x7f000000	// 0xff** Not Defined
	};

	public enum eOBJECT_TYPE : uint
	{
		// 1) Not Defined
		NOT_DEFINED								 = 0x00000000,

		// 2) Common
		EXECUTOR								 = 0x00100000,
		EXECUTOR_THREAD							 = 0x00100001,
		EXECUTOR_SCHEDULABLE					 = 0x00100002,
		EXECUTOR_COMPLETION_PORT				 = 0x00100003,
		EXECUTOR_COMPLETION_PORT_THREAD			 = 0x00100004,
		EXECUTOR_LIST							 = 0x00100005,
		EXECUTOR_LIST_PRIORITY					 = 0x00100006,
		EXECUTOR_LIST_THREAD					 = 0x00100007,
		EXECUTOR_QUEUED							 = 0x00100008,
		EXECUTOR_QUEUED_THREAD					 = 0x00100009,
		EXECUTOR_SINGLE_EXECUTE					 = 0x0010000a,
		EXECUTOR_SINGLE_EXECUTE_THREAD			 = 0x0010000b,
		EXECUTABLE								 = 0x0011000c,
		EXECUTABLE_BATCH_FUNCTION				 = 0x0011000d,
		EXECUTABLE_BATCH_FUNCTION_MEMBER		 = 0x0011000e,
		EXECUTABLE_BATCH_NOTIFY					 = 0x0011000f,
		EXECUTABLE_BATCH_WAIT					 = 0x00110010,
		EXECUTABLE_FUNCTION						 = 0x00110011,
		EXECUTABLE_FUNCTION_MEMBER				 = 0x00110012,
		EXECUTABLE_LIST							 = 0x00110013,
		EXECUTABLE_EXECUTABLE					 = 0x00110014,
		SCHEDULABLE								 = 0x00120000,
		SCHEDULABLE_EXECUTABLE					 = 0x00120001,
		SCHEDULABLE_FUNCTION					 = 0x00120002,
		SCHEDULABLE_FUNCTION_MEMBER				 = 0x00120003,
		SCHEDULABLE_LIST						 = 0x00120004,
		SCHEDULABLE_EXCEPTION_OBSERVER			 = 0x00120005,
		SCHEDULABLE_FACTORY_GARBAGE				 = 0x00120006,
		UPDATABLE								 = 0x00130000,
		UPDATABLE_BATCH							 = 0x00130001,
		FILE									 = 0x00140000,

		// 3) Socket
		SOCKET									 = 0x00200000,
		ACCEPTOR								 = 0x00210000,
		CONNECTOR								 = 0x00220000,

		// 4) Server
		EVENT_OBJECT							 = 0x0030000,
		EVENT_OBJECT_ITERATION					 = 0x0030001,
		EVENT_OBJECT_ITERATION_FUNCTION			 = 0x0030002,
		EVENT_OBJECT_ONCE						 = 0x0030003,
		EVENT_OBJECT_ONCE_FUNCTION				 = 0x0030004,
		EVENT_OBJECT_SCHEDULE					 = 0x0030005,
		EVENT_OBJECT_SCHEDULE_FUNCTION			 = 0x0030006,
		EVENT_OBJECT_START_END					 = 0x0030007,
		EVENT_OBJECT_START_END_FUNCTION			 = 0x0030008,
		EVENT_OBJECT_START_ITERATION_END		 = 0x0030009,
		EVENT_OBJECT_START_ITERATION_END_FUNCTION= 0x003000a,
		EVENT_SCHEDULE							 = 0x0031000,
		EVENT_SCHEDULE_FUNCTION					 = 0x0031001,
		EVENT_SCHEDULE_LAMBDA					 = 0x0031002,


		// 4) Server System
		GROUP									 = 0x00400000,
		GROUP_COUNT								 = 0x00410000,
		GROUP_LIST								 = 0x00420000,
		GROUP_ARRAY								 = 0x00430000,

		// 6) Account
		REPOSITORY								 = 0x00500000,
		REPOSITORY_DB							 = 0x00511000,
		REPOSITORY_REDIS						 = 0x00512000,
		REPOSITORY_HUB							 = 0x00513000,
		REPOSITORY_REMOTE						 = 0x00514000,
		ACCESSABLE								 = 0x00520000,
		ACCESSABLE_HOLDER						 = 0x00521000,
		DEPOT									 = 0x00530000,
		DEPOT_REMOTE							 = 0x00531000,

		// 7) Querier
		QUERIER									 = 0x006000000,
		QUERIER_DB_SQL							 = 0x006100000,
		QUERIER_DB_CASSANDRA					 = 0x006200001,
		QUERIER_DB_MONGODB						 = 0x006200002,
		QUERIER_REDIS							 = 0x006200003,
		QUERIER_HTTP							 = 0x006300000,

		// 16) 
		MASK									 = 0x00ffff00,
		MASK_ALL								 = 0xffffffff
	};

	public static class Function
	{
		public static DateTime	get_compile_date()
		{
			var vi		 = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
			var fileInfo = new System.IO.FileInfo(vi.FileName);

			return	fileInfo.CreationTime;
		}
	}
}
