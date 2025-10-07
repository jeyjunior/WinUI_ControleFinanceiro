using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Enumeradores
{
    public enum eTipoBancoDados
    {
        SQLite = 1,
        SQLServer = 2,
        MySQL = 3,
    }
    public enum eTipoOperacao
    {
        Adicionar = 1,
        Editar = 2,
        Excluir = 3,
        Salvar = 4,
        Cancelar = 5
    }
    public enum eHabilitarEdicao
    {
        Sim = 1,
        Nao = 2
    }
}
