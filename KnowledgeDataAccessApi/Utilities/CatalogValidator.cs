using FluentValidation;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApi.Utilities
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
                .WithMessage("Id must be set by database");
            RuleFor(cat => cat.Themes).Null().Empty();
        }
    }


}
