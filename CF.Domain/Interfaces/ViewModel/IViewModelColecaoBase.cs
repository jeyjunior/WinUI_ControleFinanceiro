using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Domain.Interfaces.ViewModel
{
    public interface IViewModelColecaoBase<T> : IViewModelBase where T : class
    {
        T ItemSelecionado { get; set; }
        void LimparItemSelecionado();
        void DefinirItemSelecionado(T item);

        ObservableCollection<T> Items { get; set; }
        void AdicionarItems(IEnumerable<T> items);
        void AdicionarItem(T item);
        void LimparItems();
    }
}
