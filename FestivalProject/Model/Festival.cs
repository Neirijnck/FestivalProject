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
    public class Festival : IDataErrorInfo
    {
        //Properties
        private DateTime _startDate;

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")] 
        [Required]
        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private DateTime _endDate;

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")] 
        [Required]
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public static DateTime? GetStartDate() 
        {
            DbDataReader reader = Database.GetData("SELECT StartDate FROM Festival");
            while (reader.Read()) 
            {
                DateTime start = Convert.ToDateTime(reader["StartDate"]);
                return start;
            }
            reader.Close();
            return null;
        }

        public static DateTime? GetEndDate()
        {
            DbDataReader reader = Database.GetData("SELECT EndDate FROM Festival");
            while (reader.Read())
            {
                DateTime end = Convert.ToDateTime(reader["EndDate"]);
                return end;
            }
            reader.Close();
            return null;
        }

        //Alle bestaande festivals ophalen
        public static ObservableCollection<Festival> GetFestivals() 
        {
            try
            {
                ObservableCollection<Festival> festivals = new ObservableCollection<Festival>();
                DbDataReader reader = Database.GetData("SELECT * FROM Festival");

                while (reader.Read())
                {
                    Festival festival = Create(reader);
                    festivals.Add(festival);
                }
                reader.Close();
                return festivals;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }

        //Een nieuw festival aanmaken
        private static Festival Create(IDataRecord record)
        {
            return new Festival() 
            {
                StartDate = Convert.ToDateTime(record["StartDate"].ToString()),
                EndDate = Convert.ToDateTime(record["EndDate"].ToString())
            };
        }

        //Een bestaand festival aanpassen
        public static int EditFestival(Festival festivaldata) 
        {
            try
            {
                String sSQL = "Update Festival Set StartDate=@StartDate, EndDate=@EndDate ";

                DbParameter par1 = Database.AddParameter("@StartDate", festivaldata.StartDate);
                if (par1.Value == null) par1.Value = DBNull.Value;

                DbParameter par2 = Database.AddParameter("@EndDate", festivaldata.EndDate);
                if (par2.Value == null) par2.Value = DBNull.Value;

                DbParameter[] pars = new DbParameter[] { par1, par2 };
                int affected = Database.ModifyData(sSQL, pars);

                return affected;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return 0; }
        }

        //Een nieuw festival toevoegen
        public static int AddFestival(Festival festivaldata) 
        {
            try
            {
                String sSQL = "INSERT INTO Festival(StartDate, EndDate) VALUES(@StartDate, @EndDate)";

                DbParameter par1 = Database.AddParameter("@StartDate", festivaldata.StartDate);
                if (par1.Value == null) par1.Value = DBNull.Value;

                DbParameter par2 = Database.AddParameter("@EndDate", festivaldata.EndDate);
                if (par2.Value == null) par2.Value = DBNull.Value;

                DbParameter[] pars = new DbParameter[] { par1, par2 };
                int affected = Database.ModifyData(sSQL, pars);

                return affected;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return 0; }
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
