using AccessControl.Models;
using AccessControl.Models.DAOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessControl.Controllers
{
    public class InicioController : Controller
    {
        UsuarioDao usuarioDao = new UsuarioDao();

        public ActionResult Inicio()
        {
            AdministracaoController.cache.Clear();
            if (Session["rfid"] != null)
            {
                var usuario = usuarioDao.Selecionar((long)Session["rfid"]);
                if (usuario == null)
                {
                    Session.Remove("rfid");
                    return RedirectToAction("Inicio", "Inicio");
                }

                return View("Perfil", usuario);
            }
            var objs = new ObjsTest();
            return View(objs);
        }

        [HttpPost]
        public ActionResult Inicio(/* long rfid *//* BEGIN TESTE */ObjsTest objs)
        {
            /* BEGIN TESTE */
            if (objs.Rfid == 0)
            {
                ModelState.AddModelError("Rfid", "Insira o código do RFID!");
                return View();
            }
            /* END TESTE */
            var usuario = usuarioDao.Selecionar(objs.Rfid);
            Session["rfid"] = objs.Rfid;
            if (usuario == null) return RedirectToAction("Cadastrar", "Cadastro");
            return View("Perfil", usuario);
        }

        public ActionResult Sobre()
        {
            return View();
        }

        public ActionResult Sair()
        {
            if (Session["rfid"] != null)
                Session.Remove("rfid");
            return RedirectToAction("Inicio");
        }
    }
}
