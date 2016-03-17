using AccessControl.Models;
using AccessControl.Models.DAOs;
using System;
using System.Collections;
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

            var massaDeDados = new Hashtable();
            foreach (var use in usuarios)
            {
                string str = use.Nome + " " + use.Email + " " + use.Descricao + " " + use.Telefone + " " + use.Nascimento + " " + use.Tipo.Equals("Adm")?"Administrador":"Normal";
                massaDeDados[use.Rfid] = str;
            }
            if (!string.IsNullOrEmpty(busca))
                foreach (var use in usuarios)
                {
                    if (massaDeDados[use.Rfid].ToString().ToLower().Contains(busca.ToLower()))
                    {
                        usuarios = usuarios.Where(u => u.Rfid.Equals(use.Rfid)).ToList();
                    }
                }
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
