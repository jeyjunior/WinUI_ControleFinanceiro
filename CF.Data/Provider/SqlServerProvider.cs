using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CF.Data.Provider
{
    internal static class SqlServerProvider
    {
        public static IDbConnection CriarConexao(string connectionString)
        {
            var conexao = new SqlConnection(connectionString);
            TestarConexao(conexao);
            return conexao;
        }

        private static void TestarConexao(SqlConnection conexao)
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
            catch (SqlException ex)
            {
                throw new InvalidOperationException(
                    $"Erro ao conectar com o SQL Server. Verifique a string de conexão: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao testar a conexão com o SQL Server: {ex.Message}", ex);
            }
            finally
            {
                if (conexao.State == ConnectionState.Open)
                    conexao.Close();
            }
        }
    }
}