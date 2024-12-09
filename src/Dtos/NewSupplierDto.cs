
namespace ControlDeVenta_Proy.src.DTOs
{
    public class NewSupplierDto
    {
        public string Name { get; set; } = string.Empty;
        public string Rut { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<int> ProductIDs { get; set; } = [];
    }
}