namespace LF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveitemspropertisActive : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Items", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Items", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}
