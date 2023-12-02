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
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

//----------------------------------------------------------------------------
//
//  <<interface>> CGDK.TRANSACTION_ID
//  <<interface>> CGDK.SOURCE_ID
//  <<interface>> CGDK.REQUEST_ID
//
//
//----------------------------------------------------------------------------
namespace CGDK
{

// ----------------------------------------------------------------------------
// 1) Transaction ID
//    요청을 수행할 때 요청마다 부여되는 고유 ID로 일반적으로 순차적인 값을 가진다.
//    주로 서버간 요청을 수행할 때 사용된다.
// ----------------------------------------------------------------------------
public struct TRANSACTION_ID
{
	public void				Generate()												{ ulong temp=(ulong)Interlocked.Increment(ref TIndexID); if(temp==0) temp=(ulong)Interlocked.Increment(ref TIndexID); m_tid=temp;}
	public void				Reset()													{ m_tid = 0;}
																				  
	public bool				IsEmpty()													{ return m_tid == 0;}
	public bool				IsExist()													{ return m_tid != 0;}

	public static bool		operator==(TRANSACTION_ID _lhs, TRANSACTION_ID _rhs)	{ return _lhs.m_tid == _rhs.m_tid;}
	public static bool		operator!=(TRANSACTION_ID _lhs, TRANSACTION_ID _rhs)	{ return _lhs.m_tid != _rhs.m_tid;}
	public static bool		operator>=(TRANSACTION_ID _lhs, TRANSACTION_ID _rhs)	{ return _lhs.m_tid >= _rhs.m_tid;}
	public static bool		operator> (TRANSACTION_ID _lhs, TRANSACTION_ID _rhs)	{ return _lhs.m_tid > _rhs.m_tid;}
	public static bool		operator<=(TRANSACTION_ID _lhs, TRANSACTION_ID _rhs)	{ return _lhs.m_tid <= _rhs.m_tid;}
	public static bool		operator< (TRANSACTION_ID _lhs, TRANSACTION_ID _rhs)	{ return _lhs.m_tid <  _rhs.m_tid;}
	public override bool	Equals(System.Object _obj)
							{
								if (_obj == null)
									return false;

								var p = _obj as TRANSACTION_ID?;

								if (p.HasValue == false)
								{
									return false;
								}

								return m_tid == p.Value.m_tid;
							}

	public override int		GetHashCode()
							{
								return (int)m_tid;
							}

	private	ulong			m_tid;

	static long				TIndexID = 0;
};

// ----------------------------------------------------------------------------
// 2) Source ID
//    전달해 준 소스를 구분해주는 ID
// ----------------------------------------------------------------------------
public struct SOURCE_ID
{
	public void				Generate()												{ ulong temp=(ulong)Interlocked.Increment(ref TIndexID); if(temp==0) temp=(ulong)Interlocked.Increment(ref TIndexID); m_sid = temp;}

	public	void			Reset()			{ m_sid=0;}
	public	ulong			Value()			{ return m_sid;}

	public	bool			IsEmpty()			{ return m_sid==0; }
	public	bool			IsExist()			{ return m_sid!=0; }

	public static bool		operator==(SOURCE_ID _lhs, SOURCE_ID _rhs)	{ return _lhs.m_sid == _rhs.m_sid; }
	public static bool		operator!=(SOURCE_ID _lhs, SOURCE_ID _rhs)	{ return _lhs.m_sid != _rhs.m_sid; }
	public static bool		operator>=(SOURCE_ID _lhs, SOURCE_ID _rhs)	{ return _lhs.m_sid >= _rhs.m_sid; }
	public static bool		operator> (SOURCE_ID _lhs, SOURCE_ID _rhs)	{ return _lhs.m_sid > _rhs.m_sid; }
	public static bool		operator<=(SOURCE_ID _lhs, SOURCE_ID _rhs)	{ return _lhs.m_sid <= _rhs.m_sid; }
	public static bool		operator< (SOURCE_ID _lhs, SOURCE_ID _rhs)	{ return _lhs.m_sid < _rhs.m_sid; }
	public override bool	Equals(System.Object _obj)
							{
								if (_obj == null)
								{
									return false;
								}

								var p = _obj as SOURCE_ID?;

								if (p.HasValue == false)
								{
									return false;
								}

								return m_sid == p.Value.m_sid;
							}

