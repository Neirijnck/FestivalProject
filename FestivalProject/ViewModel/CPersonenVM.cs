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
    class CPersonenVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Personen"; }
        }     

        //Alle contactpersonen inlezen
        private ObservableCollection<Contactperson> _persons;

        public ObservableCollection<Contactperson> Persons 
        {
            get { return _persons; }
            set { _persons = value; OnPropertyChanged("Persons"); }
        }

        //Alle contactpersoontypes inlezen
        private ObservableCollection<ContactpersonType> _cpersonTypes;

        public ObservableCollection<ContactpersonType> ContactpersonTypes
        {
            get { return _cpersonTypes; }
            set { _cpersonTypes = value; OnPropertyChanged("ContactpersonTypes"); }
        }

        //Een property voor de geselecteerde contactpersoon
        private Contactperson _selectedContactperson;

        public Contactperson SelectedContactperson
        {
            get { return _selectedContactperson; }
            set { _selectedContactperson = value; OnPropertyChanged("SelectedContactperson");}
        }

        //Een property om een nieuwe contactpersoon toe te voegen
        private Contactperson _contactperson;

        public Contactperson Contactperson
        {
            get { return _contactperson; }
            set { _contactperson = value; OnPropertyChanged("Contactperson");}
        }
        
        //Een constructor om de properties te initialiseren
        public CPersonenVM()
        {
            Persons = Contactperson.GetContactpersons();
            ContactpersonTypes = ContactpersonType.GetContactpersonTypes();
            SelectedContactperson = Persons[0];
            Contactperson = new Contactperson();
        }

        //Command om nieuw cpersoon op te slaan
        public ICommand OpslaanCommand 
        {
            get 
            {
                return new RelayCommand(AddContactperson, Contactperson.IsValid);
            }
        }

        //De method om op te slaan
        private void AddContactperson() 
        {
          int affected = Contactperson.AddContactperson(Contactperson);
          if (affected == 1) 
          {
              Persons.Add(Contactperson);
             // Persons = Contactperson.GetContactpersons();
              int LastIndex = Persons.Count - 1;
              SelectedContactperson = Persons[LastIndex];
          }
        }

        //Command om te bewerken
        public ICommand EditCommand 
        {
            get 
            {
                return new RelayCommand<SelectionChangedEventArgs>(EditContactperson);
            }
        }

        //Method om personen te bewerken
        private void EditContactperson(SelectionChangedEventArgs e) 
        {
            if (e.RemovedItems.Count > 0) 
            {
                Contactperson cp = e.RemovedItems[0] as Contactperson;
                int affected = Contactperson.EditContact(cp);
                if (affected == 1)
                {
                    Console.WriteLine("Contact werd succesvol aangepast in de database");
                }
            }
        }

        //Command om een bestaand persoon te verwijderen
        public ICommand DeleteCommand 
        {
            get 
            {
                return new RelayCommand(DeleteContactperson);
            }
        }

        private void DeleteContactperson() 
        {
            //Controle van bestaand contactpersoon
            if (SelectedContactperson.Id != null) 
            {
                if (ModernDialog.ShowMessage(SelectedContactperson.Name + " zal worden verwijderd. Bent u zeker?","Verwijderen",MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    int affected = Contactperson.DeleteContactperson(SelectedContactperson);
                    Persons = Contactperson.GetContactpersons();
                }
                else
                {
                    //doe niets en keer terug
                }
            }
        }

        //Command om te zoeken in de datagrid
        public ICommand SearchCommand 
        {
            get 
            {
                return new RelayCommand<TextBox>(SearchInDatagrid);
            }
        }

        //De method om te zoeken
        private void SearchInDatagrid(TextBox txt) 
        {
            String Search = txt.Text;
            //zoeken in datagrid
            int affected = Contactperson.SearchContacts(Search);
            Console.WriteLine(affected);
        }


    }
}
