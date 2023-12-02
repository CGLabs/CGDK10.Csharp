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
		public const uint	HEAD_3				 = 0x00300000;		// Free

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
	public class Context
	{
		// Constructor) 
		public	Context(string _key = null, string _value = null, Context _parent = null)
		{
			this.m_key =_key;
			this.m_value =_value;
			this.m_parent =_parent;
			this.m_bValid = (_value != null);
			this.m_is_new = false;
			this.m_is_updated =false;
			this.m_is_child_updated = false;
		}

		// 1) Key
		public string	Key
		{
			get { return this.m_key;}
		}

		// 2) Value
		public string	Value
		{
			get { if(IsEmpty == true) throw new System.Exception(); return this.m_value;}
			set { this.m_value = value; ProcessValidate(); }
		}

		// 3) ...
		public Context this [string _index]
		{
			get
			{
				// 1) 임시로 복사.
				var	str_key = _index;

				// 2) lower case로 변환
				str_key.ToLower();

				// 3) 먼저 존재하는지 찾는다.
				if (map_node.TryGetValue(str_key, out Context temp_value) == false)
				{
					temp_value = new Context(_index, null);
					map_node.Add(str_key, temp_value);
				}

				// return) 해당 Key를 리턴한다.
				return temp_value;
			}

			set
			{
				// 1) 임시로 복사.
				var	str_key = _index;

				// 2) lower case로 변환
				str_key.ToLower();

				// 3) 해당 Key에 값을 넣음.
				map_node[str_key] = value;
			}
		}

		public bool		IsEmpty
		{
			get
			{
				return this.m_value == null && map_node.Count == 0;
			}
		}
		public bool		IsExist
		{
			get
			{
				return this.m_value != null || map_node.Count > 0;
			}
		}

		// 4) Add/Remove
		public Context	Insert(string _key, string _value = null)
		{
			// - 생성한다.
			var nodeNew = new Context(_key, _value, this)
			{
				m_is_new = true
			};
			nodeNew.m_is_updated = nodeNew.m_bValid;
			nodeNew.m_is_child_updated = false;

			// - Validate한다.
			nodeNew.ProcessValidate();

			// - 추가한다.
			map_node.Add(_key, nodeNew);

			// return)
			return nodeNew;
		}
		public Context	Erase(string _key)
		{
			// - 찾는다.
			var nodeExist = map_node[_key];

			// check) 찾지 못했으면 그냥 null을 리턴한다.
			if(nodeExist == null)
				return null;

			// - 제거한다.
			bool Result = map_node.Remove(_key);

			// check) 제거에 실패했으면 null을 리턴한다.
			if(Result == false)
				return null;

			// return) 제가한 node를 리턴한다.
			return nodeExist;
		}

		public static implicit operator char(Context _value)		{ return char.Parse(_value.Value);}
		public static implicit operator sbyte(Context _value)		{ return sbyte.Parse(_value.Value);}
		public static implicit operator byte(Context _value)		{ return byte.Parse(_value.Value);}
		public static implicit operator short(Context _value)		{ return short.Parse(_value.Value);}
		public static implicit operator ushort(Context _value)		{ return ushort.Parse(_value.Value);}
		public static implicit operator int(Context _value)			{ return int.Parse(_value.Value);}
		public static implicit operator uint(Context _value)		{ return uint.Parse(_value.Value);}
		public static implicit operator long(Context _value)		{ return long.Parse(_value.Value);}
		public static implicit operator ulong(Context _value)		{ return ulong.Parse(_value.Value);}
		public static implicit operator float(Context _value)		{ return float.Parse(_value.Value);}
		public static implicit operator double(Context _value)		{ return double.Parse(_value.Value);}
		public static implicit operator string(Context _value)		{ return _value.Value;}
		public static implicit operator DateTime(Context _value)	{ return DateTime.Parse(_value.Value);}
		public static implicit operator IPEndPoint(Context _value)
		{
			// 1) Splite with ':'
			string[] ep = _value.Value.Split(':');

			// declare) 
			int port = 0;

			// 2)  IP Address
			if (!IPAddress.TryParse(ep[0], out IPAddress ip))
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

		public static implicit operator Context(char _value) 		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(sbyte _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(byte _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(short _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(ushort _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(int _value)			{ return new Context(null, _value.ToString());}
		public static implicit operator Context(uint _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(long _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(ulong _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(float _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(double _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(string _value)		{ return new Context(null, _value.ToString());}
		public static implicit operator Context(DateTime _value)	{ return new Context(null, _value.ToString());}
		public static implicit operator Context(IPEndPoint _value)	{ return new Context(null, _value.ToString());}

		public bool			ReadFromJSONFile(string _filename)
		{
			// 1) File을 Open한다.
			var	reader = new StreamReader(_filename);

			// 4) 값을 읽어 사용한다.
			return this.ReadFromJSONString(reader.ReadToEnd());
		}
		public bool			ReadFromJSONString(string _strJSON)
		{
			// declare)
			bool Result;

			try
			{
				// 1) Parser를 생성한다.
				var parser = new JsonTextParser();

				// 2) _strJSON Parsing하여 JSON 객체를 생성한다.
				var	objJSON = parser.Parse(_strJSON);

				// 3) 값을 읽어 사용한다.
				Result = this.ReadFromJSONObject(objJSON);
			}
			catch(System.Exception)
			{
				Result = false;
			}

			// return) 
			return Result;
		}
		public bool			ReadFromJSONObject(JsonObject _json_object)
		{
			foreach (JsonObject field in _json_object as JsonObjectCollection)
			{
				var	objContext = Insert(field.Name, field.GetValue().ToString());

				if(objContext.ReadFromJSONObject(field)==false)
				{
					return false;
				}
			}

			// return) 
			return true;
		}
		public bool			ReadFromBuffer(CGDK.buffer _buffer)
		{
			var buf_temp = _buffer;

			// 1) Key값 읽기
			this.m_key = buf_temp.Extract<string>();

			// 2) Value값 읽기
			this.m_value = buf_temp.Extract<string>();

			// 3) Flag값 읽기
			this.m_is_new = buf_temp.Extract<bool>();
			this.m_is_updated = buf_temp.Extract<bool>();
			this.m_is_child_updated = buf_temp.Extract<bool>();;
			this.m_bValid = true;

			// 4) Child값 읽기
			var	Count = buf_temp.Extract<int>();
			for(var i=0; i<Count; ++i)
			{
				var node = new Context();

				node.ReadFromBuffer(buf_temp);

				map_node.Add(node.Key, node);
			}

			// return) 
			return true;
		}
		public bool			WriteToBuffer(CGDK.buffer _buffer)
		{
			_buffer.Append(this.m_key);
			_buffer.Append(this.m_value);
			_buffer.Append(this.m_is_new);
			_buffer.Append(this.m_is_updated);

			_buffer.Append<int>(map_node.Values.Count);
			foreach(var iter in map_node.Values)
			{
				iter.WriteToBuffer(_buffer);
			}

			// return) 
			return true;
		}
		public bool			WriteToBufferUpdated(CGDK.buffer _buffer)
		{
			// 1) Key값 저장
			_buffer.Append(m_key);

			// 2) Value값 저장
			if(this.m_is_updated)
			{
				_buffer.Append<string>(this.m_value);
			}
			else
			{
				_buffer.Append<string>(null as string);
			}

			// 3) Flag들 저장
			_buffer.Append(this.m_is_new);
			_buffer.Append(this.m_is_updated);
			_buffer.Append(this.m_is_child_updated);

			// 4) Child들 저장
			if(m_is_child_updated == true)
			{
				_buffer.Append<int>(map_node.Values.Count);
				foreach(var iter in map_node.Values)
				{
					iter.WriteToBufferUpdated(_buffer);
				}
			}
			else
			{
				_buffer.Append<int>(0);
			}

			// return) 
			return true;
		}

		// 5) ...
		private string		m_key				 = null;
		private string		m_value				 = null;
		private readonly Context m_parent		 = null;
		private bool		m_bValid			 = false;
		private bool		m_is_new			 = false;
		private bool		m_is_updated		 = false;
		private bool		m_is_child_updated	 = false;
		public Dictionary<string, Context> map_node = new Dictionary<string, Context>();

		private void		ProcessValidate()
		{
			// - 신규 노드인지 확인한다.
			this.m_is_new = !this.m_bValid;

			// - 유효한 노드임을 표시한다.
			this.m_bValid = true;

			// - 부모 노드에게도 모두 동일한 처리를 한다.
			this.m_parent?.ProcessValidateChild();
		}
		private void		ProcessValidateChild()
		{
			// - 신규 노드인지 확인한다.
			this.m_is_new = !m_bValid;

			// - 유효한 노드임을 표시한다.
			this.m_bValid = true;

			// - Child가 Update되었음을 표시한다.
			this.m_is_child_updated = true;

			// - 부모 노드에게도 모두 동일한 처리를 한다.
			this.m_parent?.ProcessValidateChild();
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
