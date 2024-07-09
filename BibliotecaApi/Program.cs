using BibliotecaApi;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

//Agregando la configuracion para una base de datos en memoria
//builder.Services.AddDbContext<LibrosContext>(p => p.UseInMemoryDatabase("LibrosDB"));

//Agregar la configuracion para una base de datos en SQLServer
builder.Services.AddSqlServer<LibrosContext>(builder.Configuration.GetConnectionString("cnLibros"));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

//Agregando una nueva ruta
app.MapGet("/dbconexion", async ([FromServices] LibrosContext dbContext) =>
{ dbContext.Database.EnsureCreated(); return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory()); });


//***************************************************************************************************CRUD AUTORES**************

//Endpoint CONSULTAR
app.MapGet("/api/autores", async ([FromServices] LibrosContext dbContext) =>
{
    return Results.Ok(dbContext.Autores);
});

//Endpoint INSERTAR 
app.MapPost("/api/autores/nuevo", async ([FromServices] LibrosContext dbContext, [FromBody] Autor autor) =>
{
    await dbContext.AddAsync(autor);
    //Guardamos los cambios
    await dbContext.SaveChangesAsync();
    return Results.Ok($"El autor: {autor.Nombre}, se registro correctamente");
});

//Endpoint MODIFICAR
app.MapPut("/api/autores/modificar/{id}", async ([FromServices] LibrosContext dbContext, [FromBody] Autor autor, [FromRoute] int id) =>
{
    //Buscamos la tarea actual con base al id
    var autorActual = await dbContext.Autores.FindAsync(id);

    if (autorActual != null)
    {
        autorActual.Nombre = autor.Nombre;
        autorActual.Pais = autor.Pais;
        await dbContext.SaveChangesAsync();

        return Results.Ok($"El autor: {autorActual.Nombre}, se modificó exitosamente");
    }
    return Results.NotFound();
});

//Endpoint ELIMINAR
app.MapDelete("/api/autores/eliminar/{id}", async ([FromServices] LibrosContext dbContext, [FromRoute] int id) =>
{
    //Buscamos la tarea actual con base al id
    var autorActual = await dbContext.Autores.FindAsync(id);

    if (autorActual != null)
    {
        //Si la tarea existe la eliminamos de la BD.
        dbContext.Remove(autorActual);
        await dbContext.SaveChangesAsync();

        return Results.Ok($"El autor: {autorActual.Nombre}, se eliminó exitosamente");
    }
    return Results.NotFound();
});

//Pruebas Postman
/*
    {
        "nombre": "John Grisham",
        "pais": "Mexico"
    }
 */

//*************************************************************************************************** GET LIBROS **************
//Endpoint CONSULTAR
app.MapGet("/api/libros", async ([FromServices] LibrosContext dbContext) =>
{
    var libros = await dbContext.Libros
        .Include(l => l.Editorial) 
        .Include(l => l.LibrosAutores) 
        .ThenInclude(la => la.Autor) 
        .ToListAsync();

    return Results.Ok(libros);
});

//*************************************************************************************************** POST LIBROS & AUTORES***

app.MapPost("/api/libros/nuevo", async ([FromServices] LibrosContext dbContext, [FromBody] Libro libro) =>
{

    var nuevoLibro = new Libro
    {
        Titulo = libro.Titulo,
        NumPaginas = libro.NumPaginas,
        FechaPublicacion = libro.FechaPublicacion,
        Edicion = libro.Edicion,
        Precio = libro.Precio,
        EditorialId = libro.EditorialId,
        AutoresIdsString = libro.AutoresIdsString,
        LibrosAutores = new List<LibroAutor>()
    };

    var autoresIdsList = nuevoLibro.AutoresIdsString.Split(",").Select(int.Parse).ToList();

    foreach (var autorId in autoresIdsList)
    {
        var autorActual = await dbContext.Autores.FindAsync(autorId);
        if (autorActual == null)
        {
            return Results.Ok($"El autor: {autorActual}, no existe");
        }
        else 
        {
            nuevoLibro.LibrosAutores.Add(new LibroAutor
            {
                LibroId = nuevoLibro.LibroId,
                AutorId = autorId
            });
        }

        
    }
    await dbContext.AddAsync(nuevoLibro);
    await dbContext.SaveChangesAsync();

    // Respuesta
    //var autoresIdsString = string.Join(", ", autoresIdsList);
    return Results.Ok($"El LIBRO: {nuevoLibro.Titulo}, se registro correctamente");
});
/* Pruebas postman
 {
    "Titulo": "Título del libro",
    "NumPaginas": 300,
    "FechaPublicacion": "2024-06-12",
    "Edicion": "Primera edición",
    "Precio": 29.99,
    "EditorialId": 1,
    "AutoresIdsString": "1,2"
}
 */

