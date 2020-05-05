using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace stajpaylasim.Models
{
    public class DocTeacher
    {
        public int DocId { get; set; }
        public string SNameSurname { get; set; }
        public string Doctitle { get; set; }
        public string DocDesc { get; set; }
        public int DSId { get; set; }
        public string DocState { get; set; }
        public string DocName { get; set; }
    }
}