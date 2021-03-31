using System;
using System.Collections.Generic;
using System.Text;

namespace PuntajeClases
{
    class DiccionarioMateriaPorDia
    {
        public static string CategoriaPorDia()
        {
            string retorno = null;

            DayOfWeek dia = DateTime.Today.DayOfWeek;
            if (dia.Equals(DayOfWeek.Monday))
            {
                retorno = CasoLunes();
            }
            else
            {
                Dictionary<DayOfWeek, string> dic = InicializarDiccionario();

                retorno= dic[dia];

            }

            return retorno;
        
        }
        private static Dictionary<DayOfWeek, string> InicializarDiccionario()
        {
            Dictionary<DayOfWeek, string> dic = new Dictionary<DayOfWeek, string>();

            dic.Add(DayOfWeek.Tuesday, "PR2");
            dic.Add(DayOfWeek.Wednesday, "TP2");
            dic.Add(DayOfWeek.Thursday, "NT2");

            return dic;
        }
        private static string CasoLunes()
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
