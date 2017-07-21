using System.Collections.Generic;

namespace Api_ELearning.DependencyInjections
{
    public interface IRepository<T>
    {
        bool Add(T o);
        bool Edit(T o);
        T Get(int id);
        IEnumerable<T> All { get; set; }
        bool Remove(int id);
    }
}
