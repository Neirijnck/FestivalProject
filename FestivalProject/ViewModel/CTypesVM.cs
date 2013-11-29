using FirstFloor.ModernUI.Windows.Controls;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FestivalProject.ViewModel
{
    class CTypesVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Types"; }
        }

        //Een property aanmaken om alle contactpersontypes in te lezen
        private ObservableCollection<ContactpersonType> _cpersonTypes;

        public ObservableCollection<ContactpersonType> ContactpersonTypes
        {
            get { return _cpersonTypes; }
            set { _cpersonTypes = value; OnPropertyChanged("ContactpersonTypes"); }
        }

        //Een contactpersontype voor het selected item
        private ContactpersonType _selectedContactpersonType;

        public ContactpersonType SelectedContactpersonType
        {
            get { return _selectedContactpersonType; }
            set { _selectedContactpersonType = value; OnPropertyChanged("SelectedContactpersonType"); }
        }

        //Een nieuw contactpersontype property
        private ContactpersonType _contactpersonType;

        public ContactpersonType ContactpersonType
        {
            get { return _contactpersonType; }
            set { _contactpersonType = value; OnPropertyChanged("ContactpersonType"); }
        }
        
        //Constructor, initialiseren van de properties
        public CTypesVM()
        {
            ContactpersonTypes = ContactpersonType.GetContactpersonTypes();
            SelectedContactpersonType = ContactpersonTypes[0];
            ContactpersonType = new ContactpersonType();
        }

        //Command om een nieuw type op te slaan
        public ICommand OpslaanCommand 
        {
            get 
            {
                    return new RelayCommand(AddContactpersonType, ContactpersonType.IsValid);
                
            }
        }

        //Method om het type toe te voegen in de database
        private void AddContactpersonType()
        {
            int affected = ContactpersonType.AddContactPersonType(ContactpersonType);
            if (affected == 1)
            {
                ContactpersonTypes.Add(ContactpersonType);
                //ContactpersonTypes = ContactpersonType.GetContactpersonTypes();
                int LastIndex = ContactpersonTypes.Count - 1;
                SelectedContactpersonType = ContactpersonTypes[LastIndex];
            }
        }

        //Command om een bestaand type te editen
        public ICommand EditCommand 
        {
            get 
            {
                return new RelayCommand(EditContactpersonType, SelectedContactpersonType.IsValid);
            }
        }

        //Method om de veranderingen aan het type naar de database te schrijven
        private void EditContactpersonType()
        {
            //Controle of het wel al bestaat
            if (SelectedContactpersonType.Id != null)
            {
                int affected = ContactpersonType.EditContactPersonType(SelectedContactpersonType);
                int index = ContactpersonTypes.IndexOf(SelectedContactpersonType);
                ContactpersonTypes.Add(ContactpersonType);
                //ContactpersonTypes = ContactpersonType.GetContactpersonTypes();
                SelectedContactpersonType = ContactpersonTypes[index];
                if (affected == 1)
                {
                    Console.WriteLine("Succesvol aangepast in de database!");
                    ModernDialog.ShowMessage("Het type is aangepast in de database.", "Aanpassen", MessageBoxButton.OK);
                }
            }
        }

    }
}
