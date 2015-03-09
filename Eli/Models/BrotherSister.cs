using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eli.Models
{
    public class BrotherSister
    {
        public tblPatient patient { get; set; }
        public List<tblBrotherSister> BrotherSisters { get; set; }
        public List<tblParent> Parents { get; set; }


    }
}
