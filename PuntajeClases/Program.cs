
using PuntajeClases.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Office.Interop.Excel;
using _excel = Microsoft.Office.Interop.Excel;
using PuntajeClases.Unused;
using System.IO;

namespace PuntajeClases
{
    public class Program
    {
        public static int[] RANGO_ANIOS = { 2020,2030 };
        static ClasesContext context = new ClasesContext();
        public static void Main(string[] args)
        {
            const int FIN_PROGRAMA= 5;
            int ingresoRespuesta = -1;
            Console.WriteLine("Buen dia Mati");

            while (ingresoRespuesta != FIN_PROGRAMA)
            {
                try
                {
                    Console.WriteLine("Que hacemos?");
                    Console.WriteLine("1. Consultar datos");
                    Console.WriteLine("2. Cargar Nueva Clase");
                    Console.WriteLine("3. Cargar Informacion de Materias");
                    Console.WriteLine("4. Administrar BackUps de datos");
                    Console.WriteLine("5. Fin del programa");

                    ingresoRespuesta = IngresoRespuesta(1, 5);

                    switch (ingresoRespuesta)
                    {
                        case 1:
                            VerDatos();
                            break;
                        case 2:
                            CargarClase();
                            break;
                        case 3:
                            ModificarMaterias();
                            break;
                        case 4:
                            AdministrarBackUps();
                            break;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.GetType());
                    Console.WriteLine("NO VES QUE ROMPISTE ALGO, QUE HICISTE AHORA?");
                }
                Console.WriteLine("---------------------------------------------------------------------------------------");
            }
            Saludar();
            Console.WriteLine("Toca lo que sea para salir :)");
            Console.ReadLine();
        }

        //----------------------------------------------------------
        // EMPIEZAN LAS FUNCIONES PARA CARGAR UNA CLASE
        //----------------------------------------------------------
        private static void CargarClase()
        {
            Console.WriteLine("Hola Mati, largo dia verdad?");
            Console.WriteLine("Como estuvo la clase? ☺");

            Clases c = new Clases();

            c.DiaClase = Fecha.IngresarDia("Que dia fue la clase ?");
            c.Categoria = IngresarCategoriaCargarClase("Que tuviste ?",c.DiaClase);
            c.Puntaje = IngresarPuntaje("Del 1 al 10, cuanto estuvo la clase?");
            c.FueGrabada = IngresoRespuesta(0,1,"Se grabo la clase? 0 para NO, 1 para SI")==0 ? false: true;
            c.Comentario = Ingresar("Algun comentario extra?");
            c.Id = GenerarId(c.DiaClase);

            //c.SetCategDescrip();
            // NO SE PUEDE SETEAR LA CATEGORIA PORQUE SE ROMPE 
            // AL INTENTAR  AÑADIR A LA DATABASE, QUIZA PORQUE
            // RECIBE UN PARAMETRO QUE NO EXISTE EN ESTA

            try
            {
                context.Add(c);
                context.SaveChanges();
                Console.WriteLine("Se guardo la clase de " + c.Categoria);
            }
            catch
            {
                Console.WriteLine("Hubo un error che, trata otra vez");
            }

        }
        private static int GenerarId(string diaClase)
        {
            int i=0;
            int id;
            String num;
            Clases clase;
            diaClase=diaClase.Remove(2,1);
            diaClase=diaClase.Remove(4,1);
           
            do
            {
                i ++;
                num = i.ToString();
                id = Int32.Parse(diaClase+num);
                clase = context.Clases.Find(id);

            } while (clase!=null);
             
            return id;
         
        }
        private static double IngresarPuntaje(string v)
        {

                Console.WriteLine(v);
                string rta = Console.ReadLine();

                while (rta.Any(x => char.IsLetter(x)) || String.IsNullOrEmpty(rta))
                {
                    Console.WriteLine("Error, ingresa un numero nabo!!!");
                    rta = Console.ReadLine();
                }

                double puntaje = double.Parse(rta);

                if (puntaje < 1 || puntaje > 10)
                {
                    Console.WriteLine("Error");
                    Console.WriteLine("Te cuesta una locura pa");
                    IngresarPuntaje(v);
                }
                
                return puntaje;         

        }
        private static bool EsCateg(string rta)
        {
           Materias mat = context.Materias.Find(rta);
            
            if (mat != null)
            {
                return true;
            }

            return false;
        }
     
        //----------------------------------------------------------
        // EMPIEZAN LAS FUNCIONES PARA OBTENER DATOS
        //----------------------------------------------------------

