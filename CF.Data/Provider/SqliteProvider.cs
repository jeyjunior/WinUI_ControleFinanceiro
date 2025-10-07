using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.IO;
using Windows.Storage;

namespace CF.Data.Provider
{
    internal static class SqliteProvider
    {
        private const string NOME_APLICACAO = "ControleFinanceiro";
        private const string NOME_ARQUIVO_PADRAO = "ControleFinanceiro.db";

        public static IDbConnection CriarConexao()
        {
            string caminhoBanco = ObterCaminhoPadraoBanco();
            CriarDiretorioSeNaoExistir(caminhoBanco);

            string connectionString = $"Data Source={caminhoBanco}";
            var conexao = new SqliteConnection(connectionString);

            TestarConexao(conexao);

            return conexao;
        }

        private static string ObterCaminhoPadraoBanco()
        {
            string pastaBase = ApplicationData.Current.LocalFolder.Path;
            return Path.Combine(pastaBase, NOME_ARQUIVO_PADRAO);
        }

        private static void CriarDiretorioSeNaoExistir(string caminhoBanco)
        {
            string diretorio = Path.GetDirectoryName(caminhoBanco);

            if (!Directory.Exists(diretorio))
            {
                try
                {
                    Directory.CreateDirectory(diretorio);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException(
                        $"Não foi possível criar o diretório para o banco de dados em {diretorio}", ex);
                }
            }
        }

        private static void TestarConexao(SqliteConnection conexao)
        {
            try
            {
                conexao.Open();
                using (var cmd = conexao.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1;";
                    cmd.ExecuteScalar();
                }
            }
            catch (SqliteException ex)
            {
                throw new InvalidOperationException($"Erro no banco SQLite: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao testar a conexão com o banco de dados: {ex.Message}", ex);
            }
            finally
            {
                if (conexao.State == ConnectionState.Open)
                    conexao.Close();
            }
        }
    }
}