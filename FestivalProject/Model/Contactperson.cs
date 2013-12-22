using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FestivalProject.Model;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data;
using System.Configuration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FestivalProject
{
    public class Contactperson : IDataErrorInfo
    {
        //Properties
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;

        [Required(ErrorMessage = "Geef een naam op.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Meer dan 2 karakters nodig.")]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private ContactpersonType _jobRole;

        [Required(ErrorMessage="Kies een type.")]
        public ContactpersonType JobRole
        {
            get { return _jobRole; }
            set { _jobRole = value; }
        }

        private String _email;

        [Required(ErrorMessage="Emailadres is verplicht.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Geef geldig emailadres op.")]
        public String Email
        {
            get { return _email; }
            set { _email = value; }
        }

        private String _phone;

        [Required(ErrorMessage = "Telefoonnr. is verplicht.")]
        [RegularExpression(@"( |^|>)((((\+|00)3[12] ?(\(0\))?)|0)([0-9]{2}-? ?[0-9]{7})|([0-9]{3}-? ?[0-9]{6})|([0-9]{1}-? ?[0-9]{8}))( |$|<)", ErrorMessage = "Geef geldig telefoonnr. op.")]
        public String Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        //Alle contactpersonen ophalen uit database
        public static ObservableCollection<Contactperson> GetContactpersons() 
        {
            ObservableCollection<Contactperson> persons = new ObservableCollection<Contactperson>();
            ObservableCollection<ContactpersonType> l = ContactpersonType.GetContactpersonTypes();

            DbDataReader reader = Database.GetData("SELECT * FROM Contactperson");

            while (reader.Read()) 
            {
                int idJob = int.Parse(reader["JobRole"].ToString());
                ContactpersonType type = ContactpersonType.GetContactPersonTypeByID(l, idJob);

                Contactperson Cperson = Create(reader, type);
                persons.Add(Cperson);
            }
            reader.Close();
            return persons;
        }

        //Een nieuw contactpersoon maken
        private static Contactperson Create(IDataRecord record, ContactpersonType type)
        {
            return new Contactperson() 
            {
                Id = record["Id"].ToString(),
                Name = record["Name"].ToString(),
                JobRole = type,
                Email = record["Email"].ToString(),
                Phone = record["Phone"].ToString()
            };
        }

        //Een bestaand contact bewerken
        public static int EditContact(Contactperson c)
        {

            String sSQL = "Update Contactperson Set Name=@Name,JobRole=@JobRole,Email=@Email,Phone=@Phone WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("@Name", c.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@JobRole", c.JobRole.Id);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@ID", c.Id);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter par4 = Database.AddParameter("@Email", c.Email);
            if (par4.Value == null) par4.Value = DBNull.Value;

            DbParameter par5 = Database.AddParameter("@Phone", c.Phone);
            if (par5.Value == null) par5.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3, par4, par5 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //Een contactpersoon verwijderen
        public static int DeleteContactperson(Contactperson cp) 
        {
            String sSQL = "DELETE FROM Contactperson WHERE Id=@Id";

            DbParameter par1 = Database.AddParameter("@Id", cp.Id);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1};
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //Een nieuw contactpersoon toevoegen in database
        public static int AddContactperson(Contactperson cp) 
        {
            String sSQL = "INSERT INTO Contactperson(Name, Jobrole, Email, Phone) VALUES(@Name, @JobRole, @Email, @Phone)";

            DbParameter par1 = Database.AddParameter("@Name", cp.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@JobRole", cp.JobRole.Id);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@Email", cp.Email);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter par4 = Database.AddParameter("@Phone", cp.Phone);
            if (par4.Value == null) par4.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3, par4 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //DATAVALIDATIE
        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get 
            {
                try
                {
                    object value = this.GetType().GetProperty(columnName).GetValue(this);
                    Validator.ValidateProperty(value, new ValidationContext(this, null, null)
                    {
                        MemberName = columnName
                    });
                }
                catch (ValidationException ex)
                {
                    return ex.Message;
                }
                return String.Empty;
            }
        }
        public bool IsValid()
        {
            return Validator.TryValidateObject(this, new ValidationContext(this, null, null), null, true);
        }

        //Zoeken in contactpersonenlijst op basis van een string
        public static ObservableCollection<Contactperson> GetContactsByString(ObservableCollection<Contactperson> contacts, String search)
        {
            ObservableCollection<Contactperson> ListFoundContacts = new ObservableCollection<Contactperson>();
            foreach (Contactperson contact in contacts)
            {
                if (contact.Name.ToUpper().Contains(search.ToUpper()) || contact.Email.ToUpper().Contains(search.ToUpper()) || contact.Phone.ToUpper().Contains(search.ToUpper()) || contact.JobRole.Name.ToUpper().Contains(search.ToUpper()))
                {
                    ListFoundContacts.Add(contact);
                }
            }

            return ListFoundContacts;
        }

    }
}
