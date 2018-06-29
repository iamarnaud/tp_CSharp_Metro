using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using MaBibliotheque;
using Microsoft.Maps.MapControl.WPF;
using System.Globalization;
using MetroMVVM.Model;
using MetroMVVM.ViewModel;

namespace MetroMVVM.View
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MetroView : UserControl
    {
        private String Xentre { get; set; } // longitude
        private String Yentre { get; set; } // latitude
        private String Rentre { get; set; } // rayon

        //On déclare une variable du type du ViewModel
        private MetroViewModel _metroViewModel;

        public MetroView()
        {
            InitializeComponent();
            // à l'initialisation de l'app on instancie la variable précédement créée
            _metroViewModel = new MetroViewModel();
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Xentre = Lon.Text; // Lon
            Yentre = Lat.Text; // Lat
            Rentre = Dist.Text;
            
            //on appelle l'openData des transports via le model issu de notre ViewModel (= Lines)
            _metroViewModel.LoadStops(Xentre, Yentre, Rentre);

            //On réinitialise la valeur de base car à cause du "reset" elle a été mise à nul et ne peut plus retourner de valeurs
            ListView.ItemsSource = _metroViewModel.Stops;

            base.DataContext = _metroViewModel;

           /**
             * Boucle sur la méthode StopPin pour afficher les pushpins correspondants aux arrêts sur la map.
             * Stops est la liste que l'on récupère grace à l'instance _metreViewModel créés dans ViewModel
             */
           foreach (MetroModel line in _metroViewModel.Stops)
            {
                StopPin(line.lat, line.lon, line.name);
            }

            // On appelle la fonction qui place le pushpin "YourAreHere" sur la map
            YouAreHere(Yentre, Xentre);
        }

        // Récupération de tous les points de notre liste d'arrêts.
        public void StopPin(Double Lat, Double Lon, String name)
        {
            //Convert the mouse coordinates to a location on the map
            // Création d'un nouveau lieu avec lat et lon
            Location pinLocation = new Location(Lat, Lon);

            // The pushpin to add to the map.
            // Pushpin est la classe définie par Maps
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            // Adds the pushpin to the map.
            Map.Children.Add(pin);
            //Ajout du nom des arrets sur les pins
            ToolTipService.SetToolTip(pin, $"{name}");
        }

        // pin de localisation selon les infos rentrées par l'utilisateur
        public void YouAreHere(String latHere, String lonHere)
        {
            // On converti un double en string et en mettant un . à la place de la ,
            Double latHereConverted = Convert.ToDouble(latHere, new CultureInfo("en-GB"));
            Double lonHereConverted = Convert.ToDouble(lonHere, new CultureInfo("en-GB"));

            //Convert the mouse coordinates to a location on the map
            //Création d'un nouveau lieu avec lat et lon
            Location pinLocation = new Location(latHereConverted, lonHereConverted);

            // The pushpin to add to the map.
            // Pushpin est la classe définie par Maps
            Pushpin pinHere = new Pushpin();
            pinHere.Location = pinLocation;

            //Ajout d'un message sur le pin de géolocalisation
            ToolTipService.SetToolTip(pinHere, "You are here !");

            // Changement de la couleur du pin
            pinHere.Background = new SolidColorBrush(Color.FromRgb(33, 23, 125));

            // Adds the pushpin to the map.
            Map.Children.Add(pinHere);
            // Recentrer la map selon les infos rentrées par l'utilisateur
            Map.Center = pinLocation;
            // Zoomer map sur nouvelle zone
            Map.ZoomLevel = 17;
        }
        
        /*
         * Quand on clic sur Reset, on dit que la listView devient null : remise à 0.
         * Lorsque une nouvelle recherche sera effectuée via button_click l'itemSource de la ListView redeviendra _metroViewModel.Stops
         */
        public void Button_Reset(object sender, RoutedEventArgs e)
        {
            ListView.ItemsSource = null;
            Map.Children.Clear();
        }
    }
}