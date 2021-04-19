using CorpChat.DataAccessLayer.EntityFramework;
using CorpChat.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpChat.BusinessLayer
{
    public class Test
    {
        public Test()
        {
            Repository<Category> repo = new Repository<Category>();
            List<Category> categories = repo.List();
        }
    }
}
