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

            if (Session["rfid"] == null)
                return RedirectToAction("Inicio", "Inicio");

            var us = usuarios.Where(x => x.Rfid.Equals(Session["rfid"])).FirstOrDefault();
            if (us.Tipo != "Adm")
                return RedirectToAction("Inicio", "Inicio");

            if (!string.IsNullOrEmpty(busca))
                usuarios = usuarios.Where(u => u.Nome.ToLower().Contains(busca.ToLower())).ToList();
            usuarios = usuarios.OrderBy(u => u.Nome).ToList();

            if (Request.IsAjaxRequest())
            {
                if (busca == "")
                    usuarios = new List<Usuario>();
                return PartialView("_Resultado", usuarios);
            }

            usuarios = new List<Usuario>();
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
