using FestivalProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject
{
    public class LineUp
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

        public Band Band
        {
            get { return _band; }
            set { _band = value; }
        }

        public static ObservableCollection<LineUp> GetLineUp() 
        {
            ObservableCollection<LineUp> lineUps = new ObservableCollection<LineUp>();
            DbDataReader reader = Database.GetData("SELECT * FROM LineUp");

            while (reader.Read()) 
            {
                LineUp lineUp = Create(reader);
                lineUps.Add(lineUp);
            }
            reader.Close();
            return lineUps;
        }

        private static LineUp Create(IDataRecord record)
        {
            return new LineUp() 
            {
                Id = record["Id"].ToString(),
                Date = Convert.ToDateTime(record["Date"].ToString()),
                From = record["From"].ToString(),
                Until = record["Until"].ToString(),
                //Band = (Band)record["Band"],
                //Stage = (Stage)record["Stage"]
            };
        }
        
        
        
        
        
        
    }
}
