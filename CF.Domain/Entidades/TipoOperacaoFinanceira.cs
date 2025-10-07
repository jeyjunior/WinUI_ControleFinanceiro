using CF.Domain.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Entidades
{
    [Entidade("TipoOperacaoFinanceira")]
    public class TipoOperacaoFinanceira
    {
        [ChavePrimaria]
        public int PK_TipoOperacaoFinanceira { get; set; }

        [Obrigatorio, TamanhoString(50)]
        public string Nome { get; set; }
    }
}
