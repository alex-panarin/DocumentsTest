using DocumentsTest.Common;
using DocumentsTest.Data;
using DocumentsTest.Models;
using DocumentsTest.Services;
using DocumentsTest.Wrapers;
using System;
using System.ComponentModel;
using System.Data.SQLite;
using System.Text;
using System.Windows.Input;

namespace DocumentsTest.ViewModels
{
    public class DocumentViewModel : ViewModelBase
    {
        private string _info;
        private DocumentWrapper _document;
        private readonly IDocumentDataSource _dataSource;
        private readonly IErrorInfoService _errorService;

        public DocumentViewModel(
            IDocumentDataSource dataSource,
            IErrorInfoService errorService)
        {
            _dataSource = dataSource;
            _errorService = errorService;

            if (_errorService == null)
                throw new ArgumentNullException(nameof(errorService), "Не найден обработчик ошибок.");

            if (dataSource == null)
                throw new ArgumentNullException(nameof(dataSource), "Не найден источник данных.");

            NextCommand = new WindowCommand(ProcessDocument, () =>  ! HasErrors );

            ValidateQtyCommand = new WindowCommand(ProcessValidate);
        }

        public DocumentWrapper Document 
        {
            get => _document; 
            set
            {
                _document = value;
                OnPropertyChanged();
            }
        }

        public bool HasErrors => Document != null && Document.HasErrors;

        public ICommand NextCommand { get; }

        public ICommand ValidateQtyCommand { get; }

        public string Info
        {
            get => _info;
            set 
            { 
                _info = value;
                OnPropertyChanged();
            }
        }

        public void ProcessDocument(object cmd = default)
        {
            try
            {
                if (Document != null)
                {
                    ProcessValidate(Document.Qty);

                    if(Document.HasErrors)
                    {
                        return; 
                    }

                    Document.ErrorsChanged -= ProcessErrorChanged;

                    _dataSource.UpdateDocument(Document.Model);
                }

                Document doc = _dataSource.GetDocument();

                Document = new DocumentWrapper(doc);

                if (Document.IsEmpty)
                {
                    Info = "Больше нет документов.";
                }
                else
                {
                    Document.ErrorsChanged += ProcessErrorChanged;

                    Info = "Введите значение количества.";
                }
            }
            catch(Exception x)
            {
                if (x is SQLiteException && Document == null)
                {
                    Info = "Возможно отсутствует файл базы данных";
                }
                else
                {
                    _errorService.ShowError(x.Message);
                }
            }
        }

        private void ProcessValidate(object value)
        {
            if (Document == null) return;

            Document.Validate(value);

            if(!Document.HasErrors)
            {
                Info = "Нажмите кнопку Далее.";
            }
        }

        private void ProcessErrorChanged(object sendert, DataErrorsChangedEventArgs args)
        {
            var errors =  Document.GetErrors(args.PropertyName);
            
            if (errors == null) return;

            StringBuilder sb = new StringBuilder();
            
            foreach (var error in errors)
            {
                sb.Append($"{error}\r\n");
            }

            _errorService.ShowError(sb.ToString());

        }

        
    }
}
