using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSpec.Interfaces
{
    public interface IMapper<SourceT, TargetT>
    {
        SourceT ConvertToSource(TargetT);
        TargetT ConvertToTarget(SourceT);
    }
}
