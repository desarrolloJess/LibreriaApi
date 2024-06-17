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
app.MapPut("/api/autores/mod/{id}", async ([FromServices] LibrosContext dbContext, [FromBody] Autor autor, [FromRoute] int id) =>
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
app.MapDelete("/api/autores/elim/{id}", async ([FromServices] LibrosContext dbContext, [FromRoute] int id) =>
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
        nuevoLibro.LibrosAutores.Add(new LibroAutor
        {
            LibroId = nuevoLibro.LibroId,
            AutorId = autorId
        });
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



app.Run();
