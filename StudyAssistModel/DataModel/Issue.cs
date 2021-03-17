using System;

namespace StudyAssistModel.DataModel
{
    public class Issue
    {
        public int IssueId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public DateTime? RepeateDate { get; set; }

        public int? StudyLevel { get; set; }
    }
}
