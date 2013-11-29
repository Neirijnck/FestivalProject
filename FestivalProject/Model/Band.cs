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
    public class Band
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

        private String _picture;

        public String Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private String _description;

        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private String _twitter;

        public String Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private String _facebook;

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


        }
    }
