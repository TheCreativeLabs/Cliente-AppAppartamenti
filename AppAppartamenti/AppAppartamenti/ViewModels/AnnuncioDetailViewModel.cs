using System;

using AppAppartamenti.Models;

namespace AppAppartamenti.ViewModels
{
    public class AnnuncioDetailViewModel : BaseViewModel
    {
        public AnnuncioDtoOutput Item { get; set; }
        public AnnuncioDetailViewModel(Guid? idAnnuncio)
        {
            if (idAnnuncio == null)
            {
                Item = new AnnuncioDtoOutput();
            }
            else
            {
                Item = item;
            }
            
        }
    }
}
