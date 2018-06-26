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

        private String Xentré { get; set; } // longitude
        private String Yentré { get; set; } // latitude
        private String Rentré { get; set; } // rayon

        public void Button_Click(object sender, RoutedEventArgs e)

        {
            Xentré = Lon.Text; // Lat
            Yentré = Lat.Text; // Lon
            Rentré = Dist.Text; 

            try
            {
                AppelApi metroMvvmApi = new AppelApi("http://data.metromobilite.fr/api/linesNear/json?x=" + Xentré + "&y=" + Yentré + "&dist=" + Rentré + "&details=true");

                List<Lines> lines = JsonConvert.DeserializeObject<List<Lines>>(metroMvvmApi.responseFromServer);
                List<Lines> lineSansDoublons = lines.GroupBy(n => n.name).Select(x => x.First()).ToList();


                /* itération ligne par ligne pour récupérer infos dans Lines
                * soit lines soit lineSansDoublons
                * et ajout des pins sur la carte
                */

                foreach (Lines line in lineSansDoublons)
                {
                    Result.Items.Add(line.name);
                    StopPin(line.lat, line.lon);
                }
            }
            catch (Exception ex)
            {
                Result.Items.Add("Numbers Only. Press reset to try again!");
                Console.WriteLine(ex.GetType().FullName);
            }
        }

        public void StopPin(Double Lat, Double Lon)
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
        }

        public void Button_Reset(object sender, RoutedEventArgs e)
        {
            Result.Items.Clear();
            Map.Children.Clear();
        }

    }
}