using SQLite;

namespace CarManagerApp.Models
{
    [Table("FuelRecords")]
    public class FuelRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, NotNull]
        public int VehicleId { get; set; }

        public DateTime Date { get; set; }

        public double Odometer { get; set; }

        public double Liters { get; set; }

        public double PricePerLiter { get; set; }

        public double TotalCost { get; set; }

        public bool IsFullTank { get; set; }

        public bool MissedPreviousRecord { get; set; }

        [MaxLength(150)]
        public string StationName { get; set; } = string.Empty;
    }
}
