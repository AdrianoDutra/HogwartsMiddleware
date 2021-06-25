using System.ComponentModel.DataAnnotations;

namespace Hogwarts.Middleware.Dtos
{
    public class CharacterInsertDto
    {
        [Required(ErrorMessage = "Name é um campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Name deve ter no máximo {1} caracteres.")]
        public string name { get; set; }

        [Required(ErrorMessage = "Role é um campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Role deve ter no máximo {1} caracteres.")]
        public string role { get; set; }

        [Required(ErrorMessage = "School é um campo obrigatório!")]
        [StringLength(100, ErrorMessage = "School deve ter no máximo {1} caracteres.")]
        public string school { get; set; }

        [Required(ErrorMessage = "House é um campo obrigatório!")]
        [StringLength(36, ErrorMessage = "House deve ter no máximo {1} caracteres.")]
        public string house { get; set; }

        [Required(ErrorMessage = "Patronus é um campo obrigatório!")]
        [StringLength(100, ErrorMessage = "Patronus deve ter no máximo {1} caracteres.")]
        public string patronus { get; set; }
    }
}
