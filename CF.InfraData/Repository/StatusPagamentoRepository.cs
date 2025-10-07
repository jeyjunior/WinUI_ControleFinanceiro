using CF.Domain.Entidades;
using CF.Domain.Interfaces;
using CF.Domain.Interfaces.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.InfraData.Repository
{
    public class StatusPagamentoRepository : Repository<StatusPagamento>, IStatusPagamentoRepository
    {
        public StatusPagamentoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
