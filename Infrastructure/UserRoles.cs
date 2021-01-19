namespace LaprTrackr.Backend.Infrastructure
{
    public class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string UserAndAdmin = Admin + "," + User;
    }
}
