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
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Net.Json;


//----------------------------------------------------------------------------
//
//  CGDK definitions
// 
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
	//-----------------------------------------------------------------------------
	// Code Components
	//-----------------------------------------------------------------------------
	// Protocol
	public static class CODE_TYPE
	{
		public const uint	HEAD_0				 = 0x00000000;		// Reserved for System
		public const uint	HEAD_1				 = 0x00100000;		// Reserved for Application
		public const uint	HEAD_2				 = 0x00200000;		// Reserved for Script
		public const uint	HEAD_3				 = 0x00300000;		// free

		public const uint	SUB_0				 = 0x00000000;
		public const uint	SUB_1				 = 0x00010000;
		public const uint	SUB_2				 = 0x00020000;
		public const uint	SUB_3				 = 0x00030000;
		public const uint	SUB_4				 = 0x00040000;
		public const uint	SUB_5				 = 0x00050000;
		public const uint	SUB_6				 = 0x00060000;
		public const uint	SUB_7				 = 0x00070000;
		public const uint	SUB_8				 = 0x00080000;
		public const uint	SUB_9				 = 0x00090000;
		public const uint	SUB_10				 = 0x000a0000;
		public const uint	SUB_A				 = 0x000a0000;
		public const uint	SUB_11				 = 0x000b0000;
		public const uint	SUB_B				 = 0x000b0000;
		public const uint	SUB_12				 = 0x000c0000;
		public const uint	SUB_C				 = 0x000c0000;
		public const uint	SUB_13				 = 0x000d0000;
		public const uint	SUB_D				 = 0x000d0000;
		public const uint	SUB_14				 = 0x000e0000;
		public const uint	SUB_E				 = 0x000e0000;
		public const uint	SUB_15				 = 0x000f0000;
		public const uint	SUB_F				 = 0x000f0000;

		public const uint	TAIL_0				 = 0x00000000;
		public const uint	TAIL_1				 = 0x00001000;
		public const uint	TAIL_2				 = 0x00002000;
		public const uint	TAIL_3				 = 0x00003000;
		public const uint	TAIL_4				 = 0x00004000;
		public const uint	TAIL_5				 = 0x00005000;
		public const uint	TAIL_6				 = 0x00006000;
		public const uint	TAIL_7				 = 0x00007000;
		public const uint	TAIL_8				 = 0x00008000;
		public const uint	TAIL_9				 = 0x00009000;
		public const uint	TAIL_10				 = 0x0000a000;
		public const uint	TAIL_A				 = 0x0000a000;
		public const uint	TAIL_11				 = 0x0000b000;
		public const uint	TAIL_B				 = 0x0000b000;
		public const uint	TAIL_12				 = 0x0000c000;
		public const uint	TAIL_C				 = 0x0000c000;
		public const uint	TAIL_13				 = 0x0000d000;
		public const uint	TAIL_D				 = 0x0000d000;
		public const uint	TAIL_14				 = 0x0000e000;
		public const uint	TAIL_E				 = 0x0000e000;
		public const uint	TAIL_15				 = 0x0000f000;
		public const uint	TAIL_F				 = 0x0000f000;

		public const uint	TAIL_SUB_0			 = 0x00000000;
		public const uint	TAIL_SUB_1			 = 0x00000100;
		public const uint	TAIL_SUB_2			 = 0x00000200;
		public const uint	TAIL_SUB_3			 = 0x00000300;
		public const uint	TAIL_SUB_4			 = 0x00000400;
		public const uint	TAIL_SUB_5			 = 0x00000500;
		public const uint	TAIL_SUB_6			 = 0x00000600;
		public const uint	TAIL_SUB_7			 = 0x00000700;
		public const uint	TAIL_SUB_8			 = 0x00000800;
		public const uint	TAIL_SUB_9			 = 0x00000900;
		public const uint	TAIL_SUB_10			 = 0x00000a00;
		public const uint	TAIL_SUB_A			 = 0x00000a00;
		public const uint	TAIL_SUB_11			 = 0x00000b00;
		public const uint	TAIL_SUB_B			 = 0x00000b00;
		public const uint	TAIL_SUB_12			 = 0x00000c00;
		public const uint	TAIL_SUB_C			 = 0x00000c00;
		public const uint	TAIL_SUB_13			 = 0x00000d00;
		public const uint	TAIL_SUB_D			 = 0x00000d00;
		public const uint	TAIL_SUB_14			 = 0x00000e00;
		public const uint	TAIL_SUB_E			 = 0x00000e00;
		public const uint	TAIL_SUB_15			 = 0x00000f00;
		public const uint	TAIL_SUB_F			 = 0x00000f00;

		public const uint	WINDOWS				 = (HEAD_0 | SUB_0);
		public const uint	SYSTEM				 = (HEAD_0 | SUB_1);
		public const uint	NETWORK				 = (HEAD_0 | SUB_2);
		public const uint	SCRIPT				 = (HEAD_0 | SUB_3);
		public const uint	GRAPHICS			 = (HEAD_0 | SUB_4);
		public const uint	INIT				 = (HEAD_0 | SUB_5);
		public const uint	USER				 = (HEAD_1 | SUB_0);
	}
}


