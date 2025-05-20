namespace IpBlockApi.Models
{
    public class BlockedCountry
    {
        public Country Country { get; set; } = new();
        public bool IsTemporary { get; set; } = false;
        public DateTime? UnblockTime { get; set; }
    }
}
