//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                          Group Template Classes                           *
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CGDK;

//----------------------------------------------------------------------------
//
//  class CGDK.Server.service
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Server
{
	public class Service :	
		NService
	{
	// constructor) 
		public Service()
		{
			this.m_component_state.notifyOnInitializing = this._ProcessNotifyInitializing;
			this.m_component_state.notifyOnInitialize = this._ProcessNotifyInitialize;
			this.m_component_state.notifyOnDestroying = this._ProcessNotifyDestroying;
			this.m_component_state.notifyOnDestroy = this._ProcessNotifyDestroy;

			this.m_component_state.notifyOnStarting = this._ProcessNotifyStarting;
			this.m_component_state.notifyOnStart = this._ProcessNotifyStart;
			this.m_component_state.notifyOnStopping = this._ProcessNotifyStopping;
			this.m_component_state.notifyOnStop = this._ProcessNotifyStop;
		}
		public Service(string _name) : this()
		{
			this.Name = _name;
		}

	// frameworks) 
		protected virtual void			OnServiceSetting(Context _Context) {}
		protected virtual void			OnInitializing(Context _Context) {}
		protected virtual void			OnInitialize(Context _Context) {}
		protected virtual void			OnStarting(Context _Context) {}
		protected virtual void			OnStart(Context _Context) {}
		protected virtual void			OnStopping() {}
		protected virtual void			OnStop() {}
		protected virtual void			OnDestroying() {}
		protected virtual void			OnDestroy() {}

	// implementations) 
		protected void					_ProcessNotifyInitializing(object _object, Context _Context)
		{
			this.OnServiceSetting(_Context);
			this.OnInitializing(_Context);
		}
		protected void					_ProcessNotifyInitialize(object _object, Context _Context)
		{
			this.OnInitialize(_Context);
		}
		protected void					_ProcessNotifyStarting(object _object, Context _Context)
		{
			this.OnServiceSetting(_Context);
			this.OnStarting(_Context);
		}
		protected void					_ProcessNotifyStart(object _object, Context _Context)
		{
			this.OnStart(_Context);
		}
		protected void					_ProcessNotifyStopping(object _object)
		{
			this.OnStopping();
		}
		protected void					_ProcessNotifyStop(object _object)
		{
			this.OnStop();
		}
		protected void					_ProcessNotifyDestroying(object _object)
		{
			this.OnDestroying();
		}
		protected void					_ProcessNotifyDestroy(object _object)
		{
			this.OnDestroy();
		}
	}
}