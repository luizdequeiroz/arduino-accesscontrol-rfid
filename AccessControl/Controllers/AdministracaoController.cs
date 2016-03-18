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
        public List<Usuario> todos = new UsuarioDao().Listar();

        public ActionResult _Busca(string busca = "")
        {
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
                return PartialView(Session["_page"].ToString(), usuarios.Distinct().OrderBy(u => u.Nome).ToList());
            }

            return View(usuarios);
        }

        public ActionResult Consultar()
        {
            Session["_page"] = "_Resultado";
            return View();
        }

        public ActionResult Atualizar()
        {
            Session["_page"] = "_Formulario";
            return View();
        }

        public ActionResult Deletar()
        {
            return View();
        }
    }
}
