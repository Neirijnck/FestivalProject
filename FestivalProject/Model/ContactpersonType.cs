using FestivalProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject
{
    //Properties
    public class ContactpersonType : IDataErrorInfo
    {
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;

        [Required(ErrorMessage="Geef een naam voor het type op.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "De naam moet tussen 2 en 50 karakters liggen.")]
        public String Name
        {
            get { return _name; }
            set 
            {
                _name = value; 
            }
        }

        //Alle contactpersoontypes ophalen
        public static ObservableCollection<ContactpersonType> GetContactpersonTypes() 
        {
            ObservableCollection<ContactpersonType> cpersontypes = new ObservableCollection<ContactpersonType>();
            DbDataReader reader = Database.GetData("SELECT * FROM ContactpersonType");

            while (reader.Read()) 
            {
                ContactpersonType cpersonType = Create(reader);
                cpersontypes.Add(cpersonType);
            }
            reader.Close();
            return cpersontypes;
        }

        //Een nieuw contactpersoontype creeren
        private static ContactpersonType Create(IDataRecord record)
        {
            return new ContactpersonType() 
            {
                Id = record["Id"].ToString(),
                Name = record["Name"].ToString()
            };
        }

        //Een bestaand contactpersoontype bewerken
        public static int EditContactPersonType(ContactpersonType Type)
        {
            String sSQL = "Update ContactpersonType Set Name=@Name WHERE ID=@Id";

            DbParameter par1 = Database.AddParameter("@Name", Type.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@Id", Type.Id);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //Een contactpersoontype toevoegen
        public static int AddContactPersonType(ContactpersonType type) 
        {
            String sSQL = "INSERT INTO ContactpersonType(Name) VALUES(@Name)";

            DbParameter par1 = Database.AddParameter("@Name", type.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //Contactpersoontype teruggeven adhv zijn id
        public static ContactpersonType GetContactPersonTypeByID(ObservableCollection<ContactpersonType> l, int idJob)
        {
            foreach (ContactpersonType type in l)
            {
                if (type.Id == idJob.ToString())
                {
                    return type;
                }
            }
            return null;
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
    }
}
