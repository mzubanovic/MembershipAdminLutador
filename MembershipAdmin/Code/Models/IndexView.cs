using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembershipAdmin
{
    public class IndexView 
    {
        public int UsersCount { get; set; }
        public List<Users> UsersList { get; set; }

        public List<ClanarineVSUsers> ClanarineList { get; set; }

        //public int UserId { get; set; }
        //public bool Uplatio { get; set; }
        //public DateTime DatumUplate { get; set; }

        public IndexView()
        {

        }
    }
}