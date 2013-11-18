using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Model
{
    public class Machine
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<PropertyValue> PropertyValues { get; set; }
    }
}
