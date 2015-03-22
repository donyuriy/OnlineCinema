using OnlineLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace OnlineLibrary.Controllers
{
    public class BooksActionsController : Controller
    {
        OnlineLibDbModels db = new OnlineLibDbModels();
        [Authorize(Roles = "user")]
        public ActionResult UserCard()                  //---------КАРТОЧКА ПОЛЬЗОВАТЕЛЯ--------------
        {
            ViewBag.Books = db.Books.ToList();
            var idUser = (from ul in db.Userlogin where ul.Login == User.Identity.Name select ul.Id_User).Single();
            var data = from card in db.UserCard where card.Id_User == idUser select card;
            return View(data);
        }

        [Authorize(Roles = "administrator")]
        public ActionResult AdminCard(FormCollection collection)                 //---------КАРТОЧКА АДМИНИСТРАТОРА-----------
        {
            int sort = Convert.ToInt32(collection["sortEntry"]);
            List<UserCard> entry = new List<Models.UserCard>();
            ViewBag.Books = db.Books;
            ViewBag.Users = db.Userlogin;
            var today = DateTime.Now;

            //все книги, которые были взяты
            if (sort == 0)
            {
                entry = (from card in db.UserCard select card).Distinct().ToList();
            }

            //все невозвращенные книги
            if (sort == 1)
            {
                entry = (from card in db.UserCard where (card.DateOut == null) select card).Distinct().ToList();
            }

            //все книги взятые более месяца назад и не возвращенные
            if (sort == 2)
            {
                entry = (from card in db.UserCard where card.DateOut==null && (DateTime.Now - card.DateIn).TotalDays>5 select card).Distinct().ToList();
               
            }

            //все книги взятые более 3 месяцев назад и не возвращенные
            if (sort == 3)
            {
                entry = (from card in db.UserCard where (card.DateIn != null && card.DateOut == null) select card).Distinct().ToList();
            }

            //все книги взятые более 6 месяцев назад и не возвращенные
            if (sort == 4)
            {
                entry = (from card in db.UserCard where (card.DateIn != null && card.DateOut == null) select card).Distinct().ToList();
            }

            //все книги взятые более года назад и не возвращенные
            if (sort == 5)
            {
                entry = (from card in db.UserCard where (card.DateIn != null && card.DateOut == null) select card).Distinct().ToList();
            }

            return View(entry);
        }

        [Authorize(Roles="user")]
        public ActionResult GetTheBook(int id)          //---------ВЗЯТЬ КНИГУ(id книги)--------------
        {              
              var idUser = (from ul in db.Userlogin where ul.Login == User.Identity.Name select ul.Id_User).Single();
              bool flag = true;       //флаг - проверка можно ли взять книгу
              foreach (var item in db.UserCard)
              {
                  if (item.Id_User == idUser && item.Id_Book == id && item.DateIn != null && item.DateOut == null)    //если у данного пользователя книга уже есть и дата сдачи не указана, то книгу взять нельзя
                  {
                      flag = false;
                      break;
                  }
              }

              if (flag)   //если у данного пользователя нет такой книги, то он может её взять
              {
                  UserCard data = new Models.UserCard()
                  {
                      Id_Book = id,
                      Id_User = idUser,
                      DateIn = DateTime.Now,
                      DateOut = null    //явно задаём значение даты сдачи книги в null  чтобы не возникало конфликтов при проверке
                  };
                  db.UserCard.Add(data);
                  db.SaveChanges();
                  var b =db.Books.Find(id);
                  b.Quantity -= 1;
                  db.SaveChanges();                  
                  return RedirectToAction("Index", "OnlineLib", new { flag = "1" });    
              }
              else
              {
                  return RedirectToAction("Index", "OnlineLib", new { flag = "0" });
              }
        }

        [Authorize(Roles = "user")]
        public ActionResult ReturnTheBook(UserCard card)             //-----------ВЕРНУТЬ КНИГУ----------
        {
            try
            {
                var data = db.UserCard.Find(card.Id);
                data.DateOut = DateTime.Now;
                Books dataBook = db.Books.Find(card.Id_Book);
                dataBook.Quantity += 1;
                db.SaveChanges();
                return RedirectToAction("UserCard", "BooksActions");
            }            
            catch (Exception)
            {
                return View();
            }
        }
        [Authorize(Roles = "administrator")]
        public ActionResult Reminder(int? idBook, int? idUser)         //==========ОТПРАВИТЬ НАПОМИНАНИЕ============
        {
            try
            {
                var entry = (from card in db.UserCard where card.Id_User == idUser && card.Id_Book == idBook select card).Single();
                entry.Message = true;
                return RedirectToAction("AdminCard", "BookActions");
            }
            catch (Exception)
            {
                return View();
            }            
        }
    }
}