	public override int		GetHashCode()
							{
								return (int)m_sid;
							}

	private	ulong			m_sid;

	static long				TIndexID	= 0;
};


// ----------------------------------------------------------------------------
// 3) REQUEST ID
//    요청을 구분할 때 사용되는 ID
//    요청 ID는
//    
//    1. 요청을 한 곳을 나타내는 SOURCE_ID
//    2. 요청번호를 의미하는 TRANSACTION_ID
//
//    로 구분하여 저장한다.
//
//    다른 곳에서 생성된 TRANSACTION_ID를  전달받아 사용할 경우 TRANSACTION_ID
//   만으로 구분할 경우 TRANSACTION_ID가 중복될 수 있다.
//    따라서 전달 받은 TRANSACTION_ID에 전달받은 Source의 ID를 결합하여 만든 ID이다.
//   일반적으로 여러 Remote에서 전달받은 Transaction ID를 구분하여 사용할 때 쓰인다.
// ----------------------------------------------------------------------------
public struct REQUEST_ID
{
	public	void			Reset()											{ this.m_sid.Reset(); this.m_tid.Reset(); }
	public void				Generate(SOURCE_ID _sid)						{ this.m_sid=_sid; this.m_tid.Generate();}

	public	bool			IsEmpty()										{ return this.m_sid.IsEmpty() && this.m_tid.IsEmpty(); }
	public bool				IsExist()										{ return this.m_sid.IsExist() || this.m_tid.IsExist(); }

	public SOURCE_ID		sid												{ get { return this.m_sid; }	set { this.m_sid = value; } }
	public TRANSACTION_ID	tid												{ get { return this.m_tid; }	set { this.m_tid = value; } }

	public static bool		operator==(REQUEST_ID _lhs, REQUEST_ID _rhs)	{ return _lhs.tid == _rhs.tid && _lhs.sid == _rhs.sid; }
	public static bool		operator!=(REQUEST_ID _lhs, REQUEST_ID _rhs)	{ return _lhs.tid != _rhs.tid || _lhs.sid != _rhs.sid; }
	public static bool		operator>=(REQUEST_ID _lhs, REQUEST_ID _rhs)	{ return (_lhs.tid > _rhs.tid) ? true : ((_lhs.tid < _rhs.tid) ? false : (_lhs.sid >= _rhs.sid)); }
	public static bool		operator> (REQUEST_ID _lhs, REQUEST_ID _rhs)	{ return (_lhs.tid > _rhs.tid) ? true : ((_lhs.tid < _rhs.tid) ? false : (_lhs.sid > _rhs.sid)); }
	public static bool		operator<=(REQUEST_ID _lhs, REQUEST_ID _rhs)	{ return (_lhs.tid < _rhs.tid) ? true : ((_lhs.tid > _rhs.tid) ? false : (_lhs.sid <= _rhs.sid)); }
	public static bool		operator< (REQUEST_ID _lhs, REQUEST_ID _rhs)	{ return (_lhs.tid < _rhs.tid) ? true : ((_lhs.tid > _rhs.tid) ? false : (_lhs.sid < _rhs.sid)); }
	public override bool	Equals(System.Object _obj)
							{
								if (_obj == null)
								{
									return false;
								}

								var p = _obj as REQUEST_ID?;

								if (p.HasValue == false)
								{
									return false;
								}

								return this.m_tid == p.Value.m_tid && this.m_sid == p.Value.m_sid;
							}

	public override int		GetHashCode()
							{
								return sid.GetHashCode() + tid.GetHashCode();
							}

	public	SOURCE_ID		m_sid;		// Source ID
	public	TRANSACTION_ID	m_tid;		// Transaction ID
};





public class ManagerRegister<TOBJECT>
{
	// 1) ..
	public ManagerRegister() {}