namespace eMESSAGE 
{
	//-----------------------------------------------------------------------------
	//
	// 1. Windows Message
	//
	//-----------------------------------------------------------------------------
	public static class WINDOWS
	{
		// 1) User Messages
		public const uint	USER_RESERVED	 = (CGDK.CODE_TYPE.WINDOWS | CGDK.CODE_TYPE.TAIL_0 | CGDK.CODE_TYPE.TAIL_SUB_4);	// Windows User Message

		// Message Definitions)
		public const uint	NOTIFY			 = (USER_RESERVED+0);
		public const uint	NOTIFY_UPDATE	 = (USER_RESERVED+1);

		// 2) User Messages
		public const uint	USER			 = (CGDK.CODE_TYPE.WINDOWS | CGDK.CODE_TYPE.TAIL_0 | CGDK.CODE_TYPE.TAIL_SUB_5);   // Windows User Message
	}


	//-----------------------------------------------------------------------------
	//
	// 2. System Message
	//
	// 주의사항) CODE_TYPE_SUB_0은 일반 Message로는 사용하지 않는다.
	//  (CODE_TYPE_SYSTEM | CODE_TYPE_TAIL_0 | CODE_TYPE_TAIL_SUB_0)에서
	//  (CODE_TYPE_SYSTEM | CODE_TYPE_TAIL_0 | CODE_TYPE_TAIL_SUB_3)까지는														 
	//  사용하지 않을 것을 권장한다 그 부분은 윈도우 메시지 영역과 겹친다.
	//  WM_USER는 (CODE_TYPE_SYSTEM | CODE_TYPE_TAIL_0 | CODE_TYPE_TAIL_SUB_4)와 동일하다.
	//-----------------------------------------------------------------------------
	// 2) Message for System(5~B(11))
	public static class SYSTEM
	{
		// 1) Message
		public const uint FLAG_RELIABLE        = 0x80000000;
		public const uint MASK                 = 0x0fffffff;

		// 2) Message for System(5~B(11))
		public const uint	FACTORY			 = (CGDK.CODE_TYPE.SYSTEM | CGDK.CODE_TYPE.TAIL_0);		// (Reserved for CGObjectClasses)
		public const uint	EXECUTE			 = (CGDK.CODE_TYPE.SYSTEM | CGDK.CODE_TYPE.TAIL_1);		// (Reserved for CGExecuteClasses)
		public const uint	POOL			 = (CGDK.CODE_TYPE.SYSTEM | CGDK.CODE_TYPE.TAIL_2);		// (Reserved for CGPoolClasses)
		public const uint	LOG				 = (CGDK.CODE_TYPE.SYSTEM | CGDK.CODE_TYPE.TAIL_3);		// (Reserved for Log)
		public const uint	BUFFER			 = (CGDK.CODE_TYPE.SYSTEM | CGDK.CODE_TYPE.TAIL_4);		// (Reserved for Buffers)

		// 3) State
		public const uint	UPDATE_STATE	 = (CGDK.CODE_TYPE.SYSTEM | CGDK.CODE_TYPE.TAIL_3 | 1);	// State Change

		// 4) Contexts
		public const uint	CONTEXT			 = (CGDK.CODE_TYPE.SYSTEM | CGDK.CODE_TYPE.TAIL_4);

		// 5) Message for network message
		public const uint MESSAGE			 = eMESSAGE.SYSTEM.BUFFER | 0x02000000;
	}


