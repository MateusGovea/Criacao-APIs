namespace ScreenSound.Modelos;

public class Musica
{
    public Musica(string nome, int artistaId, int? AnoLancamento)
    {
        Nome = nome;
        if (Artista is not null)
        {
            Artista.Id = artistaId;
        }
        this.AnoLancamento = AnoLancamento;
    }

    public string Nome { get; set; }
    public int Id { get; set; }
    public int? AnoLancamento { get; set; }
    public virtual Artista? Artista { get; set; }

    public void ExibirFichaTecnica()
    {
        Console.WriteLine($"Nome: {Nome}");
      
    }

    public override string ToString()
    {
        return @$"Id: {Id}
        Nome: {Nome}";
    }
}