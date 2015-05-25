using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public static partial class Factory
    {
        public static Func<T, byte[]> CreateValueToByteConverter<T>()
        {
            Type typeOperator = typeof(T);
            if (typeOperator.IsAssignableFrom(typeof(short)))
            {
                return new Func<short, byte[]>(x => DeviceTagConverter.ShortToBytes(x)) as Func<T, byte[]>;
            }
            else if (typeOperator.IsAssignableFrom(typeof(int)))
            {
                return new Func<int, byte[]>(x => DeviceTagConverter.IntToBytes(x)) as Func<T, byte[]>;
            }
            else if (typeOperator.IsAssignableFrom(typeof(ushort)))
            {
                return new Func<ushort, byte[]>(x => DeviceTagConverter.UShortToBytes(x)) as Func<T, byte[]>;
            }
            else if (typeOperator.IsAssignableFrom(typeof(uint)))
            {
                return new Func<uint, byte[]>(x => DeviceTagConverter.UIntToBytes(x)) as Func<T, byte[]>;
            }
            else if (typeOperator.IsAssignableFrom(typeof(float)))
            {
                return new Func<float, byte[]>(x => DeviceTagConverter.FloatToBytes(x)) as Func<T, byte[]>;
            }
            else if (typeOperator.IsAssignableFrom(typeof(string)))
            {
                return new Func<string, byte[]>(x => DeviceTagConverter.StringToBytes(x)) as Func<T, byte[]>;
            }
            else
                throw new ArgumentException("The generic type is not supported");
        }

        public static Func<byte[], T> CreateByteToValueConverter<T>()
        {
            Type type = typeof(T);
            if (type.IsAssignableFrom(typeof(short)))
            {
                return new Func<byte[], short>(x => DeviceTagConverter.BytesToShort(x)) as Func<byte[], T>;
            }
            else if (type.IsAssignableFrom(typeof(int)))
            {
                return new Func<byte[], int>(x => DeviceTagConverter.BytesToInt(x)) as Func<byte[], T>;
            }
            else if (type.IsAssignableFrom(typeof(ushort)))
            {
                return new Func<byte[], ushort>(x => DeviceTagConverter.BytesToUShort(x)) as Func<byte[], T>;
            }
            else if (type.IsAssignableFrom(typeof(uint)))
            {
                return new Func<byte[], uint>(x => DeviceTagConverter.BytesToUInt(x)) as Func<byte[], T>;
            }
            else if (type.IsAssignableFrom(typeof(float)))
            {
                return new Func<byte[], float>(x => DeviceTagConverter.BytesToFloat(x)) as Func<byte[], T>;
            }
            else if (type.IsAssignableFrom(typeof(string)))
            {
                return new Func<byte[], string>(x => DeviceTagConverter.BytesToString(x)) as Func<byte[], T>;
            }
            else
                throw new ArgumentException("The generic type is not supported");
        }
    }
}
