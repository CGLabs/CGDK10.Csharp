//*****************************************************************************
//*                                                                           *
//*                      Cho sanghyun's Game Classes II                       *
//*                    for C# Ver 2.0 / Release 2019.12.11                    *
//*                                                                           *
//*                               Pool Classes                                *
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

using System.Diagnostics;

//----------------------------------------------------------------------------
//
//  CGDK.Factory.Memory
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.Factory
{
	public class Memory
	{
	// Constructor) 
		public Memory()
		{
			// 1) Pool list를 먼저 만든다.
			m_list_factory	 = new Factory.Array<byte>[53];

			// 2) 1k Byte 이하..(128 Byte 단위).
			m_list_factory[ 0]	 = new Factory.Array<byte>("MemoryBlock 128 Byte",  1*  128);
			m_list_factory[ 1]	 = new Factory.Array<byte>("MemoryBlock 256 Byte",  2*  128);
			m_list_factory[ 2]	 = new Factory.Array<byte>("MemoryBlock 384 Byte",  3*  128);
			m_list_factory[ 3]	 = new Factory.Array<byte>("MemoryBlock 512 Byte",  4*  128);
			m_list_factory[ 4]	 = new Factory.Array<byte>("MemoryBlock 640 Byte",  5*  128);
			m_list_factory[ 5]	 = new Factory.Array<byte>("MemoryBlock 768 Byte",  6*  128);
			m_list_factory[ 6]	 = new Factory.Array<byte>("MemoryBlock 896 Byte",  7*  128);
			m_list_factory[ 7]	 = new Factory.Array<byte>("MemoryBlock 1K Byte" ,  8*  128);	
																				   		  
			// 3) 8K Bytes 이하.. (1K Byte 단위)									       		  
			m_list_factory[ 8]	 = new Factory.Array<byte>("MemoryBlock 2K Byte",   2* 1024);
			m_list_factory[ 9]	 = new Factory.Array<byte>("MemoryBlock 3K Byte",   3* 1024);
			m_list_factory[10]	 = new Factory.Array<byte>("MemoryBlock 4K Byte",   4* 1024);
			m_list_factory[11]	 = new Factory.Array<byte>("MemoryBlock 5K Byte",   5* 1024);
			m_list_factory[12]	 = new Factory.Array<byte>("MemoryBlock 6K Byte",   6* 1024);
			m_list_factory[13]	 = new Factory.Array<byte>("MemoryBlock 7K Byte",   7* 1024);
			m_list_factory[14]	 = new Factory.Array<byte>("MemoryBlock 8K Byte",   8* 1024);

			// 3) 64K Bytes 이하... (4KByte 단위)
			m_list_factory[15]	 = new Factory.Array<byte>("MemoryBlock 12K Byte",   3* 4096);
			m_list_factory[16]	 = new Factory.Array<byte>("MemoryBlock 16K Byte",   4* 4096);
			m_list_factory[17]	 = new Factory.Array<byte>("MemoryBlock 20K Byte",   5* 4096);
			m_list_factory[18]	 = new Factory.Array<byte>("MemoryBlock 24K Byte",   6* 4096);
			m_list_factory[19]	 = new Factory.Array<byte>("MemoryBlock 28K Byte",   7* 4096);
			m_list_factory[20]	 = new Factory.Array<byte>("MemoryBlock 32K Byte",   8* 4096);
			m_list_factory[21]	 = new Factory.Array<byte>("MemoryBlock 36K Byte",   9* 4096);
			m_list_factory[22]	 = new Factory.Array<byte>("MemoryBlock 40K Byte",  10* 4096);
			m_list_factory[23]	 = new Factory.Array<byte>("MemoryBlock 44K Byte",  11* 4096);
			m_list_factory[24]	 = new Factory.Array<byte>("MemoryBlock 48K Byte",  12* 4096);
			m_list_factory[25]	 = new Factory.Array<byte>("MemoryBlock 52K Byte",  13* 4096);
			m_list_factory[26]	 = new Factory.Array<byte>("MemoryBlock 56K Byte",  14* 4096);
			m_list_factory[27]	 = new Factory.Array<byte>("MemoryBlock 60K Byte",  15* 4096);
			m_list_factory[28]	 = new Factory.Array<byte>("MemoryBlock 64K Byte",  16* 4096);

			// 4) 256K Bytes 이하... (16K Byte 단위)
			m_list_factory[29]	 = new Factory.Array<byte>("MemoryBlock 80K Byte" ,  5*16384);
			m_list_factory[30]	 = new Factory.Array<byte>("MemoryBlock 96K Byte" ,  6*16384);
			m_list_factory[31]	 = new Factory.Array<byte>("MemoryBlock 112K Byte",  7*16384);
			m_list_factory[32]	 = new Factory.Array<byte>("MemoryBlock 128K Byte",  8*16384);
			m_list_factory[33]	 = new Factory.Array<byte>("MemoryBlock 144K Byte",  9*16384);
			m_list_factory[34]	 = new Factory.Array<byte>("MemoryBlock 160K Byte", 10*16384);
			m_list_factory[35]	 = new Factory.Array<byte>("MemoryBlock 176K Byte", 11*16384);
			m_list_factory[36]	 = new Factory.Array<byte>("MemoryBlock 192K Byte", 12*16384);
			m_list_factory[37]	 = new Factory.Array<byte>("MemoryBlock 208K Byte", 13*16384);
			m_list_factory[38]	 = new Factory.Array<byte>("MemoryBlock 224K Byte", 14*16384);
			m_list_factory[39]	 = new Factory.Array<byte>("MemoryBlock 240K Byte", 15*16384);
			m_list_factory[40]	 = new Factory.Array<byte>("MemoryBlock 256K Byte", 16*16384);

			// 5) 1M Bytes 이하.. (64K단위)
			m_list_factory[41]	 = new Factory.Array<byte>("MemoryBlock 320K Byte",  5*65536); 
			m_list_factory[42]	 = new Factory.Array<byte>("MemoryBlock 384K Byte",  6*65536); 
			m_list_factory[43]	 = new Factory.Array<byte>("MemoryBlock 448K Byte",  7*65536); 
			m_list_factory[44]	 = new Factory.Array<byte>("MemoryBlock 512K Byte",  8*65536); 
			m_list_factory[45]	 = new Factory.Array<byte>("MemoryBlock 576K Byte",  9*65536); 
			m_list_factory[46]	 = new Factory.Array<byte>("MemoryBlock 640K Byte", 10*65536); 
			m_list_factory[47]	 = new Factory.Array<byte>("MemoryBlock 704K Byte", 11*65536); 
			m_list_factory[48]	 = new Factory.Array<byte>("MemoryBlock 768K Byte", 12*65536); 
			m_list_factory[49]	 = new Factory.Array<byte>("MemoryBlock 832K Byte", 13*65536); 
			m_list_factory[50]	 = new Factory.Array<byte>("MemoryBlock 896K Byte", 14*65536); 
			m_list_factory[51]	 = new Factory.Array<byte>("MemoryBlock 896K Byte", 15*65536); 
			m_list_factory[52]	 = new Factory.Array<byte>("MemoryBlock 1M Byte"  , 16*65536); 
		}

	// Definitions) 
		public	const int				MAX_SIZE_OF_MEMORY_BLOCK = (65536*16);

	// public) 
		public static int				SELECT_BLOCK(int _size)
		{
			if(_size<=8192)
				if (_size <= 1024)
					return (_size - 1) >> 7;
				else
					return ((_size - 1) >> 10) + 7;
			else
				if(_size<=65536)
					return ((_size-1)>>12)+13;
				else if(_size<=(16*16384))
					return ((_size-1)>>14)+25;
				else
					return ((_size-1)>>16)+37;
		}

		public static byte[]			AllocMemory(int _size)
		{
			// Declare)
			byte[]	buffer_alloced;

			if(_size <= MAX_SIZE_OF_MEMORY_BLOCK)
			{
				// 1) 어떤 Block인지 결정한다.
				int	selected = SELECT_BLOCK(_size);

				// check) Block의 Index가 0보다는 크고 Pool의 갯수보다는 작어야 한다.
				Debug.Assert(selected >= 0 && selected<m_factory_memory_block.m_list_factory.Length);

				// 2) 해당 Buffer를 할당한다.
				buffer_alloced = m_factory_memory_block.ProcessAllocMemory(selected);

				// check) 할당받은 Buffer의 크기가 요구한 크기보다는 커야 한다.
				Debug.Assert(buffer_alloced.Length >= _size);
			}
			else
			{
				// - buf를 null로 처리한다.
				buffer_alloced = new byte[_size];
			}

			// Return) 
			return buffer_alloced;
		}
		public static CGDK.buffer		AllocBuffer(int _size)
		{
			return new CGDK.buffer(Memory.AllocMemory(_size));
		}
		public static void				Free(byte[] _buffer)
		{
			// check) _buffer가 null이면 그냥 끝낸다.
			if(_buffer == null)
				return;

			if(_buffer.Length > MAX_SIZE_OF_MEMORY_BLOCK)
				return;

			// 1) 어떤 Block인지 결정한다.
			int	selected = SELECT_BLOCK(_buffer.Length);

			// check) Block의 Index가 0보다는 크고 Pool의 갯수보다는 작어야 한다.
			Debug.Assert(selected >= 0 && selected<m_factory_memory_block.m_list_factory.Length);

			// check) 
			Debug.Assert(selected >= 0 && selected<53);

			// 2) 할당해제한다.
			m_factory_memory_block.ProcessFreeMemory(selected, _buffer);
		}
		public static void				Free(ref CGDK.buffer _buffer)
		{
			// check) _buffer가 null이면 그냥 끝낸다.
			if (_buffer.Data == null)
				return;

			// check) 최대 할당 메모리 크기보다 클 경우 그냥 끝낸다.
			if (_buffer.Capacity > MAX_SIZE_OF_MEMORY_BLOCK)
			{
				// - buf를 null로 처리한다.
				_buffer.Array = null;

				// Return)
				return;
			}

			// 1) 어떤 Block인지 결정한다.
			int selected = SELECT_BLOCK(_buffer.Capacity);

			// check) Block의 Index가 0보다는 크고 Pool의 갯수보다는 작어야 한다.
			Debug.Assert(selected >= 0 && selected < m_factory_memory_block.m_list_factory.Length);

			// check) 
			Debug.Assert(selected >= 0 && selected<53);

			// 2) 할당해제한다.
			m_factory_memory_block.ProcessFreeMemory(selected, _buffer.Data);

			// 3) Null처리..
			_buffer.Array = null;
		}

	// implementation) 
		private byte[]					ProcessAllocMemory(int _block)
		{
			return m_factory_memory_block.m_list_factory[_block].Alloc();
		}
		private bool					ProcessFreeMemory(int _block, byte[] _object)
		{
			return m_factory_memory_block.m_list_factory[_block].Free(_object);
		}

	// Implementation)
		// 1) Pool list
		private	readonly Factory.Array<byte>[]	m_list_factory;

		// 2) Static- Memory Block Pool
		static	readonly Memory			m_factory_memory_block = new Memory();
	}
}