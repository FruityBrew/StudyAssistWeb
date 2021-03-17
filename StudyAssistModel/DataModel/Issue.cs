using System;
using System.Collections.Generic;
using System.Text;

namespace StudyAssistModel.DataModel
{
    public class Issue
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        public DateTime? RepeateDate { get; set; }
    }
}
