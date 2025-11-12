using CF.Data.Extensao;
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
            string where = condition;
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
                "        ON    Categoria.PK_Categoria    = OperacaoFinanceira.FK_Categoria\n"; 

            if (!string.IsNullOrWhiteSpace(where))
                query += " WHERE " + where + "\n";

            query += orderby;

            var resultado = unitOfWork.Connection.ObterLista<OperacaoFinanceiraGrid>(query,parameters, true).ToList();

            return resultado;
        }

        public OperacaoFinanceiraResumo ObterResumoOperacao(string condition = "", object parameters = null)
        {
            string query = "";
            string where = condition;
            string orderby = "";

            query = "" +
                " SELECT    Operacao.TotalReceitaPaga,\n" +
                "           Operacao.TotalReceita,\n" +
                "           Operacao.TotalDespesaPaga,\n" +
                "           Operacao.TotalDespesa,\n" +
                "           (Operacao.TotalReceita - Operacao.TotalDespesa) AS Saldo\n" +
                " FROM \n" +
                " (\n" +
                "       SELECT  SUM (CASE WHEN DataTransacao IS NOT NULL AND FK_TipoOperacao = 2 THEN Valor ELSE 0 END) AS TotalDespesaPaga,\n" +
                "               SUM (CASE WHEN FK_TipoOperacao = 2 THEN Valor ELSE 0 END) AS TotalDespesa,\n" +
                "               SUM (CASE WHEN DataTransacao IS NOT NULL AND FK_TipoOperacao = 1 THEN Valor ELSE 0 END) AS TotalReceitaPaga,\n" +
                "               SUM (CASE WHEN FK_TipoOperacao = 1 THEN Valor ELSE 0 END) AS TotalReceita\n" +
                "       FROM    OperacaoFinanceira\n" +
                "       WHERE   (OperacaoFinanceira.DataVencimento >= @DataInicial AND OperacaoFinanceira.DataVencimento < @DataFinal)\n" +
                " )     AS Operacao\n";

            query += orderby;

            var resultado = unitOfWork.Connection.QuerySingleOrDefault<OperacaoFinanceiraResumo>(
                sql: query,
                param: parameters);

            return resultado;
        }
    }
}
