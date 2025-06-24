namespace MauiApp1.Models
{
    public class Aliment
    {
        public required string Nom { get; set; }
        public List<Nutriment> Nutriments { get; set; } = new();

    }
}