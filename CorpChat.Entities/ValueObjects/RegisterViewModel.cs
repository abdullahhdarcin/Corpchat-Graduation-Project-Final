using CorpChat.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CorpChat.Entities.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("İsim"), Required(ErrorMessage = "{0} alanı boş bırakılamaz.")]
        public string Name { get; set; }

        [DisplayName("Soyisim"), Required(ErrorMessage = "{0} alanı boş bırakılamaz.")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage ="{0} alanı boş bırakılamaz.")]
        public string Username { get; set; }

        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} alanı boş bırakılamaz."), EmailAddress(ErrorMessage ="Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş bırakılamaz."),DataType(DataType.Password)]
        [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Lütfen Güçlü Bir Şifre Seçin. Şifrenizde Büyük Harf, Rakam ve Sembol Olmalıdır.")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"), Required(ErrorMessage = "{0} alanı boş bırakılamaz."), DataType(DataType.Password), Compare("Password", ErrorMessage ="{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }

    }
}