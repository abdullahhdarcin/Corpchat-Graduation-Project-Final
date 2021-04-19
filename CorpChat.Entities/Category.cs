    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpChat.Entities
{
    [Table("Categories")]
    public class Category : MyEntityBase
    {
        [DisplayName("Kategori Başlığı"), Required]
        public string Title { get; set; }
        [DisplayName("Kategori Açıklaması"), Required]
        public string Description { get; set; }

        public virtual List<Room> Rooms { get; set; }

        public Category()
        {
            Rooms = new List<Room>();
        }

    }
}
