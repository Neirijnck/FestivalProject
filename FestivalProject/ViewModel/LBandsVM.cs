using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FestivalProject.ViewModel
{
    class LBandsVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Bands"; }
        }

        //Property om de geselecteerde band te bewerken
        private Band _selectedBand;

        public Band SelectedBand
        {
            get { return _selectedBand; }
            set { _selectedBand = value; OnPropertyChanged("SelectedBand"); }
        }
        
        //Property om alle bands in te lezen
        private ObservableCollection<Band> _bands;

        public ObservableCollection<Band> Bands
        {
            get { return _bands; }
            set { _bands = value; OnPropertyChanged("Bands"); }
        }

        //Property voor een nieuwe band
        private Band _newBand;

        public Band NewBand
        {
            get { return _newBand; }
            set { _newBand = value; OnPropertyChanged("NewBand"); }
        }

        //Property om alle genres in te lezen
        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; OnPropertyChanged("Genres"); }
        }

        //Property om het pad weer te geven in de applicatie
        private String _naamAfbeelding;

        public String NaamAfbeelding
        {
            get { return _naamAfbeelding; }
            set { _naamAfbeelding = value; OnPropertyChanged("NaamAfbeelding"); }
        }

        //Property selected Genres
        private ObservableCollection<Genre> _selectedGenres;

        public ObservableCollection<Genre> SelectedGenres
        {
            get { return _selectedGenres; }
            set { _selectedGenres = value; OnPropertyChanged("SelectedGenres");}
        }

        //Constructor
        public LBandsVM()
        {
            Bands = Band.GetBands();
            Genres = Genre.GetGenres();
            SelectedBand = Bands[0];
            NewBand = new Band();
            SelectedGenres = new ObservableCollection<Genre>();
        }

        //Command om aangevinkte genres toe te voegen aan de list
        public ICommand AddGenreToListCommand
        {
            get
            {
                return new RelayCommand<CheckBox>(AddGenreToList);
            }
        }

        //Method om genres te adden aan de list
        private void AddGenreToList(CheckBox chk)
        {
            if (chk.IsChecked == true) 
            {
                Genre genre = new Genre();
                genre.Name = Convert.ToString(chk.Content);
                SelectedGenres.Add(genre);
            }
        }

        //Command om aangevinkte genres te verwijderen van de list
        public ICommand RemoveGenreFromListCommand
        {
            get
            {
                return new RelayCommand<CheckBox>(RemoveGenreFromList);
            }
        }

        //Method om genres te verwijderen van de list
        private void RemoveGenreFromList(CheckBox chk)
        {
            if (chk.IsChecked == false) 
            {
                Genre genre = new Genre();
                genre.Name = Convert.ToString(chk.Content);
                SelectedGenres.Remove(genre);
            }
        }

        //Command om een nieuwe band toe te voegen
        public ICommand AddCommand 
        {
            get 
            {
                return new RelayCommand(AddBand, NewBand.IsValid);
            }
        }

        //Method om band toe te voegen in database
        private void AddBand() 
        {
            NewBand.Picture = NaamAfbeelding;
            NewBand.Genres = SelectedGenres;
            int affected = Band.AddBand(NewBand);
            if (affected == 1)
            {
                Bands.Add(NewBand);
                OnPropertyChanged("Bands");
                int LastIndex = Bands.Count - 1;
                SelectedBand = Bands[LastIndex];
                Console.WriteLine("Band werd succesvol toegevoegd in de database.");
                ModernDialog.ShowMessage("De band/artiest werd toegevoegd.", "Toevoegen", MessageBoxButton.OK);
            }
        }

        //Command om band te bewerken
        public ICommand EditBandCommand 
        {
            get 
            {
                return new RelayCommand(EditBand);
            }
        }

        //Method om band te bewerken
        private void EditBand()
        {
            if (SelectedBand.Id != null)
            {
                if (NaamAfbeelding != null)
                {
                    SelectedBand.Picture = NaamAfbeelding;
                }
                int affected = Band.EditBand(SelectedBand);
                if (affected == 1)
                {
                    Bands = Band.GetBands();
                    int LastIndex = Bands.Count - 1;
                    SelectedBand = Bands[LastIndex];
                    Console.WriteLine("Band werd succesvol aangepast in de database.");
                    ModernDialog.ShowMessage("De band/artiest werd aangepast in de database.", "Aanpassen", MessageBoxButton.OK);
                }
            }
        }

        //Command om nieuwe afbeelding toe te voegen
        public ICommand AddPictureCommand 
        {
            get 
            {
                return new RelayCommand(AddPicture);
            }
        }

        //Method om picture toe te voegen
        private void AddPicture() 
        {
            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            ofd.Filter = "JPEG |*.jpg;*.jpeg";
            ofd.Title = "Kies een afbeelding";


            if (ofd.ShowDialog() == true)
            {
                NaamAfbeelding = System.IO.Path.GetFileName(ofd.FileName);
                try
                {
                    if (File.Exists(ofd.FileName) == true)
                    {
                        File.Copy(ofd.FileName, AppDomain.CurrentDomain.BaseDirectory + "//Assets//" +NaamAfbeelding);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

        }
    }
}
