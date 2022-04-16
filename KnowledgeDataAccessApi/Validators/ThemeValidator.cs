using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApi.Validators
{
    /// <summary>
    /// Валидатор для добавления новой темы.
    /// </summary>
    public class ThemeValidator : AbstractValidator<Theme>
    {
        public ThemeValidator()
        {
            RuleFor(theme => theme.Name).NotNull().NotEmpty();
            RuleFor(theme => theme.ThemeId)
                .Equal(0)
                .WithMessage(MessageTemplates.DB_ENTITYID_RULE);
            RuleFor(theme => theme.Issues).Null().Empty();
        }
    }
}
