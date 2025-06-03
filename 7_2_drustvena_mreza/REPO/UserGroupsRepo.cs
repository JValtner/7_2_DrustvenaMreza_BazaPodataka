using _6_1_drustvena_mreza.DOMEN;
using Microsoft.Data.Sqlite;
namespace _7_2_drustvena_mreza.REPO
{
    public class UserGroupsRepo
    {
        private readonly string connectionString;

        public UserGroupsRepo(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }
    }
}
