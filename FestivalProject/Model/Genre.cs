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
    public class Genre : IDataErrorInfo
    {
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;

        [Required(ErrorMessage = "Geef een genre op.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Meer dan 2 karakters nodig.")]
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

        public static Genre GetGenreIdByName(String GenreName) 
        {
            ObservableCollection<Genre> l = Genre.GetGenres();
            foreach (Genre genre in l) 
            {
                if (genre.Name == GenreName)
                {
                    return genre;
                }    
            }
            return null;
        }

        private static Genre Create(IDataRecord record)
        {
            return new Genre() 
            {
                Id = record["Id"].ToString(),
                Name  = record["Name"].ToString()
            };
        }

        public static int AddGenre(Genre genre)
        {
            String sSQL = "INSERT INTO Genre(Name) VALUES(@Name)";

            DbParameter par1 = Database.AddParameter("@Name", genre.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        public static int EditGenre(Genre genre)
        {
            String sSQL = "Update Genre Set Name=@Name WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("@Name", genre.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@ID", genre.Id);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        public static Genre GetGenreById(ObservableCollection<Genre> l, int IdGenre)
        {
            foreach (Genre genre in l)
            {
                if (genre.Id == IdGenre.ToString())
                {
                    return genre;
                }
            }
            return null;
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
