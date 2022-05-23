using System.Text.Json.Serialization;

namespace Shared.DomainModels
{
    public class DriverDOM
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime DOB { get; set; }        
        public int Age { get => this.DOB!=default? DateTime.Now.Year - this.DOB.Year:0 ; }

    }
}