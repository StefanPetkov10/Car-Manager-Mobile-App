using SQLite;

namespace CarManagerApp.Models
{
    [Table("Vehicles")]
    public class Vehicle
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100), NotNull]
        public string Make { get; set; } = string.Empty;

        [MaxLength(100), NotNull]
        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        [MaxLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [MaxLength(17)]
        public string VIN { get; set; } = string.Empty;

        [MaxLength(50)]
        public string FuelType { get; set; } = string.Empty;

        public double TankCapacity { get; set; }

        public double InitialOdometer { get; set; }
    }
}
