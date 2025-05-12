using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Infrastructures.Data;

namespace Infrastructures.Services
{
    

    public class DatabaseService : IDatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                // Mencoba mengakses database
                await _context.Database.CanConnectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}