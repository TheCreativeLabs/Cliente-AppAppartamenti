namespace AppAppartamentiApi
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class ComuneDto
    {

        [StringLength(256)]
        public string NomeComune { get; set; }

        public int CodiceComune { get; set; }
    }
}
