using Microsoft.AspNetCore.Mvc;
using ScreenSound.API.Requests;
using ScreenSound.Banco;
using ScreenSound.Modelos;

namespace ScreenSound.API.Endpoints
{
    public static class ArtistaExtensions
    {

        // Métodos de extensão para converter entidades em respostas e vice-versa
        private static ICollection<ArtistaResponse> EntityListToResponseList(IEnumerable<Artista> listaDeArtistas)
        {
            return listaDeArtistas.Select(a => EntityToResponse(a)).ToList();
        }


        // Converte uma entidade para uma resposta
        private static ArtistaResponse EntityToResponse(Artista artista)
        {
            return new ArtistaResponse(artista.Id, artista.Nome, artista.Bio, artista.FotoPerfil);
        }

        public static void AddEndpointArtistas(this WebApplication app)
        {
            #region Artistas

            // GET
            app.MapGet("/Artistas", ([FromServices] DAL<Artista> dal) =>
            {
                var listaDeArtistas = dal.Listar();
                if (listaDeArtistas is null)
                {
                    return Results.NotFound();
                }

                var listaDeArtistasResponse = EntityListToResponseList(listaDeArtistas);

                return Results.Ok(listaDeArtistasResponse);
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
            app.MapPost("/Artistas", ([FromServices]DAL<Artista> dal, [FromBody]ArtistaRequest artistaRequest) =>
            {
                Artista artista = new Artista(artistaRequest.nome, artistaRequest.bio);

                dal.Adicionar(artista);

                return Results.Ok();
            });

            // DELETE
            app.MapDelete("/Artistas/{Id}", ([FromServices]DAL<Artista> dal, int id) =>
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
            app.MapPut("/Artistas", ([FromServices] DAL<Artista> dal, [FromBody] ArtistaRequestEdit artistaRequestEdit) =>
            {
                var artistaAtualizar = dal.RecuperarPor(a => a.Id == artistaRequestEdit.Id);

                if (artistaAtualizar is null)
                {
                    return Results.NotFound();
                }

                // Atualizando o Artista do banco
                artistaAtualizar.Nome = artistaRequestEdit.nome;
                artistaAtualizar.Bio = artistaRequestEdit.bio;
                dal.Atualizar(artistaAtualizar);

                return Results.Ok();
            });

            #endregion
        }
    }
}
