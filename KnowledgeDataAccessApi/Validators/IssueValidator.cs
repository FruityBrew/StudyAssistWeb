using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApi.Validators
{
    public class IssueValidator : AbstractValidator<Issue>
    {
        public IssueValidator()
        {
            RuleFor(issue => issue.IssueId)
                .Equal(0)
                .WithMessage(MessageTemplates.DB_ENTITYID_RULE);
            RuleFor(issue => issue.Question)
                .NotNull()
                .NotEmpty()
                .Must(issue => issue.Length <= 200)
                .WithMessage(MessageTemplates.QUESTION_TEXT_CONSTRAINT_ERR);

            RuleFor(issue => issue.Answer).NotNull().NotEmpty();
        }
    }
}
