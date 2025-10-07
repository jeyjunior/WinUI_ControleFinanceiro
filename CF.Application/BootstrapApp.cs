using CF.Domain.Interfaces.Repository;
using CF.InfraData;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Application
{
    public static class BootstrapApp
    {
        public static void RegistrarRepositorios(IServiceCollection services)
        {
            BootstrapData.RegistrarServicos(services);
        }

        public static void IniciarConfiguracao()
        {
            Configuracao.RegistrarEntidades();
            Configuracao.InserirValoresPadroes();
        }
    }
}
