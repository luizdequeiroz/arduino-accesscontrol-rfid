using AccessControl.Models;
using AccessControl.Models.DAOs;
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

namespace AccessControl.Controllers
{
    public class AdministracaoController : Controller
    {
        public ActionResult Consultar(string busca = "")
        {
            return retornarView(busca, "_Resultado");
        }

        [NonAction]
        public ActionResult retornarView(string busca, string pagina)
        {
            var todos = new UsuarioDao().Listar();

            if (SessionInvalida(todos))
                return RedirectToAction("Inicio", "Inicio");
            if (Request.IsAjaxRequest())
                return PartialView(pagina, buscar(busca, todos));

            return View(new List<Usuario>());
        }

        public ActionResult Atualizar(string busca = "")
        {
            return retornarView(busca, "_Perfis");
        }

        public ActionResult Formulario(long Rfid)
        {
            var usuario = new UsuarioDao().Selecionar(Rfid);
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
                        new FotoDao().Atualizar(new Foto { Imagem = byts, Rfid = usuario.Rfid });
                        new UsuarioDao().Atualizar(usuario);
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

        public ActionResult ConsoleSql(string consulta = "")
        {
            if (consulta != "")
            {
                var connectionString = ConfigurationManager.ConnectionStrings["connForConsole"].ConnectionString;
                SqlConnection con = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(consulta, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                try
                {
                    var reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        var resultados = new List<Object>();
                        while (reader.Read())
                        {
                            if (reader.FieldCount == 8)
                                resultados.Add(new Usuario
                                                   {
                                                       Id = (int)reader.GetValue(0),
                                                       Nome = (string)reader.GetValue(1),
                                                       Email = (string)reader.GetValue(2),
                                                       Descricao = (string)reader.GetValue(3),
                                                       Telefone = (string)reader.GetValue(4),
                                                       Nascimento = (string)reader.GetValue(5),
                                                       Tipo = (string)reader.GetValue(6),
                                                       Rfid = (long)reader.GetValue(7)
                                                   });
                            if (reader.FieldCount == 3)
                                resultados.Add(new Foto
                                                   {
                                                       Id = (int)reader.GetValue(0),
                                                       Rfid = (long)reader.GetValue(1),
                                                       Imagem = null
                                                   });
                        }
                        return PartialView("_ConsoleResult", resultados);
                    }
                    else
                    {
                        return JavaScript("alert('Não há resultados para esta consulta!')");
                    }
                }
                catch (SqlException sqlex)
                {
                    return JavaScript("alert('Erro Sql: " + sqlex.Message + "')");
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                if (Request.IsAjaxRequest())
                    return PartialView("_ConsoleResult");
                return View();
            }
        }
    }
}