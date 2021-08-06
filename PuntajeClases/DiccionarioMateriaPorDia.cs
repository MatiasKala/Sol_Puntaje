using System;
using System.Collections.Generic;
using System.Text;

namespace PuntajeClases
{
    class DiccionarioMateriaPorDia
    {
        public static string CategoriaPorDia(string dia)
        {
            return InicializarDiccionario()[Convert.ToDateTime(dia).DayOfWeek];
        }
        private static Dictionary<DayOfWeek, string> InicializarDiccionario()
        {
            Dictionary<DayOfWeek, string> dic = new Dictionary<DayOfWeek, string>();

            dic.Add(DayOfWeek.Monday, CasoLunes());
            dic.Add(DayOfWeek.Tuesday, CasoMartes());
            dic.Add(DayOfWeek.Wednesday, CasoMiercoles());
            dic.Add(DayOfWeek.Thursday, CasoJueves());
            dic.Add(DayOfWeek.Friday, null);
            dic.Add(DayOfWeek.Saturday,null);
            dic.Add(DayOfWeek.Sunday,null);
            return dic;

        }
        private static string CasoLunes()
        {
            string retorno = null;

            if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour != 0)
            {
                retorno = "PR3";
            }
            else
            {
                retorno = "PIC";
            }

            return retorno;
        }
        private static string CasoMartes()
        {
            string retorno = null;

            if (DateTime.Now.Hour >= 22 && DateTime.Now.Hour != 0)
            {
                retorno = "CSO";
            }
            else if (DateTime.Now.Hour >= 21 )
            {
                retorno = "SIS";
            }
            else 
            {
                retorno = "EJU";
            }

            return retorno;
        }
        private static string CasoMiercoles()
        {
            return "PFI";
        }
        private static string CasoJueves()
        {
            string retorno = null;

            if (DateTime.Now.Hour >= 20)
            {
                retorno = "TP3";
            }
            else
            {
                retorno = "CAA";
            }

            return retorno;
        }
    }
}
