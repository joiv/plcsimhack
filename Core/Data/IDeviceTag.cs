using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    /// <summary>
    /// Represents a non generic interface
    /// to read and write value data on byte level.
    /// </summary>
    public interface IDeviceTag
    {
        event EventHandler DeviceTagValueChanged;
        void Write(byte[] data);
        byte[] Read();
        int Id { get; }
    }
}
