//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cars411.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblPost
    {
        public int PostID { get; set; }
        public string PostTitle { get; set; }
        public string PostDescription { get; set; }
        public int MakeID { get; set; }
        public int ModelID { get; set; }
        public int YearID { get; set; }
        public string PostImage { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<decimal> ServiceCost { get; set; }
        public Nullable<int> Relaibility { get; set; }
        public Nullable<int> Comfort { get; set; }
        public Nullable<int> Safety { get; set; }
        public string PublishBy { get; set; }
    }
}
