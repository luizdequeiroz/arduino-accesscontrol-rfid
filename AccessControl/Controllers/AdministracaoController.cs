using AccessControl.Models;
using AccessControl.Models.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessControl.Controllers
{
    public class AdministracaoController : Controller
    {
        public ActionResult Consultar(string busca = "")
        {
            var usuarios = new UsuarioDao().Listar();
            if (!string.IsNullOrEmpty(busca))
                usuarios = usuarios.Where(u => u.Nome.Contains(busca)).ToList();
            usuarios = usuarios.OrderBy(u => u.Nome).ToList();

            return View(usuarios);
        }

        public ActionResult Alterar()
        {
            return View();
        }

        public ActionResult Deletar()
        {
            return View();
        }
    }
}
