using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using Microsoft.AspNetCore.JsonPatch;
using StudyAssistModel.DataModel;

namespace KnowledgeDataAccessApi.Validators
{
    /// <summary>
    /// Валидатор патча обновления Issue
    /// </summary>
    public class IssueUpdatePatchValidator 
        : AbstractValidator<JsonPatchDocument<Issue>>
    {
        public IssueUpdatePatchValidator()
        {
            RuleFor(patch => patch.Operations)
                .NotNull()
                .NotEmpty()
                .Must(s => s.Count <= 2)
                .Must(s => s[0].op == "replace")
                .Must(s => s[0].path == "question" || s[0].path == "answer")
                .Must(s => !string.IsNullOrWhiteSpace(s[0].value.ToString()))
                .WithMessage(MessageTemplates.JSON_UPD_PATCH_ERR);

            RuleFor(patch => patch.Operations)
                .Must(s => s[1].op == "replace")
                .Must(s => s[1].path == "question" || s[1].path == "answer")
                .Must(s => !string.IsNullOrWhiteSpace(s[1].value.ToString()))
                .When(s => s.Operations.Count == 2)
                .WithMessage(MessageTemplates.JSON_UPD_PATCH_ERR);
        }
    }
}
