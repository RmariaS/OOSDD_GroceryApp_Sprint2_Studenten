
namespace Grocery.Core.Models
{
    public partial class Client : Model
    {
        // properties public gemaakt zodat ze toegankelijk worden voor andere klassen
        public string Email { get; set; }
        public string Password { get; set; }
        public Client(int id, string name, string email, string password) : base(id, name)
        {
            Email= email; // naam van parameter aangepast voor consistentie
            Password = password;
        }
    }
}
