namespace PrototypeEFSampleApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedRoleId : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserEntities", "RoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserEntities", "RoleId", c => c.Long(nullable: false));
        }
    }
}
