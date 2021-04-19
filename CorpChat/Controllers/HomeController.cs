using CorpChat.BusinessLayer;
using CorpChat.BusinessLayer.Results;
using CorpChat.Entities;
using CorpChat.Entities.Messages;
using CorpChat.Entities.ValueObjects;
using CorpChat.Filters;
using CorpChat.Models;
using CorpChat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CorpChat.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private RoomManager roomManager = new RoomManager();
        private CategoryManager categoryManager = new CategoryManager();
        private CorpChatUserManager corpchatUserManager = new CorpChatUserManager();

        
        public ActionResult Index()
        {
            //BusinessLayer.Test test = new BusinessLayer.Test();

            //if (TempData["mm"] != null)
            //{
            //    return View(TempData["mm"] as List<Room>);
            //}

            Session.Timeout = 60;


            return View(roomManager.ListQueryable().OrderByDescending(x=>x.ModifiedOn).ToList());
            //return View(rm.GetAllRoomQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Room> notes = roomManager.ListQueryable().Where(
                x => x.CategoryId == id).OrderByDescending(
                x => x.ModifiedOn).ToList();

            return View("Index", notes);
        }


        [Auth]
        public ActionResult ShowProfile()
        {
            
            BusinessLayerResult<CorpChatUser> res = corpchatUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {
            
            BusinessLayerResult<CorpChatUser> res = corpchatUserManager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(CorpChatUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                  (ProfileImage.ContentType == "image/jpeg" ||
                  ProfileImage.ContentType == "image/jpg" ||
                  ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.ProfileImageFilename = filename;
                }

                
                BusinessLayerResult<CorpChatUser> res = corpchatUserManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi !",
                        RedirectingUrl = "/Home/EditProfile/"
                    };
                    return View("Error", errorNotifyObj);
                }

                CurrentSession.Set<CorpChatUser>("login", res.Result);
                return RedirectToAction("ShowProfile");
            }
            else
            {
                return View(model);
            }
            
        }

        [Auth]
        public ActionResult DeleteProfile()
        {
            
            BusinessLayerResult<CorpChatUser> res = corpchatUserManager.RemoveUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel messages = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };

                return View("Error", messages);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                BusinessLayerResult<CorpChatUser> res = corpchatUserManager.LoginUser(model);

                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = "aaaaaa";
                    }
                    
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));


                    return View(model);
                }

                Session.Timeout = 60;
                CurrentSession.Set<CorpChatUser>("login", res.Result);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                
                BusinessLayerResult<CorpChatUser> res = corpchatUserManager.RegisterUser(model);

                if (res.Errors.Count >0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Header = "Kayıt Başarılı",
                    RedirectingUrl="/Home/Login",
                };

                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon mailini kontrol edin. Hesabınızı aktif etmediğiniz sürece işlem gerçekleştiremeyeceksiniz.");

                return View("Ok",notifyObj);
            }
            return View(model);
        }

        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<CorpChatUser> res = corpchatUserManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            } else
            {
                OkViewModel okNotifyObj = new OkViewModel()
                {
                    Title = "Hesap Aktifleştirildi",
                    RedirectingUrl = "/Home/Login"
                };

                okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık not paylaşabilir ve beğenme yapabilirsiniz.");

                return View("Ok", okNotifyObj);
            }
        }
        
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Sss()
        {
            return View();
        }



    }
}