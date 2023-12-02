//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                     tutorials object - basic.interface                    *
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
using System.Collections;
using CGDK;

namespace tutorial_object_03_object_interfaces_console
{

class Program
{
	static void Main()
	{
		TutorialObjectInterfaceBy_IsA();

		TutorialObjectInterfaceBy_HasA();
	}

	//-----------------------------------------------------------------------------
	//
	// case 1) IS-A(상속)을 통한 구현
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
	class Foo : NObjectStateable
	{
		protected override void OnInitializing(Context _context)
		{
			Console.WriteLine("Foo.OnInitializing!");
		}
		protected override void OnInitialize(Context _context)
		{
			Console.WriteLine("Foo.OnInitialize!");
		}
		protected override void OnDestroying()
		{
			Console.WriteLine("Foo.OnDestroying!");
		}
		protected override void OnDestroy()
		{
			Console.WriteLine("Foo.OnDestroy!");
		}
		protected override void OnStarting(Context _context)
		{
			Console.WriteLine("Foo.OnStarting!");
		}
		protected override void OnStart(Context _context)
		{
			Console.WriteLine("Foo.OnStart!");
		}
		protected override void OnStopping()
		{
			Console.WriteLine("Foo.OnStopping!");
		}
		protected override void OnStop()
		{
			Console.WriteLine("Foo.OnStop!");
		}
	}

	static void TutorialObjectInterfaceBy_IsA()
	{
		// 1) create 'Foo'
		var temp = new Foo();

		// 2) Start
		temp.Start(new Context());

		// 3) Destroy
		temp.Destroy();
	}

	//-----------------------------------------------------------------------------
	//
	// case 2) HAS-A (포함)을 통한 구현
	//
	// C#에서는 다중상속을 지원해주지 않기 때문에 IS-A(상속)을 다른 클래스를 상속한다면
	// NObjectStateable를 상속할 수 없다.
	// 이럴 경우 (case 2)와 같이 HAS-A(포함)을 사용해 구현해야 한다.
	// 
	//   - IObjectStateable, IInitializable, IStartable 인터페이스 클래스만 상속받는다.
	//   - ObjectState 클래스를 멤버 변수로 갖는다.
	//   - 인터페이스 클래스의 구현 함수에서 ObjectState의 함수를 호출하도록 구현한다.
	//
	// 
	// 
	// 
	// 
	//
	//-----------------------------------------------------------------------------
	class Xoo : IObjectStateable, IInitializable, IStartable
		{
		public Xoo()
		{
				component_state = new ObjectState
				{
					Target = this,
					notifyOnInitializing = OnInitializing,
					notifyOnInitialize = OnInitialize,
					notifyOnDestroying = OnDestroying,
					notifyOnDestroy = OnDestroy,
					notifyOnStarting = OnStarting,
					notifyOnStart = OnStart,
					notifyOnStopping = OnStopping,
					notifyOnStop = OnStop
				};
			}

		public eOBJECT_STATE Now				{ get { return component_state.Now; } set { component_state.Now = value; } }
		public bool SetObjectStateIf			(eOBJECT_STATE _state_compare, eOBJECT_STATE _new_states) { return component_state.SetObjectStateIf(_state_compare, _new_states); }

		public bool Initialize(Context _context){ return component_state.Initialize(_context); }
		public bool Destroy()					{ return component_state.Destroy(); }
		public bool Start(Context _context)		{ return component_state.Start(_context); }
		public bool Stop()						{ return component_state.Stop(); }

		public bool Attach(IObjectStateable _child) { return component_state.Attach(_child); }
		public int	Detach(IObjectStateable _child) { return component_state.Detach(_child); }
		public IEnumerator GetEnumerator()		{ return component_state.GetEnumerator(); }


		private void OnInitializing(object _object, Context _context)
		{
			Console.WriteLine("Xoo.OnInitializing!");
		}
		private void OnInitialize(object _object, Context _context)
		{
			Console.WriteLine("Xoo.OnInitialize!");
		}
		private void OnDestroying(object _object)
		{
			Console.WriteLine("Xoo.OnDestroying!");
		}
		private void OnDestroy(object _object)
		{
			Console.WriteLine("Xoo.OnDestroy!");
		}
		private void OnStarting(object _object, Context _context)
		{
			Console.WriteLine("Xoo.OnStarting!");
		}
		private void OnStart(object _object, Context _context)
		{
			Console.WriteLine("Xoo.OnStart!");
		}
		private void OnStopping(object _object)
		{
			Console.WriteLine("Xoo.OnStopping!");
		}
		private void OnStop(object _object)
		{
			Console.WriteLine("Xoo.OnStop!");
		}

		// - Components
		public ObjectState	component_state;
	}

	static void TutorialObjectInterfaceBy_HasA()
	{
		// 1) create 'Xoo'
		var temp = new Xoo();

		// 2) Start
		temp.Start(new Context());

		// 3) Destroy
		temp.Destroy();
	}
}
}
