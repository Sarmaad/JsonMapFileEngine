using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine;

namespace DynamicFileHelperMapping
{
    class Program
    {
        static void Main(string[] args)
        {

            var engine = new JsonEngine(File.ReadAllText("mapping.json"));

            var results = engine.Parse<NewSt>("Code-Review-Interview-Data-File.csv");


        }

        
    }

    public class NewSt
    {
        public string Name { get; set; }
        public string Email { get; set; }
        
    }
}
