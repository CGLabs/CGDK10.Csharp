//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                         Socket Classes Sample (1)                         *
//*                      TCP Echo Test Server (WinForms)                      *
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
using CGDK;
using CGDK.net;


// ----------------------------------------------------------------------------
//
// Definition
//
// ----------------------------------------------------------------------------
namespace MESSAGE_struct
{
	public struct HEADER
	{
		public int			size;
		public eMESSAGE_TYPE message_type;	

	}

	public struct A
	{
		public HEADER		header;

		public int			x;
		public int			y;
		public int			z;
	}

	public struct B
	{
		public HEADER		header;

		public int			x;
		public int			y;
		public int			z;
	}

	public struct C
	{
		public HEADER		header;

		public int			x;
		public int			y;
		public int			z;
	}

	public struct D
	{
		public HEADER		header;

		public int			x;
		public int			y;
		public int			z;
	}

	public struct E
	{
		public HEADER		header;

		public int			x;
		public int			y;
		public int			z;
	}

	public struct F
	{
		public HEADER		header;

		public int			x;
		public int			y;
		public int			z;
	}

	public struct G
	{
		public HEADER		header;

		public int			x;
		public int			y;

        [CGDK.Attribute.StaticArray(16)]
		public int[]		z;
	}
}