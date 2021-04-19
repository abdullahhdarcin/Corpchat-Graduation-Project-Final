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
    [Table("Rooms")]
    public class Room : MyEntityBase
    {
        [Required]
        [DisplayName("Oda Adı"), StringLength(15)]
        public string Title { get; set; }

        [DisplayName("Oda Açıklaması") ,Required]
        public string Description { get; set; }

        [DisplayName("Kategori")]
        public int CategoryId { get; set; }

        [DisplayName("Admin")]
        public bool IsAdmin { get; set; }
        [DisplayName("Yönetici")]
        public bool IsYonetici { get; set; }
        [DisplayName("Yetkili")]
        public bool IsYetkili { get; set; }
        [DisplayName("Kullanıcı")]
        public bool IsKullanici { get; set; }


        public virtual CorpChatUser Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<ChatMessage> ChatMessage { get; set; }

        public Room()
        {
            ChatMessage = new List<ChatMessage>();
        }

    }
}
