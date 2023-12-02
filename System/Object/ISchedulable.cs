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
//  <<interface>> CGDK.ISchedulable
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public interface ISchedulable
	{
		//! @brief 등록될 때 호출되는 함수
		void				ProcessOnRegister();

		//! @brief 등록이 해제될 때 호출되는 함수
		void				ProcessOnUnregister();

		//! @brief 스케쥴 처리를 위해 호출되는 함수.
		void				ProcessSchedule();

		IExecutor			Executor { get; set;}
	}
}