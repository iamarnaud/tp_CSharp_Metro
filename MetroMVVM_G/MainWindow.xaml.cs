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
            Yentré = Lon.Text;
            Xentré = Lat.Text;
            Rentré = Dist.Text;

            try
            {
                AppelApi metroMvvmApi = new AppelApi("http://data.metromobilite.fr/api/linesNear/json?x=" + Xentré + "&y=" + Yentré + "&dist=" + Rentré + "&details=true");

                List<Lines> lines = JsonConvert.DeserializeObject<List<Lines>>(metroMvvmApi.responseFromServer);
                List<Lines> lineSansDoublons = lines.GroupBy(n => n.name).Select(x => x.First()).ToList();


                /* itération ligne par ligne pour récupérer infos dans Lines
                * soit lines soit lineSansDoublons
                */

                foreach (Lines line in lineSansDoublons)
                {
                    Result.Items.Add(line.name);
                }
            }
            catch (Exception ex)
            {
                Result.Items.Add("Numbers Only. Press reset to try again!");
                Console.WriteLine(ex.GetType().FullName);
            }
        }

    }
}