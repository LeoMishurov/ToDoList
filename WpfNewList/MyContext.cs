using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNewList
{
    public class MyContext : DbContext
    {
        public DbSet<ToDoModel> ToDoModels { get; set; }
        /// <summary>
        /// настройки подключения к базе данных
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlite("Data Source = ToDoDb.db;");
        }

    }
}
