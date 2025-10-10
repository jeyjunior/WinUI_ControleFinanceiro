using CF.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Interfaces.ViewModel
{
    public interface IOperacaoViewModel
    {
        ObservableCollection<OperacaoFinanceiraGrid> OperacaoFinanceiraCollection { get; }
        void CarregarColecoes();
    }
}
