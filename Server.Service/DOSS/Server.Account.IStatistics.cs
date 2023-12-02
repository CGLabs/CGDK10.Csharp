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
using System;
using System.Diagnostics;

namespace CGDK
{
namespace CGServerAccount
{
namespace Statistics
{

public struct  Transactable
{
	public Int64		count_ProcessRequest_Immidiately;
	public Int64		count_ProcessRequest_Pended;
	public Int64		count_ProcessingRequest;
	public Int64		count_Success;
	public Int64		count_Fail;
}

public struct  Account
{
	public Int64		countCreateAccount_Request;
	public Int64		countCreateAccount_Complete;
	public Int64		countCreateAccount_Success;
	public Int64		countCreateAccount_Fail;
	public Int64		countLogin_Request;
	public Int64		countLogin_Success;
	public Int64		countLogin_Fail;
	public Int64		countLogout_Request;
	public Int64		countLogout_Complete;
	public Int64		countCheckin_Request;
	public Int64		countCheckin_Success;
	public Int64		countCheckin_Fail;
	public Int64		countCheckout_Request;
	public Int64		countCheckout_Success;
}

public struct  Repository
{
	public Int64		m_statisticsTransactable_Request;
	public Int64		m_statisticsTransactable_Complete;

	public Int64		m_statisticsLogin;
	public Int64		m_statisticsLogin_Request;
	public Int64		m_statisticsLogin_Success;
	public Int64		m_statisticsLogin_Fail;
	public Int64		m_statisticsLogout_Request;
	public Int64		m_statisticsLogout_Complete;

	public Int64		m_statisticsCheckin;
	public Int64		m_statisticsCheckin_Request;
	public Int64		m_statisticsCheckin_Success;
	public Int64		m_statisticsCheckin_Fail;
	public Int64		m_statisticsCheckout_Request;
	public Int64		m_statisticsCheckout_Complete;

	public Int64		m_statisticsCreate_Request;
	public Int64		m_statisticsCreate_Success;
	public Int64		m_statisticsCreate_Fail;

	public Int64		m_statisticsTransaction_Request;
	public Int64		m_statisticsTransaction_Success;
	public Int64		m_statisticsTransaction_Fail;
}


}
}
}