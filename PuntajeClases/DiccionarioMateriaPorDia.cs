using System;
using System.Collections.Generic;
using System.Text;

namespace PuntajeClases
{
    class DiccionarioMateriaPorDia
    {
        public static string CategoriaPorDia()
        {

            return InicializarDiccionario()[DateTime.Today.DayOfWeek];  
            
        }
        private static Dictionary<DayOfWeek, string> InicializarDiccionario()
        {
            Dictionary<DayOfWeek, string> dic = new Dictionary<DayOfWeek, string>();

            dic.Add(DayOfWeek.Monday, CasoLunes());
            dic.Add(DayOfWeek.Tuesday, "PR2");
            dic.Add(DayOfWeek.Wednesday, "TP2");
            dic.Add(DayOfWeek.Thursday, CasoJueves());

            return dic;
        }
        private static string CasoLunes()
        {
            string retorno = null;

            if (DateTime.Now.Hour >= 23)
            {
                retorno = "BD2";
            }
            else if (DateTime.Now.Hour <19)
            {
                retorno = "PIC";
            }
            else
            {
                retorno = "AMS";
            }

            return retorno;
        }
        private static string CasoJueves()
        {
            string retorno = null;

            if (DateTime.Now.Hour >= 20)
            {
                retorno = "NT2";
            }
            else
            {
                retorno = "CAA";
            }

            return retorno;
        }
    }
}
