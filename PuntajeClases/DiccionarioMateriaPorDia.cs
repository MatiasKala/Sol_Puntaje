using System;
using System.Collections.Generic;
using System.Text;

namespace PuntajeClases
{
    class DiccionarioMateriaPorDia
    {
        public static string CategoriaPorDia()
        {
            string categ = InicializarDiccionario()[DateTime.Today.DayOfWeek];

            return categ.Equals("")? null: categ;  
            
        }
        private static Dictionary<DayOfWeek, string> InicializarDiccionario()
        {
            Dictionary<DayOfWeek, string> dic = new Dictionary<DayOfWeek, string>();

            dic.Add(DayOfWeek.Monday, CasoLunes());
            dic.Add(DayOfWeek.Tuesday, "PR2");
            dic.Add(DayOfWeek.Wednesday, "TP2");
            dic.Add(DayOfWeek.Thursday, CasoJueves());

            return dic;
        
            /*
             SE PODRIAN MEJORAR LAS SUGERENCIAS HACIENDO QUE EL DIA QUE TOME NO ESTE HARCODEADO ACA
            SINO QUE VENGA DEL DIA QUE SE INGRESO ANTERIORMENTE EN LA CLASE. PARA ESO TENDRIA QUE PODER
            TRANSFORMAR EL STRING dd/MM/yy EN UN DATETIME Y PODER UTILIZARLO PARA VER QUE WEEKDAY ES.
             */

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
