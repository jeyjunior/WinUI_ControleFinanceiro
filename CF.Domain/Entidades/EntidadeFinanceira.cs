using CF.Domain.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Entidades
{
    [Entidade("EntidadeFinanceira")]
    public class EntidadeFinanceira
    {
        [ChavePrimaria]
        public int PK_EntidadeFinanceira { get; set; }

        [Obrigatorio, TamanhoString(50)]
        public string Nome { get; set; }

        [Relacionamento("Usuario", "PK_Usuario")]
        public int? FK_Usuario { get; set; }
    }
}
