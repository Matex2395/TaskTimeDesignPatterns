using TaskTimePredicter.Models;

namespace TaskTimeDesignPatterns.Interfaces
{
    public interface IUserFactory
    {
        User CreateUser(string userName, string userEmail, string userPassword, string userRole);
    }
}
