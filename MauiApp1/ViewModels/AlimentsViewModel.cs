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
        public ObservableCollection<AlimentJour> AlimentsParJour { get; set; } = new();
        private List<AlimentJour> _alimentsDerniereValidation = new();

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

        // Ajoutez cette propriété pour exposer la liste des noms de nutriments (pour l'en-tête)
        public List<string> NomsNutriments { get; set; } = new();

        public ICommand ValiderCommand { get; }
        public ICommand ReinitialiserCommand { get; }
        public ICommand AjouterJourCommand { get; }

        public AlimentsViewModel()
        {
            var nutrimentsReference = new List<Nutriment>
            {
                new Nutriment { Nom = "Vitamine C", Valeur = 0.0 },
                new Nutriment { Nom = "Fibre", Valeur = 0.0 },
                new Nutriment { Nom = "Potassium", Valeur = 0.0 }
            };

            // Exemple de jours
            var jours = new[] { "Lundi", "Mardi", "Mercredi" };

            foreach (var jour in jours)
            {
                var aliments = new List<Aliment>
                {
                    new Aliment
                    {
                        Nom = "Pomme",
                        Nutriments = nutrimentsReference
                            .Select(n => new Nutriment { Nom = n.Nom, Valeur = n.Nom == "Vitamine C" ? 5.0 : n.Nom == "Fibre" ? 2.0 : 0.0 })
                            .ToList()
                    },
                    new Aliment
                    {
                        Nom = "Banane",
                        Nutriments = nutrimentsReference
                            .Select(n => new Nutriment { Nom = n.Nom, Valeur = n.Nom == "Potassium" ? 10.0 : 0.0 })
                            .ToList()
                    }
                };

                foreach (var aliment in aliments)
                {
                    foreach (var nutriment in aliment.Nutriments)
                    {
                        nutriment.ValeurAffichageChanged += Nutriment_ValeurAffichageChanged;
                    }
                }

                AlimentsParJour.Add(new AlimentJour { Jour = jour, Aliments = aliments });
            }

            MemoriserValidation();

            ValiderCommand = new Command(OnValider);
            ReinitialiserCommand = new Command(OnReinitialiser);
            AjouterJourCommand = new Command(AjouterJour);
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
            AlimentsParJour.Clear();
            foreach (var alimentJour in _alimentsDerniereValidation.DeepClone())
            {
                foreach (var aliment in alimentJour.Aliments)
                {
                    foreach (var nutriment in aliment.Nutriments)
                        nutriment.ValeurAffichageChanged += Nutriment_ValeurAffichageChanged;
                }
                AlimentsParJour.Add(alimentJour);
            }
            CanClickOnThatButton = true;
        }

        private void MemoriserValidation()
        {
            _alimentsDerniereValidation = AlimentsParJour.DeepClone().ToList();
        }

        private void AjouterJour()
        {
            var nouveauJour = $"Jour {AlimentsParJour.Count + 1}";
            var reference = AlimentsParJour[0].Aliments.Select(a => a.Nom).ToList();
            var nutrimentsReference = AlimentsParJour[0].Aliments[0].Nutriments.Select(n => n.Nom).ToList();

            var nouveauxAliments = reference.Select(nom => new Aliment
            {
                Nom = nom,
                Nutriments = nutrimentsReference.Select(nomNutriment => new Nutriment { Nom = nomNutriment, Valeur = 0.0 }).ToList()
            }).ToList();

            foreach (var aliment in nouveauxAliments)
                foreach (var nutriment in aliment.Nutriments)
                    nutriment.ValeurAffichageChanged += Nutriment_ValeurAffichageChanged;

            AlimentsParJour.Add(new AlimentJour { Jour = nouveauJour, Aliments = nouveauxAliments });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}