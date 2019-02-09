using EFPrototype.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace EFPrototype.Contexts
{
    class UserRoleContext : DbContext
    {
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}
