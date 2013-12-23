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
    public class Band : IDataErrorInfo
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

        private String _picture;

        public String Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private String _description;

        [Required(ErrorMessage = "Geef een beschrijving op.")]
        [StringLength(50000, MinimumLength = 3, ErrorMessage = "Meer dan 3 karakters nodig.")]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private String _twitter;

        [RegularExpression(@"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$", ErrorMessage = "Geef geldige url op.")]
        public String Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private String _facebook;

        [RegularExpression(@"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$", ErrorMessage = "Geef geldige url op.")]
        public String Facebook
        {
            get { return _facebook; }
            set { _facebook = value; }
        }

private ObservableCollection<Genre> _genres;


	public ObservableCollection<Genre> Genres
	{
		get { return _genres;}
		set { _genres = value;}
	}

    private byte[] _pictureByte;

    public byte[] PictureByte
    {
        get { return _pictureByte; }
        set { _pictureByte = value; }
    }
    
        //Bands ophalen uit database
    public static ObservableCollection<Band> GetBands()
    {
        ObservableCollection<Band> bands = new ObservableCollection<Band>();
        ObservableCollection<Genre> genres = Genre.GetGenres();
        try
        {
            DbDataReader reader = Database.GetData("SELECT * From Band");

            while (reader.Read())
            {
                Band band = Create(reader);
                band.Genres = Band.GetGenresByBand(genres, band.Id);
                bands.Add(band);
            }
            reader.Close();
            return bands;
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
    }

        //Creeer een nieuwe band 
    private static Band Create(IDataRecord record)
    {
        return new Band() 
        {
            Id = record["Id"].ToString(),
            Name = record["Name"].ToString(),
            Picture = record["Picture"].ToString(),
            Description = record["Description"].ToString(),
            Twitter = record["Twitter"].ToString(),
            Facebook = record["Facebook"].ToString()
        };
    }

        //Genres ophalen per band
    public static ObservableCollection<Genre> GetGenresByBand(ObservableCollection<Genre> l, String IdBand) 
    {
        ObservableCollection<Genre> genres = new ObservableCollection<Genre>();

        try
        {
            String sql = "SELECT * FROM Band_Genre WHERE BandId=@BandId";

            DbParameter idPar = Database.AddParameter("@BandId", IdBand);

            DbDataReader reader = Database.GetData(sql, idPar);

            while (reader.Read())
            {
                int GenreId = int.Parse(reader["GenreId"].ToString());
                Genre genre = Genre.GetGenreById(l, GenreId);
                genres.Add(genre);
            }
            reader.Close();
            return genres;
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); return null; }

    }

        //Het id van de band ophalen door zijn naam
    public static Band GetBandIdByName(String BandName)
    {
        ObservableCollection<Band> l = Band.GetBands();
        foreach (Band band in l) 
        {
            if (band.Name == BandName) {
                return band;
            }
        }
        return null;
    }

        //Band teruggeven door zijn id
    public static Band GetBandById(ObservableCollection<Band> l, int IdBand) 
    {
        foreach (Band band in l) 
        {
            if (band.Id == IdBand.ToString()) 
            {
                return band;
            }
        }
        return null;
    }

        //Een nieuwe band toevoegen in de database
    public static int AddBand(Band band)
    {
        try
        {
            String sSQL = "INSERT INTO Band(Name, Picture, [Description], Twitter, Facebook) VALUES(@Name, @Picture, @Description, @Twitter, @Facebook)";

            DbParameter par1 = Database.AddParameter("@Name", band.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@Picture", band.Picture);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@Description", band.Description);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter par4 = Database.AddParameter("@Twitter", band.Twitter);
            if (par4.Value == null) par4.Value = DBNull.Value;

            DbParameter par5 = Database.AddParameter("@Facebook", band.Facebook);
            if (par5.Value == null) par5.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3, par4, par5 };
            int affected = Database.ModifyData(sSQL, pars);

            //OOK NOG DE GENRES TOEVOEGEN!
            //Dit kunnen we doen door een inner join met 2 tabellen
            String subSQL = "INSERT INTO Band_Genre(BandId, GenreId) VALUES(@BandId, @GenreId)";
            foreach (Genre genre in band.Genres)
            {
                Band bandID = Band.GetBandIdByName(band.Name);
                DbParameter par7 = Database.AddParameter("@BandId", bandID.Id);
                if (par7.Value == null) par7.Value = DBNull.Value;

                Genre genreID = Genre.GetGenreIdByName(genre.Name);
                DbParameter par8 = Database.AddParameter("@GenreId", genreID.Id);
                if (par8.Value == null) par8.Value = DBNull.Value;

                DbParameter[] pars2 = new DbParameter[] { par7, par8 };
                Database.ModifyData(subSQL, pars2);
            }

            return affected;
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); return 0; }
    }

        //Een bestaande band bewerken
    public static int EditBand(Band band) 
    {
        try
        {
            String sSQL = "Update Band Set Name=@Name,Picture=@Picture,Description=@Description WHERE ID=@ID";

            DbParameter par1 = Database.AddParameter("@Name", band.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@Description", band.Description);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@ID", band.Id);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter par4 = Database.AddParameter("@Picture", band.Picture);
            if (par4.Value == null) par4.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3, par4 };
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
