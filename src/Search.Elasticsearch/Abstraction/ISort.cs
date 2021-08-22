namespace Search.Elastic.Abstraction
{
    using Nest;

    public interface ISort
    {
        SortDescriptor<T> Get<T>(SortDescriptor<T> sortDescriptor) where T : class;
    }
}
