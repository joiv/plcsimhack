using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    internal class DeviceTagConverter
    {
        internal static byte[] ShortToBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        internal static byte[] IntToBytes(int x)
        {
            return BitConverter.GetBytes(x);
        }

        internal static byte[] UShortToBytes(ushort x)
        {
            return BitConverter.GetBytes(x);
        }

        internal static byte[] UIntToBytes(uint x)
        {
            return BitConverter.GetBytes(x);
        }

        internal static byte[] FloatToBytes(float x)
        {
            return BitConverter.GetBytes(x);
        }

        internal static byte[] StringToBytes(string x)
        {
            var enc = new UTF8Encoding();
            return enc.GetBytes(x);
        }

        internal static short BytesToShort(byte[] x)
        {
            return BitConverter.ToInt16(x, 0);
        }

        internal static int BytesToInt(byte[] x)
        {
            return BitConverter.ToInt32(x, 0);
        }

        internal static float BytesToFloat(byte[] x)
        {
            return BitConverter.ToSingle(x, 0);
        }

        internal static uint BytesToUInt(byte[] x)
        {
            return BitConverter.ToUInt32(x, 0);
        }

        internal static ushort BytesToUShort(byte[] x)
        {
            return BitConverter.ToUInt16(x, 0);
        }

        internal static string BytesToString(byte[] x)
        {
            var enc = new UTF8Encoding();
            return enc.GetString(x);
        }
    }
}
