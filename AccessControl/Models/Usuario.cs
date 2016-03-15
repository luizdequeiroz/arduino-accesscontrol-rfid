namespace AccessControl.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    
    public partial class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe seu nome. Năo precisa ser completo!")]
        public string Nome { get; set; }

        [Remote("EmailUnico", "Cadastro", ErrorMessage = "Alguém já está usando este e-mail!")]
        [Required(ErrorMessage = "Informe seu e-mail, nos ajudará a entrar em contato com vocę!")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "O E-mail informado năo é válido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Por favor, descreva-se. Isso nos ajudará a conhecer melhor vocę!")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Este campo deve ter entre 10 e 200 caracteres!")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Informe seu telefone, servirá como alternativa de contato além do e-mail!")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Diga-nos sua data de nascimento, isso nos fará saber sua idade e seu aniversário!" + @"\o/")]
        public string Nascimento { get; set; }

        [Required(ErrorMessage = "É de suma importância que vocę indique o tipo do cadastro!")]
        public string Tipo { get; set; }

        public long Rfid { get; set; }
    }
}
