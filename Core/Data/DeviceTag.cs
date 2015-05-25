using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DeviceTag<T> : DeviceTagBase, IDeviceTag
    {
        private Func<T, byte[]> m_ValueToByteConverter;
        private Func<byte[], T> m_ByteToValueConverter;

        protected T m_Value;

        public event EventHandler ValueChanged;

        public DeviceTag() : this(NextId)
        {
        }

        public DeviceTag(int id) : base(id)
        {
            m_ValueToByteConverter = Factory.CreateValueToByteConverter<T>();
            m_ByteToValueConverter = Factory.CreateByteToValueConverter<T>();
        }

        protected virtual void OnValueChanged()
        {
            var handler = ValueChanged;
            if(handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public T Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
                OnPropertyChanged();
                OnValueChanged();
            }
        }

        event EventHandler IDeviceTag.DeviceTagValueChanged
        {
            add
            {
                ValueChanged += value;
            }

            remove
            {
                ValueChanged -= value;
            }
        }

        int IDeviceTag.Id
        {
            get
            {
                return m_Id;
            }
        }

        void IDeviceTag.Write(byte[] data)
        {
            m_Value = m_ByteToValueConverter(data);
            OnPropertyChanged("Value");
        }

        byte[] IDeviceTag.Read()
        {
            return m_ValueToByteConverter(m_Value);
        }
    }
}
