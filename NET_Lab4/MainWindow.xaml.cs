using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using NET_Lab4.Controls;


namespace NET_Lab4
{
    public partial class MainWindow : Window
    {
        bool IsFileLoaded;
        string LastSavedFileContent;
        string LastSavedFileName;

        TextRange RTBTextRange;
        OpenFileDialog OFD;
        SaveFileDialog SFD;

        public MainWindow()
        {
            InitializeComponent();
            IsFileLoaded = false;
            RTB.IsReadOnly = true;
            RTBTextRange = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
        }

        private void AskToSaveFile()
        {
            if(MessageBox.Show("Czy zapisać ten plik?", "Zapisz", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Save_Executed();
            }
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if(AreAnyChangesMade())
            {
                AskToSaveFile();
            }
            RTB.Document.Blocks.Clear();
            RTBTextRange = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
            RTB.IsReadOnly = false;
            LastSavedFileContent = null;
            LastSavedFileName = null;
            IsFileLoaded = false;
            Title = "Nowy dokument tekstowy";

        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AreAnyChangesMade())
            {
                AskToSaveFile();
            }

            OFD = new OpenFileDialog();
            OFD.Filter = "Dokument tekstowy (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";
            if(OFD.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(OFD.FileName, FileMode.Open);

                switch (OFD.FileName.Substring(OFD.FileName.Length - 3))
                {
                    case "rtf":
                        RTBTextRange.Load(fileStream, DataFormats.Rtf);
                        break;
                    case "txt":
                        RTBTextRange.Load(fileStream, DataFormats.Text);
                        break;
                }

                IsFileLoaded = true;
                LastSavedFileContent = RTBTextRange.Text;
                LastSavedFileName = OFD.FileName;
                Title = LastSavedFileName;
                fileStream.Close();
                RTB.IsReadOnly = false;
            }
        }

        private void Save_Executed(object sender = null, ExecutedRoutedEventArgs e = null)
        {
            if (IsFileLoaded)
            {
                FileStream fileStream = new FileStream(LastSavedFileName, FileMode.Create);

                switch (LastSavedFileName.Substring(LastSavedFileName.Length - 3))
                {
                    case "rtf":
                        RTBTextRange.Save(fileStream, DataFormats.Rtf);
                        break;
                    case "txt":
                        RTBTextRange.Save(fileStream, DataFormats.Text);
                        break;
                }
                LastSavedFileContent = RTBTextRange.Text;
                IsFileLoaded = true;
                fileStream.Close();
            }
            else
            {
                SaveAs_Executed();
            }
        }

        private void SaveAs_Executed(object sender = null, ExecutedRoutedEventArgs e = null)
        {
            SFD = new SaveFileDialog();
            SFD.Filter = "Dokument tekstowy (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";
            if (SFD.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(SFD.FileName, FileMode.Create);

                switch (SFD.FileName.Substring(SFD.FileName.Length - 3))
                {
                    case "rtf":
                        RTBTextRange.Save(fileStream, DataFormats.Rtf);
                        break;
                    case "txt":
                        RTBTextRange.Save(fileStream, DataFormats.Text);
                        break;
                }

                LastSavedFileContent = RTBTextRange.Text;
                LastSavedFileName = SFD.FileName;
                Title = LastSavedFileName;
                IsFileLoaded = true;
                fileStream.Close();
            }
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (AreAnyChangesMade())
            {
                AskToSaveFile();
            }
            RTB.Document.Blocks.Clear();
            RTB.IsReadOnly = true;
            IsFileLoaded = false;
            LastSavedFileName = null;
            LastSavedFileContent = null;
            Title = "";
        }

        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("NOWY PLIK - Tworzy nowy plik tekstowy, który można dowolnie wypełnić tekstem. Jeżeli w aktualnie używanym pliku były jakiekolwiek niezapisane zmiany to użytkownik zostanie zapytany czy zapisać ten plik przed stworzeniem nowego. \n\n" +
                            "OTWÓRZ PLIK - Otwiera istniejący już i wybrany przez użytkownika plik w formacie (.rtf) lub (.txt). \n\n" +
                            "ZAPISZ PLIK - Zapisuje aktualnie używany plik prosząc o podanie przez użytkownika nazwy, lokalizacji oraz formatu pliku. Jeżeli plik został już wcześniej zapisany nadpisuje go. \n\n" +
                            "ZAPISZ PLIK JAKO - Zapisuje aktualnie używany plik prosząc o podanie przez użytkownika nazwy, lokalizacji oraz formatu pliku. \n\n" +
                            "ZAMKNIJ PLIK - Zamyka aktualnie używany plik. Jeżeli w aktualnie używanym pliku były jakiekolwiek niezapisane zmiany to użytkownik zostanie zapytany czy zapisać ten plik przed zamknięciem go.\n\n" +
                            "COFNIJ - Cofa ostatnio wykonaną akcje. \n\n" +
                            "PRZYWRÓĆ - Ponawia ostatnio cofniętą akcje. \n\n" +
                            "WYTNIJ - Wycina zaznaczony fragment tekstu. \n\n" +
                            "KOPIUJ - Kopiuje zaznaczony framgent tekstu. \n\n" +
                            "WKLEJ - Wkleja w wybrane miejsce tekst przechowywany w schowku. \n\n" +
                            "O PROGRAMIE - Wyświetla komunikat z informacją o wersji, oraz twórcy programu \n\n" +
           "", "Pomoc", MessageBoxButton.OK);
        }


        private bool IsRichTextBoxEmpty()
        {
            if (RTBTextRange == null)
            {
                return true;
            }
            else
            {
                return String.IsNullOrWhiteSpace(RTBTextRange.Text);
            }
        }

        private bool AreAnyChangesMade()
        {
            if (!IsFileLoaded)
            {
                if(IsRichTextBoxEmpty())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if(LastSavedFileContent != RTBTextRange.Text)
                {
                    return true;                  
                }
                else
                {
                    return false;
                }
            }
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !IsRichTextBoxEmpty() | AreAnyChangesMade();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (AreAnyChangesMade())
            {
                AskToSaveFile();
            }
        }

        private void RTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            RTBTextRange = new TextRange(RTB.Document.ContentStart, RTB.Document.ContentEnd);
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Notatnik  v1.0 \n" + (char)169 + " Adrian Wanczewski", "O programie", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
