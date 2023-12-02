//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                           Server Event Classes                            *
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
using System.IO;
using CGDK;

// ----------------------------------------------------------------------------
//
// <<class>> CGDK.CGFile
//
//
//
// ----------------------------------------------------------------------------
namespace CGDK
{
	public class archive_file : 
		Nobject_stateable
	{
	// publics) 
		public bool						start(string _filename)
		{
			// check)
			if(_filename == null)
				return false;

			// 1) MSG객체와 Context객체를 생성한다.
			var	context_now = new context();

			// 2) Filename을 설정한다.
			context_now["filename"]	 = _filename;

			// 3) start하기
			return base.start(context_now);
		}

		public async void				write(char[] _String)
		{
			// 1) File을 써넣는다.
			await m_file.WriteLineAsync(_String);
            m_file.Flush();
		}
		public async void				write(string _String)
		{
			// 1) File을 써넣는다.
			await m_file.WriteLineAsync(_String);
            m_file.Flush();
		}

	// implementations) 
		protected override void			_process_notify_starting(object _object, context _context)
		{
			base._process_notify_starting(_object, _context);
		}
		protected override void			_process_notify_start(object _object, context _context)
		{
			// 1) Context를 얻는다.
			var	context_now = _context;

			// Declare)
			string str_filename = context_now["filename"];

			// check) Filename이 설정되어 있지 않으면 System.Exception을 던진다.
			if(str_filename == null)
				throw new System.Exception();

			// 2) File을 Open한다.
			var file = new System.IO.StreamWriter(str_filename, true);

			// check) File을 열지 못했으면 System.Exception을 던진다.
			if(file == null)
				return;

			// 2) file을 설정
			m_file = file;
			m_strFileName = str_filename;

			// 2) 
			base._process_notify_starting(_object, _context);
		}
		protected override void			_process_notify_stopping(object _object)
		{
			// 1) Base의 ProcessNotifyStopping을 호출한다.
			base._process_notify_stopping(_object);

			// 2) File을 닫는다.
			if(m_file != null)
			{
				m_file.Close();
				m_file = null;
				m_strFileName = null;
			}
		}
		protected override void			_process_notify_stop(object _object)
		{
			// 1) Base의 ProcessNotifyStop을 호출한다.
			base._process_notify_stop(_object);
		}

		private		StreamWriter		m_file;
		private		string				m_strFileName;
	}

}
