using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationSpec.Interfaces
{
    public interface IContext
    {
        bool Contains(string key);
        T Get<T>(string key);
        void Add<T>(string key, T value);
        void Remove(string key);
        T Pop<T>(string key);
    }
}
