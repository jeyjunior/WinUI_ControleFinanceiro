using CF.Domain.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Entidades
{
    [Entidade("Categoria")]
    public class Categoria
    {
        [ChavePrimaria]
        public int PK_Categoria { get; set; }

        [Obrigatorio, TamanhoString(100)]
        public string Nome { get; set; }

        [Relacionamento("Usuario", "PK_Usuario")]
        public int? FK_Usuario { get; set; }
    }
}
