using Hello.Service.Models.Data;

namespace Hello.Service.Models.Response
{
    public class JokeResponse
    {
        public string Type { get; set; } 
        public JokeData Value { get; set; }
    }
}