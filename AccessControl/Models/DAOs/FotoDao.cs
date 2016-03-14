using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControl.Models.DAOs
{
    public class FotoDao
    {
        public Foto selecionar(long rfid)
        {
            using (var acc = new AccessControlContainer())
            {
                try
                {
                    var foto = (from f in acc.FotoSet where f.Rfid == rfid select f).FirstOrDefault();
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
                    throw new Exception("Erro ao tentar inserir a Foto: " + e);
                }
            }
        }
    }
}