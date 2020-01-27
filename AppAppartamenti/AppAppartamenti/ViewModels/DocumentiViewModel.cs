using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using System.Collections.Generic;
using System.Linq;


namespace AppAppartamenti.ViewModels
{
    public class DocumentiViewModel : BaseViewModel
    {

        public class DocumentiGroup : List<DocumentoDto>
        {
            public string Name { get; private set; }

            public DocumentiGroup(string name, List<DocumentoDto> documents) : base(documents)
            {
                Name = name;
            }
        }

        //public ICollection<DocumentoDto> Documenti { get; set; }
        //public ICollection<DocumentoDto> Links { get; set; 
        public List<DocumentiGroup> Documents { get; private set; } = new List<DocumentiGroup>();

        public bool IsVendita { get; set; }
        //public Command LoadItemsCommand { get; set; }

        public DocumentiViewModel(bool IsVendita)
        {
            this.IsVendita = IsVendita;
            //Documenti = new ObservableCollection<DocumentoDto>();
            //Links = new ObservableCollection<DocumentoDto>();
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
          
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //Documenti.Clear();
                //Links.Clear();
                Documents.Clear();

                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());

                ListaDocumentiDto all;

                if (IsVendita)
                {
                    all = await annunciClient.GetDocumentiVenditaAsync();
                } else
                {
                    all = await annunciClient.GetDocumentiAcquistoAsync();
                }

                //Documenti = all.DocumentiByTipologia["DOCUMENT"];
                //Links = all.DocumentiByTipologia["LINK"];

                //foreach(string type in all.DocumentiByTipologia.Keys)

                Documents.Add(new DocumentiGroup("DOCUMENT", (all.DocumentiByTipologia["DOCUMENT"]).ToList()));
                Documents.Add(new DocumentiGroup("LINK", (all.DocumentiByTipologia["LINK"]).ToList()));


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Task<string> DisplayActionSheet(string v1, string v2, object p, string v3, string v4)
        {
            throw new NotImplementedException();
        }
    }
}