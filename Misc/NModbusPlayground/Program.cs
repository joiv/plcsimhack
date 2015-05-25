using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NModbusPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Modbus Slave demo...");

            //var slave = new ModbusTcpSlaveDemo(503, 2);
            var slave = new DeviceTagSlaveDemo(503, 2);
            slave.Start();
            slave.BindingTest();
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            slave.StringTest();
            Console.ReadLine();
            Thread.Sleep(5000);
            slave.RampTest();
            Console.WriteLine("Press enter to quit..");
            Console.ReadLine();
            slave.Stop();
        }
    }
}
