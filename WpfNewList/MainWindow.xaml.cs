using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Load();
            Calc();
            DataGrid1.ItemsSource = _todoDateSee;
            DataGrid2.ItemsSource = _todoDateLoad;
            CalendarAdd.SelectedDate = DateTime.Now.Date;
            Сounter();

            // Запись данных в List GroupModels из базы данных
            AddListGroupModels();           
        }

        /// <summary>
        /// запись данных в List GroupModels из базы данных
        /// </summary>
        public void AddListGroupModels()
        {
            var id = CurrentGroup;
            GroupModels = repository.GetGroups();
            GroupModels.Insert(0, new GroupModel { Name = "все группы", Id = 0 });
            OnPropertyChanged(nameof(GroupModels));

            if (id == null)
            {
                cat.SelectedIndex = 0;
                return;
            }

            // FirstOrDefault() выдает обект соответствующий условию либо вернет  null
            var ob = GroupModels.FirstOrDefault(x => x.Id == id.Id);
            if (ob != null)
                cat.SelectedIndex = GroupModels.IndexOf(ob);
            else cat.SelectedIndex = 0;
        }

        // Вызов события обновления свойства через интерфейс
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        Repository repository = new Repository();

        // Список всех групп 

        public List<GroupModel> GroupModels { get; set; }
        public GroupModel CurrentGroup { get; set; }

        private BindingList<ToDoModel> _todoDate;

        private BindingList<ToDoModel> _todoDateSee = new();

        private BindingList<ToDoModel> _todoDateLoad = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Метод добавления задачи из TextBox в BindingList _todoDate
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WindowAdd windowAdd = new();
            if (windowAdd.ShowDialog() != true)
                return;

            using (var context = new MyContext())
            {
                // Подготовка переменной для сохранения
                context.ToDoModels.Add(windowAdd.ToDoModel);
                // Сохранение в бд
                context.SaveChanges();
            }

            _todoDate.Add(windowAdd.ToDoModel);
            Calc();
            Сounter();
        }

        /// <summary>
        /// Сохраняем в бд содержимое BindingList _todoDate
        /// </summary>
        public void Save(ToDoModel toDoModel)
        {
            using (var context = new MyContext())
            {   
                // Изменение данных в бд
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
                // Достаем данные из базы данных
                var toDoModelsDb = context.ToDoModels.ToList();
                // Преобразуем полученные данные в BindingList
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
            if (DataGrid1.SelectedItem != null)
            {
                _todoDate.Remove((ToDoModel)DataGrid1.SelectedItem);

                repository.DeleteToDoModel((ToDoModel)DataGrid1.SelectedItem);

                Calc();
                Сounter();
            }
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
            if (e.Row.Item is ToDoModel toDoModel)
                Save(toDoModel);
            Calc();
            Сounter();
        }

        /// <summary>
        /// Счетчик задач ипрогресс бара
        /// </summary>
        private void Сounter()
        {
            LableСounter.Content = "осталось задач - " + _todoDateSee.Where(x => !x.IsDone).Count();
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
            if (DataGrid1.SelectedItem != null)
            {
                var toDoModel = (ToDoModel)DataGrid1.SelectedItem;
                Window1 Window1 = new Window1(toDoModel);

                // Подписка на событие при закрытии Window1
                Window1.Closed += (s, e) => { Calc(); Save(toDoModel); };
                Window1.ShowDialog();
            }
        }

        /// <summary>
        /// реагирует на событие смены группы в combo box, сортируя список групп в DataGrid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CurrentGroup != null)
            {
                _todoDate = new BindingList<ToDoModel>(repository.GetToDosByGroupId(CurrentGroup.Id));
                Calc();
            }

        }

        /// <summary>
        /// открытие окна добавления группы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            WindowAddGroup windowAddGroup = new();
            windowAddGroup.ShowDialog();

            AddListGroupModels();
        }

        /// <summary>
        /// срабатывает после загрузки окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cat.SelectedIndex = 0;
        }
    }
}
