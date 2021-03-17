using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssistModel.DataModel
{
    public class Theme
    {
        public string Name { get; set; }

        public List<Issue> Issues { get; set; }
    }
}
