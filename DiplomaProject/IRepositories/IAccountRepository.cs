using System.Threading.Tasks;
using DiplomaProject.Models;

namespace DiplomaProject.IRepositories;

public interface IAccountRepository
{
    Task<User> GetUserByEmail(string email);
    
}