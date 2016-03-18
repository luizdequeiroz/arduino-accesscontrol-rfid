using AccessControl.Models;
using AccessControl.Models.DAOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            if (busca == "" && Session["_busca"] != null)
                busca = Session["_busca"].ToString();
            else
                Session["_busca"] = busca;

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
            if (Session["_local"] != null && Session["_local"] == "_edicao")
                return View();
            Session["_local"] = "_perfil";
            return View();
        }

        public ActionResult AtualizarGo(Usuario usuario)
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
                        if (new UsuarioDao().Selecionar(usuario.Rfid) != null)
                        {
                            ModelState.AddModelError("", "Cartão já cadastrado! Volte para o Início e apresente um novo cartão, a sessão atual é de um cartão cadastrado!");
                            return View();
                        }
                        new FotoDao().Atualizar(new Foto { Imagem = byts, Rfid = usuario.Rfid });
                        new UsuarioDao().Atualizar(usuario);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Insira um arquivo de formato válido para imagem (.jpg, .png, .gif)");
                        return View();
                    }
                    return View("Sucesso", usuario);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Erro ao tentar Cadastrar Usuário: " + e);
                }
            }
            return View(usuario);
        }

        public ActionResult Deletar()
        {
            return View();
        }
    }
}
