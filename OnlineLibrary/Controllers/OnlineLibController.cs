using OnlineLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using WebMatrix.WebData;

namespace OnlineLibrary.Controllers
{
    public class OnlineLibController : Controller
    {
        OnlineLibDbModels db = new OnlineLibDbModels();
        static List<Books> booksList = new List<Books>();
        int countElementsOnPage = 8;     
        public ActionResult Index(string flag="", int page=-1)                  //OK
        {
            ViewBag.Themes = db.Themes.ToList();            
            ViewBag.Cathegory = db.Cathegory.ToList();
            ViewBag.Authors = db.Authors.ToList();  
            ViewBag.Press = db.Press.ToList();
            if (flag != "") { ViewBag.Result = flag; }
            if (page == -1)
            {
                booksList = new List<Books>();
                booksList.AddRange(db.Books);
                return View(booksList.ToPagedList<Books>(1, countElementsOnPage));
            }
            else 
            {
                return View(booksList.ToPagedList<Books>(page, countElementsOnPage));
            }
        }

        public ActionResult Details(int id)                     //OK
        {
            ViewBag.Themes = db.Themes.ToList();
            ViewBag.Cathegory = db.Cathegory.ToList();
            ViewBag.Authors = db.Authors.ToList();
            ViewBag.Press = db.Press.ToList();
            return View(db.Books.Find(id));     
        }
        
        [Authorize(Roles="administrator")]        
        public ActionResult Create()                            //OK
        {
            ViewBag.Themes = db.Themes.ToList();
            ViewBag.Cathegory = db.Cathegory.ToList();
            ViewBag.Authors = db.Authors.ToList();
            ViewBag.Press = db.Press.ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult Create(FormCollection collection)   //OK
        {
            int IdThemes = 0;
            int IdCathegories = 0;
            int IdAuthor = 0;
            int IdPress = 0;
            try
            {
                if (collection["newTheme"] != "")
                {
                    Themes t1 = new Themes()
                    {
                        Name = collection["newThemes"]
                    };
                    db.Themes.Add(t1);
                    db.SaveChanges();
                    IdThemes = t1.Id;
                }
                else { IdThemes = Convert.ToInt32(collection["Id_Themes"]); }

                if (collection["newCathegory"] != "")
                {
                    Cathegory c1 = new Cathegory()
                    {
                        Name = collection["newCathegory"]
                    };
                    db.Cathegory.Add(c1);
                    db.SaveChanges();
                    IdCathegories = c1.Id;
                }
                else { IdCathegories = Convert.ToInt32(collection["Id_Cathegories"]); }

                if (collection["newAuthorFName"] != "" && collection["newAuthorLName"] != "")
                {
                    Authors a1 = new Authors()
                    {
                        FirstName = collection["newAuthorFName"],
                        LastName = collection["newAuthorLName"]
                    };
                    db.Authors.Add(a1);
                    db.SaveChanges();
                    IdAuthor = a1.Id;
                }
                else { IdAuthor = Convert.ToInt32(collection["Id_Author"]); }

                if (collection["newPress"] != "")
                {
                    Press p1 = new Press()
                    {
                        Name = collection["newPress"]
                    };
                    db.Press.Add(p1);
                    db.SaveChanges();
                    IdPress = p1.Id;
                }
                else { IdPress = Convert.ToInt32(collection["Id_Press"]); }
                
                Books data = new Books()
                {
                    Name = collection["Name"],
                    Pages = Convert.ToInt32(collection["Pages"]),
                    YearPress = Convert.ToInt32(collection["YearPress"]),
                    Quantity = Convert.ToInt32(collection["Quantity"]),
                    Id_Themes = IdThemes,
                    Id_Cathegory = IdCathegories,
                    Id_Author = IdAuthor,
                    Id_Press = IdPress,
                    Comment = collection["Comment"]
                };
                db.Books.Add(data);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "administrator")]
        public ActionResult Edit(int id)                                    //OK
        {
            ViewBag.Themes = db.Themes.ToList();
            ViewBag.Cathegory = db.Cathegory.ToList();
            ViewBag.Authors = db.Authors.ToList();
            ViewBag.Press = db.Press.ToList();
            return View(db.Books.Find(id));
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult Edit(int id, FormCollection collection)         //OK
        {
            try
            {
                var dataBooks = db.Books.Find(id);
            
                dataBooks.Name = collection["Name"];
                dataBooks.Pages = Convert.ToInt32(collection["Pages"]);
                dataBooks.YearPress = Convert.ToInt32(collection["YearPress"]);
                dataBooks.Quantity = Convert.ToInt32(collection["Quantity"]);
                dataBooks.Id_Author = Convert.ToInt32(collection["Id_Author"]);
                dataBooks.Id_Cathegory = Convert.ToInt32(collection["Id_Cathegories"]);
                dataBooks.Id_Press = Convert.ToInt32(collection["Id_Press"]);
                dataBooks.Id_Themes = Convert.ToInt32(collection["Id_Themes"]);
                dataBooks.Comment = collection["Comment"];

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "administrator")]
        public ActionResult Delete(int id)                                  //OK
        {
            ViewBag.Themes = db.Themes.ToList();
            ViewBag.Cathegory = db.Cathegory.ToList();
            ViewBag.Authors = db.Authors.ToList();
            ViewBag.Press = db.Press.ToList();
            return View(db.Books.Find(id));
        }
        
        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult Delete(int id, FormCollection collection)       //OK
        {
            try
            {
                db.Books.Remove(db.Books.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult SearchAll(string search)     //OK
        {
            ViewBag.Book = " ";
            ViewBag.Authors = db.Authors.ToList();
            List<Books> bookAll = new List<Books>();

            //List<Books>
            var b0 = from books in db.Books where books.Name.Contains(search) select books;
            var b4 = from books in db.Books where books.Comment.Contains(search) select books;

            //List<IQueryable<Books>> - промежуточный выбор
            var searchList1 = from author in db.Authors where author.FirstName.Contains(search) || author.LastName.Contains(search) select author.Id;
            var searchList2 = from theme in db.Themes where theme.Name.Contains(search) select theme.Id;
            var searchList3 = from catherogy in db.Cathegory where catherogy.Name.Contains(search) select catherogy.Id;

            //List<Books>
            var b1 = from books in db.Books from sl1 in searchList1 where books.Id_Author == sl1 select books;
            var b2 = from books in db.Books from sl2 in searchList2 where books.Id_Themes == sl2 select books;
            var b3 = from books in db.Books from sl3 in searchList3 where books.Id_Cathegory == sl3 select books;
                        
            bookAll.AddRange(b0);  //собираем все значения из разных категорий
            bookAll.AddRange(b1);
            bookAll.AddRange(b2);
            bookAll.AddRange(b3);
            bookAll.AddRange(b4);

            var x = (from a in bookAll where a.Quantity>0 select a).Distinct();   //убираем продублированные элементы и книги с нулевым количеством экземпляров

            return PartialView(x.ToPagedList<Books>(1, countElementsOnPage));
        }        
    }
}
