using Api_ELearning.DependencyInjections;
using Api_ELearning.Models;

namespace Api_ELearning.Repositories
{
    public interface ICetificateRepository: IRepository<Cetificate>
    {
        Cetificate GetCetificate(int idCourses);
    }
}
