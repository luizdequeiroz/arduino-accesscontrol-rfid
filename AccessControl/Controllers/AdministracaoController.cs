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
        public ActionResult Consultar(string busca = "", string pagina = "_Resultado")
        {
            var todos = new UsuarioDao().Listar();

            if(SessionInvalida(todos))
                return RedirectToAction("Inicio", "Inicio");
            if (Request.IsAjaxRequest())
                return PartialView(pagina, buscar(busca, todos));

            return View();
        }

        public ActionResult Atualizar()
        {
            return View();
        }

        public ActionResult Formulario(Usuario usuario)
        {
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
