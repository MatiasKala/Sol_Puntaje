using System;
using System.Collections.Generic;
using System.Text;
using PuntajeClases.Models;

namespace PuntajeClases
{
    class Fecha
    {
        private const int LONGITUD_ACEPTADA_FORMATO_FECHA = 8;

        //----------------------------------------------------------
        // FUNCIONES DE INGRESO Y VALIDACION DE FECHA
        //----------------------------------------------------------
        public static string IngresarDia(string v)
        {
            Console.WriteLine(v);
            string[] respuesta = new string[2];
            respuesta = EsFormatoFecha(Console.ReadLine());
            while (respuesta[1] == null)
            {
                Console.WriteLine("Error, " + respuesta[0] + " no es una fecha valida.\nIngrese una fecha valida en formato dd/mm/aa");
                respuesta = EsFormatoFecha(Console.ReadLine());
            }

            Console.WriteLine(respuesta[1]);

            return respuesta[1];

        }
        private static string[] EsFormatoFecha(string respuesta)
        {
            string[] retorno = { respuesta, null };

            if (!EsHoy(retorno[0]) && !EsAyer(retorno[0]) && !EsManana(retorno[0]) && EsTamanioAceptado(retorno[0]))
            {

                if (EsTamanioAceptado(retorno[0], -1))
                {
                    if (EstaFechaConUnDigitoEnDia(retorno[0]))
                    {
                        return EsFormatoFecha("0" + retorno[0]);
                    }
                    else if (EstaFechaConUnDigitoEnMes(retorno[0]))
                    {
                        retorno[0] = ArreglarFechaUnDigitoEnMes(retorno[0]);

                        return EsFormatoFecha(retorno[0]);
                    }

                }

                return retorno;

            }

            if (EsAyer(retorno[0]))
            {
                retorno[1] = GenerarDiaDeAyerFormato();

                return retorno;
            }
            else if (EsHoy(retorno[0]))
            {
                retorno[1] = GenerarDiaDeHoyFormato();

                return retorno;
            }
            else if (EsManana(retorno[0]))
            {
                retorno[1] = GenerarDiaMananaFormato();

                return retorno;
            }

            Char[] respuestaArray = retorno[0].ToCharArray();

            if (FechaEstaBienPuesta(respuestaArray))
            {
                if (EsFechaValida(respuestaArray))
                    retorno[1] = retorno[0];
            }

            return retorno;

        }
        private static bool EsTamanioAceptado(string dia)
        {
            return dia.Length != LONGITUD_ACEPTADA_FORMATO_FECHA;
        }
        private static bool EsTamanioAceptado(string dia, int restarAtamanio)
        {
            return dia.Length != (LONGITUD_ACEPTADA_FORMATO_FECHA - restarAtamanio);
        }
        private static bool EstaFechaConUnDigitoEnDia(string dia)
        {
            bool ok = false;

            if (dia.ToCharArray()[4].Equals('/'))
            {
                if (dia.ToCharArray()[1].Equals('/'))
                {
                    ok = true;
                }
            }

            return ok;
        }
        private static bool EstaFechaConUnDigitoEnMes(string dia)
        {
            bool ok = false;

            if (dia.ToCharArray()[4].Equals('/'))
            {
                if (dia.ToCharArray()[2].Equals('/'))
                {
                    ok = true;
                }
            }

            return ok;
        }
        private static string ArreglarFechaUnDigitoEnMes(string dia)
        {
            dia = "0" + dia;
            char[] caracteres = dia.ToCharArray();
            char centinela = caracteres[1];
            int i = 3;
            char aux;

            while (caracteres[0] != centinela)
            {
                aux = caracteres[i];
                caracteres[i] = caracteres[0];
                caracteres[0] = aux;
                i--;
            }

            return new string(caracteres);
        }
        private static bool FechaEstaBienPuesta(char[] respuestaArray)
        {
            int cont = 0;
            bool ok = true;
            while (cont < LONGITUD_ACEPTADA_FORMATO_FECHA && ok)
            {
                if (cont == 2 || cont == 5)
                {
                    if (!respuestaArray[cont].Equals('/'))
                    {
                        ok = false;
                    }

                }
                else
                {

                    if (char.IsLetter(respuestaArray[cont]))
                    {
                        ok = false;
                    }
                }

                cont++;

            }

            return ok;
        }
        private static bool EsAyer(string s)
        {
            return s.Equals("ayer", StringComparison.InvariantCultureIgnoreCase);
        }
        private static bool EsHoy(string s)
        {
            return s.Equals("hoy", StringComparison.InvariantCultureIgnoreCase);
        }
        private static bool EsManana(string s)
        {
            return s.Equals("mañana", StringComparison.InvariantCultureIgnoreCase);
        }
        private static string GenerarDiaDeHoyFormato()
        {
            DateTime hoy = DateTime.Now;

            string miVariable = hoy.ToString("dd/MM/yy");

            return miVariable;
        }
        private static string GenerarDiaDeAyerFormato()
        {
            DateTime ayer = DateTime.Today.AddDays(-1);

            string miVariable = ayer.ToString("dd/MM/yy");

            return miVariable;
        }
        private static string GenerarDiaMananaFormato()
        {
            DateTime manana = DateTime.Today.AddDays(1);

            string miVariable = manana.ToString("dd/MM/yy");

            return miVariable;
        }
        private static bool EsFechaValida(Char[] fecha)
        {

            bool ok = false;
            int anio = Int32.Parse((new string("" + fecha[6] + "" + fecha[7]))) + 2000;

            if (anio >= DateTime.Now.Year && anio < Program.RANGO_ANIOS[1])
            {
                int mes = Int32.Parse((new string("" + fecha[3] + "" + fecha[4])));

                if (mes > 0 && mes <= 12)
                {
                    int dia = Int32.Parse((new string("" + fecha[0] + "" + fecha[1])));

                    if (dia > 0 && dia <= ObtenerNumerosDeDiaPorMes(mes))
                        ok = true;
                }
            }

            return ok;

        }
        private static int ObtenerNumerosDeDiaPorMes(int mes)
        {
            int num = 0;

            if (mes == 1 || mes == 3 || mes == 5 || mes == 7 || mes == 8 || mes == 10 || mes == 12)
            {
                num = 31;
            }
            else if (mes == 4 || mes == 6 || mes == 9 || mes == 11)
            {
                num = 30;
            }
            else if (mes == 2)
            {
                num = (DateTime.IsLeapYear(DateTime.Now.Year)) ? num = 29 : num = 28;
            }

            return num;

        }
        public static bool EsFechaMayor(Clases clases1, Clases clases2)
        {
            bool ok = false;

            char[] fecha1 = clases1.DiaClase.ToCharArray();
            char[] fecha2 = clases2.DiaClase.ToCharArray();

            int anio1 = ConseguirNumeroPorArray(fecha1, 6, 7);
            int anio2 = ConseguirNumeroPorArray(fecha2, 6, 7);


            if (anio1 > anio2)
            {
                ok = true;

            }
            else if (anio1 == anio2)
            {
                int mes1 = ConseguirNumeroPorArray(fecha1, 3, 4);
                int mes2 = ConseguirNumeroPorArray(fecha2, 3, 4);

                if (mes1 > mes2)
                {
                    ok = true;

                }
                else if (mes1 == mes2)
                {
                    int dia1 = ConseguirNumeroPorArray(fecha1, 0, 1);
                    int dia2 = ConseguirNumeroPorArray(fecha2, 0, 1);

                    if (dia1 > dia2)
                    {
                        ok = true;

                    }

                }

            }

            return ok;
        }
        private static int ConseguirNumeroPorArray(char[] fecha, int posicion1, int posicion2)
        {
            int decena = int.Parse(fecha[posicion1].ToString()) * 10;
            int unidad = int.Parse(fecha[posicion2].ToString());

            return decena + unidad;
        }

    }
}
