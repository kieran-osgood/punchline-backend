using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQL.Data
{
    public class BugReport
    {
        /*
         * Primary Key
         */
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