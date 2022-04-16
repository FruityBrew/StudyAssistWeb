using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using StudyAssistModel.DataModel;

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
                .Equal(0)
                .WithMessage(MessageTemplates.DB_ENTITYID_RULE);
            RuleFor(cat => cat.Themes).Null().Empty();
        }
    }
}
