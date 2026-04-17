using SQLite;
using CarManagerApp.Models;

namespace CarManagerApp.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        private async Task<SQLiteAsyncConnection> GetDatabaseAsync()
        {
            if (_database is null)
            {
                _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
                await InitAsync(_database);
            }
            return _database;
        }

        private static async Task InitAsync(SQLiteAsyncConnection db)
        {
            await db.CreateTableAsync<Vehicle>();
            await db.CreateTableAsync<FuelRecord>();
            await db.CreateTableAsync<MaintenanceService>();
            await db.CreateTableAsync<RecurringExpense>();
        }

        // ─────────────────────────────────────────────
        // VEHICLE CRUD
        // ─────────────────────────────────────────────

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            var db = await GetDatabaseAsync();
            return await db.Table<Vehicle>().ToListAsync();
        }

        public async Task<Vehicle?> GetVehicleAsync(int id)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<Vehicle>()
                           .Where(v => v.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> SaveVehicleAsync(Vehicle vehicle)
        {
            var db = await GetDatabaseAsync();
            return vehicle.Id == 0
                ? await db.InsertAsync(vehicle)
                : await db.UpdateAsync(vehicle);
        }

        public async Task<int> DeleteVehicleAsync(Vehicle vehicle)
        {
            var db = await GetDatabaseAsync();
            // Cascade-delete all child records
            await DeleteFuelRecordsByVehicleAsync(vehicle.Id);
            await DeleteMaintenanceServicesByVehicleAsync(vehicle.Id);
            await DeleteRecurringExpensesByVehicleAsync(vehicle.Id);
            return await db.DeleteAsync(vehicle);
        }

        // ─────────────────────────────────────────────
        // FUEL RECORD CRUD
        // ─────────────────────────────────────────────

        public async Task<List<FuelRecord>> GetFuelRecordsForVehicleAsync(int vehicleId)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<FuelRecord>()
                           .Where(f => f.VehicleId == vehicleId)
                           .OrderByDescending(f => f.Date)
                           .ToListAsync();
        }

        public async Task<FuelRecord?> GetFuelRecordAsync(int id)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<FuelRecord>()
                           .Where(f => f.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> SaveFuelRecordAsync(FuelRecord record)
        {
            var db = await GetDatabaseAsync();
            return record.Id == 0
                ? await db.InsertAsync(record)
                : await db.UpdateAsync(record);
        }

        public async Task<int> DeleteFuelRecordAsync(FuelRecord record)
        {
            var db = await GetDatabaseAsync();
            return await db.DeleteAsync(record);
        }

        private async Task DeleteFuelRecordsByVehicleAsync(int vehicleId)
        {
            var db = await GetDatabaseAsync();
            var records = await db.Table<FuelRecord>()
                                  .Where(f => f.VehicleId == vehicleId)
                                  .ToListAsync();
            foreach (var r in records)
                await db.DeleteAsync(r);
        }

        // ─────────────────────────────────────────────
        // MAINTENANCE SERVICE CRUD
        // ─────────────────────────────────────────────

        public async Task<List<MaintenanceService>> GetMaintenanceServicesForVehicleAsync(int vehicleId)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<MaintenanceService>()
                           .Where(m => m.VehicleId == vehicleId)
                           .OrderByDescending(m => m.Date)
                           .ToListAsync();
        }

        public async Task<MaintenanceService?> GetMaintenanceServiceAsync(int id)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<MaintenanceService>()
                           .Where(m => m.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> SaveMaintenanceServiceAsync(MaintenanceService service)
        {
            var db = await GetDatabaseAsync();
            return service.Id == 0
                ? await db.InsertAsync(service)
                : await db.UpdateAsync(service);
        }

        public async Task<int> DeleteMaintenanceServiceAsync(MaintenanceService service)
        {
            var db = await GetDatabaseAsync();
            return await db.DeleteAsync(service);
        }

        private async Task DeleteMaintenanceServicesByVehicleAsync(int vehicleId)
        {
            var db = await GetDatabaseAsync();
            var services = await db.Table<MaintenanceService>()
                                   .Where(m => m.VehicleId == vehicleId)
                                   .ToListAsync();
            foreach (var s in services)
                await db.DeleteAsync(s);
        }

        // ─────────────────────────────────────────────
        // RECURRING EXPENSE CRUD
        // ─────────────────────────────────────────────

        public async Task<List<RecurringExpense>> GetRecurringExpensesForVehicleAsync(int vehicleId)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<RecurringExpense>()
                           .Where(e => e.VehicleId == vehicleId)
                           .OrderBy(e => e.ExpiryDate)
                           .ToListAsync();
        }

        public async Task<RecurringExpense?> GetRecurringExpenseAsync(int id)
        {
            var db = await GetDatabaseAsync();
            return await db.Table<RecurringExpense>()
                           .Where(e => e.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> SaveRecurringExpenseAsync(RecurringExpense expense)
        {
            var db = await GetDatabaseAsync();
            return expense.Id == 0
                ? await db.InsertAsync(expense)
                : await db.UpdateAsync(expense);
        }

        public async Task<int> DeleteRecurringExpenseAsync(RecurringExpense expense)
        {
            var db = await GetDatabaseAsync();
            return await db.DeleteAsync(expense);
        }

        private async Task DeleteRecurringExpensesByVehicleAsync(int vehicleId)
        {
            var db = await GetDatabaseAsync();
            var expenses = await db.Table<RecurringExpense>()
                                   .Where(e => e.VehicleId == vehicleId)
                                   .ToListAsync();
            foreach (var e in expenses)
                await db.DeleteAsync(e);
        }
    }
}
