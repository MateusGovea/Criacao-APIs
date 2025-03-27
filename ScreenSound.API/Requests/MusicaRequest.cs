using System.ComponentModel.DataAnnotations;

namespace ScreenSound.API.Requests
{
    public record MusicaRequest([Required] string nome, [Required] int ArtistaId, int anoLancamento);

    public record MusicaRequestEdit(int Id, string nome, int ArtistaId, int anoLancamento) : MusicaRequest(nome, ArtistaId, anoLancamento);

    public record MusicaResponse(int Id, string Nome, int ArtistaId, string NomeArtista);
}
