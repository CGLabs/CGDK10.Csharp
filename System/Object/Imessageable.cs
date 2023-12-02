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
//  <<interface>> CGDK.IMessageable
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
    public delegate void delegateNotifyInt(object _source, uint _value);
    public delegate void delegateNotifyInt64(object _source, ulong _value);
    public delegate void delegateNotifyContext(object _object, Context _context);
    public delegate int  delegateNotifyMessage(object _object, sMESSAGE _msg);
    public delegate void delegateNotify(object _object);

    public interface IMessageable
    {
        int ProcessMessage(object _source, sMESSAGE _msg);
    }
}
