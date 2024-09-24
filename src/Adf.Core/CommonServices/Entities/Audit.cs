using System;

namespace Adf.Core.CommonServices.Entities
{
    public class Audit
    {
        public Guid Id { get; set; }
        public char CrudOperation { get; set; }
        public string TableName { get; set; }
        public string KeyValues { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    

}
