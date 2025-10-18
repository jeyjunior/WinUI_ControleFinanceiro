using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Dto
{
    public class ValidacaoResultado
    {
        public bool Sucesso { get; set; }
        public List<string> Erros { get; set; } = new List<string>();
    }
}
