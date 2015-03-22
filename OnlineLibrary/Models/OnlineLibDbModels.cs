using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OnlineLibrary.Models
{
    public class OnlineLibDbModels:DbContext
    {
        public OnlineLibDbModels()
            : base("OnlineLibraryDatabase")     // изменить Web.config, раскоментировать нужную БД (все бызы данных имеют одинаковое название)
        {        }
        public DbSet<Authors> Authors { get; set; }
        public DbSet<Books> Books { get; set; }
        public DbSet<Cathegory> Cathegory { get; set; }
        public DbSet<Press> Press { get; set; }
        public DbSet<Themes> Themes { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserCard> UserCard { get; set; }
        public DbSet<Userlogin> Userlogin { get; set; }
    }
    [Table("Authors")]
    public class Authors
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
    }
    [Table("Books")]
    public class Books
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Страниц")]
        public int? Pages { get; set; }
        [Display(Name = "Год выпуска")]
        public int? YearPress { get; set; }
        [Display(Name = "В наличии")]
        public int? Quantity { get; set; }
        [Display(Name = "Тема")]
        public int Id_Themes { get; set; }
        [Display(Name = "Категория")]
        public int Id_Cathegory { get; set; }
        [Display(Name = "Автор")]
        public int Id_Author { get; set; }
        [Display(Name = "Издательство")]
        public int Id_Press { get; set; }
        [Display(Name = "Комментарии")]
        public string Comment { get; set; }
        public string PictureLink { get; set; }
    }
    [Table("Cathegory")]
    public class Cathegory
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [Table("Press")]
    public class Press
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [Table("Themes")]
    public class Themes
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
    }
    [Table("User_Card")]
    public class UserCard
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Id_User { get; set; }
        public int Id_Book { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime? DateOut { get; set; }
        public bool? Message { get; set; }
    }
    [Table("Userlogin")]
    public class Userlogin
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Id_User { get; set; }
        public string Login { get; set; }
    }
}