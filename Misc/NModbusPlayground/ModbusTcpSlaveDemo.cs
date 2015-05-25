using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Modbus.Data;
using Modbus.Device;

namespace NModbusPlayground
{
    public class ModbusTcpSlaveDemo
    {
        byte m_SlaveId;
        int m_Port;
        IPAddress m_IpAddress;
        ModbusSlave m_ModbusSlave;
        TcpListener m_TcpListener;

        public ModbusTcpSlaveDemo(int port, byte id)
        {
            m_SlaveId = id;
            m_Port = port;
            m_IpAddress = IPAddress.Loopback;
            m_TcpListener = new TcpListener(m_IpAddress, m_Port);
            m_ModbusSlave = ModbusTcpSlave.CreateTcp(m_SlaveId, m_TcpListener);
        }

        public void Start()
        {
            m_ModbusSlave.DataStore = DataStoreFactory.CreateDefaultDataStore();
            var reg = m_ModbusSlave.DataStore.HoldingRegisters;
            var data1 = Encoding.UTF8.GetBytes(new char[] { 'A', 'B' });
            var data2 = Encoding.UTF8.GetBytes(new char[] { 'D', 'E' });
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(data1);
                Array.Reverse(data2);
            }
            reg[51] = BitConverter.ToUInt16(data1, 0);
            reg[52] = BitConverter.ToUInt16(data2, 0);
            m_ModbusSlave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;
            m_ModbusSlave.Listen();
        }

        private void DataStore_DataStoreWrittenTo(object sender, DataStoreEventArgs e)
        {
            Console.Write("Modbus written to. Start addr: {0}, ", e.StartAddress);
            if (e.Data.Option == Modbus.Utility.DiscriminatedUnionOption.B)
            {
                foreach (var data in e.Data.B)
                {
                    Console.Write(string.Format("{0} ", data.ToString("X")));
                }
            }
            else
            {
                foreach(var data in e.Data.A)
                    Console.Write(data);
            }
            Console.WriteLine();
        }

        public void Stop()
        {
            //m_TcpListener.Stop();
            m_ModbusSlave.Dispose();
            m_TcpListener = null;
            m_ModbusSlave = null;
        }
    }
}
