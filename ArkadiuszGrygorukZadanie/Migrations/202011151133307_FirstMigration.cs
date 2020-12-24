namespace ArkadiuszGrygorukZadanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Moves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IMDbId = c.String(),
                        Name = c.String(nullable: false),
                        Raiting = c.Single(nullable: false),
                        ImageUrl = c.String(),
                        RelaseDate = c.String(),
                        Descryption = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Moves");
        }
    }
}