	// 2) Request/Unrequester
	public TRANSACTION_ID		Register(TOBJECT _object)
	{
		lock(this.m_map_register)
		{
			return this._RegisterObject(_object);
		}
	}
	public void				Register(TOBJECT _object, TRANSACTION_ID _id_register)
	{
		lock(this.m_map_register)
		{
			this._RegisterObject(_object, _id_register);
		}
	}
	public TOBJECT			Unregister(TRANSACTION_ID _key)
	{
		lock(this.m_map_register)
		{
			return this._UnregisterObject(_key);
		}
	}
	public List<TOBJECT>	UnregisterAll()
	{
		// Declare) 
		List<TOBJECT> vector_all = new List<TOBJECT>();

		lock(this.m_map_register)
		{
			// 1) 모든 Request의 OnUnregister를 호출한다.
			foreach(var iter in this.m_map_register)
			{
				// - vector_all에 넣는다
				vector_all.Add(iter.Value);

				// - Unregister한다.
				this.OnUnregister(iter.Value, iter.Key);
			}

			// 2) 클리어한다.
			this.m_map_register.Clear();
		}

		// Return)
		return vector_all;
	}

	// 2) Request Count
	public int				RegisteredCount
	{
		get
		{
			lock(this.m_map_register)
			{
				return this._GetRegisteredCount();
			}
		}
	}


// framework)
	public virtual void		OnRegister(TOBJECT _object, TRANSACTION_ID _key) {}
	public virtual void		OnUnregister(TOBJECT _object, TRANSACTION_ID _key)	{}


// implementation)
	protected Dictionary<TRANSACTION_ID, TOBJECT>	m_map_register = new Dictionary<TRANSACTION_ID, TOBJECT>();

	protected TRANSACTION_ID	_RegisterObject(TOBJECT _object)
	{
		// 1) 신규 생성
		var	tid_register_id = new TRANSACTION_ID();

		// 2) Key를 Generate한다.
		tid_register_id.Generate();

		// 3) Insert한다.
		this.m_map_register.Add(tid_register_id, _object);

		// 4) Hook함수 호출
		this.OnRegister(_object, tid_register_id);

		// Return) 
		return tid_register_id;
	}

	protected void			_RegisterObject(TOBJECT _object, TRANSACTION_ID _id_register)
	{
		// check) 
		Debug.Assert(_id_register.IsExist());

		// check)
		if(_id_register.IsEmpty())
			throw new System.Exception();

		// Declare) 
		TOBJECT pobject;
			
		// 1) 존재하는지 먼저 찾아본다.
		var	is_found = this.m_map_register.TryGetValue(_id_register, out pobject);

		// check) 찾지 못했다면 끝낸다.
		if(is_found == true)
			throw new System.Exception();

		// 2) 없으면 추가한다.
		this.m_map_register.Add(_id_register, _object);

		// 3) Hook함수 호출
		this.OnRegister(_object, _id_register);
	}

	protected TOBJECT		_UnregisterObject(TRANSACTION_ID _key)
	{
		// Declare) 
		TOBJECT pobject;
			
		// 1) Account ID에 해당하는 Request정보를 얻는다.
		var	bFind = this.m_map_register.TryGetValue(_key, out pobject);

		// check) 찾지 못했다면 끝낸다.
		if(bFind == false || pobject == null)
			return default(TOBJECT);

		// 2) Hook함수 호출
		this.OnUnregister(pobject, _key);

		// 3) 지운다.
		this.m_map_register.Remove(_key);

		// Return) 
		return pobject;
	}
	protected bool			_UnregisterObjectIf(TRANSACTION_ID _key, TOBJECT _object)
	{
		// 1) Account ID에 해당하는 Request정보를 얻는다.
		var	obj_find = this.m_map_register[_key];

		// check) 
		if(ReferenceEquals(_object, obj_find) == true)
			return	false;

		// 2) Hook함수 호출
		this.OnUnregister(obj_find, _key);

		// 3) 지운다.
		this.m_map_register.Remove(_key);

		// Return) 
		return	true;
	}
	protected int			_GetRegisteredCount()
	{
		return this.m_map_register.Count;
	}
	protected TOBJECT		_FindObject(TRANSACTION_ID _key)
	{
		// Declare)
		TOBJECT	obj_find;

		try
		{
			obj_find = this.m_map_register[_key];
		}
		catch(KeyNotFoundException)
		{
			obj_find = default(TOBJECT);
		}

		// Return)
		return obj_find;
	}
}


}

