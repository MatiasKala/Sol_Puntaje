using System;
using System.Collections.Generic;
using System.Text;

namespace PuntajeClases.Unused
{
    class DiccionarioMateriaPorDia
    {
        public string CategoriaPorDia()
        {
            string retorno=null;

            DayOfWeek dia = DateTime.Today.DayOfWeek;
            if (dia.Equals(DayOfWeek.Monday))
            {
                retorno = CasoLunes();
            }
            Dictionary<DayOfWeek, string> dic = new Dictionary<DayOfWeek, string> ();
          
            return retorno;
        }

        private string CasoLunes()
        {
            string retorno = null;

            if (DateTime.Now.Hour >= 23)
            {
                retorno = "BD2";
            }
            else
            {
                retorno = "AMS";
            }

            return retorno;
        }
    }
}
