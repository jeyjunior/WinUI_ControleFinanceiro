using CF.Domain.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Entidades
{
    [Entidade("StatusPagamento")]
    public class StatusPagamento
    {
        [ChavePrimaria]
        public int PK_StatusPagamento { get; set; }

        [Obrigatorio, TamanhoString(20)]
        public string Nome { get; set; }
    }
}
