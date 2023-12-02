//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Network Socket Classes                           *
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
using CGDK.Factory;
using CGDK.Net.Io;

//----------------------------------------------------------------------------
//
//  CGDK.Net.Acceptor<T>
//
//
//----------------------------------------------------------------------------
namespace CGDK.Net
{
	public class Acceptor<TSOCKET> : 
		Io.Connective.NAcceptor where TSOCKET: Nreferenceable, Io.IConnectable, new()
	{
	// constructor)
		public Acceptor()
		{
			this.m_factory_socket = new Factory.Auto<TSOCKET>("Socket pool for Acceptor(" + "" + ")");
		}
		public Acceptor(string _name, int _max_allocate = int.MaxValue): base(_name, _max_allocate)	
		{
			this.m_factory_socket = new Factory.Auto<TSOCKET>("Socket pool for Acceptor(" + (_name ?? "") + ")");
		}

	// publics) 
		public delegateNotifyContext	NotifyOnStarting;
		public delegateNotifyContext	NotifyOnStart;
		public delegateNotify			NotifyOnStopping;
		public delegateNotify			NotifyOnStop;

		public delegateNotify			NotifyOnRequestAccept;
		public delegateNotifyAccept		NotifyOnAccept;
		public delegateNotifyAccept		NotifyOnFailAccept;

		public delegateNotify			NotifyOnPrepareAccept;
		public delegateNotify			NotifyOnCloseSocket;

	// frameworks) 
		protected override void OnStarting(Context _context)
		{
			if (this.NotifyOnStarting == null)
				return;

			this.NotifyOnStarting(this, _context);
		}

		protected override void OnStart(Context _context)
		{
			if (this.NotifyOnStart == null)
				return;

			this.NotifyOnStart(this, _context);
		}

		protected override void OnStopping()
		{
			if (this.NotifyOnStopping == null)
				return;

			this.NotifyOnStopping(this);
		}

		protected override void OnStop()
		{
			if (this.NotifyOnStop == null)
				return;

			this.NotifyOnStop(this);
		}

		protected override void OnRequestAccept()
		{
			if (this.NotifyOnRequestAccept == null)
				return;
			
			this.NotifyOnRequestAccept(this);
		}

		protected override void OnAccept(IConnectable _connectable)
		{
			if (this.NotifyOnAccept == null)
				return;
			
			this.NotifyOnAccept(this, _connectable);
		}

		protected override void OnFailAccept(IConnectable _connectable)
		{
			if (this.NotifyOnFailAccept == null)
				return;
			
			this.NotifyOnFailAccept(this, _connectable);
		}

		protected override void OnPrepareAccept()
		{
			if (this.NotifyOnPrepareAccept == null)
				return;
			
			this.NotifyOnPrepareAccept(this);
		}

		protected override void OnCloseSocket()
		{
			if (this.NotifyOnCloseSocket == null)
				return;
			
			this.NotifyOnCloseSocket(this);
		}

	// implementations) 
		public override	IConnectable	ProcessAllocConnectable()
		{
			return	this.m_factory_socket.Alloc();
		}

		private Factory.Auto<TSOCKET>	m_factory_socket;
	}
}
