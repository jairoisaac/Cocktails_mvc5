//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cocktails07.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CocktailCordial
    {
        public long CocktailId { get; set; }
        public long Id { get; set; }
        public float Amount { get; set; }
    
        public virtual CockTail CockTail { get; set; }
        public virtual Cordial Cordial { get; set; }
    }
}
