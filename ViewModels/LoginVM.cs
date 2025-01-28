using System.ComponentModel.DataAnnotations;

namespace TaskTimePredicter.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El correo electrónico es Obligatorio")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "La contraseña es Obligatoria")]
        public string Password { get; set; } = null!;
    }
}
