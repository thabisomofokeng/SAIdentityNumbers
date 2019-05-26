namespace IdentityNumber.Domain.Entities
{
    public class InvalidIdentityNumber
    {
        public string IdentityNumber { get; set; }

        public string ReasonsFailed { get; set; }
    }
}
