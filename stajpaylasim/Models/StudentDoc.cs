using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace stajpaylasim.Models
{
    public class StudentDoc
    {
        [Required(ErrorMessage = "Başlık Boş Geçilemez !")]
        public int DTId { get; set; }
        [Required(ErrorMessage = "Açıklama Boş Geçilemez !")]
        public string DocDesc { get; set; }
        public string DocName { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Staj Belgesi Alani Bos Geçilemez !")]
        public HttpPostedFileBase SDoc { get; set; }
        public string DocUrl { get; set; }
        public int TId { get; set; }
        public int DocSId { get; set; }
    }
}