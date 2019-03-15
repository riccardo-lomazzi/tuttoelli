using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TuttoElliUW81.SharedModel;
using Windows.Networking.Connectivity;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace TuttoElliUW81
{
    public partial class ViewLyrics : Page, UtilitiesInterface
    { 
        //info traccia
        private SelectedTrackInfo trcInfo;
        private ViewLyricsData viewLyricsData;
        private int idCurrentTrack;
        private int idCurrentAlbum;
        //app bar
        private AppBarButton btnPrec;
        private AppBarButton btnSucc;
        //Youtube
        //private List<YoutubeVideo> youtubevideoList;
        
        //font size predefinita testo
        private readonly float _MaxFontSize = 20;
        private readonly double _MinFontSize=12;



        /// <summary>
        /// ////////////////////////////////COSTRUTTORE///////////////////////////////
        /// </summary>
        public ViewLyrics()
        {
            InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            btnPrec = (BottomAppBar as CommandBar).PrimaryCommands.First() as AppBarButton;
            btnSucc = (BottomAppBar as CommandBar).PrimaryCommands.Last() as AppBarButton;
        }

        ///<summary>
        /// INIZIALIZZAZIONE E ON NAVIGATED TO/FROM
        /// </summary>

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            String title = (String)trcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_title;
            //scrivo qui nel DB per verificare che non dia errori
            WPUtilities.updateLastViewedTracks(title);
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        private void setPage(SelectedTrackInfo trackInfo, bool FirstLoad)
        {
            //salvo la traccia scelta, mi serve per scorrere poi l'array (-1 perché la traccia è numerata come nell'album nel DB)
            idCurrentTrack = trackInfo.idTracciaScelta;
            //salvo l'indice dell'album
            idCurrentAlbum = trackInfo.current_album.idalbum_forArray;
            //verifico di non aver scelto la prima o ultima traccia, nel caso blocco i tasti di scorrimento nella AppBar
            checkBtnAppBarEnabled(btnPrec, idCurrentTrack, 0);
            checkBtnAppBarEnabled(btnSucc, idCurrentTrack, trackInfo.Array_tracce_currentAlbum.Count - 1);
            //cambio il background
            LayoutRoot.Background = new ImageBrush
            {
                Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill,
                ImageSource = Utilities.ImagePathStringToImageSourceConverter(trackInfo.current_album.album_img_src),
                Opacity = 0.2
            };
           
            //modifico gli header
            if (FirstLoad)
            {
                loadHeaders<Album>(albumsHeader, viewLyricsData.albums, trackInfo.current_album.idalbum_forArray, albumHeader_SelectionChanged);
                //e salvo già la traccia
            }
            loadHeaders<Track>(currentAlbumTracksHeader, trackInfo.Array_tracce_currentAlbum, idCurrentTrack, currentAlbumTracksHeader_SelectionChanged);
            //copio il testo
            initialize_lyrics_screen(trackInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_text);
            //popolo la schermata dell'album
            initialize_album_screen(trackInfo.current_album);
        }

        private void checkBtnAppBarEnabled(AppBarButton btn, int current_position, int index)
        {
            btn.IsEnabled =Utilities.verifyIfNotEquals(current_position,index);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //recupero i dati della pagina prec
            viewLyricsData = new ViewLyricsData();
            viewLyricsData = e.Parameter as ViewLyricsData;
            trcInfo = viewLyricsData.TrcInfo;
             //inizializzo la scheda dei video*/
            setPage(trcInfo, true);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //quando esco dalla pagina, salvo l'ultima traccia visualizzata
            //saveLastViewedTrack();
            
        }

        //esegue il binding delle combobox agli array degli album e delle tracce
        private void loadHeaders<T>(ComboBox cbx, ObservableCollection <T> itemsSource, int selectedIndex, SelectionChangedEventHandler selectionChanged)
        {
            //abilitazione
            cbx.IsEnabled = true;
            //altezza
            cbx.Height = double.NaN;
            //popolamento 
            cbx.ItemsSource = itemsSource;
            //selezione primo elemento
            cbx.SelectedIndex = selectedIndex;
            //aggiunta degli event handler
            cbx.SelectionChanged += selectionChanged;
            
        }


        private void albumHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //salvo la tracca corrente tra le visualizzate
            //reinizializzo la pagina
            currentAlbumTracksHeader.SelectionChanged -= currentAlbumTracksHeader_SelectionChanged;
            currentAlbumTracksHeader.ItemsSource = null;
            //reinizializzo la lista video
            //salvo l'id dell'album corrente e l'album stesso
            ComboBox comboBox = (sender) as ComboBox;
            idCurrentAlbum = comboBox.SelectedIndex;
            Album selectedAlbum = comboBox.SelectedItem as Album;
            //costruisco il nuovo trcInfo
                //prendo l'album
                viewLyricsData.TrcInfo.current_album = selectedAlbum;
                //prendo le tracce
                viewLyricsData.TrcInfo.Array_tracce_currentAlbum = Utilities.ToObservableCollection<Track>(
                    viewLyricsData.tracks.Where<Track>(
                        track => (track.Idalbum) == (idCurrentAlbum+1))
                );
                //prendo l'indice iniziale delle tracce (sempre la prima traccia quando cambio album)
                viewLyricsData.TrcInfo.idTracciaScelta = 0;

            //avvio della ricostruzione della pagina
            setPage(viewLyricsData.TrcInfo, false);
            //sposto il focus sulla prima pagina del pivot
            pivotLyrics.SelectedIndex = 0;
            //sposto il focus sulla prima riga
            float zoom = txtLyricsScroller.ZoomFactor;
            txtLyricsScroller.ChangeView(0, 0, zoom);
        }


        private void currentAlbumTracksHeader_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //salvo la tracca corrente tra le visualizzate
            idCurrentTrack = currentAlbumTracksHeader.SelectedIndex;
            //reinizializzo la lista video
            //inizializzo il PivotItem del testo
            initialize_lyrics_screen(trcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_text);
            //controllo i pulsanti della appbar
            checkBtnAppBarEnabled(btnPrec, idCurrentTrack, 0);
            checkBtnAppBarEnabled(btnSucc, idCurrentTrack, trcInfo.Array_tracce_currentAlbum.Count - 1);
            //sposto il focus sulla prima pagina del pivot
            pivotLyrics.SelectedIndex = 0;
            //sposto il focus sulla prima riga
            float zoom = txtLyricsScroller.ZoomFactor;
            txtLyricsScroller.ChangeView(0, 0, zoom);
        }

       

        /// <summary>
        /// ///////////////////////////////////SCHERMATA LYRICS/////////////////////////////////
        /// </summary>
        private void initialize_lyrics_screen(string TrackText)
        {
            //Testo traccia
            txtLyrics.Text = TrackText;
        }

        private void initialize_album_screen(Album album)
        {
            albumsHeader.SelectedIndex = idCurrentAlbum;
            txtAlbumYear.Text = album.Album_year; //anno album
            txtAlbumLabel.Text = album.Album_label; //etichetta album
            txtAlbumType.Text = album.Album_type; //tipo album
            txtAlbumInfo.Text = (album.Album_info != null)? album.Album_info : "Nessuna info presente"; //info album
            txtTrackLength.Text = trcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_length; //durata brano
            imgAlbum.Source = Utilities.ImagePathStringToImageSourceConverter(album.album_img_src); //caricamento immagine
        }


        //////////////////////////////////////////////////////   
        ///////////////////Video Correlati////////////////////
        /////////////////////////////////////////////////////

        //metodo chiamato dal Button CARICA LISTA VIDEO
        private async void loadList()
        {
            string uriToLaunch = "https://www.youtube.com/results?search_query=" + trcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_title.Trim().Replace(" ", "+");
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
            ////attiva il progressRingYoutube per l'utente
            //ProgressRing progressRingYoutube = new ProgressRing();
            //progressRingYoutube.Height = 90;
            //progressRingYoutube.Width = 90;
            //stackPanelVideoList.Children.Add(progressRingYoutube);
            //progressRingYoutube.IsActive = true;
            ////carica la lista di risultati
            //youtubevideoList = new List<YoutubeVideo>();
            //youtubevideoList = await GetVideos(20); //prende 20 risultati
            //progressRingYoutube.IsActive = false;
            //stackPanelVideoList.Children.Remove(progressRingYoutube);

            //if (youtubevideoList.Count()!=0)
            //{
            //    relatedVideosListBox.ItemsSource = youtubevideoList; //assegna la lista alla listbox
            //    relatedVideosListBox.Visibility = Visibility.Visible; //la rendo visibile
            //}
            //else //se non vengono restituiti video per colpa di youtube, comunica all'utente
            //{
            //    WPUtilities.visualizza("Nessun risultato trovato. Riprova fra qualche istante.","Errore Youtube");
            //}

        }

        //Get Channel Videos
        //private async Task<List<YoutubeVideo>> GetVideos(int maxResults)
        //{
        //    //creo la lista dei video
        //    List<YoutubeVideo> channelVideos = new List<YoutubeVideo>();
            
        //    //creo la richiesta di ricerca
        //    //var videoItemsListRequest = youtubeService.Search.List("snippet");

        //    Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            

        //    videoItemsListRequest.Q =
        //        rgx.Replace(trcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_title,"").ToLower() + " " +
        //        rgx.Replace(trcInfo.current_album.Album_name,"").ToLower();
        //    videoItemsListRequest.MaxResults = maxResults;

        //    //WARNING: la richiesta non funzionava perché mancava un reference all'aseembly Microsoft.BCL.Tasks
        //    //ricordarsi di scaricarlo prima!

        //    //eseguo la richiesta
        //    try
        //    {
        //        var channelItemsListResponse = await videoItemsListRequest.ExecuteAsync();
        //        //per ogni risultato della query, creo uno spazio nella lista
        //        foreach (var channelItem in channelItemsListResponse.Items)
        //        {
        //            channelVideos.Add(
        //                new YoutubeVideo
        //                {
        //                    Id = channelItem.Id.VideoId,
        //                    Title = channelItem.Snippet.Title,
        //                    Thumbnail = channelItem.Snippet.Thumbnails.Medium.Url,
        //                    YoutubeLink = "https://www.youtube.com/watch?v=" + channelItem.Id.VideoId
        //                });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        WPUtilities.visualizza(e.ToString(), "errore Request");
        //    }


        //    //restituisco la lista
        //    return channelVideos;
        //}


        //@ENSURES quando viene cliccato, parte la ricerca e si aggiorna la listbox
        private void relatedVideosButton_Click(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).IsConnected) 
                loadList();
            else
               WPUtilities.visualizza("Mi spiace, ti serve l'Internét per continuare. Ho i miei buoni motivi!","Non hai una connessione attiva");
        }

        private async void relatedVideosListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            YoutubeVideo lbItem = (sender as ListBox).SelectedItem as YoutubeVideo;
            string uriToLaunch = @lbItem.YoutubeLink;
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }


        //////////////////////////////////////////////////////   
        ///////////////////ApplicationBar////////////////////
        /////////////////////////////////////////////////////



        //@ENSURES  carica testo traccia precedente
        private void btnPrec_onClick(object sender, RoutedEventArgs e)
        {
            
            //salvo la tracca corrente tra le visualizzate
            //Mi sposto col puntatore
            idCurrentTrack--;
            //cambio l'index della combobox
            currentAlbumTracksHeader.SelectedIndex = idCurrentTrack;
            //CASO PARTICOLARE 1: inizio array
            btnPrec.IsEnabled = Utilities.verifyIfNotEquals(idCurrentTrack, 0);
            
            //reinizializzo la lista video
            //cambio il testo della canzone
            initialize_lyrics_screen(trcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_text);
            //sicuramente sono andato INDIETRO dall'ultima posizione, per cui abilito il btnSucc
            btnSucc.IsEnabled = true;

            //sposto il focus sulla prima pagina del pivot
            pivotLyrics.SelectedIndex = 0;
            //sposto il focus sulla prima riga
            float zoom = txtLyricsScroller.ZoomFactor;
            txtLyricsScroller.ChangeView(0, 0, zoom);
        }

        //@ENSURES  carica testo traccia successiva
        private void btnSucc_onClick(object sender, RoutedEventArgs e)
        {
            //salvo la tracca corrente tra le visualizzate
            //Mi sposto col puntatore
            idCurrentTrack++;
            //cambio l'index della combobox
            currentAlbumTracksHeader.SelectedIndex = idCurrentTrack;
            //CASO PARTICOLARE 1: fine array
            btnSucc.IsEnabled = Utilities.verifyIfNotEquals(idCurrentTrack, trcInfo.Array_tracce_currentAlbum.Count - 1);
            //reinizializzo la lista video
            
            //cambio il contenuto del Pivot
            initialize_lyrics_screen(trcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_text);
            //sicuramente sono andato AVANTI dalla prima posizione, per cui abilito il btnPrec
            btnPrec.IsEnabled = true;

            //sposto il focus sulla prima pagina del pivot
            pivotLyrics.SelectedIndex = 0;
            //sposto il focus sulla prima riga
            float zoom = txtLyricsScroller.ZoomFactor;
            txtLyricsScroller.ChangeView(0, 0, zoom);
        }



        //@ENSURES: aumenta la dimensione dei caratteri del testo traccia
        private void IncreaseFontSizeHandler(object sender, RoutedEventArgs e)
        {
            //se non ho ancora raggiunto la dimensione massima del font
            if (txtLyrics.FontSize <= _MaxFontSize) {
                //e se attualmente il pulsante di Diminuzione è disattivato, lo riattivo
                if (btnDecrease.IsEnabled == false)
                    enableAppBarButton(btnDecrease);
                //e infine aumento la dimensione del font
                txtLyrics.FontSize++;
            }
            else
            {
                //altrimenti ho raggiunto la dimensione massima, quindi disabilito il pulsante di Aumento
                disableAppBarButton(btnIncrease);
                //e abilito quell'altro di Diminuzione
                enableAppBarButton(btnDecrease);
            }
        }

       

        //@ENSURES: diminuisce la dimensione dei caratteri del testo traccia
        private void DecreaseFontSizeHandler(object sender, RoutedEventArgs e)
        {
            if (txtLyrics.FontSize >= _MinFontSize)
            {
                if (btnIncrease.IsEnabled == false)
                    enableAppBarButton(btnIncrease);
                txtLyrics.FontSize--;
            }
            else
            {
                disableAppBarButton(btnDecrease);
                enableAppBarButton(btnIncrease);
            }
        }

        private void btnAbout_onClick(object sender, RoutedEventArgs e)
        {
            WPUtilities.visualizza("App creata da riciloma, materiale da marok.org. Tutti i diritti appartengono ai legittimi proprietari", "Riconoscimenti");
        }

        private void btnCondividi_onClick(object sender, RoutedEventArgs e)
        {
            //crea una shareable track
            ShareableTrack shareableTrack = new ShareableTrack();
            shareableTrack.PublicAlbumTitle = viewLyricsData.TrcInfo.current_album.Album_name;
            shareableTrack.PublicTrackLyrics = txtLyrics.Text;
            shareableTrack.PublicTrackTitle = viewLyricsData.TrcInfo.Array_tracce_currentAlbum.ElementAt(idCurrentTrack).Track_title;
            Frame.Navigate(typeof(ShareText), shareableTrack);
        }

        public bool isOdd(int value)
        {
            throw new NotImplementedException();
        }

        public void enableAppBarButton(AppBarButton abb)
        {
            abb.IsEnabled = true;
        }

        public void disableAppBarButton(AppBarButton abb)
        {
            abb.IsEnabled = false;
        }

        private async void btnMoreInfo_Click(object sender, RoutedEventArgs e)
        {
            string uriToLaunch = trcInfo.current_album.Marok_link;
            var uri = new Uri(uriToLaunch);
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);
        }
    }
}