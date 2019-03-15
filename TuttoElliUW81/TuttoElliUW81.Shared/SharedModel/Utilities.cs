using System;
using Windows.UI.Popups;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;
using SQLite;

namespace TuttoElliUW81.SharedModel
{
    public class Utilities 
    {

        

        //@ ensures: COPIA IL FILE COME CONTENUTO IN UNA VARIABILE, PER POI COPIARLO NELLA CARTELLA LOCALE DEL PROGRAMMA
        public static async Task CopyDatabase()
        {
            bool isDatabaseExisting = false;

            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync("DB.sqlite");
                isDatabaseExisting = true;
            }
            catch
            {
                isDatabaseExisting = false;
            }

            if (!isDatabaseExisting)
            {
                StorageFile databaseFile = await Package.Current.InstalledLocation.GetFileAsync("DB.sqlite");
                await databaseFile.CopyAsync(ApplicationData.Current.LocalFolder);
            }
        }

        //restituisce se un numero è dispari o no
        public static Boolean IsOdd(int value)
        {
            if (value % 2 != 0) 
                return true;
            return false;
        }

       

        public static void visualizzaSuConsole(string testo, string title)
        {
            System.Diagnostics.Debug.WriteLine(title+": "+testo);
        }

        public void Splitter(string str, StackPanel sp)
        {
            StringBuilder build = new StringBuilder();
            //string[] words = str.Split(new [] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            List<string> words = new List<string>(str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));
            int f, g = 0; //contatori
            int h = 0; // incremento graduale
            int sub; //divisore n° textbox
            
            /*
            --> l'algoritmo funziona dividendo un numero per il suo divisore più piccolo (per risparmiare n° di textbox)
                e usando il risultato F per dividere il testo in F textblock.
             * CASO PARTICOLARE 1: testo dispari
               SOLUZIONE: meglio aggiungere una riga in più se il numero è dispari, 
               in modo da ridurre le textbox usate*/
            if (Utilities.IsOdd(words.Count))
                words.Add(System.Environment.NewLine);
            int length = words.Count; //conto delle parole aggiornato
            //a questo punto, dato che il numero sarà probabilmente pari, calcolo il n° di suddivisioni
            sub = Utilities.findSub(length);
            f = length / sub;
            h = f;
            TextBlock tb;
            for (; f < length; f += h)
            {

                for (; g < f; g++)
                {
                    build.Append(words.ElementAt(g) + System.Environment.NewLine);

                }
                tb = new TextBlock();
                tb.Text = build.ToString();

                tb.TextWrapping = TextWrapping.Wrap;

                sp.Children.Add(tb);
                build.Clear();
            }

            f -= h; // riporto in posizione finale 

            //se il numero non si è diviso perfettamente, prendo le words mancanti
            //f ha continuato ad aumentare.
            if ((length - f) != 0)
            {
                for (; g < length; g++)
                    build.Append(words.ElementAt(g) + System.Environment.NewLine);

                tb = new TextBlock();
                tb.Text = build.ToString();

                tb.TextWrapping = TextWrapping.Wrap;

                sp.Children.Add(tb);
                build.Clear();
            }
        }

        

        //@ENSURES: Formatta il testo letto, sostituendo \r\n con "a capo" ed eliminando eventuali occorrenze di \
        public static string Parser(string str)
        {

            //return str.Replace("\r\n", System.Environment.NewLine).Replace("\n", System.Environment.NewLine).Replace("\r", System.Environment.NewLine).Replace(@"\", ""); 
            return str.Replace("\n", System.Environment.NewLine).Replace("\r", "").Replace("\r\n", "");
        }


        //la funzione cerca il più grande divisore di un numero "valore" (diverso da valore) e lo restituisce
        public static int findSub(int valore)
        {

            if (valore == 1) return valore;

            for (int i = valore - 1; i > 2; i--)
            {
                if (valore % i == 0)
                    return i;
            }
            return 2;
        }



        //@Ensures: Carica un'immagine Contenuto dal pacchetto
        public static void LoadImage(string imageName, Image image)
        {
            //Content
            Uri uri = new Uri(imageName, UriKind.Relative);
            BitmapImage imgSource = new BitmapImage(uri);
            image.Source = imgSource;
        }

        public static ImageSource ImagePathStringToImageSourceConverter(string path)
        {
            return new BitmapImage(new Uri("ms-appx://" + path, UriKind.Absolute));
        }

        public static ObservableCollection<T> executeQueryOnDB<T>(SQLiteConnection connection, string query)
        {
            SQLiteCommand command = connection.CreateCommand(query);
            return new ObservableCollection<T>(command.ExecuteQuery<T>());
        }

        /// <summary>
        /// FUNZIONE PER TRASFORMARE IN OBSERVABLE COLLECTIONS
        /// </summary>
        /// <typeparam name="T"> QUALSIASI PARAMETRO</typeparam>
        /// <param name="list">LIST</param>
        /// <returns>OBSERVABLE COLLECTION CHE CONTIENE GLI ELEMENTI DELLA LISTA</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(List<T> list)
        {
            if (list.Count == 0)
                return null;
            else {
                ObservableCollection<T> observableCollection = new ObservableCollection<T>();
                foreach(T element in list)
                {
                    observableCollection.Add(element);
                }
                return observableCollection;
            }
        }

        /// <summary>
        /// FUNZIONE PER TRASFORMARE IN OBSERVABLE COLLECTIONS
        /// </summary>
        /// <typeparam name="T"> QUALSIASI PARAMETRO</typeparam>
        /// <param name="list">IENUMERABLE</param>
        /// <returns>OBSERVABLE COLLECTION CHE CONTIENE GLI ELEMENTI DELLA LISTA</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> list)
        {
            //se la lista è vuota, restituisci null
            if (list.ToList().Count == 0)
                return null;
            else {
                //altrimenti riempi la nuova lista con gli oggetti di list
                ObservableCollection<T> observableCollection = new ObservableCollection<T>();
                foreach (T element in list)
                {
                    observableCollection.Add(element);
                }
                return observableCollection;
            }
        }

        public static List<String> MakeUpperCase(List<String> toBeUpped)
        {
            List<String> alreadyUpped = new List<string>();
            foreach (string element in toBeUpped)
            {
                alreadyUpped.Add(element.ToUpper());
            }
            return alreadyUpped;
        }

        //Verifica che un numero sia diverso da un altro
        public static Boolean verifyIfNotEquals(int current, int verify)
        {
            if (current != verify)
                return true;
            else
                return false;
        }

        public static T FindDescendant<T>(DependencyObject obj) where T : DependencyObject
        {
            // Check if this object is the specified type
            if (obj is T)
                return obj as T;

            // Check for children
            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            if (childrenCount < 1)
                return null;

            // First check all the children
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child is T)
                    return child as T;
            }

            // Then check the childrens children
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = FindDescendant<T>(VisualTreeHelper.GetChild(obj, i));
                if (child != null && child is T)
                    return child as T;
            }

            return null;
        }

        public async static void printList(List<Album> list)
        {
            MessageDialog msgDialog = new MessageDialog("");
            
                foreach (Album elem in list)
                {
                    msgDialog.Content = elem.Album_name;
                    await msgDialog.ShowAsync();
                }
         }
        
    }
}
