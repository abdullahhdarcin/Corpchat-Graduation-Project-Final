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
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Oluşturma Tarihi")]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Düzenleme Tarihi")]
        public DateTime ModifiedOn { get; set; }
        [DisplayName("Düzenleyen Kullanıcı")]
        public string ModifiedUsername { get; set; }
    }
}
