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
    public class Genre
    {
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public static ObservableCollection<Genre> GetGenres() 
        {
            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();
            DbDataReader reader = Database.GetData("SELECT * FROM Genre");

            while (reader.Read()) 
            {
                Genre genre = Create(reader);
                genres.Add(genre);
            }
            reader.Close();
            return genres;
        }

        private static Genre Create(IDataRecord record)
        {
            return new Genre() 
            {
                Id = record["Id"].ToString(),
                Name  = record["Name"].ToString()
            };
        }

    }
}
