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
		NObjectStateable
	{
	// publics) 
		public bool						Start(string _filename)
		{
			// check)
			if(_filename == null)
				return false;

			// 1) MSG객체와 Context객체를 생성한다.
			var	Context_now = new Context();

			// 2) Filename을 설정한다.
			Context_now["filename"] = _filename;

			// 3) Start하기
			return base.Start(Context_now);
		}

		public async void				Write(char[] _String)
		{
			// 1) File을 써넣는다.
			await this.m_file.WriteLineAsync(_String);
            this.m_file.Flush();
		}
		public async void				Write(string _String)
		{
			// 1) File을 써넣는다.
			await this.m_file.WriteLineAsync(_String);
            this.m_file.Flush();
		}

	// implementations) 
		protected override void			_ProcessNotifyStarting(object _object, Context _Context)
		{
			base._ProcessNotifyStarting(_object, _Context);
		}
		protected override void			_ProcessNotifyStart(object _object, Context _Context)
		{
			// 1) Context를 얻는다.
			var	Context_now = _Context;

			// Declare)
			string str_filename = Context_now["filename"];

			// check) Filename이 설정되어 있지 않으면 System.Exception을 던진다.
			if(str_filename == null)
				throw new System.Exception();

			// 2) File을 Open한다.
			var file = new System.IO.StreamWriter(str_filename, true);

			// check) File을 열지 못했으면 System.Exception을 던진다.
			if(file == null)
				return;

			// 2) file을 설정
			this.m_file = file;
			this.m_strFileName = str_filename;

			// 2) 
			base._ProcessNotifyStarting(_object, _Context);
		}
		protected override void			_ProcessNotifyStopping(object _object)
		{
			// 1) Base의 ProcessNotifyStopping을 호출한다.
			base._ProcessNotifyStopping(_object);

			// 2) File을 닫는다.
			if(this.m_file != null)
			{
				this.m_file.Close();
				this.m_file = null;
				this.m_strFileName = null;
			}
		}
		protected override void			_ProcessNotifyStop(object _object)
		{
			// 1) Base의 ProcessNotifyStop을 호출한다.
			base._ProcessNotifyStop(_object);
		}

		private		StreamWriter		m_file;
		private		string				m_strFileName;
	}

}
