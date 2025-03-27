using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class MusicaExtensions
    {

        // Converte uma lista de entidades para uma lista de respostas
        private static ICollection<MusicaResponse> EntityListToResponseList(IEnumerable<Musica> musicaList)
        {
            return musicaList.Select(a => EntityToResponse(a)).ToList();
        }

        // Converte uma entidade para uma resposta
        private static MusicaResponse EntityToResponse(Musica musica)
        {
            return new MusicaResponse(musica.Id, musica.Nome!, musica.Artista!.Id, musica.Artista.Nome);
        }


        public static void AddEndpointMusicas(this WebApplication app)
        {
            #region Musicas

            // GET
            app.MapGet("/Musicas", ([FromServices] DAL<Musica> dal) =>
            {
                var listaDeMusicas = dal.Listar();
                if (listaDeMusicas is null)
                {
                    return Results.NotFound();
                }

                var listaDeMusicasResponse = EntityListToResponseList(listaDeMusicas);

                return Results.Ok(listaDeMusicasResponse);
            });

            // GET by Nome
            app.MapGet("/Musicas/{nome}", ([FromServices] DAL<Musica> dal, string nome) =>
            {
                var musica = dal.RecuperarPor(m => m.Nome.ToUpper().Equals(nome.ToUpper()));

                if (musica is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(musica);
            });

            // POST
            app.MapPost("/Musicas", ([FromServices] DAL<Musica> dal, [FromBody] MusicaRequest musicaRequest) =>
            {
                Musica musica = new Musica(musicaRequest.nome, musicaRequest.ArtistaId, musicaRequest.anoLancamento);

                dal.Adicionar(musica);

                return Results.Ok();
            });

            // DELETE
            app.MapDelete("/Musicas/{Id}", ([FromServices] DAL<Musica> dal, int id) =>
            {
                var musica = dal.RecuperarPor(m => m.Id == id);

                if (musica is null)
                {
                    return Results.NotFound();
                }

                dal.Deletar(musica);
                return Results.NoContent();
            });

            // PUT
            app.MapPut("/Musicas", ([FromServices] DAL<Musica> dalMusica, [FromServices] DAL<Artista> dalArtista, [FromBody] MusicaRequestEdit musicaRequestEdit) =>
            {
                var musicaRecuperada = dalMusica.RecuperarPor(m => m.Id == musicaRequestEdit.Id);
                if (musicaRecuperada == null)
                {
                    return Results.NotFound();
                }

                var artistaRecuperado = dalArtista.RecuperarPor(a => a.Id == musicaRequestEdit.ArtistaId);
                if (artistaRecuperado == null)
                {
                    return Results.NotFound();
                }

                musicaRecuperada.Nome = musicaRequestEdit.nome;
                musicaRecuperada.AnoLancamento = musicaRequestEdit.anoLancamento;
                musicaRecuperada.Artista = artistaRecuperado;

                dalMusica.Atualizar(musicaRecuperada);
                return Results.Ok();
            });

            #endregion
        }
    }
}
