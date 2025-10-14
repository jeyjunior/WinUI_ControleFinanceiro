using CF.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Interfaces.Repository
{
    public interface IOperacaoFinanceiraRepository : IRepository<OperacaoFinanceira>
    {
        IEnumerable<OperacaoFinanceiraGrid> ObterListaGrid(string condition = "", object parameters = null);
        OperacaoFinanceiraResumo ObterResumoOperacao(string condition = "", object parameters = null);
    }
}
