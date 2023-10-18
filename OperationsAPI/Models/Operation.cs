namespace OperationsAPI.Models
{
    public class Operation : ISoftDeleteEntity
    {
        public Guid Id { get; set; }
        // Guid nedir ? --> Guid, benzersiz değerler oluşturmak için kullanılmaktadır.
        public string FullName { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }
        public bool IsDeleted { get; set; }
    }
}
