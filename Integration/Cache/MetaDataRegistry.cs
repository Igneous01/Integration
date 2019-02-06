using System;
using System.Collections.Concurrent;

namespace Integration.Cache
{
    public static class MetaDataRegistry
    {
        public static ConcurrentBag<MetaDataEntry> Entries;

        static MetaDataRegistry()
        {
            Entries = new ConcurrentBag<MetaDataEntry>();
        }

        public static void Register(Type interface_, Type type, Type validator = null)
        {
            Entries.Add(new MetaDataEntry() { Interface = interface_, Type = type, Validator = validator });
        }

        public static void Register<I, T>()
        {
            Entries.Add(new MetaDataEntry() { Interface = typeof(I), Type = typeof(T) });
        }

        public static void Register<I, T, V>()
        {
            Entries.Add(new MetaDataEntry() { Interface = typeof(I), Type = typeof(T), Validator = typeof(V) });
        }
    }
}
