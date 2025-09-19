using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;

        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }

        public Client? Login(string email, string password)
        {
            // vraagen om de klantgegevens zoeken met de gegeven email
            Client? client = _clientService.Get(email);

            // bij geen klant, return null
            if (client == null)
            {
                return null;
            }

            // als de klant gevonden is controle match password
            
            bool passwordIsCorrect = PasswordHelper.VerifyPassword(password, client.Password);

            // alles klopt? klant gegevens return en anders null
            return passwordIsCorrect ? client : null;
        }
    }
}


