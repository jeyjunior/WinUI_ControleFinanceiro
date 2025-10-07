using CF.Domain.Atributos;
using CF.Domain.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CF.Data.Dicionario
{
    public static class SQLTradutorFactory
    {
        public static eTipoBancoDados TipoBancoDados { get; set; }

        public static string ObterUltimoInsert()
        {
            string query = "";

            switch (TipoBancoDados)
            {
                case eTipoBancoDados.SQLite: query = "SELECT last_insert_rowid();"; break;
                case eTipoBancoDados.SQLServer: query = "SELECT SCOPE_IDENTITY();"; break;
                case eTipoBancoDados.MySQL: query = "SELECT LAST_INSERT_ID();"; break;
            }

            return query;
        }

        public static object TratarData(object value)
        {
            DateTime dateTimeValue = (DateTime)value;

            switch (TipoBancoDados)
            {
                case eTipoBancoDados.SQLite:
                case eTipoBancoDados.MySQL:
                case eTipoBancoDados.SQLServer:
                    break;
            }

            return dateTimeValue;
        }

        public static string ObterSintaxeChavePrimaria()
        {
            switch (TipoBancoDados)
            {
                case eTipoBancoDados.SQLite:
                    return "PRIMARY KEY AUTOINCREMENT";
                case eTipoBancoDados.SQLServer:
                    return "PRIMARY KEY IDENTITY";
                case eTipoBancoDados.MySQL:
                    return "PRIMARY KEY AUTO_INCREMENT";
                default:
                    throw new InvalidOperationException("Banco de dados não suportado para sintaxe de chave primária.");
            }
        }

        public static string ObterSintaxeForeignKey(string columnName, string tabelaReferenciada, string chavePrimaria)
        {
            switch (TipoBancoDados)
            {
                case eTipoBancoDados.SQLite:
                    return $"FOREIGN KEY ({columnName}) REFERENCES {tabelaReferenciada}({chavePrimaria})";
                case eTipoBancoDados.SQLServer:
                    return $"FOREIGN KEY ({columnName}) REFERENCES {tabelaReferenciada}({chavePrimaria})";
                case eTipoBancoDados.MySQL:
                    return $"FOREIGN KEY ({columnName}) REFERENCES {tabelaReferenciada}({chavePrimaria})";
                default:
                    throw new InvalidOperationException("Banco de dados não suportado para criação de chaves estrangeiras.");
            }
        }

        public static string ObterTipoColuna(PropertyInfo propriedade)
        {
            Type propertyType = Nullable.GetUnderlyingType(propriedade.PropertyType) ?? propriedade.PropertyType;

            string tipoColuna;

            switch (propertyType.Name.ToLower())
            {
                case "string":
                    tipoColuna = "TEXT";
                    break;
                case "int32":
                    tipoColuna = "INTEGER";
                    break;
                case "int64":
                    tipoColuna = "BIGINT";
                    break;
                case "decimal":
                    tipoColuna = "DECIMAL";
                    break;
                case "double":
                    tipoColuna = "DOUBLE";
                    break;
                case "float":
                    tipoColuna = "FLOAT";
                    break;
                case "datetime":
                    tipoColuna = "DATETIME";
                    break;
                case "boolean":
                    tipoColuna = "BOOLEAN";
                    break;
                case "int16":
                    tipoColuna = "SMALLINT";
                    break;
                default:
                    throw new ArgumentException($"Tipo de propriedade não suportado: {propriedade.PropertyType.Name}");
            }

            switch (TipoBancoDados)
            {
                case eTipoBancoDados.SQLite:
                    if (propertyType == typeof(bool))
                    {
                        tipoColuna = "INTEGER";
                    }
                    else if (propertyType == typeof(decimal))
                    {
                        tipoColuna = "REAL";
                    }
                    break;

                case eTipoBancoDados.SQLServer:
                    if (propertyType == typeof(bool))
                    {
                        tipoColuna = "BIT";
                    }
                    break;

                case eTipoBancoDados.MySQL:
                    if (propertyType == typeof(bool))
                    {
                        tipoColuna = "TINYINT(1)";
                    }
                    break;

                default:
                    throw new InvalidOperationException("Banco de dados não suportado para tipos de dados.");
            }

            if (TipoBancoDados == eTipoBancoDados.MySQL || TipoBancoDados == eTipoBancoDados.SQLServer)
            {
                if (propertyType == typeof(string))
                {
                    var tamanhoAttr = propriedade.GetCustomAttributes(typeof(TamanhoString), false).FirstOrDefault() as TamanhoString;
                    tipoColuna = tamanhoAttr != null ? $"VARCHAR({tamanhoAttr.Tamanho})" : "TEXT";
                }
                else if (propertyType == typeof(decimal))
                {
                    var tamanhoAttr = propriedade.GetCustomAttributes(typeof(TamanhoDecimal), false).FirstOrDefault() as TamanhoDecimal;
                    tipoColuna = tamanhoAttr != null
                        ? $"DECIMAL({tamanhoAttr.Tamanho},{tamanhoAttr.Decimais})"
                        : "DECIMAL(18,2)";
                }
            }

            return tipoColuna;
        }

    }
}
