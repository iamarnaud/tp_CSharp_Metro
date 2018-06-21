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
            string toto;
            // Create a request for the URL.   
            WebRequest request = WebRequest.Create(
              "https://data.metromobilite.fr/api/linesNear/json?x=5.709360123&y=45.176494599999984&dist=900&details=true");

            // Get the response.  
            WebResponse response = request.GetResponse();

            // Display the status. Ex err 404 ou 300 ... 
            WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.  
            string responseFromServer = reader.ReadToEnd();

            // Display the content.  
            List<Lines> lines = JsonConvert.DeserializeObject<List<Lines>>(responseFromServer);
            // transformation liste en Json : String JSON = JsonConvert.SerializeObject(lines);

            /*On crée une liste qui groupe by nom(touts les memes noms sont mis ensemble, et on select
            * seulement le first de cette nouvelle liste
            */
           // List<Lines> lineSansDoublons = lines.GroupBy(n => n.name).Select(x => x.First()).ToList();

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

            /* itération ligne par ligne pour récupérer infos dans Lines
            * soit lines soit lineSansDoublons
           

            foreach (Lines line in lineSansDoublons)
            {
                WriteLine($"nom_ligne { line.name}");
            }
             */
            // Clean up the streams and the response.  
            reader.Close();
            response.Close();
            ReadLine(); // sinon le programme se ferme dans lire le résultat
        }
    }
}
