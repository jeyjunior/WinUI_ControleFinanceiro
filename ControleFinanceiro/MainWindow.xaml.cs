using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Org.BouncyCastle.Asn1.Crmf;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ControleFinanceiro.Componentes;
using ControleFinanceiro.Mensagem;
using ControleFinanceiro.Telas;
using CF.Domain.Entidades;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace ControleFinanceiro
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        #region Propriedades
        private const int Largura = 800;
        private const int Altura = 600;
        private AppWindow m_AppWindow;
        private NavigationViewItem paginaAtiva;
        private bool popupAtivo = false;
        #endregion

        #region Construtor
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            Notificacao.RegisterContainer(gNotificacao);
            DefinirPadraoUI();
            SetWindowMinSize();
        }
        #endregion

        #region Eventos
        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            try
            {
                var item = args.InvokedItemContainer as NavigationViewItem;
                if (item == null)
                    return;

                if (item.Tag.ToString() == "Categoria")
                {
                    MainFrame.Navigate(typeof(CategoriaPage));
                }
                else if (item.Tag.ToString() == "EntidadeFinanceira")
                {
                    MainFrame.Navigate(typeof(EntidadePage));
                }
                else if (item.Tag.ToString() == "OperacaoFinanceira")
                {
                    MainFrame.Navigate(typeof(OperacaoPage));
                }
            }
            catch
            {

            }
        }
        #endregion

        #region Metodos
        private void DefinirPadraoUI()
        {
            m_AppWindow = ObterAppWindowAtual();
            m_AppWindow.Title = "Controle Financeiro";
            m_AppWindow.SetIcon("Assets/controleFinanceiro_icone_24.ico");

            DefinirTamanhoUI();
            CentralizarUI();

            var appWindow = ObterAppWindowAtual();
            if (appWindow != null && Microsoft.UI.Windowing.AppWindowTitleBar.IsCustomizationSupported())
            {
                var titleBar = appWindow.TitleBar;

                titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 18, 18, 18);
                titleBar.ForegroundColor = Colors.White;

                titleBar.InactiveBackgroundColor = titleBar.BackgroundColor;
                titleBar.InactiveForegroundColor = titleBar.ForegroundColor;

                titleBar.ButtonBackgroundColor = titleBar.BackgroundColor;
                titleBar.ButtonForegroundColor = titleBar.ForegroundColor;
                titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 18, 18, 18);
                titleBar.ButtonHoverForegroundColor = titleBar.ForegroundColor;
                titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 18, 18, 18);
            }
        }
        private void DefinirTamanhoUI()
        {
            m_AppWindow.Resize(new Windows.Graphics.SizeInt32(Largura, Altura));
        }
        private void CentralizarUI()
        {
            var displayArea = DisplayArea.GetFromWindowId(m_AppWindow.Id, DisplayAreaFallback.Primary);
            var centralizacao = new Windows.Graphics.PointInt32
            {
                X = displayArea.WorkArea.X + (displayArea.WorkArea.Width - Largura) / 2,
                Y = displayArea.WorkArea.Y + (displayArea.WorkArea.Height - Altura) / 2
            };
            m_AppWindow.Move(centralizacao);
        }
        private AppWindow ObterAppWindowAtual()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }
        private void SetWindowMinSize()
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);

            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
            if (appWindow == null)
                return;

            var presenter = appWindow.Presenter as OverlappedPresenter;
            if (presenter == null)
                return;

            presenter.PreferredMinimumHeight = Altura;
            presenter.PreferredMinimumWidth = Largura;
        }
        #endregion
    }
}
