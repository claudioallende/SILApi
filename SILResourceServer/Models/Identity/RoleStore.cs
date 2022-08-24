using Microsoft.AspNet.Identity;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace ResourceServer.Models.Identity
{
    public class RoleStore : IQueryableRoleStore<Role, int>, IRoleStore<Role, int>, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// If true then disposing this object will also dispose (close) the session. False means that external code is responsible for disposing the session.
        /// </summary>
        public bool ShouldDisposeSession { get; set; }

        public ISession Context { get; private set; }

        public RoleStore(ISession context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            ShouldDisposeSession = true;
            this.Context = context;
        }

        public virtual Task<Role> FindByIdAsync(string roleId)
        {
            this.ThrowIfDisposed();
            return Task.FromResult(Context.Get<Role>((object)roleId));
        }

        public virtual Task<Role> FindByNameAsync(string roleName)
        {
            this.ThrowIfDisposed();
            return Task.FromResult<Role>(Queryable.FirstOrDefault<Role>(Queryable.Where<Role>(this.Context.Query<Role>(), (Expression<Func<Role, bool>>)(u => u.Name.ToUpper() == roleName.ToUpper()))));
        }

        public virtual Task CreateAsync(Role role)
        {
            this.ThrowIfDisposed();
            if ((object)role == null)
                throw new ArgumentNullException("role");
            Context.Save(role);
            Context.Flush();
            return Task.FromResult(0);
        }

        public virtual Task DeleteAsync(Role role)
        {
            this.ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            Context.Delete(role);
            Context.Flush();
            return Task.FromResult(0);
        }

        public virtual Task UpdateAsync(Role role)
        {
            this.ThrowIfDisposed();
            if ((object)role == null)
                throw new ArgumentNullException("role");
            Context.Update(role);
            Context.Flush();
            return Task.FromResult(0);
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this._disposed && ShouldDisposeSession)
                this.Context.Dispose();
            this._disposed = true;
            this.Context = (ISession)null;
        }

        public IQueryable<Role> Roles
        {
            get
            {
                this.ThrowIfDisposed();
                return this.Context.Query<Role>();
            }
        }


        public Task<Role> FindByIdAsync(int roleId)
        {
            return Task.FromResult(Context.Get<Role>(roleId));
        }
    }
}