#r "Core.dll"
#r "ModbusDeviceDriver.dll"
#r "NModbus4.dll"

/** Beijer Hackathon 					**/
/** PLC Simulator	 					**/
/** JIN@BEIJERELECTRONICS.COM (c) 2015	**/

using Core.Data;
using Modbus.Data;
using Modbus.Device;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ModbusDeviceDriver;


class ModbusTcpSlaveDemo 
{
	byte m_SlaveId;
    int m_Port;
    IPAddress m_IpAddress;
    ModbusSlave m_ModbusSlave;
    TcpListener m_TcpListener;

    public ModbusTcpSlaveDemo(int port, byte slaveId)
    {
    	m_SlaveId = slaveId;
    	m_Port = port;
    	m_IpAddress = IPAddress.Loopback;
    	m_TcpListener = new TcpListener(m_IpAddress, m_Port);
        m_ModbusSlave = ModbusTcpSlave.CreateTcp(m_SlaveId, m_TcpListener);
    }

    public void Start()
    {
    	m_ModbusSlave.DataStore = DataStoreFactory.CreateDefaultDataStore();
    	m_ModbusSlave.DataStore.DataStoreWrittenTo += DataStore_DataStoreWrittenTo;
    	m_ModbusSlave.Listen();
    }

    public void RunDeviceTagRamp()
    {    	
    	var tag = new DeviceTag<Int32>();
    	var binding = new DeviceTagBinding(40002, tag, m_ModbusSlave.DataStore);

    	for(int i=0;i<50;++i)
    	{
    		tag.Value = i*3;
    		Thread.Sleep(500);
    	}
    }

    public void TestDeviceTag()
    {
    	var tag = new DeviceTag<Int32>();
    	var binding = new DeviceTagBinding(40000, tag, m_ModbusSlave.DataStore);
    	tag.Value = 255;
    }

    public void Stop()
    {
    	m_ModbusSlave.Dispose();
    	m_TcpListener = null;
    	m_ModbusSlave = null;
    }

	private void DataStore_DataStoreWrittenTo(object sender, DataStoreEventArgs e)
	{
		Console.Write("Modbus written to. Start addr: {0}, ", e.StartAddress);
		if (e.Data.Option == Modbus.Utility.DiscriminatedUnionOption.B)
		{
			foreach (var data in e.Data.B)
				Console.Write(string.Format("{0} ", data.ToString("X")));
		}
		else
		{
			foreach (var data in e.Data.A)
				Console.Write(data);
		}
		Console.WriteLine();
	}
}

var simulator = new ModbusTcpSlaveDemo(503, 2);
simulator.Start();
simulator.TestDeviceTag();
Console.ReadLine();
simulator.RunDeviceTagRamp();
Console.WriteLine("Modbus slave started..");
Console.ReadLine();
