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
    class TBestellenVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Bestellen"; }
        }

        //Property voor alle tickettypes in te lezen
        private ObservableCollection<TicketType> _tickettypes;

        public ObservableCollection<TicketType> TicketTypes
        {
            get { return _tickettypes; }
            set { _tickettypes = value; OnPropertyChanged("TicketTypes"); }
        }
        
        //Property om alle bestellers van tickets in te lezen
        private ObservableCollection<Ticket> _holders;

        public ObservableCollection<Ticket> Holders
        {
            get { return _holders; }
            set { _holders = value; OnPropertyChanged("Holders");}
        }

        //Property om een nieuwe besteller toe te voegen
        private Ticket _ticketHolder;

        public Ticket TicketHolder
        {
            get { return _ticketHolder; }
            set { _ticketHolder = value; OnPropertyChanged("TicketHolder");}
        }

        //Property om de geselecteerde besteller aan te passen
        private Ticket _selectedTicketHolder;

        public Ticket SelectedTicketHolder
        {
            get { return _selectedTicketHolder; }
            set { _selectedTicketHolder = value; OnPropertyChanged("SelectedTicketHolder");}
        }
        
        //Property om aantal verkochte tickets op te halen
        private int _verkochteTickets;

        public int VerkochteTickets
        {
            get { return _verkochteTickets; }
            set { _verkochteTickets = value; OnPropertyChanged("VerkochteTickets"); }
        }

        //Constructor 
        public TBestellenVM()
        {
            Holders = Ticket.GetTicketHolders();
            TicketTypes = TicketType.GetTicketTypes();
            VerkochteTickets = Ticket.GetAmountSoldTickets();
            TicketHolder = new Ticket();
        }

        //Command om nieuwe toe te voegen
        public ICommand AddCommand 
        {
            get 
            {
                return new RelayCommand(AddTicketHolder, TicketHolder.IsValid);
            }
        }

        //method om een nieuwe tickethouder in te voeren
        private void AddTicketHolder() 
        {
            int affected = Ticket.AddTicketHolder(TicketHolder);
         
            if (affected == 1)
            {
                //Holders = Ticket.GetTicketHolders();
                Holders.Add(TicketHolder);
                VerkochteTickets = Ticket.GetAmountSoldTickets();

                //aantal beschikbare tickets veranderen
                int affectedTicket =  TicketType.ChangeAvailableTickets(TicketHolder.TicketType, TicketHolder.Amount);
                if (affectedTicket == 1) { Console.WriteLine("Aantal beschikbare tickets zijn succesvol aangepast"); }

                int LastIndex = Holders.Count - 1;
                SelectedTicketHolder = Holders[LastIndex];

                ModernDialog.ShowMessage("Uw tickets zijn besteld.", "Bestelling", MessageBoxButton.OK);

                //+ laten verschijnen in datagrid en alles laten aanpassen!
                //(zie mail henk!!)
            }
           
        }

        //Command om een tickethouder te bewerken
        public ICommand EditCommand 
        {
            get 
            {
                return new RelayCommand<SelectionChangedEventArgs>(EditTicketHolder);
            }
        }
        
        //Method om iemand te bewerken
        private void EditTicketHolder(SelectionChangedEventArgs e) 
        {
            if (e.RemovedItems.Count > 0)
            {
                Ticket holder = e.RemovedItems[0] as Ticket;
                int affected = Ticket.EditTicketHolder(holder);
                if (affected == 1)
                {
                    Console.WriteLine("Tickethouder werd succesvol aangepast in de database");
                }
            }
        }       

    }
}
