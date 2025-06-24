namespace MauiApp1.Models
{
    public class AlimentJour
    {
        public required string Jour { get; set; }
        public List<Aliment> Aliments { get; set; } = new();
        public int Index { get; set; } // Ajout de la propriété Index
    }
}