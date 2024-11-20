using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using StudyAssist.Model;

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
                .Null()
                .WithMessage(MessageTemplates.DB_ENTITYID_RULE);
            RuleFor(theme => theme.Issues).Null().Empty();
        }
    }
}
