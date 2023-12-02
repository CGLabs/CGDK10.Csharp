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
namespace MESSAGE_class
{
	[CGDK.Attribute.Serializable]
	public class HEADER
	{
		[CGDK.Attribute.Field]
		public int			size;

		[CGDK.Attribute.Field]
		public eMESSAGE_TYPE message_type;	
	}

	[CGDK.Attribute.Serializable]
	public class A : HEADER
	{
		[CGDK.Attribute.Field]
		public int			x;

		[CGDK.Attribute.Field]
		public int			y;

		[CGDK.Attribute.Field]
		public int			z;
	}

	[CGDK.Attribute.Serializable]
	public class B : HEADER
	{
		[CGDK.Attribute.Field]
		public int			x;

		[CGDK.Attribute.Field]
		public int			y;

		[CGDK.Attribute.Field]
		public int			z;
	}

	[CGDK.Attribute.Serializable]
	public class C : HEADER
	{
		[CGDK.Attribute.Field]
		public int			x;

		[CGDK.Attribute.Field]
		public int			y;

		[CGDK.Attribute.Field]
		public int			z;
	}

	[CGDK.Attribute.Serializable]
	public class D : HEADER
	{
		[CGDK.Attribute.Field]
		public int			x;

		[CGDK.Attribute.Field]
		public int			y;

		[CGDK.Attribute.Field]
		public int			z;
	}

	[CGDK.Attribute.Serializable]
	public class E : HEADER
	{
		[CGDK.Attribute.Field]
		public int			x;

		[CGDK.Attribute.Field]
		public int			y;

		[CGDK.Attribute.Field]
		public int			z;
	}

	[CGDK.Attribute.Serializable]
	public class F : HEADER
	{
        [CGDK.Attribute.Field]
		public int			x;

		[CGDK.Attribute.Field]
		public int			y;

		[CGDK.Attribute.Field]
		public int			z;
	}

	[CGDK.Attribute.Serializable]
	public class G : HEADER
	{
		[CGDK.Attribute.Field]
		public int			x;

		[CGDK.Attribute.Field]
		public int			y;

		[CGDK.Attribute.Field]
		public int[]		z;
	}
}