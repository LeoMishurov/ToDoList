using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfNewList
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private ToDoModel toDoModels;
        public Window1(ToDoModel toDoModels)
        {
            InitializeComponent();
            this.toDoModels = toDoModels;
            TextRed.Text = toDoModels.Text;
        }
        /// <summary>
        /// Метод редактирует задачу и закрывает окно
        /// </summary>
        private void Button_Redakt(object sender, RoutedEventArgs e)
        {
            toDoModels.Text = TextRed.Text;
            MainWindow mainWindow = new MainWindow();
            mainWindow.Calc();
            mainWindow.Save();
            Close();
            
        }

        
    }
}
