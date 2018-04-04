using System.Collections.Generic;

namespace WebUI.Models
{
    public class CriterionList
    {
        public List<Criterion> Criterions { get; set; }
    }

    public class Criterion
    {
        public string CriterionName { get; set; }
        public bool IsChecked { get; set; }
    }
}