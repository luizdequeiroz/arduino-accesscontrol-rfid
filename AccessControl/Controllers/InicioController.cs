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
        public ActionResult Inicio()
        {
            var objs = new ObjsTest();
            return View(objs);
        }

        [HttpPost]
        public ActionResult Inicio(/* long rfid *//* BEGIN TESTE */ObjsTest objs)
        {
            /* BEGIN TESTE */
            if (objs.Rfid == 0 || objs.Rfid == null)
            {
                ModelState.AddModelError("Rfid", "Insira o código do RFID!");
                return View();
            }
            /* END TESTE */
            var usuario = new UsuarioDao().Selecionar(objs.Rfid);
            Session["rfid"] = objs.Rfid;
            if (usuario == null) return RedirectToAction("Cadastrar", "Cadastro");
            return View("Perfil", usuario);
        }
    }
}
