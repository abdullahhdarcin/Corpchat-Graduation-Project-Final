using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CorpChat.BusinessLayer;
using CorpChat.BusinessLayer.Results;
using CorpChat.Entities;
using CorpChat.Filters;

namespace CorpChat.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class CorpChatUserController : Controller
    {
        private CorpChatUserManager corpChatUserManager = new CorpChatUserManager();

        // GET: CorpChatUser
        public ActionResult Index()
        {
            return View(corpChatUserManager.List());
        }

        // GET: CorpChatUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CorpChatUser corpChatUser = corpChatUserManager.Find(x => x.Id == id.Value);
            if (corpChatUser == null)
            {
                return HttpNotFound();
            }
            return View(corpChatUser);
        }

        // GET: CorpChatUser/Create
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CorpChatUser corpChatUser)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<CorpChatUser> res = corpChatUserManager.Insert(corpChatUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(corpChatUser);

                }

                return RedirectToAction("Index");
            }

            return View(corpChatUser);
        }

        // GET: CorpChatUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CorpChatUser corpChatUser = corpChatUserManager.Find(x => x.Id == id.Value);
            if (corpChatUser == null)
            {
                return HttpNotFound();
            }
            return View(corpChatUser);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( CorpChatUser corpChatUser)
        {

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<CorpChatUser> res = corpChatUserManager.Update(corpChatUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(corpChatUser);
                }
                return RedirectToAction("Index");
            }
            return View(corpChatUser);
        }

        // GET: CorpChatUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CorpChatUser corpChatUser = corpChatUserManager.Find(x => x.Id == id.Value);
            if (corpChatUser == null)
            {
                return HttpNotFound();
            }
            return View(corpChatUser);
        }

        // POST: CorpChatUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CorpChatUser corpChatUser = corpChatUserManager.Find(x => x.Id == id);
            corpChatUserManager.Delete(corpChatUser);
            return RedirectToAction("Index");
        }

    }
}