	//-----------------------------------------------------------------------------
	//
	// 3. Basese
	//
	//-----------------------------------------------------------------------------
	public static class BASE
	{
		public static class NETWORK
		{
			public const uint SOCKET			 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_0);
			public const uint P2P				 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_1);
			public const uint UPDATE			 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_2);
			public const uint SECURITY			 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_3);
			public const uint USER				 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_A);
		}

		public static class SERVER
		{
			public const uint ROOM				 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_4);
			public const uint EVENT				 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_5);
			public const uint QUERY				 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_6);
			public const uint WEB				 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_7);
			public const uint ACCOUNT			 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_8);
		}

		public const uint ADMIN					 = (CGDK.CODE_TYPE.NETWORK | CGDK.CODE_TYPE.TAIL_9);
	}


	//-----------------------------------------------------------------------------
	//
	// 4. Attributes
	//
	//-----------------------------------------------------------------------------
	// 1) 

	// 2) Attributes
	public static class ATTRIBUTE
	{
		public const uint BASE			         = (CGDK.CODE_TYPE.SCRIPT | CGDK.CODE_TYPE.TAIL_0);

		public const uint NAME					 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 1);
		public const uint GET_ALL_ATTRIBUTE_INFO = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 2);
		public const uint GET_ALL_ATTRIBUTES	 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 3);
		public const uint GET_ATTRIBUTE_COUNT	 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 4);
		public const uint GET_ALL_METHOD_INFO	 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 5);
		public const uint GET_ALL_METHODES		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 6);
		public const uint GET_METHOD_COUNT		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 7);
		public const uint GET_TYPE_NAME			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 8);
		public const uint GET_TYPE_ID			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 9);
		public const uint GET_ENTITY_NAME		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 10);
		public const uint GET_ENTITY_ID			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 11);

		public const uint ALL_POOL_NAME			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 12);
		public const uint ALL_POOL_INFO			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 13);
		public const uint POOL_INFO				 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 14);
		public const uint ADD_POOL_INFO			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 15);
		public const uint REMOVE_POOL_INFO		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 16);

		public const uint ALL_FACTORY_NAME		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 17);
		public const uint ALL_FACTORY_INFO		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 18);
		public const uint FACTORY_INFO			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 19);
		public const uint ADD_FACTORY_INFO		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 20);
		public const uint REMOVE_FACTORY_INFO	 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 21);
	}

	public static class AUTOMATION
	{
		public const uint BASE					 = (CGDK.CODE_TYPE.SCRIPT | CGDK.CODE_TYPE.TAIL_1);

		public const uint MEMBER_FUNCTION		 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 1);
		public const uint MEMBER_SET			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 2);
		public const uint MEMBER_RESET			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 3);
		public const uint MEMBER_GET			 = (BASE | CGDK.CODE_TYPE.TAIL_SUB_0 | 4);
	}


	//-----------------------------------------------------------------------------
	//
	// 5. 
	//
	//-----------------------------------------------------------------------------
	// 1) Message for Game Application(8~12)
	public static class GRAPHICS
	{
		public const uint BASE	                 = (CGDK.CODE_TYPE.GRAPHICS | CGDK.CODE_TYPE.TAIL_0);
		public const uint UPDATE				 = (CGDK.CODE_TYPE.GRAPHICS | CGDK.CODE_TYPE.TAIL_1);
	}
}


namespace CGDK
{
	public class context
	{
		// Constructor) 
		public	context(string _key = null, string _value = null, context _parent = null)
		{
			m_key =_key;
			m_value =_value;
			m_parent =_parent;
			m_bValid = (_value != null);
			m_is_new = false;
			m_is_updated =false;
			m_is_child_updated = false;
		}

		// 1) Key
		public string	key
		{
			get { return m_key;}
		}

		// 2) Value
		public string	value
		{
			get { if(empty == true) throw new System.Exception(); return m_value;}
			set { m_value = value; _validate(); }
		}

