
namespace ControlDeVenta_Proy.src.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Rut { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        
        [System.Text.Json.Serialization.JsonIgnore]
        public List<Supply> Supplies { get; } = new List<Supply>();
    }
}