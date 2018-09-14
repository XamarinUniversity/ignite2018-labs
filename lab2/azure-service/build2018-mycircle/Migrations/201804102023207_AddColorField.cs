namespace build2018_mycircle.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CircleMessages", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CircleMessages", "Color");
        }
    }
}
