using System;
using System.Collections.Generic;

namespace PuntajeClases.Models
{
    public partial class Materias
    {
        public Materias()
        {
            Clases = new HashSet<Clases>();
        }

        public string Categoria { get; set; }
        public string Descripcion { get; set; }
        public string Profesor { get; set; }
        public string Ayudante { get; set; }
        public int Anio { get; set; }
        public string TiempoAnio { get; set; }

        public virtual ICollection<Clases> Clases { get; set; }
    }
}
