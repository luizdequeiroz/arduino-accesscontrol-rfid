﻿using AccessControl.Models;
using AccessControl.Models.DAOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessControl.Controllers
{
    public class CadastroController : Controller
    {
        UsuarioDao usuarioDao = new UsuarioDao();

        public ActionResult Cadastrar()
        {            
            if (Session["autorize"] == null)
                return RedirectToAction("Autorize", "Administracao");
            var usuario = new Usuario();
            Session.Remove("autorize");
            return View(usuario);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        [HttpPost]
        public ActionResult Cadastrar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] byts;
                    var foto = Request.Files[0];
                    if (foto.FileName == "")
                    {
                        ModelState.AddModelError("", "É importante que você insira uma foto!");
                        return View();
                    }
                    using (var reader = new BinaryReader(foto.InputStream))
                        byts = reader.ReadBytes(foto.ContentLength);

                    var file = new FileInfo(foto.FileName);
                    if (file.Extension == ".jpg" || file.Extension == ".png" || file.Extension == ".gif")
                    {
                        if (usuarioDao.Selecionar(usuario.Rfid) != null)
                        {
                            ModelState.AddModelError("", "Cartão já cadastrado! Volte para o Início e apresente um novo cartão, a sessão atual é de um cartão cadastrado!");
                            return View();
                        }

                        AdministracaoController.cache["usuario"] = usuario;
                        AdministracaoController.cache["byts"] = byts;
                    }
                    else
                    {
                        ModelState.AddModelError("", "Insira um arquivo de formato válido para imagem (.jpg, .png, .gif)");
                        return View();
                    }
                    return RedirectToAction("Autorize", "Administracao");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Erro ao tentar Cadastrar Usuário: " + e);
                }
            }
            return View(usuario);
        }

        public ActionResult EmailUnico(string email)
        {
            var emails = new List<string>();
            var todos = usuarioDao.Listar();
            foreach (var u in todos)
                emails.Add(u.Email);

            return Json(emails.All(e => e.ToLower() != email.ToLower()), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sucesso(Usuario usuario)
        {
            return View(usuario);
        }
    }
}
