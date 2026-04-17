using SQLite;

namespace CarManagerApp.Models
{
    [Table("RecurringExpenses")]
    public class RecurringExpense
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed, NotNull]
        public int VehicleId { get; set; }

        [MaxLength(100), NotNull]
        public string ExpenseType { get; set; } = string.Empty;

        public DateTime PurchaseDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public double Cost { get; set; }

        public bool IsReminderActive { get; set; }
    }
}
