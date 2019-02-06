using Integration.Cache;
using Integration.Cache.Context;
using Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration
{
    class CommonStarter : IStarter
    {
        public void Start()
        {
            MetaDataRegistry.Register<ICommonAttributeMarker, PropertyAttributeContext>();
        }
    }
}
