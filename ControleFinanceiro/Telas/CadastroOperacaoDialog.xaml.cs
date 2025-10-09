using CF.Domain.Entidades;
using CF.Domain.Enumeradores;
using CF.Domain.Interfaces.ViewModel;
using CF.ViewModel;
using CF.ViewModel.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace ControleFinanceiro.Telas;

public sealed partial class CadastroOperacaoDialog : ContentDialog
{
    #region Interfaces
    private readonly ICadastroOperacaoViewModel _cadastroOperacaoViewModel;
    #endregion

    public CadastroOperacaoDialog(eTipoOperacao tipoOperacao, int PK_OperacaoFinanceira)
    {
        InitializeComponent();

        _cadastroOperacaoViewModel = Bootstrap.ServiceProvider.GetRequiredService<ICadastroOperacaoViewModel>();
        _cadastroOperacaoViewModel.DefinirOperacao(tipoOperacao, PK_OperacaoFinanceira);

        this.DataContext = _cadastroOperacaoViewModel;
    }

    private void CadastroOperacaoFinanceira_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        _cadastroOperacaoViewModel.Salvar();
    }

    private void CadastroOperacaoFinanceira_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {

    }

    private void CadastroOperacaoFinanceira_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
    {
        _cadastroOperacaoViewModel.Limpar();
    }
}
