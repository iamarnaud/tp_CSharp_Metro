using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Metro
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(
              "https://data.metromobilite.fr/api/linesNear/json?x=5.709360123&y=45.176494599999984&dist=900&details=true");

            // Get the response.  
            WebResponse response = request.GetResponse();

            // Display the status.  
            WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.  
            string responseFromServer = reader.ReadToEnd();

            // Display the content.  
            List<Lines> lines = JsonConvert.DeserializeObject<List<Lines>>(responseFromServer);
            /*On crée une liste qui groupe by nom(touts les memes noms sont mis ensemble, et on select
            * seulement le first de cette nouvelle liste
            */
           List<Lines>lineSansDoublons = lines.GroupBy(n => n.name).Select(g => g.First()).ToList();

            foreach (Lines line in lineSansDoublons) // itération ligne par ligne pour récupérer infos dans Lines // soit lines soit lineSansDoublons
            {
                WriteLine($"nom_ligne { line.name}");
            }

            // Clean up the streams and the response.  
            reader.Close();
            response.Close();
            ReadLine(); // sinon le programme se ferme dans lire le résultat
        }
    }
}
