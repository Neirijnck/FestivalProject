using FestivalProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject
{
    //Properties
    public class TicketType : IDataErrorInfo
    {
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _name;

        [Required(ErrorMessage = "Geef een naam voor het type op.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Meer dan 2 karakters nodig.")]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private Double _price;

        [Required(ErrorMessage="Geef een prijs op.")]
        public Double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private int _availableTickets;

        [Required(ErrorMessage="Geef een aantal op.")]
        [Range(10, 20000, ErrorMessage="Minimum 10 tickets")]
        public int AvailableTickets
        {
            get { return _availableTickets; }
            set { _availableTickets = value; }
        }

        //Alle verschillende tickettypes ophalen
        public static ObservableCollection<TicketType> GetTicketTypes() 
        {
            ObservableCollection<TicketType> ticketTypes = new ObservableCollection<TicketType>();
            DbDataReader reader = Database.GetData("SELECT * FROM TicketType");

            while (reader.Read()) 
            {
                TicketType ticketType = Create(reader);
                ticketTypes.Add(ticketType);
            }
            reader.Close();
            return ticketTypes;
        }

        //Een nieuw tickettype creeren
        private static TicketType Create(IDataRecord record)
        {
            return new TicketType() 
            {
                Id = record["Id"].ToString(),
                Name = record["Name"].ToString(),
                Price = Double.Parse(record["Price"].ToString()),
                AvailableTickets = Int32.Parse(record["AvailableTickets"].ToString())
            };
        }

        //Een nieuw tickettype toevoegen
        public static int AddTicketType(TicketType Type) 
        {
            String sSQL = "INSERT INTO TicketType(Name, Price, AvailableTickets) VALUES(@Name,@Price,@AvailableTickets)";

            DbParameter par1 = Database.AddParameter("@Name", Type.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@Price", Type.Price);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@AvailableTickets", Type.AvailableTickets);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3};
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //Een bestaand tickettype bewerken
        public static int EditTicketType(TicketType tType) 
        {
            String sSQL = "Update TicketType Set Name=@Name, Price=@Price, AvailableTickets=@AvailableTickets WHERE ID=@Id";

            DbParameter par1 = Database.AddParameter("@Name", tType.Name);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@Id", tType.Id);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter par3 = Database.AddParameter("@Price", tType.Price);
            if (par3.Value == null) par3.Value = DBNull.Value;

            DbParameter par4 = Database.AddParameter("@AvailableTickets", tType.AvailableTickets);
            if (par4.Value == null) par4.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2, par3, par4 };
            int affected = Database.ModifyData(sSQL, pars);

            return affected;
        }

        //Het aantal beschikbare tickets van een bepaald tickettype berekenen en teruggeven
        public static int ChangeAvailableTickets(TicketType ticketType, int VerkochteTickets) 
        {
            String sSQL = "Update TicketType Set AvailableTickets=@AvailableTickets WHERE ID=@ID";

            int resterendeTickets = ticketType.AvailableTickets - VerkochteTickets;

            DbParameter par1 = Database.AddParameter("@AvailableTickets", resterendeTickets);
            if (par1.Value == null) par1.Value = DBNull.Value;

            DbParameter par2 = Database.AddParameter("@ID", ticketType.Id);
            if (par2.Value == null) par2.Value = DBNull.Value;

            DbParameter[] pars = new DbParameter[] { par1, par2 };
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
