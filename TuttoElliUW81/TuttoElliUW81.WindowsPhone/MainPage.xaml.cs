using SQLite;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TuttoElliUW81.SharedModel;
using Windows.Phone.UI.Input;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using Windows.Security.Authentication.Web;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TuttoElliUW81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
    

        //private bool IsTrackSelectActive;
        
        private List<Suggestion> suggestions;
        

        ////////////////////////////////COSTRUTTORE///////////////////////////////
        public MainPage()
        {

            InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            //carica suggerimenti
            loadSuggestions();
        }

        //pressione del pulsante Back nella home page
        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        //Override per evitare che, al ritorno sull'app dopo TaskManager, listpicker dia problemi
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            
            
        }

 

        /// <summary>
        /// funzione che carica l'array di suggerimenti lato codice per evitare di fare una query sul DB
        /// prende i due array separati di tracce e album e fa una join, estraendo tutte le tracce e tutti gli album
        /// li aggiunge poi in suggestions.
        /// </summary>
        private void loadSuggestions()
        {
            
            suggestions = new List<Suggestion>();
            //query in LINQ che effettua la join e restituisce i 3 campi da visualizzare nella suggest box
            var query = (from album in App.alb_array
                        join track in App.trc_array on album.Idalbum equals track.Idalbum
                        select new { TrackTitle=track.Track_title, AlbumName=album.Album_name, AlbumImg=album.Album_img, IdTracks=track.Idtrack }).ToList();
            //aggiunge elementi alla lista "suggestions" con i campi estratti dalla query, ciclando il risultato di quest'ultima
            foreach ( var row in query)
            {
                suggestions.Add(new Suggestion(row.AlbumName, row.TrackTitle, row.AlbumImg, row.IdTracks));
            }
        }

  
        //metodo usato per popolare la lista di suggerimenti
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            ObservableCollection<Suggestion> visibleSuggestions = new ObservableCollection<Suggestion>();
            if (sender.Text == "")
                return;
            else {

                foreach (Suggestion suggestion in suggestions)
                {
                    if (Regex.Match(suggestion.Track_title.ToLower(), 
                        sender.Text.ToLower().Replace(@"/[-\/\\^$*+?.() |[\]{ }]/g", @"\\$&"),
                        RegexOptions.IgnoreCase, 
                        Regex.InfiniteMatchTimeout
                        ).Success)
                    {
                        visibleSuggestions.Add(suggestion);
                    }
                }
                sender.ItemsSource = visibleSuggestions;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Suggestion clickedSuggestion = args.SelectedItem as Suggestion;
            
            //metto tutto in un oggetto e passo alla pagina successiva

            Frame.Navigate(typeof(ViewLyrics), prepareDataToPass(App.alb_array, 
                App.trc_array, clickedSuggestion.Album_name, clickedSuggestion.Idtrack_forArray));
        }

        /// <summary>
        /// Prepara un pacchetto dati da mandare alla pagina ViewLyrics, 
        /// contenente lista degli album, delle tracce, nome album e id traccia scelta
        /// </summary>
        /// <param name="albums">Lista album</param>
        /// <param name="tracks">Lista tracce </param>
        /// <param name="album_name">Nome album</param>
        /// <param name="idtrack">Numero traccia scelta</param>
        /// <returns></returns>
        private ViewLyricsData prepareDataToPass(ObservableCollection<Album> albums, ObservableCollection<Track> tracks, string album_name, int idtrack)
        {
            ViewLyricsData viewLyricsData = new ViewLyricsData();
            //passo tutti gli album
            viewLyricsData.albums = albums;
            viewLyricsData.tracks = tracks;
            viewLyricsData.TrcInfo.current_album = App.alb_array.First<Album>(
                album => album.Album_name == album_name
                );
            //passo l'array di tracce dell'intero album
            viewLyricsData.TrcInfo.Array_tracce_currentAlbum = Utilities.ToObservableCollection<Track>(
                App.trc_array.Where<Track>(
                    track => track.Idalbum == viewLyricsData.TrcInfo.current_album.Idalbum)
                );
            viewLyricsData.TrcInfo.idTracciaScelta = idtrack;
            return viewLyricsData;
        }

     
        private void lastViewedTracks_OnLoaded(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            List<String> savedTracks = WPUtilities.deserializeJsonToList((String)localSettings.Values["viewedLyrics"]);

            //carica le tracce
            if (savedTracks == null || savedTracks.Count <= 0)
            {
                ObservableCollection<String> defaultValue = new ObservableCollection<String>();
                String t = "Nessun elemento da visualizzare";
                defaultValue.Add(t);
                (sender as ListBox).ItemsSource = defaultValue;
            }
            else {
                (sender as ListBox).ItemsSource = savedTracks.Distinct().Reverse();
            }
               
        }

        private void lbLastViewedTracks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            String t = (sender as ListBox).SelectedItem as String;
            Track track = App.trc_array.First(tr => tr.Track_title == t);
            //cerco il nome dell'album
            string album_name = App.alb_array.First(
                album => album.Idalbum == track.Idalbum
                ).Album_name;
            //quando clicco su una traccia, apro il menu corrispondente
            Frame.Navigate(typeof(ViewLyrics), prepareDataToPass(App.alb_array,
                App.trc_array, album_name, track.Idtrack_forArray));
        }


        private void btnAbout_onClick(object sender, RoutedEventArgs e)
        {
            WPUtilities.visualizza("App creata da riciloma, materiale da marok.org. Tutti i diritti appartengono ai legittimi proprietari", "Riconoscimenti");
        }

    }
}
