using System;
using Windows.UI.Xaml.Controls;
using TuttoElliUW81.SharedModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Documents;
using System.Linq;

namespace TuttoElliUW81
{
    public partial class ShareText : Page
    {
        private ShareableTrack shareableTrack;
        private string TestoOriginale;
        private string TestoDaCondividere;
        private string Quote;
        private DataTransferManager _dataTransferManager;

        public ShareText()
        {
            InitializeComponent();
            
            // Register the current page as a share source.
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;
        }

        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            shareableTrack = e.Parameter as ShareableTrack;
            //salvo il testo originale. Inizialmente, sarà quello da condividere se non vi è alcun input utente
            TestoOriginale = shareableTrack.PublicTrackLyrics;
            TestoDaCondividere = TestoOriginale;
            //lo aggiungo alla RichTextBlock
            rtblockToShare.Blocks.Add(createNewParagraph(shareableTrack.PublicTrackLyrics));
            //preparo la citazione da aggiungere alla fine
            Quote = "\n\n(tratto da '"+ shareableTrack.PublicTrackTitle + "'\n dall'album "+ shareableTrack.PublicAlbumTitle +")";
        }

        private Paragraph createNewParagraph(string text)
        {
            Paragraph p = new Paragraph();
            Run run = new Run();
            run.Text = text;
            p.Inlines.Add(run);
            return p;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_dataTransferManager != null)
                _dataTransferManager.DataRequested -= OnDataRequested;
        }

        protected void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = "Testo condiviso con TuttoElli per Windows Phone\n";
            e.Request.Data.SetText(TestoDaCondividere);
        }

        private void btnShare_onClick(object sender, RoutedEventArgs e)
        {
            //avvia la condivisione
            DataTransferManager.ShowShareUI();
        }

        private void rtblockToShare_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //se il testo selezionato non è nullo
            if (rtblockToShare.SelectedText != null)
            {
                //abilito la condivisione tracce e il testo da condividere sarà quello
                TestoDaCondividere = rtblockToShare.SelectedText;
            }
            else //se non è stato selezionato nulla, il testo originale rimane quello
                TestoDaCondividere = TestoOriginale;
        }
        

        private void cbxInsertQuote_Checked(object sender, RoutedEventArgs e)
        {
            if (cbxInsertQuote.IsChecked == true)
                TestoDaCondividere += Quote; //aggiungo la citazione, nel caso sia stata checkata la checkbox
            else
                //altrimenti la rimuovo
                TestoDaCondividere = TestoOriginale;
        }
    }
}