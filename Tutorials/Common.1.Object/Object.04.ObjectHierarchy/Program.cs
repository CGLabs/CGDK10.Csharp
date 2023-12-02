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

	class Aoo : NObjectStateableNameable
	{
		protected override void OnInitializing(Context _context)	{ Console.WriteLine(Name + ".OnInitializing!"); }
		protected override void OnInitialize(Context _context)		{ Console.WriteLine(Name + ".OnInitialize!"); }
		protected override void OnDestroying()						{ Console.WriteLine(Name + ".OnDestroying!"); }
		protected override void OnDestroy()							{ Console.WriteLine(Name + ".OnDestroy!"); }
		protected override void OnStarting(Context _context)		{ Console.WriteLine(Name + ".OnStarting!"); }
		protected override void OnStart(Context _context)			{ Console.WriteLine(Name + ".OnStart!"); }
		protected override void OnStopping()						{ Console.WriteLine(Name + ".OnStopping!"); }
		protected override void OnStop()							{ Console.WriteLine(Name + ".OnStop!"); }
	}

	class Boo : NObjectStateableNameable
	{
		protected override void OnInitializing(Context _context)	{ Console.WriteLine(Name + ".OnInitializing!"); }
		protected override void OnInitialize(Context _context)		{ Console.WriteLine(Name + ".OnInitialize!"); }
		protected override void OnDestroying()						{ Console.WriteLine(Name + ".OnDestroying!"); }
		protected override void OnDestroy()							{ Console.WriteLine(Name + ".OnDestroy!"); }
		protected override void OnStarting(Context _context)		{ Console.WriteLine(Name + ".OnStarting!"); }
		protected override void OnStart(Context _context)			{ Console.WriteLine(Name + ".OnStart!"); }
		protected override void OnStopping()						{ Console.WriteLine(Name + ".OnStopping!"); }
		protected override void OnStop()							{ Console.WriteLine(Name + ".OnStop!"); }

		[CGDK.Attribute.ChildObjbect("a")]
		public Aoo				child_a = null;
	}

	class Coo : NObjectStateableNameable
	{
		protected override void OnInitializing(Context _context)	{ Console.WriteLine("X.OnInitializing!"); }
		protected override void OnInitialize(Context _context)		{ Console.WriteLine("X.OnInitialize!"); }
		protected override void OnDestroying()						{ Console.WriteLine("X.OnDestroying!"); }
		protected override void OnDestroy()							{ Console.WriteLine("X.OnDestroy!"); }
		protected override void OnStarting(Context _context)		{ Console.WriteLine("X.OnStarting!"); }
		protected override void OnStart(Context _context)			{ Console.WriteLine("X.OnStart!"); }
		protected override void OnStopping()						{ Console.WriteLine("X.OnStopping!"); }
		protected override void OnStop()							{ Console.WriteLine("X.OnStop!"); }

		[CGDK.Attribute.ChildObjbect("b")]
		public Aoo				child_a = null;

		[CGDK.Attribute.ChildObjbect("c")]
		public Boo				child_b = null;
	}

	class Program
	{
		static void Main()
		{
			// 1) ...
			var temp = new Coo();

			// 2) 
			temp.Start(new Context());

			// 3) 
			temp.Destroy();
		}
	}
}
