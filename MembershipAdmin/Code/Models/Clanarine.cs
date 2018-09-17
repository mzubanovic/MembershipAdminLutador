using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MembershipAdmin
{
    public class Clanarine
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool Uplatio { get; set; }
        public DateTime DatumUplate {get; set;}
        public DateTime ZaMjesec { get; set; }
        public bool Opomena { get; set; }
        //public string Prezime { get; set; }
        //public string Ime { get; set; }
        public Clanarine()
        {

        }
    }
}