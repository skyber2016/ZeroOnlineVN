using AutoZero.Cores;
using AutoZero.MVVM.ViewModel;
using AutoZero.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoZero
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel { get; set; }
        private MiddlewareService Middleware { get; set; }
        private IUnitOfWork UnitOfWork { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.ViewModel = (MainViewModel)this.DataContext;
            this.ViewModel.Dispatcher = this.Dispatcher;
            

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                this.UnitOfWork = new UnitOfWork(ViewModel);
                this.Middleware = new MiddlewareService(UnitOfWork);
            }).Start();
        }
    }
}
