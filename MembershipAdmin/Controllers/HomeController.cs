using Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MembershipAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var listakorisnika = PublicConnection.conn.GetList<Users>("where aktivan = 1");




            var ulogirani = System.Web.HttpContext.Current.Session["userName"];
            if (ulogirani == null)
            {
                return View("Login");
            }
            else {
                var IndexModel = new IndexView
                {
                    UsersCount = DBHelper.GetActiveUsersCount(),
                    UsersList = new List<Users>(),
                    ClanarineList = new List<ClanarineVSUsers>()

                };

                var UserListTemp = DBHelper.GetActiveUsersList();
                IndexModel.UsersList = UserListTemp.ToList();
                IndexModel.ClanarineList = DBHelper.GetClanarineList();

                return View(IndexModel);
            }

            
        }

        public ActionResult UserProfile(Int32 ID = 1, string _profile_update="", HttpPostedFileBase file = null)
        {

            var indexModel = new Users();
            indexModel = DBHelper.GetUser(ID);
            

            ViewBag.Message = "Your application description page.";

            if (_profile_update != "") {
                string user_folder = Server.MapPath("~/UserFiles/uid" + ID.ToString());
                var userToUpdate = indexModel;
                var slikaDBpath = DBHelper.UploadFile(file, ID, user_folder);
                userToUpdate.Slika = slikaDBpath;
                PublicConnection.conn.Update(indexModel);
                System.Web.HttpContext.Current.Session["korisnikSlika"] = null;
                System.Web.HttpContext.Current.Session["korisnikSlika"] = slikaDBpath;
            }



            return View(indexModel);
        }

        public ActionResult Login(string email = "", string pass="")
        {
            string emailVar = email;
            string passVar = pass;
            var forwardTo = "";

            var user = DBHelper.Login(emailVar, passVar);

            if (user != null)
            {

                if (string.IsNullOrWhiteSpace(forwardTo) && forwardTo != "")
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
                else
                {
                    
                    forwardTo = Url.Action("Userprofile", "Home", new { id = user.Id });
                    return Redirect(forwardTo);
                }
            
            }

            return View();
        }

        public ActionResult Logout()
        {
            DBHelper.Logout();

            return Redirect(Url.Action("Login", "Home"));
        }

        public ActionResult RegisterUser(string ime, string prezime, string tel, string pass, string passchecked, string email)
        {

            var forwardTo = Url.Action("Index", "Home");
            var emailVar = email;
            if ((!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(pass)) && (pass == passchecked )) {
                var recordCount = PublicConnection.conn.RecordCount<Users>("where email = @email", new { email = emailVar });

                if (recordCount == 0)
                {
                    var userReg = new Users();

                    userReg.Ime = ime;
                    userReg.Prezime = prezime;
                    userReg.Email = email;
                    userReg.Tel = tel;
                    userReg.Pass = pass;
                    userReg.Aktivan = true;

                    var uid = DBHelper.SaveUser(userReg);

                    forwardTo = Url.Action("Userprofile", "Home", new { id = uid });

                }
                else {
                    forwardTo = forwardTo;
                }
            
            }
         
            return Redirect(forwardTo);
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Message(Int32 _userId = 1, string _message_from_user = "", string subject = "Upit od člana", string msg ="", string mailto = "mzubanovic@gmail.com")
        {

            var user = new Users();
            user = DBHelper.GetUser(_userId);

            bool jelPoslao = false;

            if (_message_from_user == "true" && !string.IsNullOrWhiteSpace(msg))
            {
                var punoIme = user.Ime + " " + user.Prezime;
               jelPoslao = DBHelper.SendMail(punoIme, mailto, user.Email, subject,msg);
            }

            var porukaZaKorisnika = user.Ime;

            if (jelPoslao == true) {
                porukaZaKorisnika += ", poruka je uspješno poslana";

            } else
            {
                porukaZaKorisnika += ", došlo je do pogreške. Kontaktirajte mene. Ak morate.";
            }

            ViewBag.Message = porukaZaKorisnika;

            return View(user);
        }

    }
}