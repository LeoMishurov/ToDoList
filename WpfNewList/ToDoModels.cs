using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfNewList
{
    public class ToDoModel
    {
        public bool IsDone { get; set; }

        public string Text { get; set; }
        public DateTime Data { set; get; } = DateTime.Now.Date;

        public int Id { get; set; }
        public int GroupModelId { get; set; }

        //Сылка на GroupModel для enti framevork
        public GroupModel GroupModel { get; set; }

    }

    public class GroupModel
    {
        public string Name { get; set; }

        public int Id { get; set; }

        //Сылка на ToDoModel для enti framevork
        public List<ToDoModel> ToDoModels { get; set; }

    }
}
