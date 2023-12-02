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

//----------------------------------------------------------------------------
//  <<interface>> CGDK.ICGDestroyable
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
namespace Account
{
namespace Attribute
{
		public class Synchronizable : System.Attribute
		{
			public Synchronizable()
			{
			}
		}

		public class ReadOnly : System.Attribute
		{
			public ReadOnly()
			{
			}
		}

		public class Writeable : System.Attribute
	{
		public Writeable()
		{
			m_bUpdated	 = false;
		}

		public bool					Update
		{
			get
			{
				return	m_bUpdated;

			}

			set
			{
				m_bUpdated	 = value;
			}
		}

		private	bool				m_bUpdated;
	}
}
}
}