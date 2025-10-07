using CF.Domain.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Dto
{
    public class ParametrosConfiguracao
    {
        public bool Ativo { get; set; }
        public eTipoBancoDados TipoBanco { get; set; }
        public string StringConexao { get; set; }
        public string NomeAplicacao { get; set; } = string.Empty;
    }
}
