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
    public class LineUp : IDataErrorInfo
    {
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private String _from;

        public String From
        {
            get { return _from; }
            set { _from = value; }
        }

        private String _until;

        public String Until
        {
            get { return _until; }
            set { _until = value; }
        }

        private Stage _stage;

        public Stage Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        private Band _band;

        [Required(ErrorMessage = "Selecteer een band.")]
        public Band Band
        {
            get { return _band; }
            set { _band = value; }
        }

        public static ObservableCollection<LineUp> GetLineUp() 
        {
            ObservableCollection<LineUp> lineUps = new ObservableCollection<LineUp>();
            ObservableCollection<Band> lB = new ObservableCollection<Band>();
            ObservableCollection<Stage> lS = new ObservableCollection<Stage>();

            DbDataReader reader = Database.GetData("SELECT * FROM LineUp");

            while (reader.Read()) 
            {
                int IdBand = int.Parse(reader["Band"].ToString());
                int IdStage = int.Parse(reader["Stage"].ToString());
                Band band = Band.GetBandById(lB, IdBand);
                Stage stage = Stage.GetStageById(lS, IdStage);


                LineUp lineUp = Create(reader, band, stage);
                lineUps.Add(lineUp);
            }
            reader.Close();
            return lineUps;
        }

        private static LineUp Create(IDataRecord record, Band band, Stage stage)
        {
            return new LineUp() 
            {
                Id = record["Id"].ToString(),
                Date = Convert.ToDateTime(record["Date"].ToString()),
                From = record["From"].ToString(),
                Until = record["Until"].ToString(),
                Band = band,
                Stage = stage
            };
        }

        private static LineUp CreateLineUpFromStage(IDataRecord record, Band band) 
        {
            return new LineUp()
            {
                Id = record["Id"].ToString(),
                Date = Convert.ToDateTime(record["Date"].ToString()),
                From = record["From"].ToString(),
                Until = record["Until"].ToString(),
                Band = band
            };
        }

        public static ObservableCollection<LineUp> GetLineUpByStageAndDay(Stage stage, DateTime? dag)
        {
            ObservableCollection<LineUp> LineUp = new ObservableCollection<LineUp>();
            ObservableCollection<Band> lB = Band.GetBands();

            String sql = "SELECT * FROM LineUp WHERE Stage=@StageId AND Date=@Date";

            DbParameter par1 = Database.AddParameter("@StageId", stage.Id);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@Date", dag);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2 };
            DbDataReader reader = Database.GetData(sql, pars);

            while (reader.Read())
            {
                int IdBand = int.Parse(reader["Band"].ToString());
                Band band = Band.GetBandById(lB, IdBand);
                LineUp lineup = CreateLineUpFromStage(reader, band);
                LineUp.Add(lineup);
            }
            reader.Close();
            return LineUp;

        }

        public static int AddLineUp(LineUp lineUp)
        {
            String sSQL = "INSERT INTO LineUp(Date, [From], Until, Stage, Band) VALUES(@Date, @From, @Until, @Stage, @Band)";

            DbParameter par1 = Database.AddParameter("@Date", lineUp.Date);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@From", lineUp.From);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@Until", lineUp.Until);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter par4 = Database.AddParameter("@Stage", lineUp.Stage.Id);
            if (par4.Value == null) par4.Value = DBNull.Value;

            DbParameter par5 = Database.AddParameter("@Band", lineUp.Band.Id);
            if (par5.Value == null) par5.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3, par4, par5 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        public static int DeleteLineUp(LineUp lineUp)
        {
            String sSQL = "DELETE FROM LineUp WHERE Id=@Id";

            DbParameter par1 = Database.AddParameter("@Id", lineUp.Id);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        public static int EditLineUp(LineUp lineUp)
        {
            String sSQL = "Update LineUp SET Date=@Date, From=@From, Until=@Until, Stage=@Stage, Band=@Band WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("@Date", lineUp.Date);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@From", lineUp.From);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@Until", lineUp.Until);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter par4 = Database.AddParameter("@Stage", lineUp.Stage.Id);
            if (par4.Value == null) par4.Value = DBNull.Value;

            DbParameter par5 = Database.AddParameter("@Band", lineUp.Band.Id);
            if (par5.Value == null) par5.Value = DBNull.Value;

            DbParameter par6 = Database.AddParameter("@ID", lineUp.Id);
            if (par6.Value == null) par6.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3, par4, par5, par6 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

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
