using System.Globalization;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            // Autorise uniquement chiffres, point et virgule
            if (!double.TryParse(entry.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out _))
            {
                // Annule la saisie invalide
                entry.Text = e.OldTextValue;
            }
        }
    }
}
