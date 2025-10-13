using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Data;
using CF.Domain.Interfaces;
using CF.Domain.Interfaces.Repository;
using CF.InfraData.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CF.InfraData
{
    public static class BootstrapData
    {
        public static void RegistrarServicos(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITipoOperacaoRepository, TipoOperacaoRepository>();
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IEntidadeFinanceiraRepository, EntidadeFinanceiraRepository>();
            services.AddScoped<IOperacaoFinanceiraRepository, OperacaoFinanceiraRepository>();
        }
    }
}