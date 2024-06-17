using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BibliotecaApi
{
    public class LibrosContext : DbContext
    {
        public LibrosContext(DbContextOptions<LibrosContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>(libro =>
            {
                //Especificando nombre de la tabla
                libro.ToTable("libro");
                //Llave primaria
                libro.HasKey(p => p.LibroId);
                libro.Property(p => p.LibroId).ValueGeneratedOnAdd();
                libro.Property(p => p.Titulo).IsRequired().HasMaxLength(150);
                libro.Property(p => p.NumPaginas).IsRequired();
                libro.Property(p => p.FechaPublicacion).IsRequired();
                libro.Property(p => p.Precio);
                libro.Property(p => p.Edicion);
                libro.HasOne(p => p.Editorial).WithMany(p => p.Libros).HasForeignKey(p => p.EditorialId);

                libro.Ignore(p => p.AutoresIdsString);
            });

            List<Editorial> EditorialInit = new List<Editorial>();
            EditorialInit.Add(new Editorial
            {
                EditorialId = 1,
                Nombre = "Oxford University Press"
            });
            EditorialInit.Add(new Editorial
            {
                EditorialId = 2,
                Nombre = "Pearson Education"
            });

            modelBuilder.Entity<Editorial>(editorial =>
            {
                //Especificando nombre de la tabla
                editorial.ToTable("editorial");
                //Llave primaria
                editorial.HasKey(p => p.EditorialId);
                editorial.Property(p => p.EditorialId).ValueGeneratedOnAdd();
                editorial.Property(p => p.Nombre).IsRequired().HasMaxLength(150);

                editorial.HasData(EditorialInit);
            });

            List<Autor> AutorInit = new List<Autor>();
            AutorInit.Add(new Autor
            {
                AutorId = 1,
                Nombre = "J.K. Rowling",
                Pais = "Mexico"
            });
            AutorInit.Add(new Autor
            {
                AutorId = 2,
                Nombre = "Stephen King",
                Pais = "Mexico"
            });

            modelBuilder.Entity<Autor>(autor =>
            {
                //Especificando nombre de la tabla
                autor.ToTable("autor");
                //Llave primaria
                autor.HasKey(p => p.AutorId);
                autor.Property(p => p.AutorId).ValueGeneratedOnAdd();
                autor.Property(p => p.Nombre).IsRequired().HasMaxLength(150);
                autor.Property(p => p.Pais).IsRequired();

                autor.HasData(AutorInit);
            });

            modelBuilder.Entity<LibroAutor>(libroAutor =>
            {
                //Especificando nombre de la tabla
                libroAutor.ToTable("libroAutor");
                //Llave primaria
                libroAutor.HasKey(p => new { p.LibroId, p.AutorId });

                libroAutor.HasOne(p => p.Libro).WithMany(p => p.LibrosAutores).HasForeignKey(p => p.LibroId);
                libroAutor.HasOne(p => p.Autor).WithMany(p => p.LibrosAutores).HasForeignKey(p => p.AutorId);
            });

        }


        //Coleccion de datps para Categoria
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<LibroAutor> LibrosAutores { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }

    }
}
