using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OAuth.ViewModel
{
    public class ViewModelForUserRegistration
    {
        [Required]
        [DisplayName("Identificación")]
        public string Identification { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        [DisplayName("Correo")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "El campo contraseña debe tener una longitud mínima de '6'")]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirmación de contraseña")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "Aceptar los términos")]
        public bool TermAndConditions { get; set; } = false;

        public string ErrorMessage { get; set; } = "";

        public bool Error { get; set; } = false;
    }
}