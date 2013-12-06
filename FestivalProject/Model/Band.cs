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
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Meer dan 3 karakters nodig.")]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private String _twitter;

        [RegularExpression(@"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/", ErrorMessage = "Geef geldige url op.")]
        public String Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private String _facebook;

        [RegularExpression(@"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/", ErrorMessage = "Geef geldige url op.")]
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
    

    public static ObservableCollection<Band> GetBands() 
    {
        ObservableCollection<Band> bands = new ObservableCollection<Band>();
        DbDataReader reader = Database.GetData("SELECT * From Band");

        while (reader.Read()) 
        {
            Band band = Create(reader);
            bands.Add(band);
        }
        reader.Close();
        return bands;
    }

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

    public static int AddBand(Band band)
    {
        String sSQL = "INSERT INTO Contactperson(Name, Picture, Description, Twitter, Facebook, PictureByte) VALUES(@Name, @Picture, @Description, @Twitter, @Facebook, @PictureByte)";

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

        DbParameter par6 = Database.AddParameter("@PictureByte", band.PictureByte);
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
