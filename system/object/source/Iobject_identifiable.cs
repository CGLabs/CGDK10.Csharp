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
//  <<interface>> CGDK.Iobject_identifiable
//
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{

public struct OBJECT_ID
{
	public uint			object_type;
	public uint			factory_id;
	public ulong		type { get { return (factory_id<<32) | type; } set { } }

	public uint			product_serial;
	public uint			shipment_serial;
	public ulong		serial { get { return (shipment_serial<<32) | product_serial; } set { } }

	//public bool		operator==(const OBJECT_ID& _Rhs) const	{ return qwSerial==_Rhs.qwSerial;}
	//public bool		operator!=(const OBJECT_ID& _Rhs) const	{ return qwSerial!=_Rhs.qwSerial;}
	//public bool		operator> (const OBJECT_ID& _Rhs) const	{ return qwSerial> _Rhs.qwSerial;}
	//public bool		operator>=(const OBJECT_ID& _Rhs) const	{ return qwSerial>=_Rhs.qwSerial;}
	//public bool		operator< (const OBJECT_ID& _Rhs) const	{ return qwSerial< _Rhs.qwSerial;}
	//public bool		operator<=(const OBJECT_ID& _Rhs) const	{ return qwSerial<=_Rhs.qwSerial;}
																  
	//public bool		operator==(const ulong_t& _Rhs) const	{ return qwSerial==_Rhs;}
	//public bool		operator!=(const ulong_t& _Rhs) const	{ return qwSerial!=_Rhs;}
	//public bool		operator> (const ulong_t& _Rhs) const	{ return qwSerial> _Rhs;}
	//public bool		operator>=(const ulong_t& _Rhs) const	{ return qwSerial>=_Rhs;}
	//public bool		operator< (const ulong_t& _Rhs) const	{ return qwSerial< _Rhs;}
	//public bool		operator<=(const ulong_t& _Rhs) const	{ return qwSerial<=_Rhs;}
}


public interface Iobject_identifiable
{
	OBJECT_ID				object_id		{ get; set;}
}

}
