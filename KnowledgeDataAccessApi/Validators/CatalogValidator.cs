using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using StudyAssist.Model;

namespace KnowledgeDataAccessApi.Validators
{
    /// <summary>
    /// Валидатор для добавления нового каталога
    /// </summary>
    public class CatalogValidator : AbstractValidator<Catalog>
    {
        public CatalogValidator()
        {
            RuleFor(cat => cat.Name).NotNull().NotEmpty();
            RuleFor(cat => cat.CatalogId)
                .Must(c => c.HasValue == false)
                .WithMessage(MessageTemplates.DB_ENTITYID_RULE);
            RuleFor(cat => cat.Themes).Null().Empty();
        }
    }
}
