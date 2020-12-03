using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DocumentsTest.Services
{
    public class ErrorInfoService : IErrorInfoService
    {
        public void ShowError(string error, string reason = "Ошибка")
        {
            MessageBox.Show(error, reason, MessageBoxButton.OK);
        }
    }
}
