namespace GraphQL
{
    public abstract class GlobalStates
    {
        public abstract class HttpContext
        {
            public const string Claims = nameof(Claims);
            public const string UserUid = nameof(UserUid);
        }
    }

}