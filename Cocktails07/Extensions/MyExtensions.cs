using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System;
using System.Text;
using Cocktails07.Models;
public static class MyExtensions
{
    public static string MyPictureContent(this UrlHelper helper, string name, string size)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = "default";
        }
        return helper.Content(string.Format("~/Images/{0}_{1}.png", name, size));

    }

    public static Image ResizeTo(this Image src, int w, int h)
    {
        var bmp = new Bitmap(w, h);
        var rect = new Rectangle(0, 0, w, h);
        if (src.Height > src.Width)
        {
            rect.Width = (int)((double)(rect.Width) * (double)(src.Width) / (double)(src.Height));
            rect.X += (w - rect.Width) / 2;
        }
        else
        {
            rect.Height = (int)((double)(rect.Height) * (double)(src.Height) / (double)(src.Width));
            rect.Y += (h - rect.Height) / 2;
        }
        Graphics.FromImage(bmp).SmoothingMode = SmoothingMode.AntiAlias;
        Graphics.FromImage(bmp).InterpolationMode = InterpolationMode.HighQualityBicubic;
        Graphics.FromImage(bmp).CompositingQuality = CompositingQuality.HighQuality;
        Graphics.FromImage(bmp).PixelOffsetMode = PixelOffsetMode.HighQuality;
        Graphics.FromImage(bmp).DrawImage(src, rect, 0, 0, src.Width, src.Height, GraphicsUnit.Pixel);
        return bmp;
    }
    public static MvcHtmlString ActionImage(this HtmlHelper html, string imagePath, string alt, string cssClass,
    string action, string controllerName, object routeValues)
    {
        var currentUrl = new UrlHelper(html.ViewContext.RequestContext);
        var anchorTagBuilder = new TagBuilder("a"); // build the <a> tag
        anchorTagBuilder.MergeAttribute("href", currentUrl.Action(action, controllerName, routeValues));
        anchorTagBuilder.MergeAttribute("class", "imagelink");
        anchorTagBuilder.InnerHtml = ImageTagOnly(imagePath, alt, cssClass); // include the <img> tag inside
        string anchorHtml = anchorTagBuilder.ToString(TagRenderMode.Normal);
        return MvcHtmlString.Create(anchorHtml);
    }
    public static MvcHtmlString TableImage(this HtmlHelper helper, string name, IList<component> items, string controllerName, 
        string alt, string cssClass,long mRows)
    {
        // This is to display the images of all liquors or cordials or garnishes or no alcohols or glasses ...
        //var currentUrl = new UrlHelper(helper.ViewContext.RequestContext);
        if (items == null || items.Count == 0 || string.IsNullOrEmpty(name))
        {
            return MvcHtmlString.Create(string.Empty);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("\t<tr>");
        int i = 1;
        foreach (var item in items)
        {
            //string imagePath = "~/Images/" + item.Name + "_bigger.png";
            string imagePath = "/Images/" + item.Name + "_bigger.png";
            string anchorHtml = ImageLink(imagePath, alt, cssClass, controllerName, item.Id);
            //sb.AppendFormat("\t\t<td>{0}<br>{1}</td>\n",item.Name,item.Id);
            sb.AppendFormat("\t\t<td>{0}<br>{1}</td>\n", anchorHtml, item.Name.Replace("_"," "));

            if ((i % mRows) == 0 )
            {
                sb.AppendLine("\t</tr>");
                sb.AppendLine("\t<tr>");
            }
            i++;
        }
        sb.AppendLine("\t</tr>");
        TagBuilder builder = new TagBuilder("table");
        builder.MergeAttribute("name", name);
        builder.MergeAttribute("class", "center");
        builder.InnerHtml = sb.ToString();

        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }
    private static string ImageLink(string imagePath, string alt, string cssClass, string controllerName, long Id)
    {
        // To build a link with an image for the liquor, garnishes, ... only.
        var anchorTagBuilder = new TagBuilder("a"); // build the <a> tag    
        //string Test00 = currentUrl.Action("Details", controllerName, item.Id);
        anchorTagBuilder.MergeAttribute("href", "/" + controllerName + "/" + "Details" + "/" + Id.ToString());
        if (controllerName == "Liquor" || controllerName == "Cordial")
        { anchorTagBuilder.MergeAttribute("class", "imagelink2"); }
        else
        { anchorTagBuilder.MergeAttribute("class", "imagelink1"); }
        anchorTagBuilder.InnerHtml = ImageTagOnly(imagePath, alt, cssClass); // include the <img> tag inside
        string anchorHtml = anchorTagBuilder.ToString(TagRenderMode.Normal);
        return anchorHtml;
    }
    public static MvcHtmlString TableCocktail(this HtmlHelper helper, string name, IList<Ingredient> items, string alt, string cssClass, long mRows)
    {
        //this is to display the graphic recipe of the cocktail, ej vodka,lime juice,tonic ...
        //var currentUrl = new UrlHelper(helper.ViewContext.RequestContext);
        // All the exemptions are applied here: Centain ammounts or cocktails are change to literals,
        // When the row reach certain ammount the line is split,
        // To avoid the process of pouring being display.
        if (items == null || items.Count == 0 || string.IsNullOrEmpty(name))
        {
            return MvcHtmlString.Create(string.Empty);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("\t<tr>");
        //int i = 0;
        int i = 1;
        foreach (var item in items)
        {
            //string imagePath = "~/Images/" + item.Name + "_bigger.png";
            string imagePath = "/Images/" + item.Name + "_bigger.png";
            string ImageComponent="";
            string mName = item.Name;
            if (item.Type == "P"&& item.Name == "Pouring")
                { ImageComponent = "";
                    mName = "";
                }
            else
            { ImageComponent = ImageTagOnly(imagePath, alt, cssClass); }
            String CAmount = "";
            // The following if is for showing words instead of numbers 
            // This should be somewhere in a file for localization IMPROVE PLS
            if (item.Amount == 6)
                {CAmount = "Fill With ";}
            else if (item.Amount == 3)
                {CAmount = "Half of"; }
            else if (item.Amount == 0.25)
                { CAmount = "Splash of "; }
            else if (item.Type == "G" && item.CocktailId != 40)
                { CAmount = "Garnish with a "; }
            else if (item.Type == "G" && item.CocktailId == 40)
                { CAmount = "Moddled "; }
            else if (item.Type == "S" | item.Type == "P")
                { }
            else
                { CAmount = item.Amount.ToString()+" Oz of"; }
            sb.AppendFormat("\t\t<td>{0}<br>{1}<br>{2}</td>\n", ImageComponent, CAmount, mName.Replace("_", " "));
            if ((i % mRows) == 0 )
            {
               sb.AppendLine("\t</tr>");
               sb.AppendLine("\t<tr>");
            }
            i++;
        }
        sb.AppendLine("\t</tr>");
        TagBuilder builder = new TagBuilder("table");
        builder.MergeAttribute("name", name);
        builder.InnerHtml = sb.ToString();

        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }
    private static string ImageTagOnly(string imagePath, string alt, string cssClass)
    {
        var imgTagBuilder = new TagBuilder("img"); // build the <img> tag
        //imgTagBuilder.MergeAttribute("src", currentUrl.Content(imagePath));
        imgTagBuilder.MergeAttribute("src", imagePath);
        imgTagBuilder.MergeAttribute("alt", alt);
        imgTagBuilder.MergeAttribute("class", cssClass);
        string imgHtml = imgTagBuilder.ToString(TagRenderMode.SelfClosing);
        return imgHtml;

    }
    public static MvcHtmlString TableCocktailTextOnly(this HtmlHelper helper, string name, IList<Ingredient> items, string alt, string cssClass, long mRows)
    {
        //this is to display the graphic recipe of the cocktail, ej vodka,lime juice,tonic ...
        //var currentUrl = new UrlHelper(helper.ViewContext.RequestContext);
        // All the exemptions are applied here: Centain ammounts or cocktails are change to literals,
        // When the row reach certain ammount the line is split,
        // To avoid the process of pouring being display.
        if (items == null || items.Count == 0 || string.IsNullOrEmpty(name))
        {
            return MvcHtmlString.Create(string.Empty);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("\t<tr>");
        //int i = 0;
        int i = 1;
        foreach (var item in items)
        {
            //string imagePath = "~/Images/" + item.Name + "_bigger.png";
            //string imagePath = "/Images/" + item.Name + "_bigger.png";
            //string ImageComponent = "";  // To remove the image and put only text
            string mName = item.Name;
            if (item.Type == "P" && item.Name == "Pouring")
            {
                //ImageComponent = "";
                mName = "";
            }
            //else
            //{ ImageComponent = ImageTagOnly(imagePath, alt, cssClass); }

            //if (item.Type == "P" && item.Name == "Straining")
            //    mName = "Shake and Strain";

            //sb.AppendFormat("\t\t<td>{0}<br>{1}</td>\n",item.Name,item.Id);
            String CAmount = "";
            // The following if is for showing words instead of numbers 
            // This should be somewhere in a file for localization IMPROVE PLS
            if (item.Amount == 6)
            { CAmount = "Fill With "; }
            else if (item.Amount == 3)
            { CAmount = "Half of"; }
            else if (item.Amount == 0.25)
            { CAmount = "Splash of "; }
            else if (item.Type == "G")
            { CAmount = "Garnish with a "; }
            else if (item.Type == "S" | item.Type == "P")
            { }
            else
            { CAmount = item.Amount.ToString() + " Oz of"; }
            //sb.AppendFormat("\t\t<td>{0}<br>{1}<br>{2}</td>\n", ImageComponent, CAmount, mName.Replace("_", " "));
            sb.AppendFormat("\t\t<td style='border:1px solid red;padding: 1em'>{0}<br>{1}</td>\n", CAmount, mName.Replace("_", " "));
         

                //if ((i % mRows) == 0 & !(i == 0))
            if ((i % mRows) == 0)
            {
                sb.AppendLine("\t</tr>");
                sb.AppendLine("\t<tr>");
            }
            i++;
        }
        sb.AppendLine("\t</tr>");
        TagBuilder builder = new TagBuilder("table");
        builder.MergeAttribute("name", name);
        builder.MergeAttribute("class", "textOnly");
        builder.InnerHtml = sb.ToString();

        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }
    public static MvcHtmlString TableImageTextOnly(this HtmlHelper helper, string name, IList<component> items, string controllerName,
        string alt, string cssClass, long mRows)
    {
        // This is to display the images of all liquors or cordials or garnishes or no alcohols or glasses ...
        //var currentUrl = new UrlHelper(helper.ViewContext.RequestContext);
        if (items == null || items.Count == 0 || string.IsNullOrEmpty(name))
        {
            return MvcHtmlString.Create(string.Empty);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("\t<tr>");
        int i = 1;
        foreach (var item in items)
        {
            //string imagePath = "/Images/" + item.Name + "_bigger.png";
            string anchorHtml = ImageLink4TextOnly(alt, cssClass, controllerName, item.Id,item.Name);
            //sb.AppendFormat("\t\t<td>{0}<br>{1}</td>\n",item.Name,item.Id);
            sb.AppendFormat("\t\t<td>{0}</td>\n", anchorHtml);

            if ((i % mRows) == 0)
            {
                sb.AppendLine("\t</tr>");
                sb.AppendLine("\t<tr>");
            }
            i++;
        }
        sb.AppendLine("\t</tr>");
        TagBuilder builder = new TagBuilder("table");
        builder.MergeAttribute("name", name);
        builder.MergeAttribute("class", "center");
        builder.InnerHtml = sb.ToString();

        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }
    private static string ImageLink4TextOnly(string alt, string cssClass, string controllerName, long Id,string name)
    {
        // To build a link with an image for the liquor, garnishes, ... only.
        var anchorTagBuilder = new TagBuilder("a"); // build the <a> tag    
        //string Test00 = currentUrl.Action("Details", controllerName, item.Id);
        anchorTagBuilder.MergeAttribute("href", "/" + controllerName + "/" + "Details" + "/" + Id.ToString());
        if (controllerName == "Liquor" || controllerName == "Cordial")
        { anchorTagBuilder.MergeAttribute("class", "imagelink2"); }
        else
        { anchorTagBuilder.MergeAttribute("class", "imagelink1"); }
        anchorTagBuilder.InnerHtml = name.Replace("_"," ");
        //anchorTagBuilder.InnerHtml = ImageTagOnly(imagePath, alt, cssClass); // include the <img> tag inside
        string anchorHtml = anchorTagBuilder.ToString(TagRenderMode.Normal);
        return anchorHtml;
    }
    public static MvcHtmlString TableImage(this HtmlHelper helper, string name, IList<component> items, string controllerName,
        string actionName,string alt, string cssClass, long mRows)
    {
        // This is to display the images of the options to find a cocktail by list of search
        // it doesn't use Imagelink() IS A SPECIAL CASE OF TABLE
        //var currentUrl = new UrlHelper(helper.ViewContext.RequestContext);
        if (items == null || items.Count == 0 || string.IsNullOrEmpty(name))
        {
            return MvcHtmlString.Create(string.Empty);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("\t<tr>");
        int i = 1;
        foreach (var item in items)
        {
            //string imagePath = "~/Images/" + item.Name + "_bigger.png";
            string imagePath = "/Images/" + item.Name + "_bigger.png";

            var anchorTagBuilder = new TagBuilder("a"); // build the <a> tag    
            anchorTagBuilder.MergeAttribute("href", "/" + controllerName + "/" + item.Name);
            anchorTagBuilder.MergeAttribute("class", "imagelink2"); 
            anchorTagBuilder.InnerHtml = ImageTagOnly(imagePath, alt, cssClass); // include the <img> tag inside
            string anchorHtml = anchorTagBuilder.ToString(TagRenderMode.Normal);
           


            //string anchorHtml = ImageLink(imagePath, alt, cssClass, controllerName,actionName, item.Id);

            //sb.AppendFormat("\t\t<td>{0}<br>{1}</td>\n",item.Name,item.Id);
            sb.AppendFormat("\t\t<td>{0}<br>{1}</td>\n", anchorHtml, item.Name.Replace("_", " "));

            if ((i % mRows) == 0)
            {
                sb.AppendLine("\t</tr>");
                sb.AppendLine("\t<tr>");
            }
            i++;
        }
        sb.AppendLine("\t</tr>");
        TagBuilder builder = new TagBuilder("table");
        builder.MergeAttribute("name", name);
        builder.MergeAttribute("class", "center");
        builder.InnerHtml = sb.ToString();

        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }
    public static MvcHtmlString TableAllCocktails(this HtmlHelper helper, IList<component> items, long mRows)
    {
        // This is to display all the cocktails
        // 
        //var currentUrl = new UrlHelper(helper.ViewContext.RequestContext);
        if (items == null || items.Count == 0)
        {
            return MvcHtmlString.Create(string.Empty);
        }

        TagBuilder builder1 = new TagBuilder("p");
        builder1.InnerHtml = "Basic Cocktails";
        TagBuilder builder2 = new TagBuilder("div");
        builder2.MergeAttribute("class", "Title");
        builder2.InnerHtml = builder1.ToString();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("\t<div class=\"Row\">");
        int i = 1;
        foreach (var item in items)
        {

            var anchorTagBuilder = new TagBuilder("a"); // build the <a> tag    
            anchorTagBuilder.MergeAttribute("href", "/Cocktail/SeeCocktailNoAjx/" + item.Id.ToString());
            anchorTagBuilder.MergeAttribute("class", "mylink");
            anchorTagBuilder.MergeAttribute("data-role", "button");
            anchorTagBuilder.InnerHtml = item.Name.Replace("_", " "); // include the <img> tag inside
            string anchorHtml = anchorTagBuilder.ToString(TagRenderMode.Normal);
            sb.AppendFormat("\t\t<div class=\"Cell\">{0}</div>\n", anchorHtml);

            if ((i % mRows) == 0)
            {
                sb.AppendLine("\t</div>");
                sb.AppendLine("\t<div class=\"Row\">");
            }
            i++;
        }
        sb.AppendLine("\t</div>");
        TagBuilder builder = new TagBuilder("div");
        builder.MergeAttribute("class", "Table");
        //builder.MergeAttribute("data-role", "navbar");
        builder.MergeAttribute("data-theme", "b");
        builder.InnerHtml = sb.ToString();


        return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
    }

}

