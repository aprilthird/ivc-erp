using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.DATA.Context
{
    public class IvcDbContextFactory : IDesignTimeDbContextFactory<IvcDbContext>
    {
        public IvcDbContextFactory()
        {
        }

        public IvcDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IvcDbContext>();

            builder.UseSqlServer(
                //"Server=localhost;Database=ivc210616;Trusted_Connection=True;MultipleActiveResultSets=true"
                //"Server=ivc.database.windows.net;Database=IVC.DB;User ID=ivcadmin;Password=Admin.123;MultipleActiveResultSets=true"
                "Server=erp-ivc.database.windows.net;Database=ivcdb;User ID=ivcadmin;Password=Admin.123;MultipleActiveResultSets=true"
                //"Server=erp-ivc.database.windows.net;Database=ivcdbdev;User ID=ivcadmin;Password=Admin.123;MultipleActiveResultSets=true"
                //DataConnectionString
                , opts => {
                    opts.CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds);
                });
            return new IvcDbContext(builder.Options);
        }
    }
}
