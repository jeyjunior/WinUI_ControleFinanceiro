using CF.Domain.Entidades;
using CF.Domain.Interfaces;
using CF.Domain.Interfaces.Repository;
using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CF.InfraData.Repository
{
    public class OperacaoFinanceiraRepository : Repository<OperacaoFinanceira>, IOperacaoFinanceiraRepository
    {
        public OperacaoFinanceiraRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        public IEnumerable<OperacaoFinanceiraGrid> ObterListaGrid(string condition = "", object parameters = null)
        {
            string query = "";
            string where = "";
            string orderby = "";

            query = "" +
                " SELECT OperacaoFinanceira.*,\n" +
                "        TipoOperacao.Nome AS TipoOperacao,\n" +
                "        EntidadeFinanceira.Nome AS EntidadeFinanceira,\n" +
                "        Categoria.Nome AS Categoria,\n" +
                "        StatusPagamento.Nome AS StatusPagamento\n" +
                " FROM    OperacaoFinanceira\n" +
                " INNER    JOIN  TipoOperacao\n" +
                "        ON    TipoOperacao.PK_TipoOperacao = OperacaoFinanceira.FK_TipoOperacao\n" +
                " INNER    JOIN    EntidadeFinanceira\n" +
                "        ON    EntidadeFinanceira.PK_EntidadeFinanceira    = OperacaoFinanceira.FK_EntidadeFinanceira  \n" +
                " INNER    JOIN    Categoria\n" +
                "        ON    Categoria.PK_Categoria    = OperacaoFinanceira.FK_Categoria\n" +
                " INNER    JOIN    StatusPagamento\n" +
                "        ON    StatusPagamento.PK_StatusPagamento    = OperacaoFinanceira.FK_StatusPagamento\n";

            if (!string.IsNullOrWhiteSpace(condition))
                query += " WHERE " + condition + "\n";

            query += orderby;

            var resultado = unitOfWork.Connection.Query<OperacaoFinanceiraGrid>(
                sql: query,
                param: parameters).ToList();

            return resultado;
        }
    }
}
