
using Shared.DomainModels;

namespace DomainServices.Interfaces
{
    public interface IBizService<T>
    {

        public T Add(T data);

        public int Count();
        public T Get(string id);
    }
}