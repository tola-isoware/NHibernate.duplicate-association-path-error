using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iesi.Collections;

namespace TestProject.Model
{
    public class PropertyValue
    {
        
        public virtual int Id { get; set; }
        public virtual string Value { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public virtual Machine Machine { get; set; }
    }
}
