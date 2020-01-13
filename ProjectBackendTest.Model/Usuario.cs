using ProjectBackendTest.Model.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBackendTest.Model
{
    public class Usuario
    {
        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Senha { get; set; }
        public bool IsAtivo { get; set; }
        [NotMapped]
        public string DecryptedPassword
        {
            get { return Decrypt(Senha); }
            set { Senha = Encrypt(value); }
        }
        private string Decrypt(string cipherText)
        {
            return EntityHelper.Decrypt(cipherText);
        }
        private string Encrypt(string clearText)
        {
            return EntityHelper.Encrypt(clearText);
        }
    }

}
