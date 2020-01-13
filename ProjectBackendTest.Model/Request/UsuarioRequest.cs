using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBackendTest.Models.Request
{
    public class UsuarioRequest
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [RegularExpression("^[a-zA-Z0-9àÀèÈìÌòÒùÙáÁéÉíÍóÓúÚâÂêÊîÎôÔûÛãÃõÕ\bçÇ' ]+$", ErrorMessage = "Nome não pode conter caracteres especiais.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        [EmailAddress]
        [RegularExpression("^[ 0-9a-zA-Z\b.@_\\-]+$", ErrorMessage = "E-mail não pode conter caracteres especiais.")]
        [MaxLength(100, ErrorMessage = "O E-email deve ter no máximo {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "{0} é obrigatório")]
        public bool IsAtivo { get; set; }
    }
}
