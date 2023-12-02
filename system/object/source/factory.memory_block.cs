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
//  CGDK.factory.memory
//
//
//
//
//
//----------------------------------------------------------------------------
namespace CGDK.factory
{
	public class memory
	{
	// Constructor) 
		public memory()
		{
			// 1) Pool list를 먼저 만든다.
			m_list_factory	 = new factory.array<byte>[53];

			// 2) 1k Byte 이하..(128 Byte 단위).
			m_list_factory[ 0]	 = new factory.array<byte>("MemoryBlock 128 Byte",  1*  128);
			m_list_factory[ 1]	 = new factory.array<byte>("MemoryBlock 256 Byte",  2*  128);
			m_list_factory[ 2]	 = new factory.array<byte>("MemoryBlock 384 Byte",  3*  128);
			m_list_factory[ 3]	 = new factory.array<byte>("MemoryBlock 512 Byte",  4*  128);
			m_list_factory[ 4]	 = new factory.array<byte>("MemoryBlock 640 Byte",  5*  128);
			m_list_factory[ 5]	 = new factory.array<byte>("MemoryBlock 768 Byte",  6*  128);
			m_list_factory[ 6]	 = new factory.array<byte>("MemoryBlock 896 Byte",  7*  128);
			m_list_factory[ 7]	 = new factory.array<byte>("MemoryBlock 1K Byte" ,  8*  128);	
																				   		  
			// 3) 8K Bytes 이하.. (1K Byte 단위)									       		  
			m_list_factory[ 8]	 = new factory.array<byte>("MemoryBlock 2K Byte",   2* 1024);
			m_list_factory[ 9]	 = new factory.array<byte>("MemoryBlock 3K Byte",   3* 1024);
			m_list_factory[10]	 = new factory.array<byte>("MemoryBlock 4K Byte",   4* 1024);
			m_list_factory[11]	 = new factory.array<byte>("MemoryBlock 5K Byte",   5* 1024);
			m_list_factory[12]	 = new factory.array<byte>("MemoryBlock 6K Byte",   6* 1024);
			m_list_factory[13]	 = new factory.array<byte>("MemoryBlock 7K Byte",   7* 1024);
			m_list_factory[14]	 = new factory.array<byte>("MemoryBlock 8K Byte",   8* 1024);

			// 3) 64K Bytes 이하... (4KByte 단위)
			m_list_factory[15]	 = new factory.array<byte>("MemoryBlock 12K Byte",   3* 4096);
			m_list_factory[16]	 = new factory.array<byte>("MemoryBlock 16K Byte",   4* 4096);
			m_list_factory[17]	 = new factory.array<byte>("MemoryBlock 20K Byte",   5* 4096);
			m_list_factory[18]	 = new factory.array<byte>("MemoryBlock 24K Byte",   6* 4096);
			m_list_factory[19]	 = new factory.array<byte>("MemoryBlock 28K Byte",   7* 4096);
			m_list_factory[20]	 = new factory.array<byte>("MemoryBlock 32K Byte",   8* 4096);
			m_list_factory[21]	 = new factory.array<byte>("MemoryBlock 36K Byte",   9* 4096);
			m_list_factory[22]	 = new factory.array<byte>("MemoryBlock 40K Byte",  10* 4096);
			m_list_factory[23]	 = new factory.array<byte>("MemoryBlock 44K Byte",  11* 4096);
			m_list_factory[24]	 = new factory.array<byte>("MemoryBlock 48K Byte",  12* 4096);
			m_list_factory[25]	 = new factory.array<byte>("MemoryBlock 52K Byte",  13* 4096);
			m_list_factory[26]	 = new factory.array<byte>("MemoryBlock 56K Byte",  14* 4096);
			m_list_factory[27]	 = new factory.array<byte>("MemoryBlock 60K Byte",  15* 4096);
			m_list_factory[28]	 = new factory.array<byte>("MemoryBlock 64K Byte",  16* 4096);

			// 4) 256K Bytes 이하... (16K Byte 단위)
			m_list_factory[29]	 = new factory.array<byte>("MemoryBlock 80K Byte" ,  5*16384);
			m_list_factory[30]	 = new factory.array<byte>("MemoryBlock 96K Byte" ,  6*16384);
			m_list_factory[31]	 = new factory.array<byte>("MemoryBlock 112K Byte",  7*16384);
			m_list_factory[32]	 = new factory.array<byte>("MemoryBlock 128K Byte",  8*16384);
			m_list_factory[33]	 = new factory.array<byte>("MemoryBlock 144K Byte",  9*16384);
			m_list_factory[34]	 = new factory.array<byte>("MemoryBlock 160K Byte", 10*16384);
			m_list_factory[35]	 = new factory.array<byte>("MemoryBlock 176K Byte", 11*16384);
			m_list_factory[36]	 = new factory.array<byte>("MemoryBlock 192K Byte", 12*16384);
			m_list_factory[37]	 = new factory.array<byte>("MemoryBlock 208K Byte", 13*16384);
			m_list_factory[38]	 = new factory.array<byte>("MemoryBlock 224K Byte", 14*16384);
			m_list_factory[39]	 = new factory.array<byte>("MemoryBlock 240K Byte", 15*16384);
			m_list_factory[40]	 = new factory.array<byte>("MemoryBlock 256K Byte", 16*16384);

			// 5) 1M Bytes 이하.. (64K단위)
			m_list_factory[41]	 = new factory.array<byte>("MemoryBlock 320K Byte",  5*65536); 
			m_list_factory[42]	 = new factory.array<byte>("MemoryBlock 384K Byte",  6*65536); 
			m_list_factory[43]	 = new factory.array<byte>("MemoryBlock 448K Byte",  7*65536); 
			m_list_factory[44]	 = new factory.array<byte>("MemoryBlock 512K Byte",  8*65536); 
			m_list_factory[45]	 = new factory.array<byte>("MemoryBlock 576K Byte",  9*65536); 
			m_list_factory[46]	 = new factory.array<byte>("MemoryBlock 640K Byte", 10*65536); 
			m_list_factory[47]	 = new factory.array<byte>("MemoryBlock 704K Byte", 11*65536); 
			m_list_factory[48]	 = new factory.array<byte>("MemoryBlock 768K Byte", 12*65536); 
			m_list_factory[49]	 = new factory.array<byte>("MemoryBlock 832K Byte", 13*65536); 
			m_list_factory[50]	 = new factory.array<byte>("MemoryBlock 896K Byte", 14*65536); 
			m_list_factory[51]	 = new factory.array<byte>("MemoryBlock 896K Byte", 15*65536); 
			m_list_factory[52]	 = new factory.array<byte>("MemoryBlock 1M Byte"  , 16*65536); 
		}

