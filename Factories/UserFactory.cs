using System.ComponentModel.DataAnnotations;
using TaskTimeDesignPatterns.Interfaces;
using TaskTimePredicter.Models;

namespace TaskTimeDesignPatterns.Factories
{
    public class UserFactory : IUserFactory
    {
        public User CreateUser(string userName, string userEmail, string userPassword, string userRole)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(userEmail) || !new EmailAddressAttribute().IsValid(userEmail))
            {
                throw new ArgumentException("El correo electrónico es inválido.");
            }

            if (string.IsNullOrWhiteSpace(userPassword))
            {
                throw new ArgumentException("La contraseña no puede estar vacía.");
            }

            if (string.IsNullOrWhiteSpace(userRole))
            {
                throw new ArgumentException("El rol de usuario no puede estar vacío.");
            }

            return new User
            {
                UserName = userName,
                UserEmail = userEmail,
                UserPassword = userPassword,
                UserRole = userRole,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now) // Validación 'CreatedAt' != Nulo ni vacío
            };
        }
    }
}
