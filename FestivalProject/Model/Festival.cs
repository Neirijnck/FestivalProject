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
    public class Festival
    {
        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }

        private DateTime _endDate;

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public static ObservableCollection<Festival> GetFestivals() 
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

        private static Festival Create(IDataRecord record)
        {
            return new Festival() 
            {
                StartDate = Convert.ToDateTime(record["StartDate"].ToString()),
                EndDate = Convert.ToDateTime(record["EndDate"].ToString())
            };
        }

    }
}