	// Definitions) 
		public	const int				MAX_SIZE_OF_MEMORY_BLOCK = (65536*16);

	// public) 
		public static int				SELECT_BLOCK(int _size)
		{
			if(_size<=8192)
			{
				if(_size<=1024)
				{
					return  (_size-1)>>7;
				}
				else
				{
					return	((_size-1)>>10)+7;
				}
			}
			else
			{
				if(_size<=65536)
				{
					return	((_size-1)>>12)+13;
				}
				else
				{
					if(_size<=(16*16384))
					{
						return	((_size-1)>>14)+25;
					}
					else
					{
						return	((_size-1)>>16)+37;
					}
				}
			}
		}

		public static byte[]			alloc_memory(int _size)
		{
			// Declare)
			byte[]	buffer_alloced = null;

			if(_size <= MAX_SIZE_OF_MEMORY_BLOCK)
			{
				// 1) 어떤 Block인지 결정한다.
				int	selected = SELECT_BLOCK(_size);

				// check) Block의 Index가 0보다는 크고 Pool의 갯수보다는 작어야 한다.
				Debug.Assert(selected >= 0 && selected<m_factory_memory_block.m_list_factory.Length);

				// 2) 해당 Buffer를 할당한다.
				buffer_alloced = m_factory_memory_block.process_alloc_memory(selected);

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
		public static CGDK.buffer		alloc_buffer(int _size)
		{
			return new CGDK.buffer(memory.alloc_memory(_size));
		}
		public static void				free(byte[] _buffer)
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
			m_factory_memory_block.process_free_memory(selected, _buffer);
		}
		public static void				free(ref CGDK.buffer _buffer)
		{
			// check) _buffer가 null이면 그냥 끝낸다.
			if (_buffer.data == null)
				return;

			// check) 최대 할당 메모리 크기보다 클 경우 그냥 끝낸다.
			if (_buffer.capacity > MAX_SIZE_OF_MEMORY_BLOCK)
			{
				// - buf를 null로 처리한다.
				_buffer.data	 = null;

				// Return)
				return;
			}

			// 1) 어떤 Block인지 결정한다.
			int selected = SELECT_BLOCK(_buffer.capacity);

			// check) Block의 Index가 0보다는 크고 Pool의 갯수보다는 작어야 한다.
			Debug.Assert(selected >= 0 && selected<m_factory_memory_block.m_list_factory.Length);

			// check) 
			Debug.Assert(selected >= 0 && selected<53);

			// 2) 할당해제한다.
			m_factory_memory_block.process_free_memory(selected, _buffer.data);

			// 3) Null처리..
			_buffer.data = null;
		}

	// implementation) 
		private byte[]					process_alloc_memory(int _block)
		{
			return m_factory_memory_block.m_list_factory[_block].alloc();
		}
		private bool					process_free_memory(int _block, byte[] _object)
		{
			return m_factory_memory_block.m_list_factory[_block].free(_object);
		}

	// Implementation)
		// 1) Pool list
		private	factory.array<byte>[]	m_list_factory;

		// 2) Static- Memory Block Pool
		static	memory					m_factory_memory_block = new memory();
	}
}