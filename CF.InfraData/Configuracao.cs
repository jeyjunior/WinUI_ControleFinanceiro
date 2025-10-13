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
                        uow.Connection.CriarTabela(typeof(TipoOperacao), uow.Transaction);
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
                var tipoOperacaoFinanceiraRepository = new TipoOperacaoRepository(uow);

                if (tipoOperacaoFinanceiraRepository.ObterLista().Count() <= 0)
                {
                    try
                    {
                        uow.Begin();

                        tipoOperacaoFinanceiraRepository.Adicionar(new TipoOperacao { Nome = "Entrada" });
                        tipoOperacaoFinanceiraRepository.Adicionar(new TipoOperacao { Nome = "Saída" });

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
