using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace WpfNewList
{
    /// <summary>
    /// Логика взаимодействия для WindowAdd.xaml
    /// </summary>
    public partial class WindowAdd : Window, INotifyPropertyChanged
    {
        public WindowAdd()
        {
            InitializeComponent();
            DataContext = this;
            ToDoModel = new();
            GroupModels = repository.GetGroups();
        }

        // Событие обновления свойств, реализацтия интерфейса
        public event PropertyChangedEventHandler? PropertyChanged;

        // Вызов события обновления свойства через интерфейс
        public void OnPropertyChanged([CallerMemberName] string prop = "") 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public ToDoModel ToDoModel { get; set;}

        Repository repository = new();

        public List<GroupModel> GroupModels { get; set; }

        /// <summary>
        /// Метод добавления задачи из TextBox в BindingList _todoDate
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Проверка пустоты TextBox
            if (string.IsNullOrWhiteSpace(ToDoModel.Text) || ToDoModel.GroupModel == null)
                return;
            ToDoModel.GroupModelId = ToDoModel.GroupModel.Id;
            ToDoModel.GroupModel = null;
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Реадизация кнопки "отмена"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
