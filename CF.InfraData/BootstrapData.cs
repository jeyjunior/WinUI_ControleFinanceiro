using CF.Data;
using CF.Domain.Interfaces;
using CF.Domain.Interfaces.Repository;
using CF.InfraData.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace CF.InfraData
{
    public static class BootstrapData
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IUsuarioRepository, UsuarioRepository>();
            services.AddSingleton<IStatusPagamentoRepository, StatusPagamentoRepository>();
            services.AddSingleton<ITipoOperacaoFinanceiraRepository, TipoOperacaoFinanceiraRepository>();
            services.AddSingleton<ICategoriaRepository, CategoriaRepository>();
            services.AddSingleton<IEntidadeFinanceiraRepository, EntidadeFinanceiraRepository>();
            services.AddSingleton<IOperacaoFinanceiraRepository, OperacaoFinanceiraRepository>();
        }
    }
}
