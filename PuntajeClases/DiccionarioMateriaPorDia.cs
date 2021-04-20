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
            dic.Add(DayOfWeek.Tuesday, "PR2");
            dic.Add(DayOfWeek.Wednesday, "TP2");
            dic.Add(DayOfWeek.Thursday, CasoJueves());
            dic.Add(DayOfWeek.Friday, null);
            dic.Add(DayOfWeek.Saturday,null);
            dic.Add(DayOfWeek.Sunday,null);
            return dic;

            /*
            Idea 1 Se̵ ̵p̵o̵d̵r̵i̵a̵n̵ ̵m̵e̵j̵o̵r̵a̵r̵ ̵l̵a̵s̵ ̵s̵u̵g̵e̵r̵e̵n̵c̵i̵a̵s̵ ̵h̵a̵c̵i̵e̵n̵d̵o̵ ̵q̵u̵e̵ ̵e̵l̵ ̵d̵i̵a̵ ̵q̵u̵e̵ ̵t̵o̵m̵e̵ ̵n̵o̵ ̵e̵s̵t̵e̵ ̵h̵a̵r̵c̵o̵d̵e̵a̵d̵o̵ ̵a̵c̵a̵
̵            s̵i̵n̵o̵ ̵q̵u̵e̵ ̵v̵e̵n̵g̵a̵ ̵d̵e̵l̵ ̵d̵i̵a̵ ̵q̵u̵e̵ ̵s̵e̵ ̵i̵n̵g̵r̵e̵s̵o̵ ̵a̵n̵t̵e̵r̵i̵o̵r̵m̵e̵n̵t̵e̵ ̵e̵n̵ ̵l̵a̵ ̵c̵l̵a̵s̵e̵.̵ ̵p̵a̵r̵a̵ ̵e̵s̵o̵ ̵t̵e̵n̵d̵r̵i̵a̵ ̵q̵u̵e̵ ̵p̵o̵d̵e̵r̵
̵            t̵r̵a̵n̵s̵f̵o̵r̵m̵a̵r̵ ̵e̵l̵ ̵s̵t̵r̵i̵n̵g̵ ̵d̵d̵/̵m̵m̵/̵y̵y̵ ̵e̵n̵ ̵u̵n̵ ̵d̵a̵t̵e̵t̵i̵m̵e̵ ̵y̵ ̵p̵o̵d̵e̵r̵ ̵u̵t̵i̵l̵i̵z̵a̵r̵l̵o̵ ̵p̵a̵r̵a̵ ̵v̵e̵r̵ ̵q̵u̵e̵ ̵w̵e̵e̵k̵d̵a̵y̵ ̵e̵s̵.̵

            HECHO :P

            Idea 2 Podriamos mejorar las sugerencias al fijarnos en la base de datos si la categoria que estamos
            por sugerir ya fue cargada este mismo dia, entonces elegir otra, aunque lo dejo para otro dia 
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
