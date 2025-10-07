using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CF.ViewModel
{
    public abstract class ViewModelBase : IViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void PropriedadeAlterada([CallerMemberName] string nomePropriedade = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }
    }
}
