using System.Text.Json.Serialization;

namespace BibliotecaApi.Models
{
    public class Autor
    {
        public int AutorId { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        [JsonIgnore]
        public ICollection<LibroAutor> LibrosAutores { get; set; }
    }
}
