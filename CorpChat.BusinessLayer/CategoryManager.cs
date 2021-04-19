using CorpChat.BusinessLayer.Abstract;
using CorpChat.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpChat.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            RoomManager roomManager = new RoomManager();
            
            foreach (Room room in category.Rooms.ToList())
            {
                roomManager.Delete(room);
            }

            return base.Delete(category);
        } 
    }
}
