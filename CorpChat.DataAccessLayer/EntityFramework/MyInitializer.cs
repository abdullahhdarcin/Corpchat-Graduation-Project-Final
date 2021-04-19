using CorpChat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorpChat.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>   
    {
        protected override void Seed(DatabaseContext context)
        {
            CorpChatUser admin = new CorpChatUser()
            {
                Name = "Abdullah",
                Surname = "Darcin",
                Username = "abdullahdarcin",
                Email = "abdullahdarcin01@gmail.com",
                Password = "159753bjk",
                ProfileImageFilename = "users.jpg",
                IsActive = true,
                IsAdmin = true,
                IsYonetici = true,
                IsYetkili = true,
                IsKullanici = true,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "abdullahdarcin",
                ActivateGuid = Guid.NewGuid()
            };

            CorpChatUser kullanici = new CorpChatUser()
            {
                Name = "Emine",
                Surname = "Darcin",
                Username = "eminedarcin",
                Email = "abdullahdarcin01@gmail.com",
                Password = "159753bjk",
                ProfileImageFilename = "users.jpg",
                IsActive = true,
                IsAdmin = false,
                IsYonetici = false,
                IsYetkili = false,
                IsKullanici = true,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "eminedarcin",
                ActivateGuid = Guid.NewGuid()
            };

            context.CorpChatUsers.Add(admin);
            context.CorpChatUsers.Add(kullanici);

            for (int i = 0; i < 8; i++)
            {
                CorpChatUser user = new CorpChatUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFilename = "users.jpg",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    IsYonetici = false,
                    IsYetkili = false,
                    IsKullanici = true,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };

                context.CorpChatUsers.Add(user);
            }

            context.SaveChanges();
            List<CorpChatUser> userlist = context.CorpChatUsers.ToList();

            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "abdullahdarcin"
                };

                context.Categories.Add(cat);

                for (int k = 0; k < FakeData.NumberData.GetNumber(5, 9); k++)
                {
                    CorpChatUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Room note = new Room()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 15)),
                        Description = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        Owner = owner,
                        IsAdmin = false,
                        IsYonetici = false,
                        IsYetkili = false,
                        IsKullanici = true,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username,
                    };

                    cat.Rooms.Add(note);

                    for (int j = 0; j < FakeData.NumberData.GetNumber(3, 5); j++)
                    {
                        CorpChatUser message_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                        ChatMessage chatMessage = new ChatMessage()
                        {
                            Messages = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 15)),
                            Owner = message_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = message_owner.Username
                        };

                        note.ChatMessage.Add(chatMessage);
                        
                    }

                }
                
            }

            context.SaveChanges();
        }
    }
}