using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApi.Validators
{
    /// <summary>
    /// Валидатор патча обновления каталога
    /// </summary>
    public class CatalogUpdatePatchValidator 
        : AbstractValidator<JsonPatchDocument<Catalog>>
    {
        public CatalogUpdatePatchValidator() =>
            RuleFor(patch => patch.Operations)
                .NotNull()
                .NotEmpty()
                .Must(s => s.Count == 1)
                .Must(s => s[0].op == "replace")
                .Must(s => s[0].path == "name")
                .Must(s => !string.IsNullOrWhiteSpace(s[0].value.ToString()))
                .WithMessage("JSonPatch is incorrect");
    }
}
