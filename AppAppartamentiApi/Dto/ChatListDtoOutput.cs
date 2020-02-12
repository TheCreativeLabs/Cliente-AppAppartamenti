namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    
    public partial class ChatListDtoOutput
    {
        public Guid IdAnnuncio { get; set; }

        public Guid IdChat { get; set; }

        public Guid IdUser { get; set; }
       
        public string Nome { get; set; }

        public string Cognome { get; set; }

        public byte[] FotoUtente { get; set; }

        public DateTime DataCreazione { get; set; }

        public int NumberMsgToRead { get; set; }
    }
}
