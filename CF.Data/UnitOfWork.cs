using CF.Data.Extensao;
using CF.Data.Provider;
using CF.Domain.Dto;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces;
using System;
using System.Data;

namespace CF.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;

        public UnitOfWork()
        {
            var gerenciador = new GerenciadorConfiguracao();
            var configAtiva = gerenciador.ObterConfiguracaoAtiva(); 

            if (configAtiva == null)
            {
                throw new InvalidOperationException("Nenhuma configuração de banco de dados encontrada.");
            }

            _connection = CriarConexaoPorTipo(configAtiva);
            _connection.DefinirTipoBancoDados(configAtiva.TipoBanco);
        }

        private IDbConnection CriarConexaoPorTipo(ParametrosConfiguracao config)
        {
            return config.TipoBanco switch
            {
                eTipoBancoDados.SQLite => SqliteProvider.CriarConexao(),
                _ => throw new InvalidOperationException($"Tipo de banco não suportado: {config.TipoBanco}")
            };
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction;

        public void Begin()
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                _transaction = _connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();

                _connection.Open();
                _transaction = _connection.BeginTransaction();

                throw new InvalidOperationException("Erro ao iniciar a transação", ex);
            }
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _connection?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}