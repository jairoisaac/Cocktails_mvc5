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
    
    public partial class Liquor
    {
        public Liquor()
        {
            this.CocktailLiquors = new HashSet<CocktailLiquor>();
        }
    
        public long Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<CocktailLiquor> CocktailLiquors { get; set; }
    }
}
