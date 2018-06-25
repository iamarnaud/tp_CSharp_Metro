using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using MaBibliotheque;

namespace Metro
{
    class Program
    {


        static void Main(string[] args)
        {
            AppelApi metroapi = new AppelApi("https://data.metromobilite.fr/api/linesNear/json?x=5.709360123&y=45.176494599999984&dist=900&details=true");

            // Display the content.  On get grace à la variable responseFromServer de la bibli AppelApi
            List<Lines> lines = JsonConvert.DeserializeObject<List<Lines>>(metroapi.responseFromServer);
            // transformation liste en Json : String JSON = JsonConvert.SerializeObject(lines);

            /*On crée une liste qui groupe by nom / tous les memes noms sont mis ensemble, et on select
            * seulement le first de cette nouvelle liste
            *  => Requete linq
            */

            List<Lines> lineSansDoublons = lines.GroupBy(n => n.name).Select(x => x.First()).ToList();

            /*
             * Si on utilise pas linq on peut faire un foreach qui va éliminer les doublons :
             * 
            List<Lines> lineSansDoublons2 = new List<Lines>();
            foreach (Lines line in lines)
            {
                if (!lineSansDoublons2.Contains(line))
                {
                    lineSansDoublons2.Add(line);
                }   
            }

            foreach (Lines line in lineSansDoublons2)
            {
                WriteLine($"nom_ligne2 { line.id}");
            }
            */

            /* itération ligne par ligne pour récupérer infos dans Lines
            * soit lines soit lineSansDoublons
            */

            foreach (Lines line in lineSansDoublons)
            {
                WriteLine($"nom_ligne { line.name}");
            }

            ReadLine(); // sinon le programme se ferme dans lire le résultat
        }
    }
}
