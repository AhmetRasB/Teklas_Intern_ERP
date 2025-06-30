namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialMovement
    {
        public int Id { get; set; }
        public int MaterialCardId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
    }
} 