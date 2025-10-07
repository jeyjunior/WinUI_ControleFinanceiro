using CF.Domain.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Entidades
{
    [Entidade("Usuario")]
    public class Usuario
    {
        [ChavePrimaria]
        public int PK_Usuario { get; set; }

        [Obrigatorio, TamanhoString(255)]
        public string Email { get; set; }

        [Obrigatorio, TamanhoString(255)]
        public string Nome { get; set; }

        [TamanhoString(2000)]
        public string LoginApi { get; set; } 

        [Obrigatorio]
        public DateTime DataCadastro { get; set; }

        [Obrigatorio]
        public bool Ativo { get; set; }
    }
}
