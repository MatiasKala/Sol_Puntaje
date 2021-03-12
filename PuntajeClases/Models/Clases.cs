﻿using System;
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
        public virtual Materias CategoriaNavigation { get; set; }
        public string Mostrar()
        {
            CategoriaNavigation = context.Materias.Find(Categoria);
            return"Materia :" + CategoriaNavigation.Descripcion + "\nDia clase :" + DiaClase + "\nPuntaje :" + Puntaje + "\nComentario  "+Comentario;
            
        }
    }
}