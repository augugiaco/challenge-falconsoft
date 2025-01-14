namespace FalconSoftChallenge.Entities
{
    public class User
    {
        public Guid Id { get; init; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Password { get; private set; }

        public UserStatus Status { get; private set; }

        //Empty constructor added for Entity Framework purposes.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public User()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {

        }

        public User(string name, string email, string password)
        {
            Id = Guid.NewGuid();
            Status = UserStatus.Active;
            Name = name;
            Email = email;
            Password = password;
        }
    }

    public enum UserStatus
    {
        Inactive = 0,
        Active = 1
    }
}