		// 3) ...
		public context this [string _index]
		{
			get
			{
				// 1) 임시로 복사.
				var	str_key = _index;

				// 2) lower case로 변환
				str_key.ToLower();

				// declare) 
				context	temp_value;

				// 3) 먼저 존재하는지 찾는다.
				if(map_node.TryGetValue(str_key, out temp_value)==false)
				{
					temp_value = new context(_index, null);
					map_node.Add(str_key, temp_value);
				}

				// return) 해당 key를 리턴한다.
				return temp_value;
			}

			set
			{
				// 1) 임시로 복사.
				var	str_key = _index;

				// 2) lower case로 변환
				str_key.ToLower();

				// 3) 해당 Key에 값을 넣음.
				map_node[str_key]	 = value;
			}
		}

		public bool		empty
		{
			get
			{
				return	m_value == null && map_node.Count == 0;
			}
		}
		public bool		exist
		{
			get
			{
				return	m_value != null || map_node.Count > 0;
			}
		}

		// 4) Add/Remove
		public context	insert(string _key, string _value = null)
		{
			// - 생성한다.
			var	nodeNew = new context(_key, _value, this);
			nodeNew.m_is_new = true;
			nodeNew.m_is_updated = nodeNew.m_bValid;
			nodeNew.m_is_child_updated = false;

			// - Validate한다.
			nodeNew._validate();

			// - 추가한다.
			map_node.Add(_key, nodeNew);

			// return)
			return nodeNew;
		}
		public context	erase(string _key)
		{
			// - 찾는다.
			var nodeExist = map_node[_key];

			// check) 찾지 못했으면 그냥 null을 리턴한다.
			if(nodeExist == null)
				return null;

			// - 제거한다.
			bool result = map_node.Remove(_key);

			// check) 제거에 실패했으면 null을 리턴한다.
			if(result == false)
				return null;

			// return) 제가한 node를 리턴한다.
			return	nodeExist;
		}

