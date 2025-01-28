using System.ComponentModel.DataAnnotations;

namespace TaskTimePredicter.ViewModels
{
    public class UserVM
    {
        [Required(ErrorMessage = "El Nombre de Usuario es Obligatorio")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "El correo electrónico es Obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "La contraseña es Obligatoria")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Confirme su contraseña")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
