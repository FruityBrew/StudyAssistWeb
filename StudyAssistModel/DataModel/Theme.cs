using System.Collections.Generic;

namespace StudyAssistModel.DataModel
{
    public class Theme
    {
        public  int ThemeId { get; set; }

        public string Name { get; set; }

        public List<Issue> Issues { get; set; }
    }
}
