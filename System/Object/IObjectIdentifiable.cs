﻿//*****************************************************************************
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
//  <<interface>> CGDK.IObjectIdentifiable
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
	public uint			ObjectType;
	public uint			FactoryId;
	public ulong		Type { get { return (this.FactoryId<<32) | Type; } set { } }

	public uint			ProductSerial;
	public uint			ShipmentSerial;
	public ulong		Serial { get { return (this.ShipmentSerial<<32) | this.ProductSerial; } set { } }

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


public interface IObjectIdentifiable
{
	OBJECT_ID			ObjectId		{ get; set;}
}

}