        private static void VerDatos()
        {
            Console.WriteLine("Que datos queres consultar?");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("1. Mostrar todas las clases del mes actual");
            Console.WriteLine("2. Mostrar todas las clases de algun mes");
            Console.WriteLine("3. Obtener mejor clase de algun mes");
            Console.WriteLine("4. Obtener mejor clase de algun año");
            Console.WriteLine("5. Obtener mejor clase de todos los tiempos");
            Console.WriteLine("6. Mostrar promedio de puntaje del mes");
            Console.WriteLine("7. Mostrar promedio de puntaje del año");
            Console.WriteLine("8. Mostrar promedio por materia");
            Console.WriteLine("9. Mostrar todos los promedios por materia");
            Console.WriteLine("10. Mostrar mejor clase por materia");
            Console.WriteLine("11. Mostrar materia con mejor promedio de puntaje por clase");
            Console.WriteLine("12. Mostrar informacion de materias");
            Console.WriteLine("13. Mostrar materias por cuatrimestre");
            Console.WriteLine("14. Mostrar profesores");
            Console.WriteLine("15. Mostrar clases por materia");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("\n");
            int mes;
            int cuatri;

            try
            {
                switch (IngresoRespuesta(1, 15, 99, 100))
                {
                    case 1:
                        MostrarClasesMes(DateTime.Now.Month, DateTime.Now.Year);
                        break;
                    case 2:
                        Console.WriteLine("De que mes queres ver las clases?");
                        mes = IngresoRespuesta(1, 12);
                        Console.WriteLine("Y de que año?");
                        MostrarClasesMes(mes, IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1]));
                        break;
                    case 3:
                        Console.WriteLine("De que mes queres ver la mejor?");
                        mes = IngresoRespuesta(1, 12);
                        Console.WriteLine("Y de que año?");
                        MostrarMejorClaseMes(mes, IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1]));
                        break;
                    case 4:
                        Console.WriteLine("De que año queres ver la mejor?");
                        MostrarMejorClaseAño(IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1]));
                        break;
                    case 5:
                        MostrarMejorClaseDeSiempre();
                        break;
                    case 6:
                        Console.WriteLine("De que mes queres ver el promedio?");
                        mes = IngresoRespuesta(1, 12);
                        Console.WriteLine("Y de que año?");
                        MostrarPromedioClaseMes(mes, IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1]));
                        break;
                    case 7:
                        Console.WriteLine("De que año queres ver el promedio?");
                        MostrarPromedioClaseAno(IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1]));
                        break;
                    case 8:
                        MostrarPromedioClaseMateria(IngresarCategoria("De que materia queres ver el promedio?"));
                        break;
                    case 9:
                        MostrarTodosLosPromedios();
                        break;
                    case 10:
                        MostrarMejorClasePorCateg(IngresarCategoria("De que materia queres ver la mejor ? "));
                        break;
                    case 11:
                        MostrarMejorMateria();
                        break;
                    case 12:
                        MostrarInfoMaterias();
                        break;
                    case 13:
                        Console.WriteLine("De que cuatri queres ver las materias?");
                        cuatri = IngresoRespuesta(1, 2);
                        Console.WriteLine("Y de que año?");
                        MostrarMateriasPorCuatrimestre(cuatri, IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1]));
                        break;
                    case 14:
                        MostrarProfesores();
                        break;
                    case 15:
                        MostrarClases(ObtenerClasesOrdenadasShell(ObtenerClasesPorCateg(IngresarCategoria("De que materia vemos las clases?"))));
                        break;
                    case 99:
                        OpcionSecreta();
                        break;
                    case 100:
                        OpcionSecreta2();
                        break;

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.GetType());
                Console.WriteLine("Error, algo fallo ;(");
            }
         
        }
        private static void MostrarClases(List<Clases> clases)
        {
            var contador = 1;

            foreach (Clases c in clases)
            {
                Console.WriteLine(contador + ".    " + c.Mostrar() + "\n\n");
                contador++;
            }
        }
        private static void MostrarClases(List<Clases> clases, string v)
        {
            var contador = 1;
            Console.WriteLine(v + "\n");

            foreach (Clases c in clases)
            {
                Console.WriteLine(contador + ".    "+c.Mostrar()+"\n\n");
                contador++;
            }
        }   
        private static void MostrarTodosLosPromedios()
        {

            List<Materias> todasLasMaterias = context.Materias.ToList();

            List<Materias> materiasSinCargar = new List<Materias>();

            Dictionary<double, List<string>> promedios = new Dictionary<double, List<string>>();

            foreach (Materias m in todasLasMaterias)
            {
                double prom = ObtenerPromedio(ObtenerClasesPorCateg(m.Categoria));

                if (m.Clases.Count <= 1)
                {
                    materiasSinCargar.Add(m);
                }
                else
                {
                    promedios.TryAdd(prom, new List<string>());
                    promedios[prom].Add(m.Descripcion);

                }

            }

            double[] promValores =promedios.Keys.ToArray();
            
            OrdenarPromediosInsercion(promValores);

            Console.WriteLine("Mostrando promedios por materia de mayor a menor\n\n");

            for (int i = promValores.Length-1; i >0 - 1; i--)
            {
                List<string> value;
                
                promedios.TryGetValue(promValores[i], out value);

                if (HayUnoSolo(value))
                    Console.WriteLine("El promedio de la materia " + value[0] + " es de " + promValores[i] + "\n");
                else
                    Console.WriteLine("El promedio de la materia " + ObtenerNombresConcatenados(value) + " es de " + promValores[i] + "\n");
                ;
            }

            Console.WriteLine("Estas materias tienen una o ninguna clase cargada \n\n");

            foreach (Materias m in materiasSinCargar)
            {
                Console.WriteLine(m.Descripcion+"\n");
            }

        }
        private static bool HayUnoSolo(List<string> value)
        {
            return value.Count == 1;
        }
        private static string ObtenerNombresConcatenados(List<string> value)
        {
            string retorno = value[0].Trim();
            for (int i=1;i<value.Count;i++)        
            {
                retorno += " ," + value[i].Trim();
            }
            return retorno;
        }
       
        //private static void OrdenarPromediosBurbuja(double[] promValores)
        //{

        //    double aux;

        //    for (int i = 0; i < promValores.Length-1; i++)
        //    {
        //        for (int j = 0; j < promValores.Length-1; j++)
        //        {
        //            if (promValores[j] > promValores[j + 1])
        //            {
        //                aux=promValores[j];
        //                promValores[j] = promValores[j + 1];
        //                promValores[j + 1] = aux;
        //            }
        //        }
        //    }

        //}
        private static void OrdenarPromediosInsercion(double[] promValores)
        {
            int tamanio = promValores.Length;

            // Solo va a dar una vuelta
            for (int i = 1; i < tamanio; ++i)
            {
                // Agarra un numero clave para iterar
                double key = promValores[i];
                int j = i - 1;

                // Ahora itera desde el numero clave para atras hasta que llegue al principio o el mismo sea mayor al apuntado 
                // Va reemplazando los numeros que son menores por el numero clave

                while (j >= 0 && promValores[j] > key)
                {
                    promValores[j + 1] = promValores[j];
                    j = j - 1;
                }

                //Finalmente ubica el numero clave cuando llegua al principio o se encuentra un numero menor
                promValores[j + 1] = key;
            }

            // Explicacion con imagenes: https://www.geeksforgeeks.org/insertion-sort/

        }
        private static void MostrarPromedioClaseMateria(string categ)
        {
            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesDeMateria = new List<Clases>();

            foreach (Clases c in clases)
            {
                string cat = c.Categoria;

                if (cat.Equals(categ))
                {
                    clasesDeMateria.Add(c);
                } 
            }

            if (clasesDeMateria.Count != 0)
            {

                double promedio = ObtenerPromedio(clasesDeMateria);

                Console.WriteLine("\nEl promedio de puntaje en las clases de  " + categ + " es de " + promedio + "\n");

            }
            else
            {
                Console.WriteLine("No hay clases cargadas de " + categ);
            }

        }
        private static void MostrarMejorMateria()
        {
            List<Clases> clasesPorMateria;

            List<Materias> materias = context.Materias.ToList();

            double promedioMax = double.MinValue;

            double promedio;

            String categoriaMax = null;

            foreach (Materias m in materias)
            {
                clasesPorMateria = ObtenerClasesPorCateg(m.Categoria);

                if (clasesPorMateria.Count != 0)
                {
                    promedio = ObtenerPromedio(clasesPorMateria);

                    if (promedio > promedioMax)
                    {
                        promedioMax = promedio;
                        categoriaMax = m.Categoria;
                    }

                }
            }

            Console.WriteLine("La mejor materia es " + categoriaMax + " con un promedio de " + promedioMax);

        }
        private static List<Clases> ObtenerClasesPorCateg(string categoria)
        {
            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesPorMateria = new List<Clases>();

            foreach (Clases c in clases)
            {
                if (c.Categoria.Equals(categoria))
                {
                    clasesPorMateria.Add(c);
                }
            }

            return clasesPorMateria;
        }
        private static void MostrarMejorClasePorCateg(string categ)
        {
            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesDeCateg = new List<Clases>();

            foreach (Clases c in clases)
            {

                if (EsDeCateg(c, categ))
                {
                    clasesDeCateg.Add(c);
                }
            }

            if (clasesDeCateg.Count != 0)
            {

                List<Clases> mejorClaseDeCateg = ObtenerMejorClase(clasesDeCateg);

                Console.WriteLine();

                MostrarClases(mejorClaseDeCateg, "\n La mejor clase de la materia " + categ + " es:\n");

            }
            else
            {
                Console.WriteLine("No hay clases de la materia " + categ);
            }
        }
        private static List<Clases> ObtenerMejorClase(List<Clases> clasesDeCateg)
        {
            double puntajeMax = Double.MinValue;
            List<Clases> mejores = new List<Clases>();

            foreach (Clases c in clasesDeCateg)
            {
                double puntaje = c.Puntaje;
                if (puntaje > puntajeMax)
                {
                    mejores.Clear();
                    mejores.Add(c);
                    puntajeMax = puntaje;
                }
                else if (puntaje == puntajeMax)
                {
                    mejores.Add(c);
                }
            }

            return mejores;
        }
        private static bool EsDeCateg(Clases c, string categ)
        {
            if (c.Categoria.Equals(categ))
            {
                return true;
            }

            return false;
        }
        private static void MostrarPromedioClaseAno(int anio)
        {
            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesDelAnio = new List<Clases>();

            foreach (Clases c in clases)
            {
                string dia = c.DiaClase;

                if (EsDeAnio(dia, anio))
                {
                    clasesDelAnio.Add(c);
                }

            }

            if (clasesDelAnio.Count != 0)
            {

                double promedio = ObtenerPromedio(clasesDelAnio);

                Console.WriteLine("\nEl promedio de puntaje en las clases del año " + anio + " es de " + promedio + "\n");

            }
            else
            {
                Console.WriteLine("No hay clases en el año " + anio);
            }

        }
        private static void MostrarPromedioClaseMes(int month, int year)
        {
            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesDelMes = new List<Clases>();

            foreach (Clases c in clases)
            {
                string dia = c.DiaClase;

                if (EsDeAnio(dia, year))
                {

                    if (EsDeMes(dia, month))
                    {
                        clasesDelMes.Add(c);
                    }

                }
            }

            var nombreMes = ObtenerNombreMesPorNumero(month);

            if (clasesDelMes.Count != 0)
            {

                double promedio = ObtenerPromedio(clasesDelMes);

                Console.WriteLine("\nEl promedio de puntaje en las clases del mes numero " + nombreMes + " es de " + promedio + "\n");

            }
            else
            {
                Console.WriteLine("No hay clases del mes " + nombreMes + " del " + year);
            }

        }
        private static void MostrarMejorClaseMes(int month, int year)
        {

            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesDeMes = new List<Clases>();

            foreach (Clases c in clases)
            {
                string dia = c.DiaClase;

                if (EsDeAnio(dia, year))
                {
                    if (EsDeMes(dia, month))
                    {

                        clasesDeMes.Add(c);

                    }
                }
            }

            var nombreMes = ObtenerNombreMesPorNumero(month);


            if (clasesDeMes.Count != 0)
            {

                List<Clases> mejorClase = ObtenerMejorClase(clasesDeMes);

                Console.WriteLine();

                MostrarClases(mejorClase,"\nMostrando la/s clase/s con mejor puntaje del mes " + nombreMes + "\n");

            }
            else
            {
                Console.WriteLine("No hay clases del mes " + nombreMes + " del " + year);

            }

        }
        private static void MostrarMejorClaseAño(int year)
        {
            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesDelAño = new List<Clases>();

            foreach (Clases c in clases)
            {
                string dia = c.DiaClase;

                if (EsDeAnio(dia, year))
                {
                    clasesDelAño.Add(c);
                }


            }

            if (clasesDelAño.Count != 0)
            {

                List<Clases> mejorClase = ObtenerMejorClase(clasesDelAño);

                MostrarClases(mejorClase, "\nMostrando la/s clase/s con mejor puntaje del año " + year + "\n");

            }
            else
            {
                Console.WriteLine("No hay clases del año " + year);

            }

        }
        private static void MostrarMejorClaseDeSiempre()
        {
            List<Clases> clases = context.Clases.ToList();

            if (clases.Count != 0)
            {
                List<Clases> mejorClase = ObtenerMejorClase(clases);

                MostrarClases(mejorClase, "Esta/s son la/s clases con mejor puntuacion:\n");
            }

        }
        private static void MostrarClasesMes(int month, int year)
        {

            List<Clases> clases = context.Clases.ToList();

            List<Clases> clasesDelMes = new List<Clases>();

            foreach (Clases c in clases)
            {
                string dia = c.DiaClase;

                if (EsDeAnio(dia, year))
                {
                    if (EsDeMes(dia, month))
                    {
                        clasesDelMes.Add(c);
                    }
                }

            }

            var nombreMes = ObtenerNombreMesPorNumero(month);

            if (clasesDelMes.Count != 0)
            {

                MostrarClases(clasesDelMes, "\nMostrando clases del mes " + nombreMes + "\n");

            }
            else
            {
                Console.WriteLine("No hay clases del mes " + nombreMes + " del " + year);
            }

        }
        private static bool EsDeMes(string dia, int month)
        {
            char[] diaArray = dia.ToCharArray(3, 2);

            dia = new string(diaArray);

            int mes = int.Parse(dia);

            if (mes == month)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool EsDeAnio(string dia, int anio)
        {
            const int RESTAR_AL_ANIO = 2000;

            int anioAusar = anio - RESTAR_AL_ANIO;

            char[] diaArray = dia.ToCharArray(6, 2);

            dia = new string(diaArray);

            int anioDia = int.Parse(dia);

            if (anioAusar == anioDia)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void MostrarInfoMaterias()
        {
            List<Materias> materias = context.Materias.ToList<Materias>();

            for (int i = 0; i < materias.Count; i++)
            {
                Materias m = (Materias)materias[i];

                Console.WriteLine("Materia '" + m.Descripcion + "' Profesor " + m.Profesor);
                if (m.Ayudante != null && m.Ayudante.Length!=0)
                {
                    Console.WriteLine("Ayudante:" + m.Ayudante);
                }
                Console.WriteLine("Año " + m.Anio + " del " + m.TiempoAnio+ "\n");
            }

            Console.WriteLine("Cantidad de materias cargadas:" + materias.Count);
        }
        private static void MostrarMateriasPorCuatrimestre(int cuatrim, int anio)
        {
            List<Materias> materias = context.Materias.ToList<Materias>();

            List<Materias> materiasDelCuatrim = new List<Materias>();

            bool esPrimerCuatri = cuatrim == 1;

            foreach(Materias m in materias)
            {
                if (m.Anio == anio)
                {
                    if (m.TiempoAnio.StartsWith("1") == esPrimerCuatri)
                    {
                        materiasDelCuatrim.Add(m);
                    }
                }
            }

            foreach (Materias m in materiasDelCuatrim)
            {
                Console.WriteLine(m.Descripcion +" Profesor: "+m.Profesor);
            }

            Console.WriteLine("\nCantidad Materias en el " + cuatrim + "° cuatrimestre del año " + anio+":  "+materiasDelCuatrim.Count);
        }
        private static double ObtenerPromedio(List<Clases> clases)
        {
            int contador = 0;
            double acum = 0;

            foreach (Clases c in clases)
            {
                contador++;
                acum += c.Puntaje;
            }

            double prom = Math.Round(acum / contador, 4);

            return prom;

        }
        private static void MostrarProfesores()
        {
            Materias[] materias = context.Materias.ToArray<Materias>();

            LinkedList<Materias> aux = new LinkedList<Materias>();

            OrdenarProfesoresShell(materias);

            for(int i = 0; i < materias.Length; i++)
            {
                if (materias[i] != null)
                {
                    aux=BuscarOtraMateriaMismoProfe(materias[i],materias);

                    foreach(Materias m in aux)
                    {
                        if (m.Profesor != null)
                        {
                            Console.WriteLine("Profesor: " + m.Profesor.PadRight(25) + " Materia: " + m.Descripcion);
                        }
                    }

                }  
            }
        }
        private static LinkedList<Materias> BuscarOtraMateriaMismoProfe(Materias materia, Materias[] materias)
        {
            LinkedList<Materias> materiasRepe = new LinkedList<Materias>();

            materiasRepe.AddFirst(materia);

            if (materia.Profesor != null)
            {
                for (int i = 0; i < materias.Length; i++)
                {
                    Materias m = materias[i];
                    if (m != null && m.Profesor != null && m.Profesor == materia.Profesor && m != materia)
                    {
                        materias[i] = null;
                        materiasRepe.AddLast(m);
                    }
                }
            }

            return materiasRepe;

        }
        private static void OrdenarProfesoresShell(Materias[] array)
        {
            int length = array.Length;

            for (int h = length / 2; h > 0; h /= 2)
            {
                for (int i = h; i < length; i += 1)
                {
                    Materias temp = array[i];

                    int j;
                    for (j = i; j >= h && array[j - h].Profesor.CompareTo(temp.Profesor) > 0; j -= h)
                    {
                        array[j] = array[j - h];
                    }

                    array[j] = temp;
                }
            }

        }
        private static Clases[] ObtenerClasesOrdenadasShell()
        {
            int cantidad = context.Clases.Count()-1;

            Clases[] clases = context.Clases.ToArray();

            int length = clases.Length;

            for (int h = length / 2; h > 0; h /= 2)
            {
                for (int i = h; i < length; i += 1)
                {
                    Clases temp = clases[i];

                    int j;
                    for (j = i; j >= h && Fecha.EsFechaMayor(clases[j - h],temp); j -= h)
                    {
                        clases[j] =clases[j - h];
                    }

                    clases[j] = temp;
                }
            }


            return clases;
        }
        private static List<Clases> ObtenerClasesOrdenadasShell(List<Clases> clases)
        {

            int length = clases.Count;

            for (int h = length / 2; h > 0; h /= 2)
            {
                for (int i = h; i < length; i += 1)
                {
                    Clases temp = clases[i];

                    int j;
                    for (j = i; j >= h && Fecha.EsFechaMayor(clases[j - h], temp); j -= h)
                    {
                        clases[j] = clases[j - h];
                    }

                    clases[j] = temp;
                }
            }


            return clases;
        }
      

        //----------------------------------------------------------
        // EMPIEZAN LAS FUNCIONES PARA MODIFICAR MATERIAS  
        //----------------------------------------------------------

        private static void ModificarMaterias()
        {
            Console.WriteLine("Que datos queres ingresar?");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("1. Ingresar nueva materia");
            Console.WriteLine("2. Editar materia");
            Console.WriteLine("3. Eliminar materia");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("\n");
            int rta = IngresoRespuesta(1, 3);

            try
            {
                switch (rta)
                {
                    case 1:
                        CargarNuevaMateria();       
                        break;
                    case 2:
                        EditarMateria();
                        break;
                    case 3:
                        BorrarMateria();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());
                Console.WriteLine("Error, algo fallo ;(");
            }
        }
        private static void CargarNuevaMateria()
        {
            Materias m = new Materias();

            m.Descripcion = Ingresar("Ingrese el nombre de la materia");
            m.Categoria = IngresarCategoriaNueva("Ingrese la abreviatura de la materia (Ej:TP1)");
            m.Anio = IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1],"Ingrese el año de la Materia");
            m.TiempoAnio = Ingresar("Ingrese en que tiempo del año se cursa la materia (Ej 1er Cuatrimestre)");
            m.Profesor = Ingresar("Ingrese el nombre del profesor de la materia");
            m.Ayudante = Ingresar("Ingrese el ayudante de la materia (En caso de no haberlo, no ingrese nada)");
            m.Institucion = Ingresar("Ingrese la institucion en la que se da la clase");
            try
            {
                context.Add(m);
                context.SaveChanges();
                Console.WriteLine("Se logro cargar la materia " + m.Categoria);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());

                Console.WriteLine("Hubo un error che, trata otra vez");
            }

        }
        private static void EditarMateria()
        {
            List<Materias> materias = context.Materias.ToList<Materias>();

            int tamanio = materias.Count;
            int j =1;

            for (int i = 0; i < tamanio; i+=2)
            {
                
                if ((i+1)<tamanio && materias[i + 1] != null)
                {
                    Console.WriteLine(j +"."+ materias[i].Descripcion + "" + (j+1) +"."+ materias[i+1].Descripcion);
                }
                else
                {
                    Console.WriteLine(j +"."+ materias[i].Descripcion);
                }
                j += 2;
            }

            Console.WriteLine();

            int eleccion = IngresoRespuesta(1, tamanio,"Que materia queres editar?") - 1;

            EditarMateria(materias[eleccion]);

        }
        private static void EditarMateria(Materias m)
        {

            int eleccion = IngresoRespuesta(0, 7, "Que modificamos de " + m.Descripcion + "?\n" +
                "1.Nombre de la materia\n" +
                "2.Abreviatura\n" +
                "3.Año\n" +
                "4.Epoca del año\n" +
                "5.Profesor\n" +
                "6.Ayudante\n" +
                "7.Institucion");

            while (eleccion != 0)
            {
                switch (eleccion)
                {
                    case 1:
                        m.Descripcion = Ingresar("Ingrese el nombre de la materia");
                        break;
                    case 2:
                        m.Categoria = IngresarCategoria("Ingrese la abreviatura de la materia (Ej:TP1)");
                        break;
                    case 3:
                        m.Anio = IngresoRespuesta(RANGO_ANIOS[0], RANGO_ANIOS[1], "Ingrese el año de la Materia");
                        break;
                    case 4:
                        m.TiempoAnio = Ingresar("Ingrese en que tiempo del año se cursa la materia (Ej 1er Cuatrimestre)");
                        break;
                    case 5:
                        m.Profesor = Ingresar("Ingrese el nombre del profesor de la materia");
                        break;
                    case 6:
                        m.Ayudante = Ingresar("Ingrese el ayudante de la materia (En caso de no haberlo, no ingrese nada)");
                        break;
                    case 7:
                        m.Institucion= Ingresar("Ingrese la institucion en la que se da la clase");
                        break;
                }
                Console.WriteLine("Desea cambiar algo mas?\n0. Salir\n" +
                "1.Nombre de la materia\n" +
                "2.Abreviatura\n" +
                "3.Año\n" +
                "4.Epoca del año\n" +
                "5.Profesor\n" +
                "6.Ayudante\n"+
                "7.Institucion");
                eleccion = IngresoRespuesta(0, 7);
            }

            try
            {
                context.Materias.Update(m);

                context.SaveChanges();

                Console.WriteLine("Se logra editar la materia " + m.Categoria);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());

                Console.WriteLine("Se rompio algo pa, trata otra vez");
            }
           
        }
        private static void BorrarMateria()
        {
            List<Materias> materias = context.Materias.ToList<Materias>();

            int tamanio = materias.Count;
            int j = 1;

            for (int i = 0; i < tamanio; i += 2)
            {

                if (materias[i + 1] != null)
                {
                    Console.WriteLine(j + "." + materias[i].Descripcion + "" + (j + 1) + "." + materias[i + 1].Descripcion);
                }
                else
                {
                    Console.WriteLine(j + "." + materias[i].Descripcion);
                }
                j += 2;
            }

            Console.WriteLine();

            int eleccion = IngresoRespuesta(1, tamanio, "Que materia queres editar?") - 1;

            BorrarMateria(materias[eleccion]);

        }
        private static void BorrarMateria(Materias m)
        {
            try
            {
                context.Materias.Remove(m);

                context.SaveChanges();

                Console.WriteLine("Se logra borrar la materia " + m.Categoria);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());

                Console.WriteLine("Se rompio algo pa, trata otra vez");
            }

        }

        //----------------------------------------------------------
        // FUNCIONES GENERALES   
        //----------------------------------------------------------

        private static void Saludar()
        {
            DayOfWeek wk = DateTime.Today.DayOfWeek;
            if (wk.Equals(DayOfWeek.Thursday) || wk.Equals(DayOfWeek.Friday) || wk.Equals(DayOfWeek.Saturday) || wk.Equals(DayOfWeek.Sunday))
            {
                Console.WriteLine("Ya me encargue de todo lo que querias. Nos vemos el lunes, buen finde Mati!! ");
            }
            else
            {
                Console.WriteLine("Todo hecho. Nos vemos Mati, buen  " + wk);
            }
        }
        private static string Ingresar(string v)
        {
            Console.WriteLine(v);
            return Console.ReadLine();
        }
        public static int IngresoRespuesta(int min, int max)
        {
            int rta = 0;

            Console.WriteLine("Ingresa un numero entre " + min + " y " + max);
            rta = int.Parse(Console.ReadLine());

            while (rta < min || rta > max)
            {
                try
                {
                    Console.WriteLine("Error capo");
                    Console.WriteLine("Ingresa un numero entre " + min + " y " + max);
                    rta = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("NUMERO DIJE!!!!");
                }
            }

            Console.WriteLine("\n");

            return rta;
        }
        public static int IngresoRespuesta(int min, int max,int excepcion1,int excepcion2)
        {
            int rta = 0;

            Console.WriteLine("Ingresa un numero entre " + min + " y " + max);
            rta = int.Parse(Console.ReadLine());

            while (rta < min || rta > max && rta != excepcion1 && rta != excepcion2)
            {
                try
                {
                    Console.WriteLine("Error, nada que ver jajaj");
                    Console.WriteLine("Ingresa un numero entre " + min + " y " + max);
                    rta = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("NUMERO DIJE!!!!");
                }
            }

            Console.WriteLine("\n");

            return rta;
        }
        public static int IngresoRespuesta(int min, int max,string v)
        {
            int rta = 0;

            Console.WriteLine(v);
            Console.WriteLine("Ingresa un numero entre " + min + " y " + max);
            rta = int.Parse(Console.ReadLine());

            while (rta < min || rta > max)
            {
                try
                {
                    Console.WriteLine("Error, deja de mandar fruta");
                    Console.WriteLine("Ingresa un numero entre " + min + " y " + max);
                    rta = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("NUMERO DIJE!!!!");
                }
            }

            Console.WriteLine("\n");

            return rta;
        }
        public static string IngresarCategoria(string v)
        {
            Console.WriteLine(v);
            string rta = Console.ReadLine();

            while (rta.Length != 3 || !EsCateg(rta.ToUpper()))
            {
                Console.WriteLine("No existe " + rta + " capo");
                Console.WriteLine("Ingresa una categoria valida (3 letras, ej: nt1)");
                rta = Console.ReadLine();
            }

            return rta.ToUpper();
        }
        public static string IngresarCategoriaNueva(string v)
        {
            Console.WriteLine(v);
            string rta = Console.ReadLine();

            while (rta.Length != 3)
            {
                Console.WriteLine( rta + "??? Flasheaste feo");
                Console.WriteLine("Ingresa una categoria valida (3 letras, ej: nt1)");
                rta = Console.ReadLine();
            }

            return rta.ToUpper();
        }
        private static string IngresarCategoriaCargarClase(string v,string ddMMyy)
        {
            string retorno;

            string categPorDia = ObtenerCategoriaPorDiaDeSemana(ddMMyy);

            if(categPorDia!=null && IngresoRespuesta(0,1,"Tuviste " + categPorDia + "?\n0.NO 1.SI")==1)
            {
                retorno = categPorDia;
            }
            else
            {
                retorno=IngresarCategoria(v);
            }

            return retorno;

        }
        private static string ObtenerCategoriaPorDiaDeSemana(string day)
        {
            return DiccionarioMateriaPorDia.CategoriaPorDia(day);
        }
        private static void OpcionSecreta()
        {
            Console.WriteLine("Ah jajajja la opcion secreta? Toca lo que sea");
            Console.ReadLine();
            Clases[] todasLasClases = ObtenerClasesOrdenadasShell();

            int i = 1;

            foreach (Clases c in todasLasClases)
            {
                string s = i + ".    " + c.Mostrar();
                Console.WriteLine(s + "\n");
                i = i + 1;
            }

            i = i - 1;

            Console.WriteLine("Clases cargadas por Materias\n");

            Materias[] m = context.Materias.ToArray();
            Dictionary<string, int> cantidadClasesMaterias = new Dictionary<string, int>();


            foreach (var mat in m)
            {
                if (mat.Clases.Count == 0)
                    continue;
                cantidadClasesMaterias.Add(mat.Descripcion, mat.Clases.Count);
            }

            var newArray = cantidadClasesMaterias.Keys.OrderBy(x => x.ToLower()).ToArray();

            foreach (var item in newArray)
            {
                Console.WriteLine(item + ":" + cantidadClasesMaterias[item]);
            }


            Console.WriteLine("\nCantidad de clases cargadas: " + i);

        }
        private static void OpcionSecreta2()
        {
            //MEJORAR FUNCION

            Console.WriteLine("Que clase borramos o Editamos? Ingresa el ID (nrodia+nromes+nroaño+nroclasedeldia)");

            int id = int.Parse(Console.ReadLine());

            try
            {

                var query = from c in context.Clases
                        where c.Id==id  
                        select c;

                var clase = query.FirstOrDefault<Clases>();

                if (clase != null)
                {
                    if (IngresoRespuesta(1, 2, "Que queres hacer con esta clase? "+clase.Categoria+" del "+clase.DiaClase+". Borrar o Editar?. Ingresa 1 para borrar 2 para editar") == 1)
                    {
                        
                        BorrarClase(clase);

                    }
                    else
                    {
                        EditarClase(clase);
                    }
                }
                else
                {
                    Console.WriteLine("La clase no existe");
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message,e.InnerException);
            }
        }
        private static void EditarClase(Clases clase)
        {
            int eleccion = IngresoRespuesta(0, 5, "Que modificamos de la clase " + clase.Categoria+" "+clase.DiaClase + "?\n" +
                "0.Salir\n" +
                "1.Dia de la clase\n" +
                "2.Materia\n" +
                "3.Puntaje de la clase\n" +
                "4.Fue grabada\n" +
                "5.Comentario");

            while (eleccion != 0)
            {
                switch (eleccion)
                {
                    case 1:
                        Console.WriteLine("Actual\n" + clase.DiaClase);
                        clase.DiaClase = Fecha.IngresarDia("Ingrese el nuevo dia de la clase");
                        break;
                    case 2:
                        Console.WriteLine("Actual\n" + clase.Categoria);
                        clase.Categoria = IngresarCategoria("Ingrese la abreviatura de la materia (Ej:TP1)");
                        break;
                    case 3:
                        Console.WriteLine("Actual\n" + clase.Puntaje);
                        clase.Puntaje = IngresarPuntaje("Ingresa el nuevo puntaje de la clase (1 al 10)");
                        break;
                    case 4:
                        Console.WriteLine("Actual\n" + clase.FueGrabada);
                        clase.FueGrabada = IngresoRespuesta(0, 1, "Se grabo la clase? 0 para NO, 1 para SI") == 0 ? false : true;
                        break;
                    case 5:
                        Console.WriteLine("Actual\n" + clase.Comentario);
                        if (IngresoRespuesta(0, 1, "Desea añadir algo a lo escrito anteriormente o rehacer?\n0 AÑADIR\n1 REHACER") == 0)
                        {
                            string comment = clase.Comentario.Trim();
                            comment+= " " + Ingresar("Extienda el comentario");
                            clase.Comentario = comment;
                        }
                        else
                        {
                            clase.Comentario = Ingresar("Modifique el comentario");
                        }
                        Console.WriteLine("Nuevo Comentario\n" + clase.Comentario);
                        break;
                }
                eleccion = IngresoRespuesta(0, 5, "Desea cambiar algo mas?\n" +
                "0.Salir\n" +
                "1.Dia de la clase\n" +
                "2.Materia\n" +
                "3.Puntaje de la clase\n" +
                "4.Fue grabada\n"+
                "5.Comentario");
            }

            try
            {
                context.Clases.Update(clase);

                context.SaveChanges();

                Console.WriteLine("Se logra editar la clase de " + clase.Categoria);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType());

                Console.WriteLine("Se rompio algo pa, trata otra vez");
            }
        }
        private static void BorrarClase(Clases clase)
        {
            if(IngresoRespuesta(1,2,"Seguro que queres borrar la clase de "+clase.Categoria+" del dia "+clase.DiaClase+".\n1. SI 2. NO") == 1) {

            context.Remove(clase);

            context.SaveChanges();

            Console.WriteLine("Borrada");
            }
        }
        private static string ObtenerNombreMesPorNumero(int month)
        {
            switch (month)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
            }
            throw new ArgumentException("No existe el mes numero"+month);
        }
        //----------------------------------------------------------
        // BACK UP   
        //----------------------------------------------------------
        private static void AdministrarBackUps()
        {
            Console.WriteLine("Que hacemos con los BackUps?");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("1. Crear nuevo BackUp");
            Console.WriteLine("2. Mostrar todos los BackUps");
            Console.WriteLine("3. Mostrar ultimo BackUp");

            switch (IngresoRespuesta(1, 3))
            {
                case 1:
                    CrearBackUp();
                    break;
                case 2:
                    MostrarTodosBackUps();
                    break;
                case 3:
                    MostrarUltimoBackUp();
                    break;
            }
        }
        private static void CrearBackUp()
        {
            try
            {
                Application app = new Application();
                Workbook workbook = app.Workbooks.Add();
                Worksheet worksheet1 = workbook.Worksheets[1];
                Worksheet worksheet2 = workbook.Worksheets.Add();

                worksheet1.Name = "Clases";
                worksheet2.Name = "Materias";

                CrearBackUpClases(worksheet1);
                CrearBackUpMaterias(worksheet2);

                string diaDelBackUp =DateTime.Now.ToString("dd_MM_yyyy__HH_mm");
                diaDelBackUp = diaDelBackUp.Insert(14, "hs");
                diaDelBackUp = diaDelBackUp.Insert(diaDelBackUp.Length, "mins");
                diaDelBackUp = "Back up " + diaDelBackUp;

                workbook.SaveAs(@"C:\Users\matia\source\repos\Sol_Puntaje\PuntajeClases\BackUps\"+diaDelBackUp+".xlsx");
                workbook.Close();
                app.Quit();
                Console.WriteLine(diaDelBackUp + ".xlsx creado");
            }
            catch (Exception e)
            {
                Console.WriteLine("Algo falló \n"+e.Message);
            }

        }
        private static void CrearBackUpClases(Worksheet worksheet1)
        {
            CrearTitulosBackUpClases(worksheet1);

            Clases[] todasLasClases = ObtenerClasesOrdenadasShell();

            int cantidadClases = todasLasClases.Length;
            int contClases = 0;

            for (int row = 2; row < cantidadClases + 2; row++)
            {

                worksheet1.Cells[row, 1] = todasLasClases[contClases].DiaClase;
                worksheet1.Cells[row, 2] = todasLasClases[contClases].Categoria;
                worksheet1.Cells[row, 3] = todasLasClases[contClases].Puntaje;
                worksheet1.Cells[row, 4] = todasLasClases[contClases].Comentario;
                worksheet1.Cells[row, 5] = todasLasClases[contClases].FueGrabada;

                contClases++;

            }

        }
        private static void CrearTitulosBackUpClases(Worksheet w)
        {
            for(int i = 1; i < 6; i++)
            {
                w.Cells[1, i] = GetValorTitulo(i);
            }
        }
        private static dynamic GetValorTitulo(int i)
        {
            switch (i)
            {
                case 1:
                    return "Dia Clase";
                case 2:
                    return "Materia";
                case 3:
                    return "Puntaje";
                case 4:
                    return "Comentario";
                case 5:
                    return "Fue Grabada?";
                case 6:
                    return "Categoria";
                case 7:
                    return "Descripcion";
                case 8:
                    return "Profesor";
                case 9:
                    return "Ayudante";
                case 10:
                    return "Año";
                case 11:
                    return "Periodo";
            }

            return null;
            
        }
        private static void CrearBackUpMaterias(Worksheet worksheet2)
        {
            CrearTitulosBackUpMaterias(worksheet2);

            Materias[] materias = context.Materias.ToArray<Materias>();

            int cantidadMaterias = materias.Length;
            int contMaterias = 0;

            for (int row = 2; row < cantidadMaterias + 2; row++)
            {

                worksheet2.Cells[row, 1] = materias[contMaterias].Categoria;
                worksheet2.Cells[row, 2] = materias[contMaterias].Descripcion;
                worksheet2.Cells[row, 3] = materias[contMaterias].Profesor;
                worksheet2.Cells[row, 4] = materias[contMaterias].Ayudante;
                worksheet2.Cells[row, 5] = materias[contMaterias].Anio;
                worksheet2.Cells[row, 6] = materias[contMaterias].TiempoAnio;

                contMaterias++;

            }
        }
        private static void CrearTitulosBackUpMaterias(Worksheet w)
        {
            for (int i = 1; i < 5; i++)
            {
                w.Cells[1, i] = GetValorTitulo(i+5);
            }
        }
        private static void MostrarUltimoBackUp()
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\matia\source\repos\Sol_Puntaje\PuntajeClases\BackUps\");
            FileInfo[] files = di.GetFiles();
            Console.WriteLine("El ultimo BackUp registrado es: " + ObtenerUltimoBackUp(files).Name);
        }
        private static FileInfo ObtenerUltimoBackUp(FileInfo[] files)
        {
            FileInfo mayor = files[0];

            for(int i = 1; i < files.Length; i++)
            {
                if (files[i].Extension == ".xlsx" && Fecha.EsBackUpMayorFecha(files[i],mayor))
                {
                    mayor = files[i];
                }
            }

            return mayor;
        }
        private static void MostrarTodosBackUps()
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\matia\source\repos\Sol_Puntaje\PuntajeClases\BackUps\");
            FileInfo[] files = di.GetFiles();

            Console.WriteLine("Mostrando los BackUps hechos hasta ahora:");
            int cont = 0;
            foreach(FileInfo f in files) {
                cont++;
                Console.WriteLine(cont+". "+f.Name);
            }
        }
    }
}
