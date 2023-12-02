//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network socket Classes                           *
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

using CGDK;
using CGDK.factory;
using CGDK.net.io;

//----------------------------------------------------------------------------
//
//  CGDK.net.acceptor<T>
//
//
//----------------------------------------------------------------------------
namespace CGDK.net
{
	public class acceptor<TSOCKET> : 
		io.connective.Nacceptor where TSOCKET: Nreferenceable, io.Iconnectable, new()
	{
	// constructor)
		public acceptor()
		{
			m_factory_socket = new factory.auto<TSOCKET>("socket pool for acceptor(" + "" + ")");
		}
		public acceptor(string _name, int _max_allocate = int.MaxValue): base(_name, _max_allocate)	
		{
			m_factory_socket = new factory.auto<TSOCKET>("socket pool for acceptor(" + (_name != null ? _name : "") + ")");
		}

	// publics) 
		public delegate_notify_context	notify_on_starting;
		public delegate_notify_context	notify_on_start;
		public delegate_notify			notify_on_stopping;
		public delegate_notify			notify_on_stop;

		public delegate_notify			notify_on_request_accept;
		public delegate_notify_accept	notify_on_accept;
		public delegate_notify_accept	notify_on_fail_accept;

		public delegate_notify			notify_on_prepare_accept;
		public delegate_notify			notify_on_close_socket;

	// frameworks) 
		protected override void			on_starting(context _context)	{ if (notify_on_starting != null) notify_on_starting(this, _context); }
		protected override void			on_start(context _context)		{ if (notify_on_start != null) notify_on_start(this, _context); }
		protected override void			on_stopping()				    { if (notify_on_stopping != null) notify_on_stopping(this); }
		protected override void			on_stop()					    { if (notify_on_stop != null) notify_on_stop(this); }

		protected override void			on_request_accept()		    	{ if (notify_on_request_accept != null) notify_on_request_accept(this); }
		protected override void			on_accept(Iconnectable _connectable) { if (notify_on_accept != null) notify_on_accept(this, _connectable); }
		protected override void			on_fail_accept(Iconnectable _connectable) { if (notify_on_fail_accept != null) notify_on_fail_accept(this, _connectable); }

		protected override void			on_prepare_accept()			    { if (notify_on_prepare_accept != null) notify_on_prepare_accept(this); }
		protected override void			on_closesocket()			    { if (notify_on_close_socket != null) notify_on_close_socket(this); }

	// implementations) 
		public override	Iconnectable	process_alloc_connectable()
		{
			return	m_factory_socket.alloc();
		}

		private factory.auto<TSOCKET>	m_factory_socket;
	}
}
