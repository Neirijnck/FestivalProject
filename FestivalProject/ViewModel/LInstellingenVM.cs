using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FestivalProject.ViewModel
{
    class LInstellingenVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Instellingen"; }
        }

        //Property voor festival
        private ObservableCollection<Festival> _festivals;

        public ObservableCollection<Festival> Festivals
        {
            get { return _festivals; }
            set { _festivals = value; OnPropertyChanged("Festivals"); }
        }

        //Property om alle stages in te lezen
        private ObservableCollection<Stage> _stages;

        public ObservableCollection<Stage> Stages
        {
            get { return _stages; }
            set { _stages = value; OnPropertyChanged("Stages"); }
        }

        //Property om alle genres in te lezen
        private ObservableCollection<Genre> _genres;

        public ObservableCollection<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; OnPropertyChanged("Genres"); }
        }

        //Property om een nieuw festival toe te voegen
        private Festival _festivalData;

        public Festival FestivalData
        {
            get { return _festivalData; }
            set { _festivalData = value; OnPropertyChanged("FestivalData"); }
        }        

        //Property voor de geselecteerd startdatum
        private DateTime? _selectedStartDate;

        public DateTime? SelectedStartDate
        {
            get { return _selectedStartDate; }
            set { _selectedStartDate = value; OnPropertyChanged("SelectedStartDate"); }
        }
        
        //Property voor de geselecteerde einddatum
        private DateTime? _selectedEndDate;

        public DateTime? SelectedEndDate
        {
            get { return _selectedEndDate; }
            set { _selectedEndDate = value; OnPropertyChanged("SelectedEndDate"); }
        }

        //Property om de uiteindelijk datum weer te geven in textbox
        private String _fullDate;

        public String FullDate
        {
            get { return _fullDate; }
            set { _fullDate = value; OnPropertyChanged("FullDate"); }
        }

        //Property om nieuwe stage toe te voegen
        private Stage _newStage;

        public Stage NewStage
        {
            get { return _newStage; }
            set { _newStage = value; OnPropertyChanged("NewStage"); }
        }    
    
        //Property om nieuw genre toe te voegen
        private Genre _newGenre;

        public Genre NewGenre
        {
            get { return _newGenre; }
            set { _newGenre = value; OnPropertyChanged("NewGenre"); }
        }

        //Property voor de geselecteerde stage
        private Stage _selectedStage;

        public Stage SelectedStage
        {
            get { return _selectedStage; }
            set { _selectedStage = value; OnPropertyChanged("SelectedStage"); }
        }

        //Property voor geselecteerde genre
        private Genre _selectedGenre;

        public Genre SelectedGenre
        {
            get { return _selectedGenre; }
            set { _selectedGenre = value; OnPropertyChanged("SelectedGenre"); }
        }

        //Constructor
        public LInstellingenVM()
        {
            Festivals = Festival.GetFestivals();
            Stages = Stage.GetStages();
            Genres = Genre.GetGenres();
            FestivalData = new Festival();
            //eind en begin datum zetten indien er al een in de database aanwezig is
            SelectedStartDate = Festival.GetStartDate();
            SelectedEndDate = Festival.GetEndDate();
            FullDate = Convert.ToDateTime(SelectedStartDate).ToShortDateString() + "    -    " + Convert.ToDateTime(SelectedEndDate).ToShortDateString();

            NewStage = new Stage();
            NewGenre = new Genre();
        }

        //Command om Genre aan te passen
        public ICommand EditGenreCommand 
        {
            get 
            {
                return new RelayCommand<SelectionChangedEventArgs>(EditGenre);
            }
        }

        //Method om genre aan te passen
        private void EditGenre(SelectionChangedEventArgs e) 
        {
            if (e.RemovedItems.Count > 0)
            {
                Genre genre = e.RemovedItems[0] as Genre;
                if (genre.Id != null)
                {
                    int affected = Genre.EditGenre(genre);
                    if (affected == 1)
                    {
                        Console.WriteLine("Genre werd succesvol aangepast in de database");
                    }
                }
            }
        }

        //Command om Genre toe te voegen
        public ICommand OpslaanGenreCommand 
        {
            get 
            {
                return new RelayCommand(AddGenre, NewGenre.IsValid);
            }
        }

        //Method om genre toe te voegen
        private void AddGenre() 
        {
            int affected = Genre.AddGenre(NewGenre);
            if (affected == 1)
            {
                Genres.Add(NewGenre);
                NewGenre = new Genre();
                int LastIndex = Genres.Count - 1;
                SelectedGenre = Genres[LastIndex];
            }
        }

        //Command om Stage toe te voegen in database
        public ICommand OpslaanStageCommand 
        {
            get 
            {
                return new RelayCommand(AddStage, NewStage.IsValid);
            }
        }

        //Method om stage toe te voegen
        private void AddStage() 
        {
            int affected = Stage.AddStage(NewStage);
            if (affected == 1) 
            {
                Stages.Add(NewStage);
                NewStage = new Stage();
                int LastIndex = Stages.Count - 1;
                SelectedStage = Stages[LastIndex];
            }
        }

        //Command om stage te bewerken
        public ICommand EditStageCommand
        {
            get
            {
                return new RelayCommand<SelectionChangedEventArgs>(EditStage);
            }
        }

        //Method om stage te bewerken
        private void EditStage(SelectionChangedEventArgs e) 
        {
            if (e.RemovedItems.Count > 0)
            {
                Stage stage = e.RemovedItems[0] as Stage;
                if (stage.Id != null)
                {
                    int affected = Stage.EditStage(stage);
                    if (affected == 1)
                    {
                        Console.WriteLine("Stage werd succesvol aangepast in de database");
                    }
                }
            }
        }

        //Command om data toe te voegen in database
        public ICommand OpslaanDataCommand 
        {
            get 
            {
                return new RelayCommand(AddDates, FestivalData.IsValid);
            }
        } 

        //Method om de data toe te voegen in de database
        private void AddDates() 
        {
            DateTime? testStart = Festival.GetStartDate();

            //Al een datum aanwezig, moet enkel aanpassen
            if (testStart != null) 
            {
                int affected = Festival.EditFestival(FestivalData);
                if (affected == 1) 
                {
                    ModernDialog.ShowMessage("De data is gewijzigd.", "Data", MessageBoxButton.OK);
                }
            }
             //Nog niets van data aanwezig in database, toevoegen
            else
            {
                int affected = Festival.AddFestival(FestivalData);
                if (affected == 1)
                {
                    Festivals.Add(FestivalData);
                    ModernDialog.ShowMessage("De data is toegevoegd.", "Data", MessageBoxButton.OK);
                }
            }
        }

        //Command om de data te controleren
        public ICommand ControlDates 
        {
            get 
            {
                return new RelayCommand<Button>(ControleerData);
            }
        }

        //Method om de data te controleren
        private void ControleerData(Button btnData)
        {
            String Date1 = "";
            String Date2 = "";

            //Eerste datum opvragen en tonen in textbox
            if (SelectedStartDate == null) { FullDate = Date1; }
            else 
            {
                Date1 = SelectedStartDate.Value.ToShortDateString();
                FullDate = Date1;
            }

            //Tweede datum opvragen en tonen in textbox
            if (SelectedEndDate == null) { FullDate = Date1 + "    -    " + Date2; }
            else 
            {
                Date2 = SelectedEndDate.Value.ToShortDateString();
                FullDate = Date1 + "    -    " + Date2;
            }

            //Controle dat eerste datum niet groter is dan tweede
            DateTime EersteDatum = Convert.ToDateTime(SelectedStartDate);
            DateTime TweedeDatum = Convert.ToDateTime(SelectedEndDate);

            //TimeSpan Difference = TweedeDatum.Subtract(EersteDatum);
            int resultCompare = DateTime.Compare(EersteDatum, TweedeDatum);

            if (Date1 != "" && Date2 != "" && resultCompare <= 0)
            {
                FestivalData.StartDate = EersteDatum;
                FestivalData.EndDate = TweedeDatum;
                Console.WriteLine("Data is OK!");
                btnData.IsEnabled = true;
            }
            else 
            {
                Console.WriteLine("Data verkeerd!");

                //Data is niet ok, button moet gedisabled worden!!!
                //FestivalData.IsValid = false;
                btnData.IsEnabled = false;
            }
        }
    }
}
