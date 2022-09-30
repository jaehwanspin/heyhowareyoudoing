using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSystemSimulator.Net
{
    class Checksum
    {

        public static byte ComputeCRC8(byte[] bytes)
        {
            const byte generator = 0x1D;
            byte crc = 0;

            foreach (byte currByte in bytes)
            {
                crc ^= currByte;

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (byte)((crc << 1) ^ generator);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            return crc;
        }

        public static ushort ComputeCRC16(byte[] bytes)
        {
            const ushort generator = 0x1021;
            ushort crc = 0;

            foreach (byte b in bytes)
            {
                crc ^= (ushort)(b << 8);

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = (ushort)((crc << 1) ^ generator);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            return crc;
        }

        public static uint ComputeCRC32(byte[] bytes)
        {
            const uint polynomial = 0x04C11DB7;
            uint crc = 0;

            foreach (byte b in bytes)
            {
                crc ^= (uint)(b << 24);

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x80000000) != 0)
                    {
                        crc = (uint)((crc << 1) ^ polynomial);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            return crc;
        }

    }

    
}
