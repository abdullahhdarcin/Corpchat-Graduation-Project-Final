using CorpChat.BusinessLayer.Abstract;
using CorpChat.BusinessLayer.Results;
using CorpChat.Common.Helpers;
using CorpChat.DataAccessLayer.EntityFramework;
using CorpChat.Entities;
using CorpChat.Entities.Messages;
using CorpChat.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpChat.BusinessLayer
{
    public class CorpChatUserManager : ManagerBase<CorpChatUser>
    {
        public BusinessLayerResult<CorpChatUser> RegisterUser(RegisterViewModel data)
        {
           CorpChatUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists ,"E-Posta Adresi Kayıtlı");
                }
            } else
            {
                int dbResult = base.Insert(new CorpChatUser() {
                    Username = data.Username,
                    Name = data.Name,
                    Surname = data.Surname,
                    ProfileImageFilename="users.jpg",
                    Password = data.Password,
                    Email = data.Email,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    IsYonetici = false,
                    IsYetkili = false,
                    IsKullanici = false,
                    
                });

                if (dbResult > 0 )
                {
                    res.Result = Find(x => x.Email == data.Email && x.Username == data.Username);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/UserActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username};<br><br> Hesabınızı Aktifleştirmek İçin <a href='{activateUri}' target='_blank' >Tıklayın</a>";

                    MailHelper.SendMail(body, res.Result.Email,"CorpChat Hesap Aktifleştirme");
                }
            }

            return res;
        }

        public BusinessLayerResult<CorpChatUser> GetUserById(int id)
        {
            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, " Kullanıcı Bulunamadı ");
            }

            return res;
        }

        public BusinessLayerResult<CorpChatUser> LoginUser(LoginViewModel data)
        {
            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);
            
            
            if (res.Result != null)
            {
                if (!res.Result.IsActive )
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı Aktif Edilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen E-posta Hesabınızı Kontrol Edin.");
                }
                
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong ,"Kullanıcı Adı veya Şifre Hatalı. Kontrol Ediniz.");
            }

            return res;
        }

        public BusinessLayerResult<CorpChatUser> UpdateProfile(CorpChatUser data)
        {
            CorpChatUser db_user = Find(x => x.Username == data.Username && x.Email == data.Email);
            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            res.Result.IsYonetici = data.IsYonetici;
            res.Result.IsYetkili = data.IsYetkili;
            res.Result.IsKullanici = data.IsKullanici;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<CorpChatUser> RemoveUserById(int id)
        {

            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();
            CorpChatUser user = Find(x => x.Id == id);

            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }

        public BusinessLayerResult<CorpChatUser> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı Zaten Aktif Edilmiş.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek Kullanıcı Bulunamadı.");
            }
            return res;
        }

        public new BusinessLayerResult<CorpChatUser> Insert(CorpChatUser data)
        {
            CorpChatUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "user_boy.png";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }

        public new BusinessLayerResult<CorpChatUser> Update(CorpChatUser data)
        {
            CorpChatUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<CorpChatUser> res = new BusinessLayerResult<CorpChatUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            res.Result.IsYonetici = data.IsYonetici;
            res.Result.IsYetkili = data.IsYonetici;
            res.Result.IsKullanici = data.IsKullanici;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    }
}
