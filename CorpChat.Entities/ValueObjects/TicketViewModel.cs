using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpChat.Entities.ValueObjects
{
    public class TicketViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı boş bırakılamaz.")]
        public string TicketUsername { get; set; }

        [DisplayName("Kullanıcı Maili"), Required(ErrorMessage = "{0} alanı boş bırakılamaz.")]
        public string TicketMail { get; set; }

        [DisplayName("Ticket Detayı"), Required(ErrorMessage = "{0} alanı boş bırakılamaz.")]
        public string TicketDetail { get; set; }

        

    }
}
