using System;

namespace StudyAssistModel.DataModel
{
    /// <summary>
    /// Элемент вопроса на изучении
    /// </summary>
    public class IssueUnderStudy
    {
        public int? IssueUnderStudyId { get; set; }

        public int IssueId { get; set; }

        public int? UserId { get; set; }

        public DateTime? RepeateDate { get; set; }

        public int StudyLevel { get; set; }

        public Issue Issue { get; set; }
    }
}
