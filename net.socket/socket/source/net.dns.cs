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

using System;
using System.Net;
using System.Globalization;

//----------------------------------------------------------------------------
//
//  class CGDK.net.io.Isender
//
//
//----------------------------------------------------------------------------
namespace CGDK.net
{
	class dns
	{
		public static IPAddress resolve(string _host_name, System.Net.Sockets.AddressFamily _af = System.Net.Sockets.AddressFamily.InterNetwork)
		{
			// 1) get host entry
			var host = Dns.GetHostEntry(_host_name);

			// 2) get address
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == _af)
					return ip;
			}

			// return) 
			return null;
		}

		public static IPAddress parse(string _host_name)
		{
			// declare) 
			IPAddress ip;

			// 1) parse
			var result = IPAddress.TryParse(_host_name, out ip);

			// check) 
			if (result == false)
				throw new FormatException("Invalid address string");

			// return) 
			return ip;
		}

		public static IPAddress make_address(string _host_name, System.Net.Sockets.AddressFamily _af = System.Net.Sockets.AddressFamily.InterNetwork)
		{
			// declare) 
			IPAddress ip;

			// 1) try parsing first
			var result = IPAddress.TryParse(_host_name, out ip);

			// 2) try resolve if try parsing is failed
			if (result == false)
			{
				// - resolve
				ip = resolve(_host_name, _af);

				// check) ip가 null이면 Exception을 던진다.
				if (ip == null)
					throw new FormatException("Invalid ip-adress");
			}

			// return) 
			return ip;
		}

		public static IPEndPoint make_endpoint(string _host_name, System.Net.Sockets.AddressFamily _af = System.Net.Sockets.AddressFamily.InterNetwork)
		{
			// 1) splite with ':'
			string[] ep = _host_name.Split(':');

			// 2) get addresss
			IPAddress ip = make_address(ep[0], _af);

			// declare) 
			int port = 0;

			// 3) get port
			if (ep.Length == 2)
			{
				if (!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
					throw new FormatException("Invalid port");
			}

			// 4) IPEndPoint를 생성한다.
			return new IPEndPoint(ip, port);
		}

		public static IPEndPoint make_endpoint(string _host_name, string _service, System.Net.Sockets.AddressFamily _af = System.Net.Sockets.AddressFamily.InterNetwork)
		{
			// 1) splite with ':'
			string[] ep = _host_name.Split(':');

			// 2) get addresss
			IPAddress ip = make_address(ep[0], _af);

			// declare) 
			int port = 0;

			// 3) get port
			if (ep.Length == 2)
			{
				if (!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
					throw new FormatException("Invalid port");
			}

			// 4) IPEndPoint를 생성한다.
			return new IPEndPoint(ip, port);
		}
	}
}
