﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gymme.Data.Models;
using System.Data.Linq;
using Gymme.Data.Core;

namespace Gymme.Data.Repository
{
    public abstract class RepositoryBase<T> 
        where T : Model
    {
        private readonly Table<T> _table;

        public RepositoryBase()
        {
            _table = DatabaseContext.Instance.GetTable<T>();
        }

        protected Table<T> Table
        {
            get { return _table;}
        }

        public virtual IEnumerable<T> FindAll()
        {
            return Table.Select(x => x);
        }

        public abstract T FindById(long id);

        public abstract bool Exists(long id);
        
        public virtual void Save(T entity)
        {
            InsertOnDemand(entity);
            DatabaseContext.Instance.SubmitChanges();
        }

        public virtual void Save(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                InsertOnDemand(entity);
            }

            DatabaseContext.Instance.SubmitChanges();
        }

        private void InsertOnDemand(T entity)
        {
            if (entity.IsNew)
            {
                Table.InsertOnSubmit(entity);
            }
        }

        public virtual void Delete(T entity)
        {
            Table.DeleteOnSubmit(entity);
            DatabaseContext.Instance.SubmitChanges();
        }
    }
}