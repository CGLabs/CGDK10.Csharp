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

using System.Collections;

//----------------------------------------------------------------------------
//  <<interface>> CGDK.ICGExecutable
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public class NObjectStateableNameable :
		NObjectStateable,
		INameable
	{
		// public) 
		public string Name 
		{
			get { return this.m_name; }
			set { this.m_name = value; }
		}

		// implementation) 
		private string m_name;
	}
}
