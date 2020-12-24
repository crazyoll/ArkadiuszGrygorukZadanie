namespace ArkadiuszGrygorukZadanie.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NamesFixing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Moves", "Title", c => c.String(nullable: false));
            AddColumn("dbo.Moves", "Rating", c => c.Single(nullable: false));
            AddColumn("dbo.Moves", "ReleaseDate", c => c.String());
            AddColumn("dbo.Moves", "Description", c => c.String());
            DropColumn("dbo.Moves", "Name");
            DropColumn("dbo.Moves", "Raiting");
            DropColumn("dbo.Moves", "RelaseDate");
            DropColumn("dbo.Moves", "Descryption");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Moves", "Descryption", c => c.String());
            AddColumn("dbo.Moves", "RelaseDate", c => c.String());
            AddColumn("dbo.Moves", "Raiting", c => c.Single(nullable: false));
            AddColumn("dbo.Moves", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Moves", "Description");
            DropColumn("dbo.Moves", "ReleaseDate");
            DropColumn("dbo.Moves", "Rating");
            DropColumn("dbo.Moves", "Title");
        }
    }
}
