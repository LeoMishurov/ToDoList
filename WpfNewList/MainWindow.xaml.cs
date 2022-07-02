using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfNewList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
            Calc();
            DataGrid1.ItemsSource = _todoDateSee;       
            DataGrid2.ItemsSource = _todoDateLoad;
            CalendarAdd.SelectedDate = DateTime.Now.Date;
            Сounter();
        }
        private BindingList<ToDoModel> _todoDate;

        private BindingList<ToDoModel> _todoDateSee=new();

        private BindingList<ToDoModel> _todoDateLoad=new();

        /// <summary>
        /// Метод добавления задачи из TextBox в BindingList _todoDate
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(TextBox.Text))
            {
                _todoDate.Add(new ToDoModel() { Text = TextBox.Text, Data = CalendarAdd.SelectedDate ?? DateTime.Now });
                TextBox.Clear();
                Save();
                Calc();
                Сounter();
            }
        }
        /// <summary>
        /// Сохраняем в Json содержимое BindingList _todoDate
        /// </summary>
        public void Save()
        {
            string objSave = JsonConvert.SerializeObject(_todoDate);
            File.WriteAllText("save.txt", objSave);
        }
        /// <summary>
        /// Загружаем данные из Json в BindingList _todoDate
        /// </summary>
        public void Load()
        {
            if (File.Exists("save.txt"))
            {
                string text = File.ReadAllText("save.txt");
                _todoDate = JsonConvert.DeserializeObject<BindingList<ToDoModel>>(text);

            }

        }
        /// <summary>
        /// Метод срабатывает при закрытии программы.
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            Save();
        }
        /// <summary>
        /// Метод производит сортировку данных, по дате и галочкам в чекбоксах. 
        /// Очишает _todoDateSee
        /// затем записывает в него не завершенные задачи и задачи с сегодняшним числом из_todoDate
        /// </summary>
        public void Calc()
        {
            _todoDateSee?.Clear();
            _todoDate.Where(x => (!x.IsDone && x.Data < DateTime.Now.Date) || x.Data == DateTime.Now.Date)
                .OrderBy(x => x.IsDone).ThenBy(x => x.Data)
                .ToList().ForEach(x => _todoDateSee.Add(x));
        }
        /// <summary>
        /// Метод удаляет выделенный элимент DataGrid1 из _todoDate.
        /// И перезаписывает _todoDateSee
        /// </summary>
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            _todoDate.Remove((ToDoModel)DataGrid1.SelectedItem);
            Calc();
            Сounter();
        }
        /// <summary>
        /// Метод переносит задачи с выборкой по дате из _todoDate в _todoDateLoad.
        /// В зависимости от даты выбранной в календаре.
        /// И перезаписывает _todoDateSee
        /// </summary>
        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            _todoDateLoad?.Clear();
            _todoDate.Where(x => x.Data == CalendarAdd.SelectedDate && x.Data != DateTime.Now.Date).ToList().ForEach(x => _todoDateLoad.Add(x));
        }
        /// <summary>
        /// Метод вызывается при завершении редактирования поля в _todoDateSee "при установке галочки в чекбокс".
        /// И перезаписывает _todoDateSee
        /// </summary>
        private void DataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {         
            Calc();
            Сounter();
        }
        /// <summary>
        /// Счетчик задач ипрогресс бара
        /// </summary>
        private void Сounter()
        {             
            LableСounter.Content = "ОСТАЛОСЬ ЗАДАЧ - " + _todoDateSee.Where(x => !x.IsDone).Count();
            double a = _todoDateSee.Where(x => x.IsDone).Count();
            double b = _todoDateSee.Count;
            ProgressBar.Value = b == 0 ? 0 : (int) a / b * 100;          
            LabelProgress.Content = "прогресс выполнения " +(int)(a/b*100) + "%";
           
        }
        /// <summary>
        /// Метод открывающий окно редактирования задач
        /// </summary>
        private void Button_Redakt(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem!=null)
            {
                Window1 Window1 = new Window1((ToDoModel)DataGrid1.SelectedItem);
                Window1.Closed += Window1_Closed; // подписка на событие при закрытии Window1
                Window1.Show();
            }

        }
        /// <summary>
        ///событие при закрытии Window1
        /// </summary>
        private void Window1_Closed(object? sender, EventArgs e)
        {
            Calc();
            Save();

        }       
    }
}
