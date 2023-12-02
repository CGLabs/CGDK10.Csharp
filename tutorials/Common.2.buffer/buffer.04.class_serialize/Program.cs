//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     tutorials buffer - class_serialize                    *
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

namespace tutorial.buffer._05.class_serialize.console
{
	class Program
	{
		// ------------------------------------------------------------------
		// 1. 클래스(class)를 사용한 직렬화
		//    구조체와 마찬가지로 정의후 그냥 append/extract해주면 된다.
		//    클래스는 선언시동적 할당이 발생하므로 구조체를 사용하는 것에 비해
		//    비용이 비쌀 수 있다는 단점이 있으므로 잘 사용할 필요가 있다.
		//    간단하고 빈번하게 사용하는 경우 구조체를 사용하고
		//    복잡하고 빈번하게 사용하지 않는 경우 클래스를 사용하면 효과적일 것이다.
		// ------------------------------------------------------------------
		[System.Serializable]
		public class MESSAGE_A
		{
			public int x;
			public string y;
			public List<int> z;
		}

		public static void tutorial_buffer_1()
		{
			// - 메시지를 작성한다.
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
		// 2. 클래스는 상속의 사용이 가능하다.
		//    구조체는 상속을 사용할 수 없었다. 하지만 클래스는 사용이 가능하다.
		//    따라서 상속을 사용해 메시지의 확장도 가능하며 생성자 등을 이용해
		//    다양한 초기화 역시 가능하다.
		// ------------------------------------------------------------------
		public class MESSAGE_B
		{
			public int x;
			public int y;
		}

		[System.Serializable]
		public class MESSAGE_C : MESSAGE_B
		{
			public string z;
			public float w;
		}

		public static void tutorial_buffer_2()
		{
			// - 메시지를 작성한다.
			var foo = new MESSAGE_C();
			foo.x = 100;
			foo.y = 150;
			foo.z = "Test String";
			foo.w = 3.141592f;

			// 1) foo 객체 크기를 얻어 그 크기만큼 메모리를 할당받는다.
			var baz = memory.alloc_buffer(1000);

			// 2) 데이터를 append한다.
			baz.append<MESSAGE_C>(foo);

			// - baz의 size_값을 출력한다. 
			Console.WriteLine("size_: " + baz.size);

			// 2) 데이터를 읽어낸다.
			var v1 = baz.extract<MESSAGE_C>();

			// - v1 출력
			Console.WriteLine(v1.x + "  " + v1.y);
			Console.WriteLine(v1.z);
			Console.WriteLine(v1.w);
		}

		// ------------------------------------------------------------------
		// 3. get_size_of를 사용한 메모리 할당
		//    get_size_of 함수를 사용해 딱 직렬화에 필요한 만큼 메모리를 할당할
		//    수가 있다.
		// ------------------------------------------------------------------
		public static void tutorial_buffer_3()
		{
			// - 메시지를 작성한다.
			var foo = new MESSAGE_A();
			foo.x = 100;
			foo.y = "Test String";
			foo.z = new List<int> { 10, 20, 30 };

			// 1) foo 객체 크기를 얻어 그 크기만큼 메모리를 할당받는다.
			var baz = memory.alloc_buffer(CGDK.buffer.get_size_of(foo));

			// 2) 데이터를 append한다.
			baz.append<MESSAGE_A>(foo);

			// - baz의 size_값을 출력한다. 
			Console.WriteLine("size_: " + baz.size);

			// 2) 데이터를 읽어낸다.
			var v1 = baz.extract<MESSAGE_A>();

			// - v1 출력
			Console.WriteLine(v1.x);
			Console.WriteLine(v1.y);
			Console.WriteLine(v1.z);
		}

		// ------------------------------------------------------------------
		// 4. 직렬화 옵션
		//    구조치나 클래스의 직렬화시 몇가지 다양한 옵션의 사용이 가능하다.
		//
		//    1) [System.NonSerialized]
		//    해당 멤버르르 직렬화 대상에서 제외한다.
		//
		//    2) [CGDK.Attribute.StaticArray( SIZE )]
		//    SIZE크기의 고정크기 배열로 정의한다. C++의 배열과 호환을 위해 사용한다.
		//
		// ------------------------------------------------------------------
		[System.Serializable]
		public class MESSAGE_D
		{
			[System.NonSerialized]
			public int x;
			public int y;

			[CGDK.Attribute.StaticArray(8)]
			public int[] z;
		}

		public static void tutorial_buffer_4()
		{
			// - 메시지를 작성한다.
			var foo = new MESSAGE_D();
			foo.x = 100;
			foo.y = 50;

			// 1) foo 객체 크기를 얻어 그 크기만큼 메모리를 할당받는다.
			var baz = memory.alloc_buffer(CGDK.buffer.get_size_of(foo));

			// 2) 데이터를 append한다.
			baz.append<MESSAGE_D>(foo);

			// - baz의 size_값을 출력한다. 
			Console.WriteLine("size_: " + baz.size);

			// 2) 데이터를 읽어낸다.
			var v1 = baz.extract<MESSAGE_D>();

			// - v1 출력
			Console.WriteLine(v1.x);
			Console.WriteLine(v1.y);
			foreach (var iter in v1.z)
				Console.Write(iter + " ");
			Console.Write("\n");
		}


		// ------------------------------------------------------------------
		// main function
		// ------------------------------------------------------------------
		static void Main(string[] args)
		{
			// 1)
			tutorial_buffer_1();
			tutorial_buffer_2();
			tutorial_buffer_3();
			tutorial_buffer_4();
		}
	}
}
