using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfNewList
{
    internal class Repository
    {
        /// <summary>
        /// Возвращает все группы из GroupModels
        /// </summary>
        /// <returns></returns>
        public List<GroupModel> GetGroups ()
        {
            // Using отвечает за закрытие после использования обьекта класса MyContext
            using (var context = new MyContext())
            {
                return context.GroupModel.ToList();
            }
        }

        /// <summary>
        /// Возвращает все задачи сопадающие с id группы переданной в метод
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<ToDoModel> GetToDosByGroupId(int groupId)
        {           
            using (var context = new MyContext())
            {
                return context.ToDoModels.Where(x => groupId == 0 || x.GroupModelId==groupId).ToList();
            }
        }

        /// <summary>
        /// Сохранение группы в бд
        /// </summary>
        /// <param name="groupModel"></param>
        public void SaveGroup(GroupModel groupModel)
        {
            using (var context = new MyContext())
            {
                if(groupModel.Id == 0)
                    // Подготовка переменной для сохранения
                    context.GroupModel.Add(groupModel);
                else
                    context.GroupModel.Update(groupModel);
                // Сохранение в бд
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление обьекта GroupModel из бд
        /// </summary>
        /// <param name="groupModel"></param>
        public void DeleteGroup(GroupModel groupModel)
        {
            using (var context = new MyContext())
            {              
               // Подготовка переменной для удаления
                context.GroupModel.Remove(groupModel);
                
                // Сохранение в бд
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление обьекта ToDoModel из бд
        /// </summary>
        /// <param name="groupModel"></param>
        public void DeleteToDoModel(ToDoModel toDoModel)
        {
            using (var context = new MyContext())
            {
                // Подготовка переменной для удаления
                context.ToDoModels.Remove(toDoModel);

                // Сохранение в бд
                context.SaveChanges();
            }
        }
    }
}
