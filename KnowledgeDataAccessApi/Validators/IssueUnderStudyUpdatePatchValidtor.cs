using FluentValidation;
using KnowledgeDataAccessApi.Constants;
using Microsoft.AspNetCore.JsonPatch;
using StudyAssist.Model;
using System;
using System.Linq;

namespace KnowledgeDataAccessApi.Validators
{
    /// <summary>
    /// Валидатор патча обновления задачи на изучении.
    /// </summary>
    public class IssueUnderStudyUpdatePatchValidtor
        : AbstractValidator<JsonPatchDocument<IssueUnderStudy>>
    {
        public IssueUnderStudyUpdatePatchValidtor()
        {
            RuleFor(patch => patch.Operations)
                .NotNull()
                .NotEmpty()
                .Must(s => s.Count <= 2)
                .Must(s => s.All(op => op.op == "replace"))
                .Must(s => s.All(
                    op => op.path == "studylevel" || op.path == "repeateDate"))
                .WithMessage(MessageTemplates.JSON_UPD_PATCH_ERR);

            RuleFor(path => path.Operations)
                .Must(operations => operations
                    .Where(op => op.path == "studylevel")
                    .All(op => int.TryParse(op.value.ToString(), out int val)
                            && val >= 0))
                .WithMessage(MessageTemplates.STUDY_LEVEL_CONSTRAINT_ERR);

            RuleFor(path => path.Operations)
                .Must(operations => operations
                    .Where(op => op.path == "repeatedate")
                    .All(op => DateTime.TryParse(op.value.ToString(), out DateTime val)
                        && val >= DateTime.Today))
                .WithMessage(MessageTemplates.REPEATE_DATE_CONSTRAINT_ERR);
        }
    }
}
