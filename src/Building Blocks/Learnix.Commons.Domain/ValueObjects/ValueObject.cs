﻿namespace Learnix.Commons.Domain.ValueObjects
{
    public abstract record ValueObject
    {
        protected abstract void Validate();
    }
}