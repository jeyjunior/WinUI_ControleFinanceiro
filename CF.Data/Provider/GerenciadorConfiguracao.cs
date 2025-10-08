using CF.Domain.Dto;
using CF.Domain.Enumeradores;
using Microsoft.UI.Xaml;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace CF.Data.Provider
{
    public class GerenciadorConfiguracao
    {
        private const string NomeArquivoConfig = "parametros.json";
        private readonly string _caminhoArquivo;

        public GerenciadorConfiguracao()
        {
            _caminhoArquivo = Path.Combine(ApplicationData.Current.LocalFolder.Path, NomeArquivoConfig);
        }

        // Métodos SÍNCRONOS para uso no UnitOfWork
        public ParametrosConfiguracao ObterConfiguracaoAtiva()
        {
            if (!File.Exists(_caminhoArquivo))
            {
                System.Diagnostics.Debug.WriteLine("Arquivo de configuração não encontrado. Criando um novo...");
                CriarArquivoConfiguracaoPadrao();
            }

            try
            {
                string jsonContent = File.ReadAllText(_caminhoArquivo);
                var listaParametros = JsonConvert.DeserializeObject<List<ParametrosConfiguracao>>(jsonContent);

                if (listaParametros == null || !listaParametros.Any())
                {
                    throw new Exception("O arquivo de configuração está vazio ou em um formato inválido.");
                }

                return listaParametros.FirstOrDefault(p => p.Ativo);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao ler o arquivo de configuração. Detalhes: {ex.Message}", ex);
            }
        }

        public void SalvarParametros(List<ParametrosConfiguracao> parametros)
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(parametros, Formatting.Indented);
                File.WriteAllText(_caminhoArquivo, jsonString);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao salvar o arquivo de configuração. Detalhes: {ex.Message}", ex);
            }
        }

        private void CriarArquivoConfiguracaoPadrao()
        {
            var listaPadrao = new List<ParametrosConfiguracao>
            {
                new ParametrosConfiguracao
                {
                    Ativo = true,
                    TipoBanco = eTipoBancoDados.SQLite,
                    NomeAplicacao = "ControleFinanceiro_SQLite",
                    StringConexao = $"Data Source={Path.Combine(ApplicationData.Current.LocalFolder.Path, "ControleFinanceiro.db")};Version=3;"
                },
                new ParametrosConfiguracao
                {
                    Ativo = false,
                    TipoBanco = eTipoBancoDados.SQLServer,
                    NomeAplicacao = "ControleFinanceiro",
                    StringConexao = "Server=Instancia;Database=NomeBaseDados;Integrated Security=True;TrustServerCertificate=True;"
                },
                new ParametrosConfiguracao
                {
                    Ativo = false,
                    TipoBanco = eTipoBancoDados.MySQL,
                    NomeAplicacao = "ControleFinanceiro",
                    StringConexao = ";"
                }
            };

            SalvarParametros(listaPadrao);
        }

        public List<ParametrosConfiguracao> ObterTodosParametros()
        {
            if (!File.Exists(_caminhoArquivo))
            {
                CriarArquivoConfiguracaoPadrao();
            }

            string jsonContent = File.ReadAllText(_caminhoArquivo);
            return JsonConvert.DeserializeObject<List<ParametrosConfiguracao>>(jsonContent) ?? new List<ParametrosConfiguracao>();
        }

        public void AtualizarParametro(ParametrosConfiguracao parametroAtualizado)
        {
            var todosParametros = ObterTodosParametros();
            var parametroExistente = todosParametros.FirstOrDefault(p => p.NomeAplicacao == parametroAtualizado.NomeAplicacao);

            if (parametroExistente != null)
            {
                var index = todosParametros.IndexOf(parametroExistente);
                todosParametros[index] = parametroAtualizado;
            }
            else
            {
                todosParametros.Add(parametroAtualizado);
            }

            SalvarParametros(todosParametros);
        }

        public void DefinirConfiguracaoAtiva(string nomeAplicacao)
        {
            var todosParametros = ObterTodosParametros();

            foreach (var parametro in todosParametros)
            {
                parametro.Ativo = (parametro.NomeAplicacao == nomeAplicacao);
            }

            SalvarParametros(todosParametros);
        }

        // Mantenha os métodos async se precisar em outras partes do código
        public async Task<ParametrosConfiguracao> ObterConfiguracaoAtivaAsync()
        {
            // Implementação async se necessário
            return await Task.Run(() => ObterConfiguracaoAtiva());
        }
    }
}