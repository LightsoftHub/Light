using System.ComponentModel;

namespace Sample.Models
{
    [DisplayName("Test Model Display Name")]
    public class TestModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
