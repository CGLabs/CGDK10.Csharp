//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                    tutorials object - object_hierarchy                    *
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
using CGDK;

namespace tutorial_object_04_object_hierarchy
{
	//-----------------------------------------------------------------------------
	//
	// case 1) object hierarchy
	//
	// 상속을 통해 object_stateable를 구한할 경우 간단히 on_ 함수들만 재정의해 주면 된다.
	// 
	// 하지만 C#에서는 다중상속을 지원해주지 않기 때문에 IS-A(상속)을 이미 다른 클래스를
	// 상속해야 한다면 IS-A(상속)을 통해 구현할 수가 없을 수가 있다.
	// 그럴 경우 (case 2)와 같이 HAS-A(포함)을 사용해 구현해야 한다.
	// 
	// 
	// 
	//
	//-----------------------------------------------------------------------------

	class aoo : Nobject_stateable_nameable
	{
		protected override void on_initializing(context _context)	{ Console.WriteLine(name + ".on_initializing!"); }
		protected override void on_initialize(context _context)		{ Console.WriteLine(name + ".on_initialize!"); }
		protected override void on_destroying()						{ Console.WriteLine(name + ".on_destroying!"); }
		protected override void on_destroy()						{ Console.WriteLine(name + ".on_destroy!"); }
		protected override void on_starting(context _context)		{ Console.WriteLine(name + ".on_starting!"); }
		protected override void on_start(context _context)			{ Console.WriteLine(name + ".on_start!"); }
		protected override void on_stopping()						{ Console.WriteLine(name + ".on_stopping!"); }
		protected override void on_stop()							{ Console.WriteLine(name + ".on_stop!"); }
	}

	class boo : Nobject_stateable_nameable
	{
		protected override void on_initializing(context _context)	{ Console.WriteLine(name + ".on_initializing!"); }
		protected override void on_initialize(context _context)		{ Console.WriteLine(name + ".on_initialize!"); }
		protected override void on_destroying()						{ Console.WriteLine(name + ".on_destroying!"); }
		protected override void on_destroy()						{ Console.WriteLine(name + ".on_destroy!"); }
		protected override void on_starting(context _context)		{ Console.WriteLine(name + ".on_starting!"); }
		protected override void on_start(context _context)			{ Console.WriteLine(name + ".on_start!"); }
		protected override void on_stopping()						{ Console.WriteLine(name + ".on_stopping!"); }
		protected override void on_stop()							{ Console.WriteLine(name + ".on_stop!"); }

		[CGDK.Attribute.ChildObjbect("a")]
		public aoo				child_a = null;
	}

	class coo : Nobject_stateable_nameable
	{
		protected override void on_initializing(context _context)	{ Console.WriteLine("X.on_initializing!"); }
		protected override void on_initialize(context _context)		{ Console.WriteLine("X.on_initialize!"); }
		protected override void on_destroying()						{ Console.WriteLine("X.on_destroying!"); }
		protected override void on_destroy()						{ Console.WriteLine("X.on_destroy!"); }
		protected override void on_starting(context _context)		{ Console.WriteLine("X.on_starting!"); }
		protected override void on_start(context _context)			{ Console.WriteLine("X.on_start!"); }
		protected override void on_stopping()						{ Console.WriteLine("X.on_stopping!"); }
		protected override void on_stop()							{ Console.WriteLine("X.on_stop!"); }

		[CGDK.Attribute.ChildObjbect("b")]
		public aoo				child_a = null;

		[CGDK.Attribute.ChildObjbect("c")]
		public boo				child_b = null;
	}

	class Program
	{
		static void Main(string[] args)
		{
			// 1) ...
			var temp = new coo();

			// 2) 
			temp.start(new context());

			// 3) 
			temp.destroy();
		}
	}
}
