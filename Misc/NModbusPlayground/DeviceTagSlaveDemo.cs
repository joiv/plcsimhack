using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Modbus.Data;
using Modbus.Device;

namespace NModbusPlayground
{
    public class DeviceTagSlaveDemo
    {
        byte m_SlaveId;
        int m_Port;
        IPAddress m_IpAddress;
        ModbusSlave m_ModbusSlave;
        TcpListener m_TcpListener;

        DeviceTag<Int32> testTag1;
        DeviceTag<Int32> testTag2;

        public DeviceTagSlaveDemo(int port, byte id)
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
            m_ModbusSlave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;
            InitDeviceTags();
            m_ModbusSlave.Listen();
        }

        public void Stop()
        {
            m_ModbusSlave.Dispose();
            m_TcpListener = null;
            m_ModbusSlave = null;
        }

        public void StringTest()
        {
            var stringTestTag = new DeviceTag<string>();
            var binding = new DeviceTagModbusBinding(40050, stringTestTag, m_ModbusSlave.DataStore);
            stringTestTag.Value = "ABCD";
            Thread.Sleep(2000);
            stringTestTag.Value = "EFGH";
        }

        public void RampTest()
        {
            Console.WriteLine("Running ramp test");
            for(int i=0;i<100;++i)
            {
                testTag1.Value = i;
                testTag2.Value = i * 3;
                Thread.Sleep(1000);
            }
        }

        bool isRampRunning;

        public void Ramp()
        {
            Thread t = new Thread(new ThreadStart(RampFunction));
            t.Start();
        }

        void RampFunction()
        {
            var tag = new DeviceTag<Int32>();
             
            while(isRampRunning)
            {

            }
        }

        public void BindingTest()
        {
            testTag1.Value = 0xff;
        }

        private void InitDeviceTags()
        {
            testTag1 = new DeviceTag<int>();
            testTag2 = new DeviceTag<int>();

            var testTagBinding1 = new DeviceTagModbusBinding(40000, testTag1, m_ModbusSlave.DataStore);
            var testTagBinding2 = new DeviceTagModbusBinding(40002, testTag2, m_ModbusSlave.DataStore);
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
                foreach (var data in e.Data.A)
                    Console.Write(data);
            }
            Console.WriteLine();
        }
    }
}
