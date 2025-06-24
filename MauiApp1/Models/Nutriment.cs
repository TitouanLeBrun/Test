using System.ComponentModel;
using System.Globalization;

namespace MauiApp1.Models
{
    public class Nutriment : INotifyPropertyChanged
    {
        public Nutriment()
        {
            _valeur = 0.0;
            _valeurAffichage = _valeur.ToString("0.##", CultureInfo.InvariantCulture);
        }
        private double _valeur;
        private string _valeurAffichage;

        public required string Nom { get; set; }

        public double Valeur
        {
            get => _valeur;
            set
            {
                if (_valeur != value)
                {
                    _valeur = value;
                    _valeurAffichage = _valeur.ToString("0.##", CultureInfo.InvariantCulture);
                    OnPropertyChanged(nameof(Valeur));
                    OnPropertyChanged(nameof(ValeurAffichage));
                }
            }
        }

        public string ValeurAffichage
        {
            get => _valeurAffichage;
            set
            {
                if (_valeurAffichage != value)
                {
                    // Tente de parser la saisie utilisateur
                    if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var d))
                    {
                        Valeur = d; // Cela mettra à jour _valeurAffichage avec le bon format
                        ValeurAffichageChanged?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        // Si la saisie n'est pas valide, on ne modifie pas la valeur réelle,
                        // mais on garde la saisie pour permettre la correction
                        _valeurAffichage = value;
                        OnPropertyChanged(nameof(ValeurAffichage));
                        ValeurAffichageChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? ValeurAffichageChanged;

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}