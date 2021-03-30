using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PuntajeClases.Models
{
    public partial class ClasesContext : DbContext
    {
        public ClasesContext()
        {
        }

        public ClasesContext(DbContextOptions<ClasesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clases> Clases { get; set; }
        public virtual DbSet<Materias> Materias { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Data Source=LAPTOP-MATI\SQLEXPRESS;Initial Catalog=Clases;Integrated Security=True;
");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clases>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Categoria)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Comentario)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsFixedLength();

                entity.Property(e => e.DiaClase)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsFixedLength();

                entity.Property(e => e.FueGrabada).HasColumnName("FUE_GRABADA");

                entity.HasOne(d => d.CategoriaNavigation)
                    .WithMany(p => p.Clases)
                    .HasForeignKey(d => d.Categoria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Clases__Categori__4F7CD00D");
            });

            modelBuilder.Entity<Materias>(entity =>
            {
                entity.HasKey(e => e.Categoria)
                    .HasName("PK__Materia__08015F8A635531DB");

                entity.Property(e => e.Categoria)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.Property(e => e.Ayudante).HasMaxLength(50);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(40)
                    .IsFixedLength();

                entity.Property(e => e.Profesor).HasMaxLength(50);

                entity.Property(e => e.TiempoAnio)
                    .HasColumnName("Tiempo_Anio")
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
