using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Core.Data
{
    public abstract class DeviceTagBase : INotifyPropertyChanged
    {
        private static int m_IdIndex;

        protected int m_Id;

        public event PropertyChangedEventHandler PropertyChanged;

        protected DeviceTagBase() : this(NextId)
        {
        }

        protected DeviceTagBase(int id)
        {
            m_Id = id;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                var args = new PropertyChangedEventArgs(propertyName);
                handler(this, args);
            }
        }

        protected static int NextId
        {
            get { return ++m_IdIndex; }
        }

    }
}
