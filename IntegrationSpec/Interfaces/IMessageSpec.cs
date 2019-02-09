using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSpec.Interfaces
{
    public interface IMessageSpec<TargetT, SourceT, MapperT, ContextT>
                           where MapperT : IMapper<TargetT, SourceT>
                           where ContextT : IContext
    {
        TargetT CreateMessage(SourceT data, MapperT mapper, ContextT context);
        SourceT ConsumeMessage(TargetT message, MapperT mapper, ContextT context);
    }
}
