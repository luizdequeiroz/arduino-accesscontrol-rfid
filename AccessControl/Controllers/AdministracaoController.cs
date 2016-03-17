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
            var todos = new UsuarioDao().Listar();
            var usuarios = new List<Usuario>();

            if (Session["rfid"] == null)
                return RedirectToAction("Inicio", "Inicio");

            var uSessao = todos.Where(u => u.Rfid.Equals(Session["rfid"])).FirstOrDefault();
            if (uSessao.Tipo != "Adm")
                return RedirectToAction("Inicio", "Inicio");

            if (Request.IsAjaxRequest())
            {
                if (busca.ToLower() == "normal")
                    usuarios.AddRange(todos.Where(u => u.Tipo.ToLower().Equals("nor")).ToList());
                if (busca.ToLower().Contains("adm"))
                    usuarios.AddRange(todos.Where(u => u.Tipo.ToLower().Equals("adm")).ToList());

                if (!string.IsNullOrEmpty(busca))
                {
                    var massaDeDados = new Hashtable();
                    foreach (var u in todos)
                    {
                        string str = u.Nome + u.Email + u.Descricao + u.Telefone + u.Nascimento;
                        massaDeDados[u.Rfid] = str;
                    }
                    foreach (var us in todos)
                    {
                        if (massaDeDados[us.Rfid].ToString().ToLower().Contains(busca.ToLower()))
                        {
                            usuarios.Add(todos.Where(u => u.Rfid.Equals(us.Rfid)).FirstOrDefault());
                        }
                    }
                }
                return PartialView("_Resultado", usuarios.Distinct().OrderBy(u => u.Nome).ToList());
            }

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
