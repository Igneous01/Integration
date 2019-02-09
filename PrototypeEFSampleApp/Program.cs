using EFPrototype.Contexts;
using EFPrototype.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeEFSampleApp
{
    interface IMapper<SourceT, TargetT>
    {
        SourceT ConvertToSource(TargetT type);
        TargetT ConvertToTarget(SourceT source);
    }
    class GenericDude<T>
    {
        T Object { get; set; }

        public GenericDude(T Object)
        {
            this.Object = Object;
        }
    }

    class Dude<GenericDudeType> where GenericDudeType : GenericDude<>
    {
        public int MethodA<NewT>(NewT Object)
        {
            GenericDudeType gdt = new GenericDudeType<NewT>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new UserRoleContext())
            {
                db.Roles.Add(new RoleEntity()
                {
                    Name = "Basic Role",
                    ReadAccess = true
                });
                db.Roles.Add(new RoleEntity()
                {
                    Name = "Admin Role",
                    AdminAccess = true
                });
                db.Roles.Add(new RoleEntity()
                {
                    Name = "CRUD Role",
                    ReadAccess = true,
                    CreateAccess = true,
                    UpdateAccess = true,
                    DeleteAccess = true
                });

                db.SaveChanges();



                db.Users.Add(new UserEntity()
                {
                    Name = "Bob Parker",
                    Roles = new List<RoleEntity>()
                    {
                        (from role in db.Roles
                        where role.Name == "Basic Role"
                        select role).First()
                    }
                });

                db.Users.Add(new UserEntity()
                {
                    Name = "Rob Mahan",
                    Roles = new List<RoleEntity>()
                    {
                        (from role in db.Roles
                        where role.Name == "Admin Role"
                        select role).First()
                    }
                });

                db.Users.Add(new UserEntity()
                {
                    Name = "Steve Reich",
                    Roles = new List<RoleEntity>()
                    {
                        (from role in db.Roles
                        where role.Name == "CRUD Role"
                        select role).First()
                    }
                });

                db.SaveChanges();
            }
        }
    }
}
