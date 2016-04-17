namespace Categorizer.Data
{
    public interface IDataProvider<T>
    {
        void Save(T something);
        T Read();
    }
}
