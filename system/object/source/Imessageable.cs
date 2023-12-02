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

//----------------------------------------------------------------------------
//
//  <<interface>> CGDK.Imessageable
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
    public delegate void delegate_notify_int(object _source, uint _value);
    public delegate void delegate_notify_int64(object _source, ulong _value);
    public delegate void delegate_notify_context(object _object, context _context);
    public delegate int  delegate_notify_message(object _object, sMESSAGE _msg);
    public delegate void delegate_notify(object _object);

    public interface Imessageable
    {
        int process_message(object _source, sMESSAGE _msg);
    }
}
