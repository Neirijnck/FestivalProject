using FestivalProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject
{
    //Properties
    public class Stage : IDataErrorInfo
    {
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;

        [Required(ErrorMessage = "Geef een stage op.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Meer dan 2 karakters nodig.")]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //Alle stages ophalen
        public static ObservableCollection<Stage> GetStages() 
        {
            ObservableCollection<Stage> stages = new ObservableCollection<Stage>();

            DbDataReader reader = Database.GetData("SELECT * FROM Stage");

            while (reader.Read()) 
            {
                Stage stage = Create(reader);
                stages.Add(stage);
            }
            reader.Close();
            return stages;
        }

        //Een nieuwe stage maken
        private static Stage Create(IDataRecord record)
        {
            return new Stage() 
            {
                Id = record["Id"].ToString(),
                Name = record["Name"].ToString()
            };
        }

        //Een stage ophalen met zijn id
        public static Stage GetStageById(ObservableCollection<Stage> l, int IdStage) 
        {
            foreach (Stage stage in l) 
            {
                if (stage.Id == IdStage.ToString()) 
                {
                    return stage;
                }
            }
            return null;
        }

        //Een nieuwe stage toevoegen in database
        public static int AddStage(Stage stage) 
        {
            String sSQL = "INSERT INTO Stage(Name) VALUES(@Name)";

            DbParameter par1 = Database.AddParameter("@Name", stage.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1};
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //Een bestaande stage bewerken
        public static int EditStage(Stage stage)
        {
            String sSQL = "Update Stage Set Name=@Name WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("@Name", stage.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@ID", stage.Id);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2};
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
    }
}