		public static implicit operator char(context _value)		{ return char.Parse(_value.value);}
		public static implicit operator sbyte(context _value)		{ return sbyte.Parse(_value.value);}
		public static implicit operator byte(context _value)		{ return byte.Parse(_value.value);}
		public static implicit operator short(context _value)		{ return short.Parse(_value.value);}
		public static implicit operator ushort(context _value)		{ return ushort.Parse(_value.value);}
		public static implicit operator int(context _value)			{ return int.Parse(_value.value);}
		public static implicit operator uint(context _value)		{ return uint.Parse(_value.value);}
		public static implicit operator long(context _value)		{ return long.Parse(_value.value);}
		public static implicit operator ulong(context _value)		{ return ulong.Parse(_value.value);}
		public static implicit operator float(context _value)		{ return float.Parse(_value.value);}
		public static implicit operator double(context _value)		{ return double.Parse(_value.value);}
		public static implicit operator string(context _value)		{ return _value.value;}
		public static implicit operator DateTime(context _value)	{ return DateTime.Parse(_value.value);}
		public static implicit operator IPEndPoint(context _value)
		{
			// 1) Splite with ':'
			string[] ep = _value.value.Split(':');

			// declare) 
			IPAddress ip;
			int port = 0;

			// 2)  IP Address
			if(!IPAddress.TryParse(ep[0], out ip))
			{
				throw new FormatException("Invalid ip-adress");
			}

			// 3) Port
			if(ep.Length == 2)
			{
				if(!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
				{
					throw new FormatException("Invalid port");
				}
			}

			// 4) IPEndPoint를 생성한다.
			return new IPEndPoint(ip, port);
		}

		public static implicit operator context(char _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(sbyte _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(byte _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(short _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(ushort _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(int _value)			{ return new context(null, _value.ToString());}
		public static implicit operator context(uint _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(long _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(ulong _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(float _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(double _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(string _value)		{ return new context(null, _value.ToString());}
		public static implicit operator context(DateTime _value)	{ return new context(null, _value.ToString());}
		public static implicit operator context(IPEndPoint _value)	{ return new context(null, _value.ToString());}

		public bool			read_from_JSON_file(string _filename)
		{
			// 1) File을 Open한다.
			var	reader = new StreamReader(_filename);

			// 4) 값을 읽어 사용한다.
			return	read_from_json_string(reader.ReadToEnd());
		}
		public bool			read_from_json_string(string _strJSON)
		{
			// declare)
			bool result = false;

			try
			{
				// 1) Parser를 생성한다.
				var parser = new JsonTextParser();

				// 2) _strJSON Parsing하여 JSON 객체를 생성한다.
				var	objJSON = parser.Parse(_strJSON);

				// 3) 값을 읽어 사용한다.
				result = read_from_JSON_object(objJSON);
			}
			catch(System.Exception)
			{
				result = false;
			}

			// return) 
			return result;
		}
		public bool			read_from_JSON_object(JsonObject _json_object)
		{
			foreach (JsonObject field in _json_object as JsonObjectCollection)
			{
				var	objContext = insert(field.Name, field.GetValue().ToString());

				if(objContext.read_from_JSON_object(field)==false)
				{
					return false;
				}
			}

			// return) 
			return true;
		}
		public bool			read_from_buffer(CGDK.buffer _buffer)
		{
			var buf_temp = _buffer;

			// 1) Key값 읽기
			m_key = buf_temp.extract<string>();

			// 2) Value값 읽기
			m_value = buf_temp.extract<string>();

			// 3) Flag값 읽기
			m_is_new = buf_temp.extract<bool>();
			m_is_updated = buf_temp.extract<bool>();
			m_is_child_updated = buf_temp.extract<bool>();;
			m_bValid = true;

			// 4) Child값 읽기
			var	count = buf_temp.extract<int>();
			for(var i=0; i<count; ++i)
			{
				var node = new context();

				node.read_from_buffer(buf_temp);

				map_node.Add(node.key, node);
			}

			// return) 
			return	true;
		}
		public bool			write_to_buffer(CGDK.buffer _buffer)
		{
			_buffer.append(m_key);
			_buffer.append(m_value);
			_buffer.append(m_is_new);
			_buffer.append(m_is_updated);

			_buffer.append<int>(map_node.Values.Count);
			foreach(var iter in map_node.Values)
			{
				iter.write_to_buffer(_buffer);
			}

			// return) 
			return	true;
		}
		public bool			write_to_buffer_updated(CGDK.buffer _buffer)
		{
			// 1) Key값 저장
			_buffer.append(m_key);

			// 2) Value값 저장
			if(m_is_updated)
			{
				_buffer.append<string>(m_value);
			}
			else
			{
				_buffer.append<string>(null);
			}

			// 3) Flag들 저장
			_buffer.append(m_is_new);
			_buffer.append(m_is_updated);
			_buffer.append(m_is_child_updated);

			// 4) Child들 저장
			if(m_is_child_updated == true)
			{
				_buffer.append<int>(map_node.Values.Count);
				foreach(var iter in map_node.Values)
				{
					iter.write_to_buffer_updated(_buffer);
				}
			}
			else
			{
				_buffer.append<int>(0);
			}

			// return) 
			return	true;
		}

		// 5) ...
		private string		m_key				 = null;
		private string		m_value				 = null;
		private context		m_parent			 = null;
		private bool		m_bValid			 = false;
		private bool		m_is_new			 = false;
		private bool		m_is_updated		 = false;
		private bool		m_is_child_updated	 = false;
		public Dictionary<string, context> map_node = new Dictionary<string, context>();

		private void		_validate()
		{
			// - 신규 노드인지 확인한다.
			m_is_new = !m_bValid;

			// - 유효한 노드임을 표시한다.
			m_bValid = true;

			// - 부모 노드에게도 모두 동일한 처리를 한다.
			if(m_parent != null)
				_validate_child();
		}
		private void		_validate_child()
		{
			// - 신규 노드인지 확인한다.
			m_is_new = !m_bValid;

			// - 유효한 노드임을 표시한다.
			m_bValid = true;

			// - Child가 Update되었음을 표시한다.
			m_is_child_updated = true;

			// - 부모 노드에게도 모두 동일한 처리를 한다.
			if(m_parent!= null)
				m_parent._validate_child();
		}
	}

	public enum eLOG_TYPE : int
	{
		INFO				 = 0,
		PROGRESS			 = 1,
		DEBUG				 = 2,
		EXCEPTION			 = 3,
		ERROR				 = 4,
		WARNING				 = 5,
		USER				 = 6,
		SYSTEM				 = 7,
		MAX,
		CONTINUE			 = 0x8000,
		UNDEFINED			 = 0xffff,
	};

	public class eLOG_LEVEL
	{
		public const int	LOWEST	 = -65536;
		public const int	LOWER	 = -256;
		public const int	NORMAL	 = 0;
		public const int	HIGHER	 = 256;
		public const int	HIGHEST	 = 65536;
	}


}
