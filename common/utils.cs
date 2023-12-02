//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 1.0 / Release 2015.01.05                    *
//*                                                                           *
//*                          Network Socket Classes                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*                                                                           *
//*  This Program is programmed by Cho SangHyun. sangducks@cgcii.co.kr        *
//*  Best for Game Developement and Optimized for Game Developement.          *
//*                                                                           *
//*                    (C) 2008 CGLabs All right reserved.                    *
//*                          http://www.CGCII.co.kr                           *
//*                                                                           *
//*****************************************************************************


//----------------------------------------------------------------------------
//  CGDK.Network.CGNetSocketClasses
//
//  class CSocketTCP
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK
{

public class Util
{
	public static string			ToString(float p_Value)
	{
		if(p_Value > (1024.0f*1024.0f*1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}T", p_Value/(1024.0f*1024.0f*1024.0f*1024.0f));
		}
		else if(p_Value > (1024.0f*1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}G", p_Value/(1024.0f*1024.0f*1024.0f));
		}
		else if(p_Value > (1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}M", p_Value/(1024.0f*1024.0f));
		}
		else if(p_Value > 1024.0f)
		{
			return	string.Format("{0:F2}K", p_Value/1024.0f);
		}
		else
		{
			return	string.Format("{0:F2}", p_Value);
		}
	}
	public static string			ToString(long p_Value)
	{
		if(p_Value > (1024.0f*1024.0f*1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}T", p_Value/(1024.0f*1024.0f*1024.0f*1024.0f));
		}
		else if(p_Value > (1024.0f*1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}G", p_Value/(1024.0f*1024.0f*1024.0f));
		}
		else if(p_Value > (1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}M", p_Value/(1024.0f*1024.0f));
		}
		else if(p_Value > 1024.0f)
		{
			return	string.Format("{0:F2}K", p_Value/1024.0f);
		}
		else
		{
			return	string.Format("{0:F2}", p_Value);
		}
	}
	public static string			ToString(int p_Value)
	{
		if(p_Value > (1024.0f*1024.0f*1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}T", p_Value/(1024.0f*1024.0f*1024.0f*1024.0f));
		}
		else if(p_Value > (1024.0f*1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}G", p_Value/(1024.0f*1024.0f*1024.0f));
		}
		else if(p_Value > (1024.0f*1024.0f))
		{
			return	string.Format("{0:F2}M", p_Value/(1024.0f*1024.0f));
		}
		else if(p_Value > 1024.0f)
		{
			return	string.Format("{0:F2}K", p_Value/1024.0f);
		}
		else
		{
			return	string.Format("{0:F2}", p_Value);
		}
	}
}


}
