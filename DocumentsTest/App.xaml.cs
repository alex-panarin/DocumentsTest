using DocumentsTest.Data;
using DocumentsTest.Services;
using DocumentsTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentsTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IErrorInfoService errorInfoService = new ErrorInfoService();
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.MainWindow = new MainWindow();

            var dataContext =  new MainViewModel(errorInfoService).DocumentModel;

            this.MainWindow.DataContext = dataContext;
            this.MainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, 
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var error = "Unexpected error occured. Please inform the admin." + Environment.NewLine + e.Exception.Message;

            errorInfoService.ShowError(error, "Unexpected error");

            e.Handled = true;
        }
    }
}
