using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace EFPrototype.Entities
{
    public class UserEntity
    {
        [Key]
        public long UserId { get; set; }
        public Guid GUID { get; set; }
        public string Name { get; set; }

        public virtual ICollection<RoleEntity> Roles { get; set; }
    }
}
