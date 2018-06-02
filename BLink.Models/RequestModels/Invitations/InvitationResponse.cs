namespace BLink.Models.RequestModels.Invitations
{
    public class InvitationResponse
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ClubName { get; set; }

        public string PlayerName { get; set; }

        public byte[] Thumbnail { get; set; }
    }
}
