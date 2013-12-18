using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
                if (holder.Id != null)
                {
                    int affected = Ticket.EditTicketHolder(holder);
                    if (affected == 1)
                    {
                        Console.WriteLine("Tickethouder werd succesvol aangepast in de database");
                    }
                }
            }
        }       

        //Command om tickets te printen
        public ICommand PrintCommand
        {
            get
            {
                return new RelayCommand(PrintTickets);
            }
        }

        //Method om te printen
        private void PrintTickets()
        {
            Ticket test = Ticket.GetLastTicketHolder();
            if (TicketHolder.TicketHolder == test.TicketHolder && TicketHolder.TicketHolderEmail == test.TicketHolderEmail && TicketHolder.TicketType.Name == test.TicketType.Name && TicketHolder.Amount == test.Amount)
            {
                //export naar word

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Word Doc|*.docx";
                sfd.FileName = "FestivalTicket_" + TicketHolder.TicketHolder + ".docx";
                if (sfd.ShowDialog() == true)
                {

                    if (File.Exists("template.docx") == true)
                    {
                        File.Copy("template.docx", sfd.FileName, true);
                    }

                    WordprocessingDocument newdoc = WordprocessingDocument.Open(sfd.FileName, true);

                    IDictionary<String, BookmarkStart> bookmarks = new Dictionary<String, BookmarkStart>();

                    foreach (BookmarkStart bms in newdoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>())
                    {
                        bookmarks[bms.Name] = bms;
                    }

                    Run run1 = new Run(new Text(TicketHolder.TicketHolder));
                    RunProperties prop1 = new RunProperties();
                    RunFonts font1 = new RunFonts() { Ascii = "Source Sans Pro", HighAnsi = "Source Sans Pro" };
                    FontSize size1 = new FontSize() { Val = "20pt" };
                    prop1.Append(font1);
                    prop1.Append(size1);
                    run1.PrependChild<RunProperties>(prop1);

                    bookmarks["TicketHolder"].Parent.InsertAfter<Run>(run1, bookmarks["TicketHolder"]);

                    Run run2 = new Run(new Text(TicketHolder.TicketType.Name));
                    RunProperties prop2 = new RunProperties();
                    RunFonts font2 = new RunFonts() { Ascii = "Source Sans Pro", HighAnsi = "Source Sans Pro" };
                    FontSize size2 = new FontSize() { Val = "20pt" };
                    prop2.Append(font2);
                    prop2.Append(size2);
                    run2.PrependChild<RunProperties>(prop2);
                    bookmarks["Day"].Parent.InsertAfter<Run>(run2, bookmarks["Day"]);

                    Run run3 = new Run(new Text(TicketHolder.TicketType.Price.ToString()));
                    RunProperties prop3 = new RunProperties();
                    RunFonts font3 = new RunFonts() { Ascii = "Source Sans Pro", HighAnsi = "Source Sans Pro" };
                    FontSize size3 = new FontSize() { Val = "20pt" };
                    prop3.Append(font3);
                    prop3.Append(size3);
                    run3.PrependChild<RunProperties>(prop3);
                    bookmarks["Price"].Parent.InsertAfter<Run>(run3, bookmarks["Price"]);

                    Run run4 = new Run(new Text(TicketHolder.Amount.ToString()));
                    RunProperties prop4 = new RunProperties();
                    RunFonts font4 = new RunFonts() { Ascii = "Source Sans Pro", HighAnsi = "Source Sans Pro" };
                    FontSize size4 = new FontSize() { Val = "20pt" };
                    prop4.Append(font4);
                    prop4.Append(size4);
                    run4.PrependChild<RunProperties>(prop4);
                    bookmarks["Amount"].Parent.InsertAfter<Run>(run4, bookmarks["Amount"]);

                    String code = TicketHolder.TicketHolder.Substring(0, 2);
                    code += TicketHolder.TicketHolderEmail.Substring(0, 1);
                    code += TicketHolder.TicketType.Name.Substring(0, 3);
                    code += TicketHolder.TicketType.Id.Substring(0, 1);

                    Run run = new Run(new Text(code));
                    RunProperties prop = new RunProperties();
                    RunFonts font = new RunFonts() { Ascii = "Free 3 of 9 Extended", HighAnsi = "Free 3 of 9 Extended" };
                    FontSize size = new FontSize() { Val = "36pt" };
                    prop.Append(font);
                    prop.Append(size);
                    run.PrependChild<RunProperties>(prop);
                    bookmarks["Barcode"].Parent.InsertAfter<Run>(run, bookmarks["Barcode"]);


                    newdoc.Close();
                    ModernDialog.ShowMessage("Uw ticket werd opgeslagen om te printen.", "Printen", MessageBoxButton.OK);
                }
            }
            else
            {
                ModernDialog.ShowMessage("Gelieve eerst uw ticket te bestellen.", "Bestellen", MessageBoxButton.OK);
            }
        }

    }
}
