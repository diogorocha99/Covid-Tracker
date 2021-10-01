//Serviços REST
//
//Criado por Diogo Rocha nº16966


using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

using TP2.Model;

namespace TP2.Controllers
{
    public class ServicosExternosController : Controller
    {
        
        //serviços externos a retornar o json
        [Route("/COVID")]
        [HttpGet]
        public string COVID()
        {
            string link = "https://services.arcgis.com/CCZiGSEQbAxxFVh3/arcgis/rest/services/COVID19_ConcelhosDiarios/FeatureServer/0/query?where=1%3D1&outFields=*&outSR=4326&f=json";

            HttpWebRequest request = WebRequest.Create(link) as HttpWebRequest;

            var response = request.GetResponse() as HttpWebResponse;
            var reader = new StreamReader(response.GetResponseStream());
            var objText = reader.ReadToEnd();

            return objText.ToString();
        }



    }
}
