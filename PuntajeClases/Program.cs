
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
                    Console.WriteLine("4. Generar BackUp de datos");
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
                            CrearBackUp();
                            break;
                    }

                }
                catch (Exception e)
                {
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

            c.DiaClase = Fecha.IngresarDia("Que dia es hoy?");
            c.Categoria = IngresarCategoriaCargarClase("Que tuviste hoy?");
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
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("\n");
            int rta = IngresoRespuesta(1, 14,99,100);
            int mes;
            int cuatri;

            try
            {
                switch (rta)
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
        private static void MostrarTodosLosPromedios()
        {

            List<Materias> todasLasMaterias = context.Materias.ToList();

            List<Materias> materiasSinCargar = new List<Materias>();

            Dictionary<double, string> promedios = new Dictionary<double, string>();

            foreach (Materias m in todasLasMaterias)
            {
                double prom = ObtenerPromedio(ObtenerClasesPorCateg(m.Categoria));
                if (m.Clases.Count == 0)
                {
                    materiasSinCargar.Add(m);
                }
                else
                {
                    promedios.Add(prom, m.Descripcion);
                }

            }

            double[] promValores =promedios.Keys.ToArray();
            
            OrdenarPromedios(promValores);

            Console.WriteLine("Mostrando promedios por materia de mayor a menor\n\n");

            for (int i = promValores.Length-1; i >0 - 1; i--)
            {
                string value;
                promedios.TryGetValue(promValores[i], out value);
                Console.WriteLine("El promedio de la materia " + value + "es de "+promValores[i]+"\n");
            }

            Console.WriteLine("Estas materias todavia no comenzaron o no tienen ninguna clase cargada\n\n");

            foreach (Materias m in materiasSinCargar)
            {
                Console.WriteLine(m.Descripcion+"\n");
            }

        }
        private static void OrdenarPromedios(double[] promValores)
        {

            double aux;

            for (int i = 0; i < promValores.Length-1; i++)
            {
                for (int j = 0; j < promValores.Length-1; j++)
                {
                    if (promValores[j] > promValores[j + 1])
                    {
                        aux=promValores[j];
                        promValores[j] = promValores[j + 1];
                        promValores[j + 1] = aux;
                    }
                }
            }

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

                Console.WriteLine("\n La mejor clase de la materia " + categ + " es:\n");

                foreach (Clases c in mejorClaseDeCateg)
                {
                    Console.WriteLine(c.Mostrar());
                }

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

            if (clasesDelMes.Count != 0)
            {

                double promedio = ObtenerPromedio(clasesDelMes);

                Console.WriteLine("\nEl promedio de puntaje en las clases del mes numero " + month + " es de " + promedio + "\n");

            }
            else
            {
                Console.WriteLine("No hay clases del mes " + month + " del " + year);
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

            if (clasesDeMes.Count != 0)
            {

                List<Clases> mejorClase = ObtenerMejorClase(clasesDeMes);

                Console.WriteLine("\nMostrando la/s clase/s con mejor puntaje del mes numero " + month + "\n");

                foreach (Clases c in mejorClase)
                {

                    Console.WriteLine(c.Mostrar() + "\n");

                }

            }
            else
            {
                Console.WriteLine("No hay clases del mes " + month + " del " + year);

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

                Console.WriteLine("\nMostrando la/s clase/s con mejor puntaje del año " + year + "\n");

                List<Clases> mejorClase = ObtenerMejorClase(clasesDelAño);

                foreach (Clases c in mejorClase)
                {

                    Console.WriteLine(c.Mostrar() + "\n");

                }

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

                Console.WriteLine("Esta/s son la/s clases con mejor puntuacion:\n");

                foreach (Clases c in mejorClase)
                {
                    Console.WriteLine(c.Mostrar() + "\n");
                }
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

            if (clasesDelMes.Count != 0)
            {

                Console.WriteLine("\nMostrando clases del mes numero " + month + "\n");

                foreach (Clases c in clasesDelMes)
                {
                    Console.WriteLine(c.Mostrar() + "\n");
                }

            }
            else
            {
                Console.WriteLine("No hay clases del mes " + month + " del " + year);
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
                if (m.Ayudante != null)
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

            OrdenarProfesores(materias);

            for(int i = 0; i < materias.Length; i++)
            {
                if (materias[i] != null)
                {
                    aux=BuscarOtraMateriaMismoProfe(materias[i],materias);

                    foreach(Materias m in aux)
                    {
                        if (m.Profesor != null)
                        {
                            Console.WriteLine("Profesor: " + m.Profesor + " Materia: " + m.Descripcion);
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
        private static void OrdenarProfesores(Materias[] materias)
        {

            Materias aux;

            for (int i = 0; i < materias.Length - 1; i++)
            {
                for (int j = 0; j < materias.Length - 1; j++)
                {
                    if (materias[i].Profesor!=null & materias[j].Profesor!=null && materias[j].Profesor.CompareTo(materias[j + 1].Profesor) > 0)
                    {
                        aux = materias[j];
                        materias[j] = materias[j + 1];
                        materias[j + 1] = aux;
                    }
                }
            }

        }
        private static Clases[] ObtenerClasesOrdenadas()
        {
            int cantidad = context.Clases.Count()-1;

            Clases[] clases = context.Clases.ToArray();

            Clases aux;

            for (int i = 0; i < cantidad; i++)
            {
                for (int j = 0; j < cantidad; j++)
                {
                    if (EsFechaMayor(clases[j], clases[j + 1]))
                    {
                        aux = clases[j];
                        clases[j] = clases[j + 1];
                        clases[j + 1] = aux;
                    }
                }
            }

            return clases;
        }
        private static bool EsFechaMayor(Clases clases1, Clases clases2)
        {
            bool ok = false;

            char[] fecha1 = clases1.DiaClase.ToCharArray();
            char[] fecha2 = clases2.DiaClase.ToCharArray();

            int anio1 = ConseguirNumeroPorArray(fecha1,6,7);
            int anio2 = ConseguirNumeroPorArray(fecha2,6,7);


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

            int eleccion = IngresoRespuesta(0, 6,"Que modificamos de "+m.Descripcion+"?\n" +
                "1.Nombre de la materia\n" +
                "2.Abreviatura\n" +
                "3.Año\n" +
                "4.Epoca del año\n" +
                "5.Profesor\n" +
                "6.Ayudante");

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
                }
                Console.WriteLine("Desea cambiar algo mas?\n0. Salir\n" +
                "1.Nombre de la materia\n" +
                "2.Abreviatura\n" +
                "3.Año\n" +
                "4.Epoca del año\n" +
                "5.Profesor\n" +
                "6.Ayudante");
                eleccion = IngresoRespuesta(0, 6);
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
        private static string IngresarCategoriaCargarClase(string v)
        {
            string retorno;

            string categPorDia = ObtenerCategoriaPorDiaDeSemana();

            if(IngresoRespuesta(0,1,"Tuviste hoy " + categPorDia + "?\n0.NO 1.SI")==1)
            {
                retorno = categPorDia;
            }
            else
            {
                retorno=IngresarCategoria(v);
            }

            return retorno;

        }
        private static string ObtenerCategoriaPorDiaDeSemana()
        {
            string retorno = null;

            if (HoyEsDiaDeClases())
            {
                retorno=DiccionarioMateriaPorDia.CategoriaPorDia();
            }

            return retorno;

        }
        private static bool HoyEsDiaDeClases()
        {
            DayOfWeek hoy = DateTime.Today.DayOfWeek;
            return hoy == DayOfWeek.Monday || hoy == DayOfWeek.Tuesday ||
                   hoy == DayOfWeek.Wednesday || hoy == DayOfWeek.Thursday;
        }
        private static void OpcionSecreta()
        {
            Console.WriteLine("Ah jajajja la opcion secreta? Toca lo que sea");
            Console.ReadLine();
            Clases[] todasLasClases = ObtenerClasesOrdenadas();

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
                    if (IngresoRespuesta(1, 2, "Que queres hacer con esta clase? Borrar o Editar?. Ingresa 1 para borrar 2 para editar") == 1)
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
                        clase.DiaClase = Fecha.IngresarDia("Ingrese el nuevo dia de la clase");
                        break;
                    case 2:
                        clase.Categoria = IngresarCategoria("Ingrese la abreviatura de la materia (Ej:TP1)");
                        break;
                    case 3:
                        clase.Puntaje = IngresarPuntaje("Ingresa el nuevo puntaje de la clase (1 al 10)");
                        break;
                    case 4:
                        clase.FueGrabada = IngresoRespuesta(0, 1, "Se grabo la clase? 0 para NO, 1 para SI") == 0 ? false : true;
                        break; 
                    case 5:
                        clase.Comentario = Ingresar("Modifique el comentario");
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

        //----------------------------------------------------------
        // BACK UP   
        //----------------------------------------------------------
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

            Clases[] todasLasClases = ObtenerClasesOrdenadas();

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
    }
}
