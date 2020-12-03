using DocumentsTest.Models;

namespace DocumentsTest.Data
{
    public interface IDocumentDataSource
    {
        Document GetDocument();
        void UpdateDocument(Document doc);
    }
}