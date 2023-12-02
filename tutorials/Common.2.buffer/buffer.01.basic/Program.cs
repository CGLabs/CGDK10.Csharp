//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                         tutorials buffer - basic                          *
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
using CGDK.factory;

namespace tutorial.buffer._01.basic.console
{
	class Program
	{
		static void Main(string[] args)
		{
			// ------------------------------------------------------------------
			// 1. CGDK.buffer란?
			// ------------------------------------------------------------------
			{
				// 1) CGDK.buffer -> CGDK에서 사용하는 버퍼 객체
				CGDK.buffer baz;

				// 2) memory.alloc_buffer()함수를 사용해 메머리를 할당받을 수 있다.
				//    - 1000 byte의 메모리를 할당받는다.
				//    - 그리고 data_값에 할당받은 메모리의 포인터를 넣는다.
				//    - size_(버퍼의 길이)는 0으로 초기화 한다.
				baz = memory.alloc_buffer(1000);

				// 3) 버퍼의 offset과 size를 출력한다.
				Console.Write ("offset: " + baz.offset + "  size_: " + baz.size + '\n');
			}

			// ------------------------------------------------------------------
			// 2. CGDK.buffer는 스마트포인터 기능을 가지고 있다.
			// ------------------------------------------------------------------
			{
				// 1) CGDK.buffer는 스마트 포인터의 기능을 가지고 있다.
				{
					// - 2000 byte를 할당받는다.
					CGDK.buffer buf_x = memory.alloc_buffer(2000);

					// - 이 블럭이 끝나면 buf_x에 할당받은 버퍼는 자동적으로 소멸된다.
				}
			}

			// ------------------------------------------------------------------
			// 3. 버퍼에 값을 써넣고 일기
			// ------------------------------------------------------------------
			{
				// - CGDK.buffer에 1000 byte할당받기
				var baz = memory.alloc_buffer(1000);

				// - 먼저 baz의 offset과 size값을 출력한다.
				Console.Write("offset: " + baz.offset + "  size_: " + baz.size + '\n');

				// 1) append()함수를 사용해 baz에 값을 써넣는다.
				baz.append<int>(100);	  // int형으로 100을 써넣는다.
				baz.append<float>(3.14f); // float형으로 3.14f을 써넣는다.
				baz.append<ulong>(123);  // ulong_t형으로 123을 써넣는다.

				// - 이렇게 append를 사용해 int(100)-4byte, float(3.14f)-4byte, ulong_t(123)-8byte을 써넣으면
				//   총 크기인 16byte만큼 size_값이 늘어나게 된다.
				Console.Write("offset: " + baz.offset + "  size_: " + baz.size + '\n');

				// 2) extract()함수를 사용해 baz에서 값을 읽어낸다.
				//    써넣은 순서와 자료형 대로 값을 일어낸다.
				var v1 = baz.extract<int>();
				var v2 = baz.extract<float>();
				var v3 = baz.extract<ulong>();

				// - 읽어낸 값 출력
				Console.Write("v1: " + v1 + "  v2: " + v2 + "  v3: " + v3 + '\n');

				// - extract()함수를 사용해 써넣은 대로 값을 읽어내면 
				///  총크기인 16byte만큼 size_는 줄어들게 되며 포인터인 data값은 읽어낸 크기(16byte)만큼 증가하게 된다.
				Console.Write("offset: " + baz.offset + "  size_: " + baz.size + '\n');
			}

			// ------------------------------------------------------------------
			// 4. 값을 읽기만 하기(peek)
			// ------------------------------------------------------------------
			{
				// - baz
				var baz = memory.alloc_buffer(1000);
				baz.append<int>(100);
				baz.append<float>(3.14f);
				baz.append<ulong>(123);

				// 1) front()함수를 사용하면 그냥 값을 읽어만 오고 len값을 변경시키지 않는다.
				//    front()함수는 offset값을 넣어주면 그 위치에서 값을 일어온다.
				//    front()함수를 사용해 값을 변경할 수도 있다.
				var v1 = baz.get_front<int>(0);
				var v2 = baz.get_front<float>(4);
				var v3 = baz.get_front<ulong>(8);

				// - 읽어낸 값 출력
				Console.Write("v1: " + v1 + "  v2: " + v2 + "  v3: " + v3 + '\n');

				// - front()함수는 size_값을 변경시키지 않으므로  size_값은 front()함수 변경되지 않는다.
				Console.Write("offset: " + baz.offset + "  size_: " + baz.size + '\n');
			}

			// ------------------------------------------------------------------
			// 5. 버퍼 복사
			// ------------------------------------------------------------------
			{
				// - baz
				var baz = memory.alloc_buffer(1000);
				baz.append<int>(100);
				baz.append<float>(3.14f);
				baz.append<ulong>(123);

				// 1) baz값을 복사하면? 버퍼는 스마트포인터로 공유되며 얕은 복사가 수행된다.
				//    qux와 baz는 버퍼를 공유한테 data_와 size_가 동일한 값을 가진다.
				var qux = baz;

				// 2) qux의 값을 변경시키면?
				//    qux와 baz는 버퍼를 공유하고 있기 때문에 baz의 해당 값 역시 변경된다.
				qux.set_front<int>(0, 33);

				// - qux와 baz의 제일 앞 int값 출력
				Console.Write("baz: " + baz.get_front<int>(0) + "  qux: " + qux.get_front<int>(0) + '\n');

				// 3) qux에 extract를 수행하면? qux의 data_값과 size_값만 변경되며
				//    baz의 data_와 size_값은 변경되지 않는다.
				qux.extract<int>();

				// 4) baz에 append를 하면 baz의 size_값은 변경되지만 qux의 값은 변하지 않는다.
				baz.append<int>(100);

				// 설명) 즉 CGDK.buffer를 복사하게 되면 data_와  size_는 독립적인 값을 가지며
				//      메모리 버퍼 자체는 공유한다는 것이다.

				// - baz의 data_와 size_값을 출력한다.
				Console.Write("offset: " + baz.offset + "  size_: " + baz.size + '\n');

				// 5) clone을 하면 깊은 복사를 할 수 있다.
				//    clon을 하면 버퍼를 신규 할당받아 복사하기 때문에 버퍼를 완전히 분리한다.
				var zoo = baz.clone();
			}

			// ------------------------------------------------------------------
			// 6. 얕은 복사 및 버퍼 조작
			// ------------------------------------------------------------------
			{
				// - baz
				var baz = memory.alloc_buffer(1000);
				baz.append<int>(100);
				baz.append<float>(3.14f);
				baz.append<ulong>(123);

				// 1) offset 8byte를 줘서 얕은 복사를 한다.
				//    - baz에서 offset(8byte) 만큼 extract 후 qux에 넣는다.
				var qux = baz + 8;

				// 2) ^ 연산자를 사용하면 size_값만 4로 바꾸어 얕은 복사를 한다.
				var koo = baz ^ 4;

				// 3) ^ 연산자를 사용해 data_는 data_ + 2, 길이는 4로 변경
				var hoo_1 = baz + 2 ^ 4;
			}
		}
	}
}
