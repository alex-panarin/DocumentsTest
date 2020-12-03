using DocumentsTest.Models;
using System;
using System.Data;

namespace DocumentsTest.Data
{
    public class DocumentDataSource : IDocumentDataSource
    {
        readonly IDbConnection _connection;
        public DocumentDataSource(IDbConnection connection)
        {
            _connection = connection;
        }

        public Document GetDocument()
        {
            using (var command = _connection.CreateCommand())
            {
                try
                {
                    command.CommandText = 
                        $"SELECT Name,DocumentId,LineId,ItemName,GivenQty,Qty " +
                        $"FROM [DocumentLine] " +
                        $"INNER JOIN [Document] USING (DocumentId) " +
                        $"WHERE Qty IS NULL " +
                        $"LIMIT 1";

                    command.Prepare();

                    _connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            object[] vals = new object[reader.FieldCount];
                            reader.GetValues(vals);

                            return new Document
                            {
                                Name = Convert.ToString(vals[0]),
                                DocumentId = Convert.ToInt32(vals[1]),
                                LineId = Convert.ToInt32(vals[2]),
                                ItemName = Convert.ToString(vals[3]),
                                GivenQty = Convert.ToInt32(vals[4]),
                                Qty =  
                                    string.IsNullOrEmpty(vals[5].ToString()) 
                                    ? new int?() 
                                    : Convert.ToInt32(vals[5]),
                            };
                        }
                    }
                }
                finally
                {
                    _connection.Close();
                }

                return new Document();

            }
        }

        public void UpdateDocument(Document document)
        {
            try
            {
                using (var command = _connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE [DocumentLine] SET Qty = {document.Qty} WHERE LineId = {document.LineId}";
                    
                    command.Prepare();

                    _connection.Open(); 

                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
