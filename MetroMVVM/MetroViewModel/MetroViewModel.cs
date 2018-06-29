using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaBibliotheque;
using MetroMVVM.Model;
using Newtonsoft.Json;

namespace MetroMVVM.ViewModel
{
    public class MetroViewModel
    {
        /**
         * MetroModel = classe lines dans les autres projets
         * Création d'une liste des lignes
         */
        public List<MetroModel> Stops { get; set; }

        public void LoadStops(String Xentre, String Yentre, String Rentre)
        {
            try
            {
                AppelApi metroMvvmApi = new AppelApi("http://data.metromobilite.fr/api/linesNear/json?x=" + Xentre +
                                                     "&y=" + Yentre + "&dist=" + Rentre + "&details=true");

                // on fait un liste de la classe Lignes que l'on converti grâce au nuget json.net pour pouvoir utiliser les objets
                List<MetroModel> lines = JsonConvert.DeserializeObject<List<MetroModel>>(metroMvvmApi.responseFromServer);

                List<MetroModel> lineSansDoublons = lines.GroupBy(n => n.name).Select(x => x.First()).ToList();

                // on stock lineSansDoublons dans la liste créée et on récupérère les infos avec Stops sur lequel on va binder
                Stops = lineSansDoublons;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType().FullName);
            }
        }
    }
}
