using SQLite;

namespace CarManagerApp.Models
{
    [Table("MaintenanceServices")]
    public class MaintenanceService
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, NotNull]
        public int VehicleId { get; set; }

        public DateTime Date { get; set; }

        public double Odometer { get; set; }

        [MaxLength(100), NotNull]
        public string ServiceType { get; set; } = string.Empty;

        public double Cost { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(150)]
        public string GarageName { get; set; } = string.Empty;
    }
}
