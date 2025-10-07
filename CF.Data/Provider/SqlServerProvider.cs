using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CF.Data.Provider
{
    using System;
    using System.Data;
    using Microsoft.Data.SqlClient;

    namespace CF.Data.Provider
    {
        internal static class SqlServerProvider
        {
            private const string STRING_CONEXAO_PADRAO =
                "Server=(localdb)\\MSSQLLocalDB;Database=ControleFinanceiro;TrustServerCertificate=True;";

            public static IDbConnection CriarConexao()
            {
                string connectionString = STRING_CONEXAO_PADRAO;
                var conexao = new SqlConnection(connectionString);

                return conexao;
            }

            public static IDbConnection CriarConexao(string connectionString)
            {
                return new SqlConnection(connectionString);
            }
        }
    }
}