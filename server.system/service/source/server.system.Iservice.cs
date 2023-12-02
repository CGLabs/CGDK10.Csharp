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
//  class CGDK.server.Nservice
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{
namespace server
{
	public class Nservice :
		Iobject_stateable,
		Iinitializable,
		Istartable,
		Inameable
	{
	// constructor) 
		public Nservice()
		{
			m_component_state		 = new Cobject_state();
			m_component_state.target = this;
		}

	// publics) 
		public string					name
		{
			get
			{
				return m_name;
			}
			set
			{
				m_name = value;
			}
		}

		public eOBJECT_STATE			object_state
		{
			get
			{
				return m_component_state.object_state;
			}
			set
			{
				m_component_state.object_state=value;
			}
		}
		public bool						set_object_state_if(eOBJECT_STATE _value, eOBJECT_STATE _compare)
		{
			return	m_component_state.set_object_state_if(_value, _compare);
		}

		public bool						initialize()
		{
			context temp_context = new context();

			return initialize(temp_context);
		}
		public bool						initialize(context _context)
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> initialize SERVICE['" + name + "']");

			var	result = m_component_state.initialize(_context);

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> initialize SERVICE['" + name + "']");

			// return)
			return result;
		}
		public bool						destroy()
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> destroy SERVICE['" + name + "']");

			var	result	= m_component_state.destroy();

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> destroy SERVICE['" + name + "']");

			// return)
			return	result;
		}
		public bool						start()
		{
			context temp_context = new context();

			return start(temp_context);
		}
		public bool						start(context _context)
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> start SERVICE['" + name + "']");

			var	result	= m_component_state.start(_context);

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> start SERVICE['" + name + "']");

			// return)
			return	result;
		}
		public bool						stop()
		{
			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <BEGIN> stop SERVICE['" + name + "']");

			var	result	= m_component_state.stop();

			// Trace)
			LOG.PROGRESS_IMPORTANT(null, "@ <END> stop SERVICE['" + name + "']");

			// return)
			return	result;
		}
		public bool						attach(Iobject_stateable _child)
		{
			return m_component_state.attach(_child);
		}
		public int						detach(Iobject_stateable _child)
		{
			return m_component_state.detach(_child);
		}
		public IEnumerator 				GetEnumerator()
		{
			return m_component_state.GetEnumerator();
		}

	// implementations) 
		private string					m_name;
		protected Cobject_state			m_component_state;
	}
}
}
