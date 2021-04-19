using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpChat.Entities
{
    public class CorpChatUser : MyEntityBase
    {
        [DisplayName("İsim"),Required]
        public string Name { get; set; }
        [DisplayName("Soyisim"), Required]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"), Required]
        public string Username { get; set; }
        [DisplayName("E-Posta"), Required]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required]
        public string Password { get; set; }

        [StringLength(30), ScaffoldColumn(false)]
        public string ProfileImageFilename { get; set; }

        [DisplayName("Aktif"), Required]
        public bool IsActive { get; set; }
        [DisplayName("Admin")]
        public bool IsAdmin { get; set; }
        [DisplayName("Yönetici")]
        public bool IsYonetici { get; set; }
        [DisplayName("Yetkili")]
        public bool IsYetkili { get; set; }
        [DisplayName("Kullanıcı")]
        public bool IsKullanici { get; set; }
        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        public virtual List<Room> Rooms { get; set; }
        public virtual List<ChatMessage> ChatMessages { get; set; }
        public virtual List<Ticket> Tickets { get; set; }

        
    }
}
