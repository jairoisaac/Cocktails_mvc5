using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cocktails07.Models
{
    public class CocktailSumary
    {
        //public int CocktailId { get; set; }
        //public string Cocktail { get; set; }
        public IEnumerable<SelectListItem> Cocktails { get; set; }
    }
    public class IngredientSumary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<SelectListItem> Cocktails { get; set; }
    }
    public class CocktaiIngredients
    {
        public int CocktailId { get; set; }
        public string Name { get; set; }
        public IEnumerable<SelectListItem> Liquors { get; set; }
        public IEnumerable<SelectListItem> NoAlcohols { get; set; }
        public IEnumerable<SelectListItem> Cordials { get; set; }
        public IEnumerable<SelectListItem> Garnishes { get; set; }
        public IEnumerable<SelectListItem> Glass { get; set; }
        public IEnumerable<SelectListItem> Process { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }

    public class Ingredient
    {
        public int CocktailId { get; set; }
        public long Id { get; set; }
        public string Type { get; set; }
        public float Amount { get; set; }
        //public double Amount { get; set; }
        public string Name { get; set; }
    }
    public class component
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
    public class IngredientItsCocktails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<component> ItsCocktails { get; set; }
    }

}
