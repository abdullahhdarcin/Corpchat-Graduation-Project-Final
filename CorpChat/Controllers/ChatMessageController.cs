using CorpChat.BusinessLayer;
using CorpChat.Entities;
using CorpChat.Filters;
using CorpChat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CorpChat.Controllers
{
    [Exc]
    public class ChatMessageController : Controller
    {
        private RoomManager roomManager = new RoomManager();
        private ChatMessageManager chatMessageManager = new ChatMessageManager();

        public ActionResult Chat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Room room = roomManager.ListQueryable().Include("ChatMessage").FirstOrDefault(x => x.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialMessages", room.ChatMessage.OrderByDescending(x=>x.ModifiedOn).ToList());
        }


        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ChatMessage chatMessage = chatMessageManager.Find(x => x.Id == id);

            if (chatMessage == null)
            {
                return new HttpNotFoundResult();
            }

            chatMessage.Messages = text;

            if (chatMessageManager.Update(chatMessage) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ChatMessage chatMessage = chatMessageManager.Find(x => x.Id == id);

            if (chatMessage == null)
            {
                return new HttpNotFoundResult();
            }

            if (chatMessageManager.Delete(chatMessage) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }



        [Auth]
        [HttpPost]
        public ActionResult Create(ChatMessage chatMessage, int? roomid)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            ModelState.Remove("When");

            if (ModelState.IsValid)
            {
                if (roomid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Room room = roomManager.Find(x => x.Id == roomid);

                if (room == null)
                {
                    return new HttpNotFoundResult();
                }

                chatMessage.Room = room;
                chatMessage.Owner = CurrentSession.User;

                if (chatMessageManager.Insert(chatMessage) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Room room = roomManager.ListQueryable().Include("ChatMessage").FirstOrDefault(x => x.Id == id);

            if (room == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialInMessage", room.ChatMessage.OrderByDescending(x => x.ModifiedOn).ToList());
        }
    }
}
