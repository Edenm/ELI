using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Eli.Models;
using System.ComponentModel.DataAnnotations;

namespace Eli.ViewModel
{
    public partial class DateModel
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }

}