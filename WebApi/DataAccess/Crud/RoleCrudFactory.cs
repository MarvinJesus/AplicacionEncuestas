using DataAccess.Mapper;
using Entities_POJO;
using System;
using System.Collections.Generic;

namespace DataAccess.Crud
{
    public class RoleCrudFactory : CrudFactory
    {
        private RoleMapper _mapper { get; set; }

        public RoleCrudFactory()
        {
            _mapper = new RoleMapper();
        }
        public override T Create<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override int Delete(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override T Retrieve<T>(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public override ICollection<T> RetrieveAll<T>()
        {
            throw new NotImplementedException();
        }

        public override int Update(BaseEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
