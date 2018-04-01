using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ServiceModel;
using System.Windows.Documents;
using System.Windows.Media;

namespace ClientWPFFozzy
{
    public partial class MainWindow : Window
    {
        private IContract channel;
        private int requestId = 0;
        public MainWindow()
        {
            InitializeComponent();
            Uri address = new Uri("http://localhost:40000/IContract");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endPoint = new EndpointAddress(address);
            ChannelFactory<IContract> factory = new ChannelFactory<IContract>(binding, endPoint);
            channel = factory.CreateChannel();
        }

        #region Window behavior
        private void WindowBehavior(object sender, MouseButtonEventArgs argument)
        {
            this.DragMove();
        }
        private void WindowClose(object sender, MouseButtonEventArgs argument)
        {
            this.Close();
        }
        #endregion

        #region Send request on server
        private void SendRequest(object sender, MouseButtonEventArgs argument)
        {
            if (requestInput.Text != "")
            {
                requestId++;
                var mainExpression = new Run($"Request (#{requestId}):");
                mainExpression.Foreground = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush))
                            .ConvertFromInvariantString("#27DA46");
                request.Inlines.Add(mainExpression);
                var requestExpression = new Run($" {requestInput.Text}{Environment.NewLine}");
                requestExpression.Foreground = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush))
                            .ConvertFromInvariantString("#0066CC");
                request.Inlines.Add(requestExpression);
                if (requestInput.Text.Contains("Number of fibonacci"))
                {
                    string number = "", expression = requestInput.Text;
                    int length = requestInput.Text.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (Char.IsDigit(expression[i]))
                        {
                            number += expression[i];
                        }
                    }
                    AnswerNumberOfFibonacci(Convert.ToInt32(number), requestId);
                }
                else
                {
                    AnswerCalculateTheExpression(requestInput.Text, requestId);
                }
                requestInput.Clear();
            }
        }
        #endregion

        #region Processing the response from the server
        private async void AnswerNumberOfFibonacci(int input, int requestId)
        {
            Task<int> task = channel.NumberOfFibonacci(input);
            await task;
            DispatcherView(task.Result.ToString(), requestId);
        }
        private async void AnswerCalculateTheExpression(string input, int requestId)
        {
            Task<double> task = channel.CalculateTheExpression(input);
            await task;
            DispatcherView(task.Result.ToString(), requestId);
        }
        private void DispatcherView(string resultOperation, int requestId)
        {
            var mainExpression = new Run($"Request (#{requestId}) answer:");
            mainExpression.Foreground = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush))
                        .ConvertFromInvariantString("#27DA46");
            answer.Inlines.Add(mainExpression);
            var answerExpression = new Run($" {resultOperation}{Environment.NewLine}");
            answerExpression.Foreground = (Brush)System.ComponentModel.TypeDescriptor.GetConverter(typeof(Brush))
                        .ConvertFromInvariantString("#0066CC");
            answer.Inlines.Add(answerExpression);
        }
        #endregion
    }
}