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

namespace MetroMVVM
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private String Xentre { get; set; } // longitude
        private String Yentre { get; set; } // latitude
        private String Rentre { get; set; } // rayon

        public void Button_Click(object sender, RoutedEventArgs e)

        {
            Xentre = Lon.Text; // Lon
            Yentre = Lat.Text; // Lat
            Rentre = Dist.Text;

            try
            {
                AppelApi metroMvvmApi = new AppelApi("http://data.metromobilite.fr/api/linesNear/json?x=" + Xentre + "&y=" + Yentre + "&dist=" + Rentre + "&details=true");

                List<Lines> lines = JsonConvert.DeserializeObject<List<Lines>>(metroMvvmApi.responseFromServer);
                List<Lines> lineSansDoublons = lines.GroupBy(n => n.name).Select(x => x.First()).ToList();

                /* itération ligne par ligne pour récupérer infos dans Lines
                * soit lines soit lineSansDoublons
                * et ajout des pins sur la carte
                */

                YouAreHere(Yentre, Xentre);

                foreach (Lines line in lineSansDoublons)
                {
                    Result.Items.Add(line.name);
                    StopPin(line.lat, line.lon, line.name);
                }
            }
            catch (Exception ex)
            {
                Result.Items.Add("Numbers Only. Press reset to try again!");
                Console.WriteLine(ex.GetType().FullName);
            }
        }

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

        public void Button_Reset(object sender, RoutedEventArgs e)
        {
            Result.Items.Clear();
            Map.Children.Clear();
        }
    }
}