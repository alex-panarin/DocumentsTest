using DocumentsTest.Models;
using System;
using System.Collections.Generic;

namespace DocumentsTest.Wrapers
{
    public class DocumentWrapper : ValidateModelWrapper<Document>
    {
        public DocumentWrapper(Document doc) :
            base(doc)
        {

        }

        public string Name => GetValue<string>();
        public string  ItemName => GetValue<string>();
        public int GivenQty => GetValue<int>();

        public string Qty
        {
            get
            { 
                int? qty = GetValue<int?>();

                return qty.HasValue ? qty.ToString() : string.Empty;
            }
            set
            {
                SetValue(value); 
            }
        }

        protected override IEnumerable<string> ValidateProperty(string propertyName, object currentValue)
        {
            var valueString = $"{currentValue ?? string.Empty}";

            switch (propertyName)
            {
                case nameof(Qty):

                    if(String.IsNullOrEmpty(valueString))
                    {
                        yield return "Количество не может быть 0.";
                    }
                    
                    var qty = Convert.ToInt32(valueString);

                    if (qty > GivenQty)
                    {
                        yield return $"Количество не может быть больше заданного ({GivenQty})";
                    }
                
                    break;
            }
        }

        internal void Validate(object value)
        {
            ValidatePropertyInternal("Qty", value);
        }

        public bool IsEmpty => string.IsNullOrEmpty(Name); 
        
        
    }
}
