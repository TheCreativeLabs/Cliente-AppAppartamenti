namespace AppAppartamentiApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    
    public partial class ChatDtoOutput
    {
        public Guid IdChat { get; set; }

        public Guid IdAnnuncio { get; set; }

        public Guid IdUser { get; set; }

        public List<MessaggioDto>Messaggi { get; set; }
    }
}
