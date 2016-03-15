namespace AccessControl.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    
    public partial class Foto
    {
        public int Id { get; set; }
        public long Rfid { get; set; }

        [Remote("Cadastrar", "Cadastro", ErrorMessage = "É importante que vocę insira uma foto!")]
        public byte[] Imagem { get; set; }
    }
}
