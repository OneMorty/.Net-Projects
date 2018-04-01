using System;
using System.ServiceModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerWCFFozzy
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri address = new Uri("http://localhost:40000/IContract"); 
            BasicHttpBinding binding = new BasicHttpBinding();        
            Type contract = typeof(IContract);
            ServiceHost host = new ServiceHost(typeof(Processing));
            Console.WriteLine($"Server working (Time of start: {DateTime.Now})...");
            host.AddServiceEndpoint(contract, binding, address);
            host.Open();

            Console.ReadKey();
            host.Close();
        }
    }
}
