using System.Text.Json.Serialization;
using System.Threading;

namespace BibliotecaApi.Models
{
    public class Editorial
    {
        public int EditorialId { get; set; }
        public string Nombre { get; set; }

        [JsonIgnore]
        public virtual ICollection<Libro> Libros { get; set; }

    }
}
