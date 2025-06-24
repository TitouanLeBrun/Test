namespace MauiApp1.Models
{
    public class AlimentJour
    {
        public required string Jour { get; set; }
        public List<Aliment> Aliments { get; set; } = new();
    }
}