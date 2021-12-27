using DataAccess.DbAccess;

namespace DataAccess.Access
{
    public abstract class Access
    {
        protected readonly ISQLDataAccess _db;

        public Access(ISQLDataAccess db)
        {
            _db = db;
        }
    }
}
