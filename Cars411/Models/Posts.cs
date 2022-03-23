using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cars411.Models
{
    public class Posts
    {
        public int PostID { get; set; }
        public string PostTitle { get; set; }

        public string PostImage { get; set; }
       
        public Nullable<decimal> ServiceCost { get; set; }
        //public Nullable<int> Relaibility { get; set; }
        //public Nullable<int> Comfort { get; set; }
        //public Nullable<int> Safety { get; set; }
       
        public string Maketitle { get; set; }
        public string Modeltitle { get; set; }
        public string Yeartitle { get; set; }
        public string PublishBy { get; set; }
    }
}