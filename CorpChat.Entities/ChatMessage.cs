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
    [Table("ChatMessage")]
    public class ChatMessage : MyEntityBase
    {
        [Required]
        [DisplayName("Oda Adı")]
        public string Messages { get; set; }
                
        public virtual Room Room { get; set; }
        public virtual CorpChatUser Owner { get; set; }
    }
}
