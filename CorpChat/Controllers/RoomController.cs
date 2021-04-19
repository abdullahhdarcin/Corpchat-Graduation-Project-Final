using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CorpChat.BusinessLayer;
using CorpChat.Entities;
using CorpChat.Filters;
using CorpChat.Models;

namespace CorpChat.Controllers
{
    [Auth]
    [Exc]
    public class RoomController : Controller
    {
        RoomManager roomManager = new RoomManager();
        CategoryManager categoryManager = new CategoryManager();

        // GET: Room
        public ActionResult Index()
        {
            if (CurrentSession.User.IsAdmin)
            {
                return View(roomManager.List());
            }
            else
            {
                var rooms = roomManager.ListQueryable().Include("Category").Include("Owner").Where(
                    x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                    x => x.ModifiedOn);

                return View(rooms.ToList());
            }

        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = roomManager.Find(x => x.Id == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                room.Owner = CurrentSession.User;
                roomManager.Insert(room);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", room.CategoryId);
            return View(room);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = roomManager.Find(x => x.Id == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", room.CategoryId);
            return View(room);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room room)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Room db_room = roomManager.Find(x => x.Id == room.Id);
                db_room.CategoryId = room.CategoryId;
                db_room.Title = room.Title;
                db_room.Description = room.Description;
                db_room.IsAdmin = room.IsAdmin;
                db_room.IsKullanici = room.IsKullanici;
                db_room.IsYetkili = room.IsYetkili;
                db_room.IsYonetici = room.IsYonetici;

                roomManager.Update(db_room);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", room.CategoryId);
            return View(room);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = roomManager.Find(x => x.Id == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = roomManager.Find(x => x.Id == id);
            roomManager.Delete(room);
            return RedirectToAction("Index");
        }

        public ActionResult GetRoom(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Room room = roomManager.Find(x => x.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            return View(room);
        }
    }
}
