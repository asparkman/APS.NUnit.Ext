using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APS.NUnit.Ext.Example.Data
{
    public partial class LocalDatabaseEntities1 : DbContext, ILocalDatabaseEntities1
    {
        public async Task<List<Person>> GetPersons()
        {
            return await (
                from person in People
                select person
            ).ToListAsync();
        }
    }
}
