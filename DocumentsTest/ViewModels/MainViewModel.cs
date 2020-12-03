using DocumentsTest.Data;
using DocumentsTest.Services;
using System;
using System.Configuration;
using System.Data.SQLite;

namespace DocumentsTest.ViewModels
{
    public class MainViewModel
    {
        private readonly IErrorInfoService _errorInfoService;

        public MainViewModel(IErrorInfoService errorInfoService)
        {
            _errorInfoService = errorInfoService;
        }
        public DocumentViewModel DocumentModel
        {
            get
            {
                try
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                    var sqliteConnection = new SQLiteConnection(connectionString);
                    var documentDataSource = new DocumentDataSource(sqliteConnection);
                    var model = new DocumentViewModel(documentDataSource, _errorInfoService);

                    model.ProcessDocument();

                    return model;
                }
                catch
                {
                    throw; 
                }
            }
        }
    }
}
