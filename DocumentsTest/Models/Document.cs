using System.ComponentModel.DataAnnotations;

namespace DocumentsTest.Models
{
    public class Document
    {
        
        public string Name { get; set; }
        public string ItemName{ get; set; }

        public int GivenQty { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage ="Количество должно быть больше 0.")]
        public int? Qty { get; set; }

        public int DocumentId { get; set; }

        public int LineId { get; set; }
    }
}
