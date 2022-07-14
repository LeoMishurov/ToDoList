using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для WindowAddGroup.xaml
    /// </summary>
    public partial class WindowAddGroup : Window
    {
        public WindowAddGroup()
        {
            InitializeComponent();

            _groupModels = new BindingList<GroupModel>( repository.GetGroups());

            ListVievGroup.ItemsSource = _groupModels;
            
        }
        Repository repository = new();

        private BindingList<GroupModel> _groupModels;


        /// <summary>
        /// добавление группы из TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBottonGroup_Clic(object sender, RoutedEventArgs e)
        {
            GroupModel groupModel = new();
            groupModel.Name = TextBoxGroup.Text;
            Repository repository = new();
            repository.SaveGroup(groupModel);
            _groupModels.Add(groupModel);
            TextBoxGroup.Clear();
           
        }
        /// <summary>
        /// удаление выббранной группы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeieteBottonGroup_Clic(object sender, RoutedEventArgs e)
        {
            repository.DeleteGroup((GroupModel)ListVievGroup.SelectedItem);
            
            _groupModels.Remove((GroupModel)ListVievGroup.SelectedItem);

            

        }

        
    }
}
