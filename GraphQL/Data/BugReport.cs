namespace GraphQL.Data
{
    public class BugReport
    {
        /*
         * Primary Key
         */
        [ID]
        public int Id { get; set; }

        /*
         * Fields
         */
        public string Description { get; set; } = "";

        /**
         * Entity Mappings
         */
        public User ReportingUser { get; set; } = new();
 
        /*
         * Unmapped Resolver Fields
         */
    }

}