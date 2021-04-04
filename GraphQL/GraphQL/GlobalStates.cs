namespace GraphQL.GraphQL
{
    public abstract class GlobalStates
    {
        public abstract class HttpContext
        {
            public const string Claims = nameof(Claims);
            public const string UserId = nameof(UserId);
        }
    }

}