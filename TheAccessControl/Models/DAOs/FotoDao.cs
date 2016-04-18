using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheAccessControl.Models.DAOs
{
    public class FotoDao
    {
        public Foto Selecionar(string rfid)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var foto = (from f in acc.FotoSet where f.Rfid == rfid select f).SingleOrDefault();
                    return foto;
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar selecionar Foto pelo Rfid: " + e.Message);
                }
            }
        }

        public int Inserir(Foto foto)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    acc.FotoSet.Add(foto);
                    return acc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar inserir a Foto: " + e.Message);
                }
            }
        }

        public Foto Atualizar(Foto foto)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var Foto = acc.FotoSet.Where(f => f.Rfid == foto.Rfid).SingleOrDefault();
                    Foto.Imagem = foto.Imagem;

                    acc.SaveChanges();
                    return foto;
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar atualizar a Foto: " + e.Message);
                }
            }
        }

        public void Deletar(string rfid)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var foto = acc.FotoSet.Where(f => f.Rfid == rfid).SingleOrDefault();
                    acc.FotoSet.Remove(foto);
                    acc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception("Erro ao tentar deletar a Foto: " + e.Message);
                }
            }
        }
    }
}