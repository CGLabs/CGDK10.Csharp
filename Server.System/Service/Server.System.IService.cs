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

using System.Collections;
using CGDK;

//----------------------------------------------------------------------------
//
//  class CGDK.Server.Nservice
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
namespace Server
{
	public class NService :
		IObjectStateable,
		IInitializable,
		IStartable,
		INameable
	{
	// constructor) 
		public NService()
		{
			this.m_component_state = new ObjectState();
			this.m_component_state.Target = this;
		}

	// publics) 
		public string					Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		public eOBJECT_STATE			Now
		{
			get
			{
				return this.m_component_state.Now;
			}
			set
			{
				this.m_component_state.Now = value;
			}
		}
		public bool						SetObjectStateIf(eOBJECT_STATE _value, eOBJECT_STATE _compare)
		{
			return	this.m_component_state.SetObjectStateIf(_value, _compare);
		}

		public bool						Initialize()
		{
			var temp_Context = new Context();

			return this.Initialize(temp_Context);
		}
		public bool						Initialize(Context _Context)
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> Initialize SERVICE['" + Name + "']");

			var	result = this.m_component_state.Initialize(_Context);

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> Initialize SERVICE['" + Name + "']");

			// return)
			return result;
		}
		public bool						Destroy()
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> Destroy SERVICE['" + Name + "']");

			var	result = this.m_component_state.Destroy();

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> Destroy SERVICE['" + Name + "']");

			// return)
			return result;
		}
		public bool						Start()
		{
			Context temp_Context = new Context();

			return this.Start(temp_Context);
		}
		public bool						Start(Context _Context)
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> Start SERVICE['" + Name + "']");

			var	result	= this.m_component_state.Start(_Context);

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> Start SERVICE['" + Name + "']");

			// return)
			return	result;
		}
		public bool						Stop()
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> Stop SERVICE['" + Name + "']");

			var	result = m_component_state.Stop();

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> Stop SERVICE['" + Name + "']");

			// return)
			return result;
		}
		public bool						Attach(IObjectStateable _child)
		{
			return this.m_component_state.Attach(_child);
		}
		public int						Detach(IObjectStateable _child)
		{
			return this.m_component_state.Detach(_child);
		}
		public IEnumerator 				GetEnumerator()
		{
			return this.m_component_state.GetEnumerator();
		}

	// implementations) 
		private string					m_name;
		protected ObjectState			m_component_state;
	}
}
}
