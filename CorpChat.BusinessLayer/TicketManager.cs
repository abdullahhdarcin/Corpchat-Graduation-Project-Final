using CorpChat.BusinessLayer.Abstract;
using CorpChat.BusinessLayer.Results;
using CorpChat.Common.Helpers;
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
    public class TicketManager : ManagerBase<Ticket>
    {

        public BusinessLayerResult<Ticket> TicketUser(TicketViewModel data)
        {
            Ticket ticket = Find(x => x.TicketDetail == data.TicketDetail);
            BusinessLayerResult<Ticket> res = new BusinessLayerResult<Ticket>();

            if (ticket != null)
            {
                if (ticket.TicketDetail == data.TicketDetail)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Böyle bir ticket mevcut.");
                }

            }
            else
            {
                int dbResult = base.Insert(new Ticket()
                {
                    TicketUsername = data.TicketUsername,
                    TicketMail = data.TicketMail,
                    TicketDetail = data.TicketDetail,
                    IsSolved = false,

                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.TicketDetail == data.TicketDetail);
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Ticket/Index/{res.Result.Id}";
                    string body = $"Merhaba {res.Result.TicketUsername};<br><br> Ticketınız oluşturuldu. Ulaşmak için <a href='{activateUri}' target='_blank' >Tıklayın</a>";

                    var mail = MailHelper.SendMail(body, res.Result.TicketMail, "CorpChat Ticket Bildirimi");

                    if (mail)
                    {
                        string body2 = $"Merhaba. {res.Result.TicketUsername} adlı kullanıcı ticket oluşturdu. Ulaşmak için <a href='{activateUri}' target='_blank' >Tıklayın</a>";
                        MailHelper.SendMail(body2, "abdullahdarcin01@gmail.com", "CorpChat Ticket Bildirimi");
                    }
                }

            }

            return res;
        }

    }
}
