using CF.Domain.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Entidades
{
    [Entidade("TipoOperacao")]
    public class TipoOperacao
    {
        [ChavePrimaria]
        public int PK_TipoOperacao { get; set; }

        [Obrigatorio, TamanhoString(50)]
        public string Nome { get; set; }
    }
}
