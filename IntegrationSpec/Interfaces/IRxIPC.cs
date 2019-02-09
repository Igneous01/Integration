using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSpec.Interfaces
{
    public interface IRxIPC<T, ConfigType> where ConfigType : IIPCConfig
    {
        void Create(ConfigType config);
        IObservable<bool> Connect();
        void Disconnect();
        IObservable<T> Send(T message);
        bool IsConnected { get; }
    }
}
