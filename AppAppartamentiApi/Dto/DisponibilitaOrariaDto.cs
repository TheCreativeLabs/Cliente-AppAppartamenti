namespace AppAppartamentiApi
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class DisponibilitaOrariaDto
    {
        public string fasceOrarieLunedi { get; set; }
        public string fasceOrarieMartedi { get; set; }
        public string fasceOrarieMercoledi { get; set; }
        public string fasceOrarieGiovedi { get; set; }
        public string fasceOrarieVenerdi { get; set; }
        public string fasceOrarieSabato { get; set; }
        public string fasceOrarieDomenica { get; set; }
    }
}
