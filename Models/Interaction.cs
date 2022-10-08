namespace RealTimeChat.Models
{
  public class Interaction
  {
    public string FromUser { get; set; }

    public string ToUser { get; set; }
    public Interaction(string fromUser, string toUser)
      {
        FromUser = fromUser;
        ToUser = toUser;
      }
  }  



  public class MessageInteraction : Interaction
  {
    public List<byte> Message { get; set; }

    public MessageInteraction(string fromUser, string toUser, List<byte> message) : base(fromUser, toUser)
    {
      Message = message;
    }
  }
   public class PublicKeyInteraction: Interaction
  {
    public string PublicKey { get; set; } = null!;

    public PublicKeyInteraction(string fromUser, string toUser, string publicKey) : base(fromUser, toUser)
    {
      PublicKey = publicKey;
    }
  }
}