using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssistModel.DataModel
{
    public class Catalog
    {
        public string Name { get; set; }

        public List<Theme> Themes { get; set; }
    }
}
