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
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // проверка пустоты TextBox
            if (string.IsNullOrWhiteSpace(TextBox.Text))
                return;
            var  newToDoModel = new ToDoModel() { Text = TextBox.Text, Data = CalendarAdd.SelectedDate ?? DateTime.Now };

            using (var context = new MyContext()) 
            {
                // подготовка переменной для сохранения
                context.ToDoModels.Add(newToDoModel);
                // сохранение в бд
                context.SaveChanges();
            }

            _todoDate.Add(newToDoModel);
            TextBox.Clear();         
            Calc();
            Сounter();
            
        }
        /// <summary>
        /// Сохраняем в Json содержимое BindingList _todoDate
        /// </summary>
        public void Save(ToDoModel toDoModel)
        {
            using (var context = new MyContext())
            {   // изменение данных в бд
                context.ToDoModels.Update(toDoModel);
                context.SaveChanges();
            }           
        }
        /// <summary>
        /// Загружаем данные из БД в BindingList _todoDate
        /// </summary>
        public void Load()
        {
            using (var context = new MyContext()) 
            {
                //достаем данные из базы данных
                var toDoModelsDb = context.ToDoModels.ToList();
                //преобразуем полученные данные в BindingList
                _todoDate = new BindingList<ToDoModel>(toDoModelsDb);
            }

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
            if(e.Row.Item is ToDoModel toDoModel)
                Save(toDoModel);
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
            var c = b == 0 ? 0 : a / b * 100;
            ProgressBar.Value = c;         
            LabelProgress.Content = $"прогресс выполнения {(int)c}%";
           
        }
        /// <summary>
        /// Метод открывающий окно редактирования задач
        /// </summary>
        private void Button_Redakt(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedItem!=null)
            {
                var toDoModel = (ToDoModel)DataGrid1.SelectedItem;
                Window1 Window1 = new Window1(toDoModel);
                // подписка на событие при закрытии Window1
                
                Window1.Closed += (s,e) => { Calc(); Save(toDoModel); }; 
                Window1.ShowDialog();
            }

        }
        
    }
}
