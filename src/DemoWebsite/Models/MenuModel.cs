using System.Collections.Generic;
using Lightcore.Kernel.Data;

namespace WebApp.Models
{
    public class MenuModel
    {
        public IEnumerable<IItem> MainNavigation { get; set; }
        public IEnumerable<IItem> LanguageNavigation { get; set; }
    }
}