using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using MauiApp1.Models;
using System.Linq;
using Force.DeepCloner;

namespace MauiApp1.ViewModels
{
    public class AlimentsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Aliment> Aliments { get; set; } = new();
        private List<Aliment> _alimentsDerniereValidation = new();

        private bool _canClickOnThatButton = true;
        public bool CanClickOnThatButton
        {
            get => _canClickOnThatButton;
            set
            {
                if (_canClickOnThatButton != value)
                {
                    _canClickOnThatButton = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanClickOnThatButton)));
                }
            }
        }

        public ICommand ValiderCommand { get; }
        public ICommand ReinitialiserCommand { get; }

        public AlimentsViewModel()
        {
            var aliment1 = new Aliment
            {
                Nom = "Pomme",
                Nutriments = new List<Nutriment>
                {
                    new Nutriment { Nom = "Vitamine C", Valeur = 5.0 },
                    new Nutriment { Nom = "Fibre", Valeur = 2.0 }
                }
            };
            var aliment2 = new Aliment
            {
                Nom = "Banane",
                Nutriments = new List<Nutriment>
                {
                    new Nutriment { Nom = "Potassium", Valeur = 10.0 }
                }
            };
            Aliments.Add(aliment1);
            Aliments.Add(aliment2);

            foreach (var aliment in Aliments)
            {
                foreach (var nutriment in aliment.Nutriments)
                {
                    nutriment.ValeurAffichageChanged += Nutriment_ValeurAffichageChanged;
                }
            }

            // Sauvegarde initiale
            MemoriserValidation();

            ValiderCommand = new Command(OnValider);
            ReinitialiserCommand = new Command(OnReinitialiser);
        }

        private void Nutriment_ValeurAffichageChanged(object? sender, EventArgs e)
        {
            CanClickOnThatButton = false;
        }

        private void OnValider()
        {
            MemoriserValidation();
            CanClickOnThatButton = true;
        }

        private void OnReinitialiser()
        {
            Aliments.Clear();
            foreach (var aliment in _alimentsDerniereValidation.DeepClone())
            {
                foreach (var nutriment in aliment.Nutriments)
                    nutriment.ValeurAffichageChanged += Nutriment_ValeurAffichageChanged;
                Aliments.Add(aliment);
            }
            CanClickOnThatButton = true;
        }

        private void MemoriserValidation()
        {
            _alimentsDerniereValidation = Aliments.DeepClone().ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}