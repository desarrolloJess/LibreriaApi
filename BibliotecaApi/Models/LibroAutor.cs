using System.Text.Json.Serialization;

namespace BibliotecaApi.Models
{
    public class LibroAutor
    {
        public int LibroId { get; set; }
        [JsonIgnore]
        public virtual Libro Libro { get; set; }
        public int AutorId { get; set; }
        
        public virtual Autor Autor{ get; set; }
    }
}
