using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ArkadiuszGrygorukZadanie.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DBconnection")
        {
            if (!Database.Exists()) ;
                //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, System.Data.Entity.Migrations.Configuration>(useSuppliedContext: true));
        }
        public DbSet<Move> Moves { get; set; }
    }
}