using System;
using System.Collections.Generic;

namespace PuntajeClases.Models
{

    public partial class Clases
    {
        static ClasesContext context = new ClasesContext();

        public string DiaClase { get; set; }
        public string Categoria { get; set; }
        public double Puntaje { get; set; }
        public string Comentario { get; set; }
        public int Id { get; set; }
        public bool? FueGrabada { get; set; }

        public virtual Materias CategoriaNavigation { get; set; }

        public string Mostrar()
        {
            setCategoriaNavigation();
            return "Materia :" + CategoriaNavigation.Descripcion + "\nDia clase :" + DiaClase + "\nPuntaje :" + Puntaje 
                +"\nFue Grabada? :" + FueGrabada + "\nComentario  :" + Comentario;

        }
        public void setCategoriaNavigation()
        {
            this.CategoriaNavigation = context.Materias.Find(Categoria);
        }
    }
}
