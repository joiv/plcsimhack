using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Modbus.Data;

namespace ModbusDeviceDriver
{
    public class DeviceTagBinding
    {
        private int m_Address;
        private int m_DataStoreArrayPosition;
        private IDeviceTag m_DeviceTag;
        private readonly DataStore m_DataStore;
        private ModbusDataType m_ModbusDataType;

        public DeviceTagBinding(int modbusAddress, IDeviceTag deviceTag, DataStore dataStore)
        {
            m_Address = modbusAddress;
            m_DeviceTag = deviceTag;
            m_DataStore = dataStore;
            InitDataStoreParams();
            m_DeviceTag.DeviceTagValueChanged += DeviceTagValueChanged;
        }

        private void InitDataStoreParams()
        {
            int dt = m_Address / 10000;
            int arrayPos = m_Address % 10000;
            switch (dt)
            {
                case 0: //Coil outputs
                    m_ModbusDataType = ModbusDataType.Coil;
                    m_DataStoreArrayPosition = arrayPos + 1;
                    break;
                case 1: //Digital inputs
                    m_ModbusDataType = ModbusDataType.Input;
                    m_DataStoreArrayPosition = arrayPos + 1;
                    break;
                case 3: //Analogue inputs
                    m_ModbusDataType = ModbusDataType.InputRegister;
                    m_DataStoreArrayPosition = arrayPos + 1;
                    break;
                case 4: //Holding registers
                    m_ModbusDataType = ModbusDataType.HoldingRegister;
                    m_DataStoreArrayPosition = arrayPos + 1;
                    break;
                default:
                    break;
            }
        }

        private List<UInt16> ConvertDeviceTagValue()
        {
            var list = new List<UInt16>();
            byte[] valueData = m_DeviceTag.Read();
            for (int i = 0; i < valueData.Length; i += 2)
            {
                byte[] tmp;
                if (valueData.Length <= i + 1)
                    tmp = new byte[] { valueData[i], 0 };
                else
                    tmp = new byte[] { valueData[i], valueData[i + 1] };

                list.Add(BitConverter.ToUInt16(tmp, 0));
            }
            if (m_DeviceTag.GetType() != typeof(DeviceTag<string>))
                list.Reverse();
            return list;
        }

        private List<UInt16> ConvertStringDeviceTagValue()
        {
            var list = new List<UInt16>();
            byte[] valueData = m_DeviceTag.Read();
            for (int i = 0; i < valueData.Length; i += 2)
            {
                byte[] tmp;
                if (valueData.Length <= i + 1)
                    tmp = new byte[] { valueData[i], 0 };
                else
                    tmp = new byte[] { valueData[i], valueData[i + 1] };
                Array.Reverse(tmp);
                list.Add(BitConverter.ToUInt16(tmp, 0));
            }
            return list;
        }

        private void DeviceTagValueChanged(object sender, EventArgs e)
        {
            switch (m_ModbusDataType)
            {
                case ModbusDataType.HoldingRegister:
                    WriteHoldingRegister();
                    break;
                case ModbusDataType.InputRegister:
                    break;
                case ModbusDataType.Coil:
                    break;
                case ModbusDataType.Input:
                    break;
                default:
                    break;
            }
        }

        private void WriteHoldingRegister()
        {
            List<UInt16> data;
            if (m_DeviceTag.GetType() != typeof(DeviceTag<string>))
                data = ConvertDeviceTagValue();
            else
                data = ConvertStringDeviceTagValue();
            for (int i = 0; i < data.Count; ++i)
            {
                m_DataStore.HoldingRegisters[m_DataStoreArrayPosition + i] = data[i];
            }
        }
    }
}
