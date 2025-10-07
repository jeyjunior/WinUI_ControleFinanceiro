using CF.Data;
using CF.Data.Extensao;
using CF.Domain.Entidades;
using CF.InfraData.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.InfraData
{
    public static class Configuracao
    {
        public static void RegistrarEntidades()
        {
            using (var uow = new UnitOfWork())
            {
                if (!uow.Connection.VerificarTabelaExistente<Usuario>())
                {
                    try
                    {
                        uow.Begin();

                        uow.Connection.CriarTabela(typeof(Usuario), uow.Transaction);
                        uow.Connection.CriarTabela(typeof(StatusPagamento), uow.Transaction);
                        uow.Connection.CriarTabela(typeof(TipoOperacaoFinanceira), uow.Transaction);
                        uow.Connection.CriarTabela(typeof(EntidadeFinanceira), uow.Transaction);
                        uow.Connection.CriarTabela(typeof(Categoria), uow.Transaction);
                        uow.Connection.CriarTabela(typeof(OperacaoFinanceira), uow.Transaction);

                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        uow.Rollback();
                        throw new Exception("Falha ao tentar gerar tabelas na base de dados.");
                    }
                }
            }
        }

        public static void InserirValoresPadroes()
        {
            using (var uow = new UnitOfWork())
            {
                var tipoOperacaoFinanceiraRepository = new TipoOperacaoFinanceiraRepository(uow);
                var statusPagamentoRepository = new StatusPagamentoRepository(uow);

                if (tipoOperacaoFinanceiraRepository.ObterLista().Count() <= 0)
                {
                    try
                    {
                        uow.Begin();

                        tipoOperacaoFinanceiraRepository.Adicionar(new TipoOperacaoFinanceira { Nome = "Entrada" });
                        tipoOperacaoFinanceiraRepository.Adicionar(new TipoOperacaoFinanceira { Nome = "Saída" });

                        statusPagamentoRepository.Adicionar(new StatusPagamento { Nome = "Pago" });
                        statusPagamentoRepository.Adicionar(new StatusPagamento { Nome = "Em Aberto" });
                        statusPagamentoRepository.Adicionar(new StatusPagamento { Nome = "Vencido" });

                        uow.Commit();
                    }
                    catch (Exception ex)
                    {
                        uow.Rollback();
                        throw new Exception("Falha ao tentar gerar tabelas na base de dados.");
                    }
                }
            }
        }
    }
}
