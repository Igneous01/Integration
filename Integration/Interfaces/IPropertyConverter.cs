namespace Integration.Interfaces
{
    public interface IPropertyConverter<T1, T2> : IAttributeMarker
    {
        T1 ConvertToSourceType(T2 source);
        T2 ConvertToDestinationType(T1 source);
    }
}
