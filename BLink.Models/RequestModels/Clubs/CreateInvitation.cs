namespace BLink.Models.RequestModels.Clubs
{
    public class CreateInvitation
    {
        //[Required]
        public int PlayerId { get; set; }

        //[Required]
        public string Description { get; set; }
    }
}
