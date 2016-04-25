using TheAccessControl.Models;
using TheAccessControl.Models.DAOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheAccessControl.Controllers
{
    public class InicioController : Controller
    {
        UsuarioDao usuarioDao = new UsuarioDao();

        public ActionResult Inicio()
        {
            if (Session["rfid"] != null)
            {
                var usuario = usuarioDao.Selecionar((string)Session["rfid"]);
                if (usuario == null)
                {
                    Session.Remove("rfid");
                    return RedirectToAction("Inicio", "Inicio");
                }
                return View("Perfil", usuario);
            }
            var objs = new ObjsTag();
            return View(objs);
        }

        [HttpPost]
        public ActionResult Inicio(ObjsTag objs)
        {
            if (objs.Rfid == "")
            {
                ModelState.AddModelError("Rfid", "Insira o código do RFID!");
                return View();
            }

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
