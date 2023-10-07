using System.Net.Http.Json;

namespace WiredBrainCoffee.Models.Services
{
    public class ContactService : IContactService
    {
        private readonly HttpClient http;

        public ContactService(HttpClient http)
        {
            this.http = http;
        }

        public async Task PostContact(Contact contact)
        {
            await http.PostAsJsonAsync("contact", contact);
        }
    }
}