//*************************************************************************************************** PUT LIBROS *********************
app.MapPut("/api/libros/mod/{id}", async ([FromServices] LibrosContext dbContext, [FromBody] Libro libro, [FromRoute] int id) =>
{
    //Buscamos la tarea actual con base al id
    var libroActual = await dbContext.Libros.FindAsync(id);

    if (libroActual != null)
    {
        libroActual.Titulo = libro.Titulo;
        libroActual.NumPaginas = libro.NumPaginas;
        libroActual.FechaPublicacion = libro.FechaPublicacion;
        libroActual.Edicion = libro.Edicion;
        libroActual.Precio = libro.Precio;

        await dbContext.SaveChangesAsync();

        return Results.Ok($"El libro: {libroActual.Titulo}, se modificó exitosamente");
    }
    return Results.NotFound();
});

// lenguaje liq (TAREA) -------------------------------------------------------------------------------------------------------

// 1. Listado de todos los libros incluyendo en nombre de la editorial y ordenados por titulo en orden descendente.
app.MapGet("/api/listadoLibrosCompleto", async ([FromServices] LibrosContext dbContext) =>
{
    var libros = await dbContext.Libros
        .Include(l => l.Editorial)  
        .OrderByDescending(l => l.Titulo) 
        .Select(l => new
        {
            TituloLibro = l.Titulo,
            NumeroPaginas = l.NumPaginas,
            FechaPublicacion = l.FechaPublicacion,
            Edicion = l.Edicion,
            Precio = l.Precio,
            NombreEditorial = l.Editorial.Nombre  
        })
        .ToListAsync();

    return Results.Ok(libros);
});

// 2. Listado de todos los libros incluyendo los datos de cada uno de su(s} autor(es).
app.MapGet("/api/listadoLibrosConAutores", async ([FromServices] LibrosContext dbContext) =>
{
    var librosConAutores = await dbContext.Libros
        .Include(l => l.Editorial)  
        .Include(l => l.LibrosAutores)  
        .ThenInclude(la => la.Autor)  
        .OrderByDescending(l => l.Titulo) 
        .Select(l => new
        {
            TituloLibro = l.Titulo,
            NombreEditorial = l.Editorial.Nombre,
            NumeroPaginas = l.NumPaginas,
            FechaPublicacion = l.FechaPublicacion,
            Edicion = l.Edicion,
            Precio = l.Precio,
            Autores = l.LibrosAutores.Select(la => new
            {
                NombreAutor = la.Autor.Nombre,
                PaisAutor = la.Autor.Pais
            }).ToList()
        })
        .ToListAsync();

    return Results.Ok(librosConAutores);
});

// 3. Listado de todos los autores incluyendo los datos de cada unos de los libros que han escrito.
app.MapGet("/api/listadoAutoresConLibros", async ([FromServices] LibrosContext dbContext) =>
{
    var autoresConLibros = await dbContext.Autores
        .Include(a => a.LibrosAutores)  
        .ThenInclude(la => la.Libro)    
        .OrderBy(a => a.Nombre)         
        .Select(a => new
        {
            NombreAutor = a.Nombre,
            Pais = a.Pais,
            LibrosEscritos = a.LibrosAutores.Select(la => new
            {
                TituloLibro = la.Libro.Titulo,
                NumeroPaginas = la.Libro.NumPaginas,
                FechaPublicacion = la.Libro.FechaPublicacion,
                Edicion = la.Libro.Edicion,
                Precio = la.Libro.Precio,
                Editorial = la.Libro.Editorial.Nombre
                
            }).ToList()
        })
        .ToListAsync();

    return Results.Ok(autoresConLibros);
});


// 4. Listado que muestre cuantos libros hay de cada editorial. (Group By)
app.MapGet("/api/cantidadLibrosPorEditorial", async ([FromServices] LibrosContext dbContext) =>
{
    var cantidadLibrosPorEditorial = await dbContext.Libros
        .Include(l => l.Editorial)  
        .GroupBy(l => l.Editorial.Nombre)  
        .Select(g => new
        {
            Editorial = g.Key,
            CantidadLibros = g.Count()
        })
        .ToListAsync();

    return Results.Ok(cantidadLibrosPorEditorial);
});

// 5. Los dos libros más costosos y a que autor pertenecen
app.MapGet("/api/librosConCostoMasAlto", async ([FromServices] LibrosContext dbContext) =>
{
    var librosConCostoMasAlto = await dbContext.Libros
        .Include(l => l.LibrosAutores)
        .ThenInclude(la => la.Autor)
        .OrderByDescending(l => l.Precio)
        .Take(2)
        .Select(l => new
        {
            TituloLibro = l.Titulo,
            Precio = l.Precio,
            Autores = l.LibrosAutores.Select(la => la.Autor.Nombre).ToList()
        })
        .ToListAsync();

    return Results.Ok(librosConCostoMasAlto);
});


app.Run();
