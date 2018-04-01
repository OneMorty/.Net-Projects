using System.ServiceModel;
using System.Threading.Tasks;

namespace ClientWPFFozzy
{
    [ServiceContract]
    interface IContract
    {
        [OperationContract]
        Task<int> NumberOfFibonacci(int input);
        [OperationContract]
        Task<double> CalculateTheExpression(string input);
    }
}
