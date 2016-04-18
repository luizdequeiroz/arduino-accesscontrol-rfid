using TheAccessControl.Models;
using TheAccessControl.Models.DAOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheAccessControl.Controllers
{
    public class AdministracaoController : Controller
    {
        UsuarioDao usuarioDao = new UsuarioDao();
        FotoDao fotoDao = new FotoDao();
        public static Hashtable cache = new Hashtable();

        public ActionResult Consultar(string busca = "")
        {
            return retornarView(busca, "_Resultado");
        }

        public ActionResult Atualizar(string busca = "")
        {
            return retornarView(busca, "_Perfis");
        }

        public ActionResult Formulario(string email)
        {
            var usuario = usuarioDao.SelecionarPorEmail(email);
            var tipo = usuarioDao.Selecionar((string)Session["rfid"]).Tipo;
            if ((usuario.Tipo.Equals("Adm") && !usuario.Rfid.Equals((string)Session["rfid"])) || tipo.Equals("Nor"))
                return RedirectToAction("Inicio", "Inicio");
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Atualizar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] byts;
                    bool fileValid = false;
                    var foto = Request.Files[0];
                    if (foto.FileName == "")
                    {
                        byts = new FotoDao().Selecionar(usuario.Rfid).Imagem;
                        fileValid = true;
                    }
                    else
                    {
                        using (var reader = new BinaryReader(foto.InputStream))
                            byts = reader.ReadBytes(foto.ContentLength);

                        var file = new FileInfo(foto.FileName);
                        if (file.Extension == ".jpg" || file.Extension == ".png" || file.Extension == ".gif")
                            fileValid = true;
                    }
                    if (fileValid)
                    {
                        fotoDao.Atualizar(new Foto { Imagem = byts, Rfid = usuario.Rfid });
                        usuarioDao.Atualizar(usuario);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Insira um arquivo de formato válido para imagem (.jpg, .png, .gif)");
                        return View();
                    }
                    return View("Atualizar", new List<Usuario> { usuario });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Erro ao tentar Cadastrar Usuário: " + e);
                }
            }
            return View(usuario);
        }

        public ActionResult Deletar(string busca = "", string email = "")
        {
            if (string.IsNullOrEmpty(email))
                return retornarView(busca, "_Deletaveis");
            try
            {
                var rfid = new UsuarioDao().SelecionarPorEmail(email).Rfid;
                usuarioDao.Deletar(rfid);
                fotoDao.Deletar(rfid);
                return View(new List<Usuario> { usuarioDao.Selecionar((string)Session["Rfid"]) });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Erro ao tentar deletar Usuário: " + e);
            }
            return View();
        }

        public ActionResult Autorize()
        {
            var objs = new ObjsTest();
            return View(objs);
        }

        [HttpPost]
        public ActionResult Autorize(/*string rfid*/ObjsTest objs)
        {
            /* BEGIN TESTE */
            if (objs.Rfid == "")
            {
                ModelState.AddModelError("", "Insira o código do RFID!");
                return View();
            }
            /* END TESTE */

            var usuario = usuarioDao.Selecionar(objs.Rfid);
            if (usuario == null || usuario.Tipo.Equals("Nor"))
            {
                ModelState.AddModelError("", "Rfid inválido, não possui autoridade ou não existe!");
                return View();
            }
            if (AdministracaoController.cache.Count == 2)
            {
                try
                {
                    usuario = (Usuario)AdministracaoController.cache["usuario"];
                    var byts = (byte[])AdministracaoController.cache["byts"];
                    AdministracaoController.cache.Remove("usuario");
                    AdministracaoController.cache.Remove("byts");
                    fotoDao.Inserir(new Foto { Imagem = byts, Rfid = usuario.Rfid });
                    usuarioDao.Inserir(usuario);
                    AdministracaoController.cache["nomeCadastrado"] = usuario.Nome;
                    return RedirectToAction("Sucesso", "Cadastro");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Erro ao tentar Cadastrar Usuário: " + e);
                }
            }
            Session["autorize"] = objs.Rfid;
            return RedirectToAction("Cadastrar", "Cadastro");
        }

        [NonAction]
        public ActionResult retornarView(string busca, string pagina)
        {
            var todos = usuarioDao.Listar();

            if (SessionInvalida(todos))
                return RedirectToAction("Inicio", "Inicio");
            if (Request.IsAjaxRequest())
                return PartialView(pagina, buscar(busca, todos));

            return View(new List<Usuario>());
        }

        [NonAction]
        public List<Usuario> buscar(string busca, List<Usuario> todos)
        {
            var usuarios = new List<Usuario>();

            if (busca.ToLower() == "normal")
                usuarios.AddRange(todos.Where(u => u.Tipo.ToLower().Equals("nor")).ToList());
            if (busca.ToLower().Contains("adm"))
                usuarios.AddRange(todos.Where(u => u.Tipo.ToLower().Equals("adm")).ToList());
            if (busca.ToLower() == "todos" || busca.ToLower() == "all")
                return todos.OrderBy(u => u.Nome).ToList();

            if (!string.IsNullOrEmpty(busca))
            {
                var massaDeDados = new Hashtable();
                foreach (var u in todos)
                {
                    string str = u.Nome + u.Email + u.Descricao + u.Telefone + u.Nascimento;
                    massaDeDados[u.Rfid] = str;
                }
                foreach (var us in todos)
                    if (massaDeDados[us.Rfid].ToString().ToLower().Contains(busca.ToLower()))
                        usuarios.Add(todos.Where(u => u.Rfid.Equals(us.Rfid)).FirstOrDefault());
            }
            return usuarios.Distinct().OrderBy(u => u.Nome).ToList();
        }

        [NonAction]
        public bool SessionInvalida(List<Usuario> todos)
        {
            if (Session["rfid"] == null)
                return true;
            var uSessao = todos.Where(u => u.Rfid.Equals(Session["rfid"])).FirstOrDefault();
            if (uSessao.Tipo != "Adm")
                return true;
            return false;
        }
    }
}