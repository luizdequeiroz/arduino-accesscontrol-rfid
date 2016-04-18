using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheAccessControl.Models.DAOs
{
    public class UsuarioDao
    {
        public Usuario Selecionar(string rfid)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var usuario = (from u in acc.UsuarioSet where u.Rfid == rfid select u).SingleOrDefault();
                    return usuario;
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar selecionar Usuário pelo Rfid: " + e.Message);
                }
            }
        }

        public Usuario SelecionarPorEmail(string email)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var usuario = (from u in acc.UsuarioSet where u.Email == email select u).SingleOrDefault();
                    return usuario;
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar selecionar Usuário pelo E-mail: " + e.Message);
                }
            }
        }

        public int Inserir(Usuario usuario)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    acc.UsuarioSet.Add(usuario);
                    return acc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar inserir o Usuário: " + e.Message);
                }
            }
        }

        public List<Usuario> Listar()
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    return acc.UsuarioSet.ToList();
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar listar os Usuários: " + e.Message);
                }
            }
        }

        public Usuario Atualizar(Usuario usuario)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var Usuario = acc.UsuarioSet.Where(u => u.Rfid == usuario.Rfid).SingleOrDefault();
                    Usuario.Nome = usuario.Nome;
                    Usuario.Descricao = usuario.Descricao;
                    Usuario.Telefone = usuario.Telefone;
                    Usuario.Nascimento = usuario.Nascimento;

                    acc.SaveChanges();
                    return usuario;
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar atualizar o Usuário: " + e.Message);
                }
            }
        }

        public void Deletar(string rfid)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var usuario = acc.UsuarioSet.Where(u => u.Rfid == rfid).SingleOrDefault();
                    acc.UsuarioSet.Remove(usuario);
                    acc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar deletar o Usuário: " + e.Message);
                }
            }
        }
    }
}