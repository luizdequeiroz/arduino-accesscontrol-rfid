using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheAccessControl.Models
{
    public class ObjsTag
    {
        [Required(ErrorMessage = "Insira o código do RFID!")]
        public string Rfid { get; set; }
    }
}