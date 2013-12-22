using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FestivalProject.ViewModel
{
    class TTypesVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Types"; }
        }

        //Alle tickettypes worden ingelezen
        private ObservableCollection<TicketType> _tickettypes;

        public ObservableCollection<TicketType> TicketTypes
        {
            get { return _tickettypes; }
            set { _tickettypes = value; OnPropertyChanged("TicketTypes"); }
        }

        //Property voor het geselecteerde tickettype
        private TicketType _selectedTicketType;

        public TicketType SelectedTicketType
        {
            get { return _selectedTicketType; }
            set { _selectedTicketType = value; OnPropertyChanged("SelectedTicketType"); }
        }

        //Property voor een nieuw tickettype
        private TicketType _ticketType;

        public TicketType TicketType
        {
            get { return _ticketType; }
            set { _ticketType = value; OnPropertyChanged("TicketType"); }
        }

        //Constructor die properties initialiseert
        public TTypesVM()
        {
            TicketTypes = TicketType.GetTicketTypes();
            SelectedTicketType = TicketTypes[0];
            TicketType = new TicketType();
        }

        //Command om nieuw type op te slaan
        public ICommand OpslaanCommand
        {
            get
            {
                return new RelayCommand(AddTicketType, TicketType.IsValid);     //eigen methode aanroepen
            }
        }

        //Method om het type op te slaan in de database
        private void AddTicketType() 
        {
            int affected = TicketType.AddTicketType(TicketType);
            if (affected == 1) 
            {
                TicketTypes.Add(TicketType);
                //TicketTypes = TicketType.GetTicketTypes();
                TicketType = new TicketType();
                int LastIndex = TicketTypes.Count - 1;
                SelectedTicketType = TicketTypes[LastIndex];
            }
        }

        //Command om een tickettype te bewerken
        public ICommand EditCommand 
        {
            get 
            {
                return new RelayCommand(EditTicketType, SelectedTicketType.IsValid);
            }
        }

        //Method om type te bewerken
        private void EditTicketType() 
        {
            if (SelectedTicketType.Id != null) 
            {
                int affected = TicketType.EditTicketType(SelectedTicketType);
                int index = TicketTypes.IndexOf(SelectedTicketType);
                //TicketTypes = TicketType.GetTicketTypes();
                SelectedTicketType = TicketTypes[index];
                if (affected == 1) 
                {
                    Console.WriteLine("TicketType werd succesvol aangepast in de database.");
                    ModernDialog.ShowMessage("Het tickettype werd aangepast in de database.", "Aanpassen", MessageBoxButton.OK);
                }
            }
        }
    }
}
