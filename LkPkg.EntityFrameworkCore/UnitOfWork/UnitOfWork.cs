using LkPkg.EntityFrameworkCore.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LkPkg.EntityFrameworkCore.UnitOfWork
{
    public class UnitOfWork
    {
        private readonly DbContext _context;
        private readonly ConcurrentDictionary<string, IGenericRepository> _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }
    }
}
