using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFPrototype.Entities
{
    public class RoleEntity
    {
        [Key]
        public long RoleId { get; set; }
        public Guid GUID { get; set; }
        public string Name { get; set; }
        public bool AdminAccess { get; set; }
        public bool CreateAccess { get; set; }
        public bool UpdateAccess { get; set; }
        public bool DeleteAccess { get; set; }
        public bool ReadAccess { get; set; }

        public ICollection<UserEntity> Users { get; set; }
    }
}
