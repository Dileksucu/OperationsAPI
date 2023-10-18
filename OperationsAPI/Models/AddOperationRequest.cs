namespace OperationsAPI.Models
{
    public class AddOperationRequest
    {
        //Dto 
        // Neden ayrı bir entity oluşturduk ; Nedeni bir ürün eklerken ben manuel olarak ıd girmem sistem kendi oluşturur.

        public string FullName { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Address { get; set; }


    }
}
