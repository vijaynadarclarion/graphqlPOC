using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Data;


namespace Adf.Core.CommonServices.Entities
{

   
    public class CustomAppSettings //: FullAuditedEntityBase<int>
    {
         public int  Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }
}
