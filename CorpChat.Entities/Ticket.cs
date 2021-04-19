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
    [Table("Tickets")]
    public class Ticket : MyEntityBase
    {
        [Required]
        [DisplayName("Kullanıcı Adı")]
        public string TicketUsername { get; set; }
        [Required]
        [DisplayName("Ticket Detayı")]
        public string TicketDetail { get; set; }

        [Required]
        [DisplayName("Kullanıcı Maili")]
        public string TicketMail { get; set; }
        [DisplayName("Çözüldü")]
        public bool IsSolved { get; set; }

        public virtual CorpChatUser Owner { get; set; }

    }
}
