using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.Results;
using Learnix.Commons.Domain.ValueObjects;
using Users.Domain.DomainEvents;
using Users.Domain.Errors;
using Users.Domain.Models;

namespace Users.Domain.Entities
{
    public sealed class User : Entity, IAggregateRoot
    {
        private readonly List<Role> _roles = [];

        private User(string fullName, string email, DateTime birthDate)
        {
            Name = fullName;
            Email = email;
            Age = birthDate;
            Validate();
        }

        private User()
        { }

        public Name Name { get; private set; } = default!;
        public Email Email { get; private set; } = default!;
        public Age Age { get; private set; } = default!;
        public Guid IdentityId { get; private set; } = default!;
        public IReadOnlyCollection<Role> Roles => [.. _roles];

        public static Result<User> Create(string fullName, string email, DateTime birthDate)
        {
            var user = new User(fullName, email, birthDate);

            user._roles.Add(Role.Standard);

            user.AddDomainEvent(new UserCreatedDomainEvent(user.Id));

            return user;
        }

        public void SetIdentityId(Guid id)
        {
            IdentityId = id;
            AssertionConcern.EnsureFalse(IdentityId == Guid.Empty, UserErrors.IdentityIdMustBeNotEmpty.Description);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureNotNull(Name, UserErrors.NameMustBeNotNull.Description);
            AssertionConcern.EnsureNotNull(Email, UserErrors.EmailMustBeNotNull.Description);
            AssertionConcern.EnsureNotNull(Age, UserErrors.AgeMustBeNotNull.Description);
            AssertionConcern.EnsureNotNull(IdentityId, UserErrors.IdentityIdMustBeNotNull.Description);
        }
    }
}