using System;

namespace StudyAssistModel.DataModel
{
    public class IssueUnderStudy
    {
        public int IssueUnderStudyId { get; set; }

        //public Issue Issue { get; set; }
        public int IssueId { get; set; }

        public int UserId { get; set; }

        public DateTime? RepeateDate { get; set; }

        public int StudyLevel { get; set; }
    }
}
