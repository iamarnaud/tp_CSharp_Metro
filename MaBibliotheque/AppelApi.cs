using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Console;


namespace MaBibliotheque
{
    public class AppelApi
    {
        // Création d'une variable pour récupérer le retour de l'API
        public string responseFromServer { get; set; }

        public AppelApi(string url)
        {

            // Create a request for the URL.   
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);

            // Get the response.  
            System.Net.WebResponse response = request.GetResponse();

            // Display the status. Ex err 404 ou 300 ... 
            WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);

            // Read the content. On set grace grace à la variable responseFromServer
            responseFromServer = reader.ReadToEnd();


            // Clean up the streams and the response.  
            reader.Close();
            response.Close();
        }

    }
}
