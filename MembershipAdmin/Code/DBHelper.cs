using Dapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;



namespace MembershipAdmin
{
    public class DBHelper
    {

        public static List<ClanarineVSUsers> GetClanarineList()
        {
            var result = new List<ClanarineVSUsers>();
            var ClanarineListTemp = PublicConnection.conn.Query<ClanarineVSUsers>("SELECT c.userID, u.Ime, u.Prezime, c.Uplatio, c.ZaMjesec,c.DatumUplate " +
                "FROM Clanarine c " +
                "INNER JOIN Users u on c.UserID = u.Id " +
                "order by ZaMjesec desc ");
            if (ClanarineListTemp != null)
            {
                result = ClanarineListTemp.ToList();
            }
            else {
                result = null;
            }
            return result;
        }

        public static List<Users> GetActiveUsersList()
        {
            var result = new List<Users>();
            var UserListTemp = PublicConnection.conn.GetList<Users>("where Aktivan = 1");
            if (UserListTemp != null)
            {
                result = UserListTemp.ToList();
            }
            else {
                result = null;
            }
            return result;
        }

        public static int GetActiveUsersCount() {
            var result = 0;

            var recordCount = PublicConnection.conn.RecordCount<Users>("where aktivan = 1");
            if (recordCount > 0) {
                result = recordCount;
            }

            return result;
        }

        public static Users GetUser(int ID = 0)
        {
            var user = PublicConnection.conn.Get<Users>(ID);

            return user;
        }

        public static int? SaveUser(Users RegUser)
        {
            int? result = 0;

            result = PublicConnection.conn.Insert<Users>(RegUser);


            //result = PublicConnection.conn.Get();

            return result;
        }

        public static string UploadFile(HttpPostedFileBase file, int uid = 0, string user_folder = "")
        {
            string result = "";
            var ID = uid;
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0 && file.ContentLength < (5 * 1024 * 1024))
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                    || Path.GetExtension(file.FileName).ToLower() != ".png"
                    || Path.GetExtension(file.FileName).ToLower() != ".jpeg")
                {

                    bool folder_exists = Directory.Exists(user_folder);

                    if (!folder_exists)
                    {
                        Directory.CreateDirectory(user_folder);
                    }

                    var fileName = Path.GetFileName(file.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(user_folder, fileName);
                    file.SaveAs(path);
                }
                result = "/uid" + ID + "/" + file.FileName;
            }

            return result;
        }

        public static Users Login(string emailVar, string passVar)
        {

            var user = PublicConnection.conn.GetList<Users>("where email = @email and Pass = @pass", new { email = emailVar, pass = passVar }).FirstOrDefault();

            if (user != null)
            {
                System.Web.HttpContext.Current.Session["userName"] = user.Ime;
                System.Web.HttpContext.Current.Session["userEmail"] = user.Email;
                System.Web.HttpContext.Current.Session["userId"] = user.Id;
                if (!string.IsNullOrEmpty(user.Slika))
                {
                    System.Web.HttpContext.Current.Session["korisnikSlika"] = user.Slika;
                }
                //HttpContext.Current.Session[EnumStrings.userSession] = user;

            }
            return user;
        }

        public static bool Logout()
        {

            System.Web.HttpContext.Current.Session["userName"] = null;
            System.Web.HttpContext.Current.Session["userEmail"] = null;
            System.Web.HttpContext.Current.Session["userId"] = null;

            return true;
        }


        public static int GetClanarinaPayed()
        {
            var result = 0;


            var recordCount = PublicConnection.conn.RecordCount<Clanarine>("where uplatio = 1 and month(zamjesec) = MONTH(getdate())");
            if (recordCount > 0)
            {
                result = recordCount;
            }

            return result;
        }
        public static bool setClanarina(DateTime zaMjesec, int UserID = 0)
        {
            bool result = false;
            //ukoliko ne postoji zapis napraviti insert, također provjeriti datum plaćanja (and datum =) 
            //var selectedClanarina = PublicConnection.conn.Get<Clanarine>(UserID);
            var uidVar = UserID;
            var mjesecVar = zaMjesec;
            var selectedClanarina = PublicConnection.conn.GetList<Clanarine>("where  userId = @uid and ZaMjesec = @zamjesec", new { zamjesec = mjesecVar, uid = uidVar }).FirstOrDefault();
            if (selectedClanarina != null)
            {
                selectedClanarina.DatumUplate = DateTime.Now;
                if (selectedClanarina.Uplatio == true)
                {
                    selectedClanarina.Uplatio = false;
                }
                else
                {
                    selectedClanarina.Uplatio = true;
                }

                selectedClanarina.ZaMjesec = zaMjesec;

                PublicConnection.conn.Update<Clanarine>(selectedClanarina);

                result = true;
            }
            else {
                //var selectedUser = PublicConnection.conn.Get<Users>(UserID);
                var clanarina = new Clanarine();


                clanarina.UserID = UserID;
                clanarina.Uplatio = true;
                clanarina.DatumUplate = DateTime.Now;
                clanarina.ZaMjesec = zaMjesec;

                PublicConnection.conn.Insert<Clanarine>(clanarina);

            }

            return result;
        }

        public static bool SendMail(string username = "", string mailto = "", string mailfrom="", string subject = "" ,string msg = "") 
        {
            var result = false;
          
            SmtpClient client = new SmtpClient();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailfrom);
            mail.To.Add(mailto);
            mail.Subject = subject;
            mail.Body = msg;

            try
            {
                client.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

           return result;
        }
    }
}