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
using CorpChat.Entities.ValueObjects;
using CorpChat.Filters;
using CorpChat.Models;
using CorpChat.ViewModels;

namespace CorpChat.Controllers
{
    [Auth]
    [Exc]
    public class TicketController : Controller
    {
        private TicketManager ticketManager = new TicketManager();

        public ActionResult Index()
        {
            if (CurrentSession.User.IsAdmin)
            {
                return View(ticketManager.List());
            } else
            {
                var tickets = ticketManager.ListQueryable().Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                x => x.ModifiedOn);


                return View(tickets.ToList());
            }

            
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = ticketManager.Find(x => x.Id == id.Value);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Ticket ticket, TicketViewModel tic)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {

                BusinessLayerResult<Ticket> res = ticketManager.TicketUser(tic);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(tic);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Header = "Ticket Başarıyla Oluşturuldu.",
                    RedirectingUrl = "/Ticket/Index",
                };

                ticketManager.Insert(ticket);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }

            return View(ticket);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = ticketManager.Find(x => x.Id == id.Value);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Ticket ticket)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                Ticket tic = ticketManager.Find(x => x.Id == ticket.Id);
                tic.TicketUsername = ticket.TicketUsername;
                tic.TicketDetail = ticket.TicketDetail;
                tic.IsSolved = ticket.IsSolved;

                ticketManager.Update(ticket);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(ticket);
        }

        // GET: Ticket/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = ticketManager.Find(x => x.Id == id.Value);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = ticketManager.Find(x => x.Id == id);
            ticketManager.Delete(ticket);

            CacheHelper.RemoveCategoriesFromCache();

            return RedirectToAction("Index");
        }

    }
}
