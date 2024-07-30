using API.Entities;

namespace API;

// Another many to many relationship join table between 2 users
public class Message
{
    public int Id { get; set; }
    public required string SenderUsername { get; set; }
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    
    /*
        Users can delete messages, but not from other users inboxes,
        Only once both users have deleted the message will we remove from our database 
    */
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }

    // Navigation Properties (EF)
    public int SenderId { get; set; }
    public AppUser Sender { get; set; } = null!;
    public int RecipientId { get; set; }
    public AppUser Recipient { get; set; } = null!;
}



