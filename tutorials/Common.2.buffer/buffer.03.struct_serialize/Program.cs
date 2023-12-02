//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                    tutorials buffer - struct_serialize                    *
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
using System.Collections.Generic;
using CGDK;
using CGDK.factory;

namespace tutorial.buffer._04.struct_serialize.console
{
	class Program
	{
		// ------------------------------------------------------------------
		// 1. 구조체(struct)를 사용한 직렬화
		//    간단히 그냥 구조체를 정의해주고 동일하게 append/extract해주면 된다.
		// ------------------------------------------------------------------
		public struct MESSAGE_A
		{
			public int x;
			public string y;
			public List<int> z;
		}

		public static void tutorial_buffer_1()
		{
			// var 메시지를 작성한다.
			MESSAGE_A foo = new MESSAGE_A();
			foo.x = 100;
			foo.y = "Test String";
			foo.z = new List<int> { 10, 20, 30 };

			// - 버퍼를 할당한다.
			var baz = memory.alloc_buffer(1000);

			// 1) 데이터를 append한다.
			baz.append<MESSAGE_A>(foo);

			// - baz의 size_값을 출력한다. 
			Console.WriteLine("size_: " + baz.size);

			// 2) 데이터를 읽어낸다.
			var v1 = baz.extract<MESSAGE_A>();

			// - v1 출력
			Console.WriteLine(v1.x);
			Console.WriteLine(v1.y);
			foreach (var iter in v1.z)
				Console.WriteLine(iter);
		}


		// ------------------------------------------------------------------
		// 2. 구조체(struct)를 사용한 직렬화의 단점
		//    상속이 되지 않는다. 따라서 상속 대신 포함으로 해야 한다.
		// ------------------------------------------------------------------
		public struct MESSAGE_B
		{
			public int x;
			public int y;
		}

		public struct MESSAGE_C
		{
			public MESSAGE_B a;
			public string	 b;
			public float     c;
		}


		public static void tutorial_buffer_2()
		{
			// - 메시지를 작성한다.
			MESSAGE_C foo = new MESSAGE_C();
			foo.a = new MESSAGE_B();
			foo.a.x = 10;
			foo.a.y = 20;
			foo.b = "Test String";
			foo.c = 20.0f;

			// - 버퍼를 할당한다.
			var baz = memory.alloc_buffer(1000);

			// 1) 데이터를 append한다.
			baz.append<MESSAGE_C>(foo);

			// - baz의 size_값을 출력한다. 
			Console.WriteLine("size_: " + baz.size);

			// 2) 데이터를 읽어낸다.
			var v1 = baz.extract<MESSAGE_C>();

			// - v1 출력
			Console.WriteLine(v1.a.x + "  " + v1.a.y);
			Console.WriteLine(v1.b);
			Console.WriteLine(v1.c);
		}


		// ------------------------------------------------------------------
		// main function
		// ------------------------------------------------------------------
		static void Main(string[] args)
		{
			// 1)
			tutorial_buffer_1();
			tutorial_buffer_2();
		}
	}
}
