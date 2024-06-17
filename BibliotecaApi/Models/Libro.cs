using System.ComponentModel.DataAnnotations.Schema;

namespace BibliotecaApi.Models
{
    public class Libro
    {
        public int LibroId { get; set; }
        public string Titulo { get; set; }
        public int NumPaginas { get; set; }
        public DateOnly FechaPublicacion { get; set; }
        public string Edicion { get; set; }
        public Double Precio { get; set;}

        public string AutoresIdsString { get; set; }

        [ForeignKey("EditorialId")]
        public int EditorialId { get; set;}
        public virtual Editorial Editorial { get; set; }

        public ICollection<LibroAutor> LibrosAutores { get; set; }


    }
}
