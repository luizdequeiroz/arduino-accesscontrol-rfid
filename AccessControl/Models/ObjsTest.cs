﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccessControl.Models
{
    public class ObjsTest
    {
        [Required(ErrorMessage = "Insira o código do RFID!")]
        public long Rfid { get; set; }
    }
}