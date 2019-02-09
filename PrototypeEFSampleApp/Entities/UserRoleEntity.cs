using EFPrototype.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeEFSampleApp.Entities
{
    class UserRoleEntity
    {
        [Key]
        [Column(Order = 0)]
        [ForeignKey(nameof(UserEntity))]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey(nameof(RoleEntity))]
        public int RoleId { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual RoleEntity Role { get; set; }
    }
}
