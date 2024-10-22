using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using StudyAssist.Model;

namespace KnowledgeDataAccessApi.Validators
{
    /// <summary>
    /// Валидатор для задачи на изучении.
    /// </summary>
    public class IssueUnderStudyValidator 
        : AbstractValidator<IssueUnderStudy>
    {
        public IssueUnderStudyValidator()
        {
            RuleFor(ius => ius.IssueId).NotEqual(0);
            RuleFor(ius => ius.StudyLevel).GreaterThan(-1)
                .WithMessage(MessageTemplates.STUDY_LEVEL_CONSTRAINT_ERR);
            RuleFor(ius => ius.RepeateDate)
                .NotNull()
                .GreaterThan(System.DateTime.Today.AddDays(-1))
                .WithMessage(MessageTemplates.REPEATE_DATE_CONSTRAINT_ERR);
        }
    }
}
