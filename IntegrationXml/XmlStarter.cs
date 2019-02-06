using Integration.Cache;
using Integration.Interfaces;
using Integration.Xml.Context;
using Integration.Xml.Interfaces;
using IntegrationXml.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationXml
{
    class XmlStarter : IStarter
    {
        public void Start()
        {
            MetaDataRegistry.Register<IXmlAttributeMarker, XmlPropertyAttributeContext, XmlPropertyAttributeValidator>();
        }
    }
}
