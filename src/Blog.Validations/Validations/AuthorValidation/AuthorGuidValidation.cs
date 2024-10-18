﻿using Blog.Translations.Abstractions;
using Blog.Translations.Constants;
using FluentValidation;

namespace Blog.Validations.Validations.AuthorValidation
{
    public class AuthorGuidValidation : AbstractValidator<Guid>
    {
        public AuthorGuidValidation(ITranslationResource translateResource)
        {
            RuleFor(x => x)
                .Must(delegate (Guid id)
                {
                    return id != Guid.Empty;
                })
                .WithErrorCode(translateResource.GetCodeResource(AuthorConstant.ValidationsIdEmpty))
                .WithMessage(translateResource.GetResource(AuthorConstant.ValidationsIdEmpty));
        }
    }
}
