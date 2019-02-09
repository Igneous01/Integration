using IntegrationSpec.Interfaces;
using System;
using System.Reactive;
using System.Reactive.Linq;

namespace IntegrationSpec.Abstracts
{
    abstract class IntegrationAction<DataT, TargetT, ContextT, IpcType, IpcConfigType, MapperT, MessageSpecT>
        where ContextT : IContext
        where IpcConfigType : IIPCConfig
        where IpcType : IRxIPC<TargetT, IpcConfigType>
        where MapperT : IMapper<TargetT, DataT>, new()
        where MessageSpecT : IMessageSpec<TargetT, DataT, MapperT, ContextT>, new()
    {
        private readonly IpcType _ipc;
        private readonly MapperT _mapper;
        private readonly MessageSpecT _spec;
        private bool IsConnected = false;

        public IntegrationAction(IpcType Ipc)
        {
            _ipc = Ipc;
            _mapper = new MapperT();
            _spec = new MessageSpecT();

            if (!_ipc.IsConnected)
                _ipc.Connect().Subscribe((result) => IsConnected = result);
        }

        protected abstract ContextT BeforeAction(ContextT context);

        public IObservable<DataT> Action(DataT data, ContextT context)
        {
            context = BeforeAction(context);
            if (IsConnected)
                return PerformAction(data, context);
            else
            {
                IsConnected = _ipc.Connect().Wait();
                if (IsConnected)
                    return PerformAction(data, context);
                else
                    throw new Exception("Failed to connect to target");
            }
        }

        private IObservable<DataT> PerformAction(DataT data, ContextT context)
        {
            return _ipc.Send(_spec.CreateMessage(data, _mapper, context))
                           .Select((TargetT message) => _spec.ConsumeMessage(message, _mapper, context));
        }
    }
}
