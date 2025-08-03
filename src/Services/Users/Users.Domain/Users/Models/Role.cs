namespace Users.Domain.Users.Models
{
    public sealed class Role(string name)
    {
        public static readonly Role Administrator = new("Administrator");
        public static readonly Role Premium = new("Premium");
        public static readonly Role Standard = new("Standard");

        public string Name { get; private set; } = name;
    }
}