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
        public DateTime Data { set; get; } = DateTime.Now;

        public int Id { get; set; }
       



    }
}
