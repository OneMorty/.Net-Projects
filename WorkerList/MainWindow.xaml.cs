using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PeopleListFozzy
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<People> peoples = new ObservableCollection<People>();
        readonly XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<People>));
        private static int index = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                FileStream stream = new FileStream("WorkingList.xml", FileMode.Open, FileAccess.Read);
                peoples = serializer.Deserialize(stream) as ObservableCollection<People>;
                stream.Close();
            }
            catch { }
            PeoplesItem.ItemsSource = peoples;
            ShowWorking();
        }

        #region Поведінка вікна і серіалізація 
        // При натисканні панель веде себе як справжня рамка вікна
        private void StackPanel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // Закриття вікна
        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            File.Delete("WorkingList.xml");
            FileStream stream = new FileStream("WorkingList.xml", FileMode.Create, FileAccess.Write);
            serializer.Serialize(stream, peoples);
            stream.Close();
            this.Close();
        }
        #endregion

        #region Пошук працівників та відобрадення інформації про них
        // Обробка зміни тексту
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PeoplesItem.ItemsSource = ChangedText((sender as TextBox).Text);
        }

        // Фільтрування елементів
        private IEnumerable<People> ChangedText(string text)
        {
            for (int i = 0; i < peoples.Count; i++)
                if (peoples[i].FullName.Contains(text) || peoples[i].Position.Contains(text))
                    yield return peoples[i];
        }

        // Відображення інформації про вибраного працівника 
        private void PeoplesItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            index = PeoplesItem.SelectedIndex;
            try
            {
                LabelFullName.Content = (PeoplesItem.SelectedItem as People).FullName;
                LabelPosition.Content = (PeoplesItem.SelectedItem as People).Position;
                LabelSalary.Content = (PeoplesItem.SelectedItem as People).Salary;
                LabelID.Content = (PeoplesItem.SelectedItem as People).Id;
            }
            catch { }
        }

        // Показ наступного працівника по списку 
        private void NextWorker(object sender, MouseButtonEventArgs e)
        {
            if (index < peoples.Count)
            {
                index++;
                ShowWorking();
            }
        }

        // Показ попереднього по списку працівника
        private void LastWorker(object sender, MouseButtonEventArgs e)
        {
            if (index - 1 != -1)
            {
                index--;
                ShowWorking();
            }
        }

        // Показ вибраного працівника
        private void ShowWorking()
        {
            try
            {
                LabelFullName.Content = peoples[index].FullName;
                LabelPosition.Content = peoples[index].Position;
                LabelSalary.Content = peoples[index].Salary;
                LabelID.Content = peoples[index].Id;
            }
            catch { }
        }
        #endregion

        #region Додавання та видалення працівників
        // Додавання нового працівника
        private void AddWorker(object sender, MouseButtonEventArgs e)
        {
            string FullName = "", Position = "", Salary = "", ID = "";

            if (TextFullName.Text != "")
                FullName = TextFullName.Text;
            else
                FullName = "*********";
            TextFullName.Clear();

            if (TextPosition.Text != "")
                Position = TextPosition.Text;
            else
                Position = "*********";
            TextPosition.Clear();

            if (TextSalary.Text != "")
                Salary = TextSalary.Text;
            else
                Salary = "*********";
            TextSalary.Clear();

            if (TextID.Text != "")
                ID = TextID.Text;
            else
                ID = "*********";
            TextID.Clear();

            peoples.Add(new People(FullName, Position, Salary, ID));
            PeoplesItem.ItemsSource = peoples;
        }

        // Видалення вибраного працівника
        private void DeleteWorker(object sender, MouseButtonEventArgs e)
        {
            try
            {
                peoples.Remove(peoples[index]);
                PeoplesItem.ItemsSource = peoples;
                LabelFullName.Content = "";
                LabelPosition.Content = "";
                LabelSalary.Content = "";
                LabelID.Content = "";
                ShowWorking();
            }
            catch { }
        }
        #endregion
    }
}
