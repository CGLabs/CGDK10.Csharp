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
	static void Main(string[] args)
	{
		tutorial_object_interface_by_IS_A();

		tutorial_object_interface_by_HAS_A();
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
	class foo : Nobject_stateable
	{
		protected override void on_initializing(context _context)
		{
			Console.WriteLine("foo.on_initializing!");
		}
		protected override void on_initialize(context _context)
		{
			Console.WriteLine("foo.on_initialize!");
		}
		protected override void on_destroying()
		{
			Console.WriteLine("foo.on_destroying!");
		}
		protected override void on_destroy()
		{
			Console.WriteLine("foo.on_destroy!");
		}
		protected override void on_starting(context _context)
		{
			Console.WriteLine("foo.on_starting!");
		}
		protected override void on_start(context _context)
		{
			Console.WriteLine("foo.on_start!");
		}
		protected override void on_stopping()
		{
			Console.WriteLine("foo.on_stopping!");
		}
		protected override void on_stop()
		{
			Console.WriteLine("foo.on_stop!");
		}
	}

	static void tutorial_object_interface_by_IS_A()
	{
		// 1) create 'foo'
		var temp = new foo();

		// 2) start
		temp.start(new context());

		// 3) destroy
		temp.destroy();
	}

	//-----------------------------------------------------------------------------
	//
	// case 2) HAS-A (포함)을 통한 구현
	//
	// C#에서는 다중상속을 지원해주지 않기 때문에 IS-A(상속)을 다른 클래스를 상속한다면
	// Nobject_stateable를 상속할 수 없다.
	// 이럴 경우 (case 2)와 같이 HAS-A(포함)을 사용해 구현해야 한다.
	// 
	//   - Iobject_stateable, Iinitializable, Istartable 인터페이스 클래스만 상속받는다.
	//   - Cobject_state 클래스를 멤버 변수로 갖는다.
	//   - 인터페이스 클래스의 구현 함수에서 Cobject_state의 함수를 호출하도록 구현한다.
	//
	// 
	// 
	// 
	// 
	//
	//-----------------------------------------------------------------------------
	class xoo : Iobject_stateable, Iinitializable, Istartable
		{
		public xoo()
		{
			component_state = new Cobject_state();
			component_state.target					 = this;
			component_state.notify_on_initializing	 = on_initializing;
			component_state.notify_on_initialize	 = on_initialize;
			component_state.notify_on_destroying	 = on_destroying;
			component_state.notify_on_destroy		 = on_destroy;
			component_state.notify_on_starting		 = on_starting;
			component_state.notify_on_start			 = on_start;
			component_state.notify_on_stopping		 = on_stopping;
			component_state.notify_on_stop			 = on_stop;
		}

		public eOBJECT_STATE object_state		{ get { return component_state.object_state; } set { component_state.object_state = value; } }
		public bool set_object_state_if			(eOBJECT_STATE _state_compare, eOBJECT_STATE _new_states) { return component_state.set_object_state_if(_state_compare, _new_states); }

		public bool initialize(context _context){ return component_state.initialize(_context); }
		public bool destroy()					{ return component_state.destroy(); }
		public bool start(context _context)		{ return component_state.start(_context); }
		public bool stop()						{ return component_state.stop(); }

		public bool attach(Iobject_stateable _child) { return component_state.attach(_child); }
		public int	detach(Iobject_stateable _child) { return component_state.detach(_child); }
		public IEnumerator GetEnumerator()		{ return component_state.GetEnumerator(); }


		protected void on_initializing(object _object, context _context)
		{
			Console.WriteLine("xoo.on_initializing!");
		}
		protected void on_initialize(object _object, context _context)
		{
			Console.WriteLine("xoo.on_initialize!");
		}
		protected void on_destroying(object _object)
		{
			Console.WriteLine("xoo.on_destroying!");
		}
		protected void on_destroy(object _object)
		{
			Console.WriteLine("xoo.on_destroy!");
		}
		protected void on_starting(object _object, context _context)
		{
			Console.WriteLine("xoo.on_starting!");
		}
		protected void on_start(object _object, context _context)
		{
			Console.WriteLine("xoo.on_start!");
		}
		protected void on_stopping(object _object)
		{
			Console.WriteLine("xoo.on_stopping!");
		}
		protected void on_stop(object _object)
		{
			Console.WriteLine("xoo.on_stop!");
		}

		// - Components
		public Cobject_state	component_state;
	}

	static void tutorial_object_interface_by_HAS_A()
	{
		// 1) create 'xoo'
		var temp = new xoo();

		// 2) start
		temp.start(new context());

		// 3) destroy
		temp.destroy();
	}
}
}
