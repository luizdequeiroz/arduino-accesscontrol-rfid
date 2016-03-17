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
                string str = use.Nome + use.Email + use.Descricao + use.Telefone + use.Nascimento;
                massaDeDados[use.Rfid] = str;
            }
            if (!string.IsNullOrEmpty(busca))
                foreach (var use in usuarios)
                {
                    if (massaDeDados[use.Rfid].ToString().ToLower().Contains(busca.ToLower()))
                    {
                        usuarios.Add(usuarios.Where(u => u.Rfid.Equals(use.Rfid)).FirstOrDefault());
                    }
                }
            usuarios = usuarios.OrderBy(u => u.Nome).ToList();

            if (Request.IsAjaxRequest())
            {
                if (busca.ToLower() == "normal")
                    usuarios.AddRange(usuarios.Where(u => u.Tipo.ToLower().Equals("nor")));
                if(busca.ToLower().Contains("adm"))
                    usuarios.AddRange(usuarios.Where(u => u.Tipo.ToLower().Equals("adm")));
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
