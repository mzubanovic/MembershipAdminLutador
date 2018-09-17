using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembershipAdmin
{
    public class Users
    {

        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Belt { get; set; }
        public int? RoleID { get; set; }
        public string Napomena { get; set; }
        public string Pass { get; set; }
        public string Slika { get; set; }
        public bool Aktivan { get; set; }

        public Users()
        {
            
        }
    }
}