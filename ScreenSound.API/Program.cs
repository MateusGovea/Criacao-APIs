using Microsoft.AspNetCore.Mvc;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Recebendo o Context e DAL
builder.Services.AddDbContext<ScreenSoundContext>();
builder.Services.AddTransient<DAL<Artista>>();

var app = builder.Build();

// Pagina Inicial
app.MapGet("/", () => "Página Carregada");

// GET
app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
{
    return Results.Ok(dal.Listar());
});

// GET by Nome
app.MapGet("/Artistas/{nome}", ([FromServices] DAL<Artista> dal, string nome) =>
{
    var artista = dal.RecuperarPor(a => a.Nome.ToUpper().Equals(nome.ToUpper()));

    if (artista is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(artista);
});

// POST
app.MapPost("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody]Artista artista) => 
{
    dal.Adicionar(artista);

    return Results.Ok();
});

// DELETE
app.MapDelete("/Artistas/{Id}", ([FromServices] DAL<Artista> dal, int id) =>
{
    var artista = dal.RecuperarPor(a => a.Id == id);

    if (artista is null)
    {
        return Results.NotFound();
    }

    dal.Deletar(artista);
    return Results.NoContent();
});

// UPDATE
app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] Artista artista) =>
{
    var artistaAtualizar = dal.RecuperarPor(a => a.Id == artista.Id);

    if (artistaAtualizar is null)
    {
        return Results.NotFound();
    }

    // Atualizando o Artista do banco
    artistaAtualizar.Nome = artista.Nome;
    artistaAtualizar.Bio = artista.Bio;
    artistaAtualizar.FotoPerfil = artista.FotoPerfil;
    dal.Atualizar(artistaAtualizar);

    return Results.Ok();
});

app.Run();
