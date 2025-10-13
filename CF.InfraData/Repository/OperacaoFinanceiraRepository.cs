using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
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

        public void DefinirStatusPagamento(ref eStatusPagamento statusPagamento, DateTime dataVencimento, DateTime? dataTransacao)
        {
            DateTime hoje = DateTime.Now;

            if (dataTransacao != null)
            {
                statusPagamento = eStatusPagamento.Pago;
            }
            else if (dataVencimento < hoje)
            {
                statusPagamento = eStatusPagamento.Vencido;
            }
            else
            {
                statusPagamento = eStatusPagamento.EmAberto;
            }
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
                "        Categoria.Nome AS Categoria\n" +
                " FROM    OperacaoFinanceira\n" +
                " INNER    JOIN  TipoOperacao\n" +
                "        ON    TipoOperacao.PK_TipoOperacao = OperacaoFinanceira.FK_TipoOperacao\n" +
                " INNER    JOIN    EntidadeFinanceira\n" +
                "        ON    EntidadeFinanceira.PK_EntidadeFinanceira    = OperacaoFinanceira.FK_EntidadeFinanceira  \n" +
                " INNER    JOIN    Categoria\n" +
                "        ON    Categoria.PK_Categoria    = OperacaoFinanceira.FK_Categoria\n"; ;

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
