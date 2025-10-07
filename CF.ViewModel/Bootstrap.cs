using CF.Application;
using CF.Domain.Entidades;
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

                BootstrapApp.IniciarConfiguracao();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro na inicializa��o: {ex}");
            }
        }
        private static void RegistrarViewModels(IServiceCollection services)
        {
            //services.AddSingleton<ICategoriaVM, CategoriaVM>();
            //services.AddSingleton<ITipoInvestimentoVM, TipoInvestimentoVM>();
            //services.AddSingleton<IEntidadeFinanceiraVM, EntidadeFinanceiraVM>();
        }
    }
}
