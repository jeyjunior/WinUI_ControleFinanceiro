using CF.Application;
using CF.Data.Provider;
using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.ViewModel;
using CF.ViewModel.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CF.ViewModel
{
    public static class Bootstrap
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Iniciar()
        {
            try
            {
                var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        CF.Application.BootstrapApp.RegistrarRepositorios(services);
                        RegistrarViewModels(services);
                    })
                    .Build();

                ServiceProvider = host.Services;

                try
                {
                    BootstrapApp.IniciarConfiguracao();
                }
                catch (Exception dbEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Erro na configuração do banco: {dbEx}");
                    ConfigurarFallbackParaSQLite();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro na inicialização: {ex}");
            }
        }

        private static void ConfigurarFallbackParaSQLite()
        {
            // Lógica para automaticamente mudar para SQLite se SQL Server falhar
            var gerenciador = new GerenciadorConfiguracao();
            var todosParametros = gerenciador.ObterTodosParametros();

            var sqliteConfig = todosParametros.FirstOrDefault(p => p.TipoBanco == eTipoBancoDados.SQLite);
            if (sqliteConfig != null)
            {
                gerenciador.DefinirConfiguracaoAtiva(sqliteConfig.NomeAplicacao);
            }
        }

        //public static void Iniciar()
        //{
        //    try
        //    {
        //        var host = Host.CreateDefaultBuilder()
        //            .ConfigureServices((context, services) =>
        //            {
        //                CF.Application.BootstrapApp.RegistrarRepositorios(services);
        //                RegistrarViewModels(services);
        //            })
        //            .Build();

        //        ServiceProvider = host.Services;

        //        BootstrapApp.IniciarConfiguracao();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Erro na inicialização: {ex}");
        //    }
        //}
        private static void RegistrarViewModels(IServiceCollection services)
        {
            services.AddSingleton<ICategoriaViewModel, CategoriaViewModel>();
        }
    }
}
