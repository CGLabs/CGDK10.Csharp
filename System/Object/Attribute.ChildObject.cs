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
//
//  <<interface>> CGDK.Attribute.ChildObjbect
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
namespace Attribute
{
	public class ChildObjbect : System.Attribute
	{
		public ChildObjbect()
		{
		}
		public ChildObjbect(string _name)
		{
			m_Name = _name;
		}

		public	string		Name
		{
			get => this.m_Name;
		}

		private	string		m_Name;
	}
}
}
