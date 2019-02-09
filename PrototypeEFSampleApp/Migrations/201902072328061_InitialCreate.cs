namespace PrototypeEFSampleApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleEntities",
                c => new
                    {
                        RoleId = c.Long(nullable: false, identity: true),
                        GUID = c.Guid(nullable: false),
                        Name = c.String(),
                        AdminAccess = c.Boolean(nullable: false),
                        CreateAccess = c.Boolean(nullable: false),
                        UpdateAccess = c.Boolean(nullable: false),
                        DeleteAccess = c.Boolean(nullable: false),
                        ReadAccess = c.Boolean(nullable: false),
                        UserEntity_UserId = c.Long(),
                    })
                .PrimaryKey(t => t.RoleId)
                .ForeignKey("dbo.UserEntities", t => t.UserEntity_UserId)
                .Index(t => t.UserEntity_UserId);
            
            CreateTable(
                "dbo.UserEntities",
                c => new
                    {
                        UserId = c.Long(nullable: false, identity: true),
                        GUID = c.Guid(nullable: false),
                        Name = c.String(),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleEntities", "UserEntity_UserId", "dbo.UserEntities");
            DropIndex("dbo.RoleEntities", new[] { "UserEntity_UserId" });
            DropTable("dbo.UserEntities");
            DropTable("dbo.RoleEntities");
        }
    }
}
