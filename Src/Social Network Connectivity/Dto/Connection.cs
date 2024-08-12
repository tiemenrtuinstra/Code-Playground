namespace SocialNetworkConnectivity.Dto;

public record Connection
{
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public DateTime DateTime { get; set; }

    public int Timestamp
    {
        get
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(DateTime);
            return (int)dateTimeOffset.ToUnixTimeSeconds();
        }
    }
}