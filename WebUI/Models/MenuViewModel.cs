using System.Collections.Generic;

namespace WebUI.Models
{
    public class MenuViewModel
    {
        public IEnumerable<string> MenuLinks { get; set; }
        public IEnumerable<string> AccountLinks { get; set; }
        public string SelectedLink { get; set; }
    }
}