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

using System;
using System.Net;

//----------------------------------------------------------------------------
//  <<interface>> CGDK.CGMSG
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	public struct sMESSAGE
	{
		public uint					message;
		public object				source;
		public CGDK.buffer			bufMessage;
		public object				param;
		public IPEndPoint			address;

		public sMESSAGE(uint _message=0)									{ message = _message;					source = null;	    bufMessage = new CGDK.buffer(null, 0);	param = null; address = null; }
		public sMESSAGE(uint _message, object _source)						{ message = _message;					source = _source;	bufMessage = new CGDK.buffer(null, 0);	param = null; address = null; }
		public sMESSAGE(uint _message, object _source, CGDK.buffer _buffer)	{ message = _message;					source = _source;	bufMessage = _buffer;					param = null; address = null; }
		public sMESSAGE(uint _message, object _source, CGDK.buffer _buffer, IPEndPoint _address)
																			{ message = _message;					source = _source;	bufMessage = _buffer;					param = null; address = _address;}
		public sMESSAGE(uint _message, CGDK.buffer _buffer)					{ message = _message;					source = null;	    bufMessage = _buffer;					param = null; address = null; }
		public sMESSAGE(CGDK.buffer _buffer)							   	{ message = 0;						    source = null;	    bufMessage = _buffer;					param = null; address = null; }
		public sMESSAGE(CGDK.buffer _buffer, object _source)			   	{ message = 0;					    	source = _source;	bufMessage = _buffer;					param = null; address = null; }
		public sMESSAGE(Context _context)									{ message = eMESSAGE.SYSTEM.CONTEXT;	source = null;	    bufMessage = new CGDK.buffer(null, 0);	param = _context; address = null; }
		public sMESSAGE(Context _context, object _source)					{ message = eMESSAGE.SYSTEM.CONTEXT;	source = _source;	bufMessage = new CGDK.buffer(null, 0);	param = _context; address = null; }
	}                                                                                                                       
}