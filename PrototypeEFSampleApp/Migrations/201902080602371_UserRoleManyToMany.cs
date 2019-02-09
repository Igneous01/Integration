namespace PrototypeEFSampleApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRoleManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RoleEntities", "UserEntity_UserId", "dbo.UserEntities");
            DropIndex("dbo.RoleEntities", new[] { "UserEntity_UserId" });
            CreateTable(
                "dbo.UserEntityRoleEntities",
                c => new
                    {
                        UserEntity_UserId = c.Long(nullable: false),
                        RoleEntity_RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserEntity_UserId, t.RoleEntity_RoleId })
                .ForeignKey("dbo.UserEntities", t => t.UserEntity_UserId, cascadeDelete: true)
                .ForeignKey("dbo.RoleEntities", t => t.RoleEntity_RoleId, cascadeDelete: true)
                .Index(t => t.UserEntity_UserId)
                .Index(t => t.RoleEntity_RoleId);
            
            DropColumn("dbo.RoleEntities", "UserEntity_UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RoleEntities", "UserEntity_UserId", c => c.Long());
            DropForeignKey("dbo.UserEntityRoleEntities", "RoleEntity_RoleId", "dbo.RoleEntities");
            DropForeignKey("dbo.UserEntityRoleEntities", "UserEntity_UserId", "dbo.UserEntities");
            DropIndex("dbo.UserEntityRoleEntities", new[] { "RoleEntity_RoleId" });
            DropIndex("dbo.UserEntityRoleEntities", new[] { "UserEntity_UserId" });
            DropTable("dbo.UserEntityRoleEntities");
            CreateIndex("dbo.RoleEntities", "UserEntity_UserId");
            AddForeignKey("dbo.RoleEntities", "UserEntity_UserId", "dbo.UserEntities", "UserId");
        }
    }
}
