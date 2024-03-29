﻿using FestivalProject.Model;
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
    public class Ticket: IDataErrorInfo
    {
        private String _id;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _ticketHolder;

        [Required(ErrorMessage = "Geef een naam op.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Meer dan 2 karakters nodig.")]
        public String TicketHolder
        {
            get { return _ticketHolder; }
            set { _ticketHolder = value; }
        }

        private String _ticketHolderEmail;

        [Required(ErrorMessage = "Emailadres is verplicht.")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Geef geldig emailadres op.")]
        public String TicketHolderEmail
        {
            get { return _ticketHolderEmail; }
            set { _ticketHolderEmail = value; }
        }

        private TicketType _ticketType;

        [Required(ErrorMessage = "Kies een type.")]
        public TicketType TicketType
        {
            get { return _ticketType; }
            set { _ticketType = value; }
        }

        private int _amount;

        [Required(ErrorMessage = "Geef een aantal op.")]
        [Range(1, 20000, ErrorMessage = "Minimum 1 ticket")]
        public int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        //Alle ticket(houders) ophalen
        public static ObservableCollection<Ticket> GetTicketHolders() 
        {
            try
            {
                ObservableCollection<Ticket> holders = new ObservableCollection<Ticket>();
                ObservableCollection<TicketType> l = TicketType.GetTicketTypes();

                DbDataReader reader = Database.GetData("SELECT * FROM Ticket");

                while (reader.Read())
                {
                    int idTicketType = int.Parse(reader["TicketType"].ToString());
                    TicketType type = GetTicketTypeByID(l, idTicketType);

                    Ticket holder = Create(reader, type);
                    holders.Add(holder);
                }
                reader.Close();
                return holders;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }

        //De laatste nieuwe toegevoegde ophalen
        public static Ticket GetLastTicketHolder() 
        {
            try
            {
                Ticket LastTicket = new Ticket();
                ObservableCollection<TicketType> l = TicketType.GetTicketTypes();

                DbDataReader reader = Database.GetData("SELECT * FROM Ticket WHERE Id = (SELECT max(Id) FROM Ticket)");
                while (reader.Read())
                {
                    int idTicketType = int.Parse(reader["TicketType"].ToString());
                    TicketType type = GetTicketTypeByID(l, idTicketType);

                    Ticket holder = Create(reader, type);
                    return holder;
                }
                return null;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return null; }
        }

        //Een tickettype tonen adhv zijn id
        private static TicketType GetTicketTypeByID(ObservableCollection<TicketType> l, int idTicketType)
        {
            foreach (TicketType type in l) 
            {
                if (type.Id == idTicketType.ToString()) 
                {
                    return type;
                }
            }
            return null;
        }

        //Een nieuw ticket creeren
        private static Ticket Create(IDataRecord record, TicketType type)
        {
            return new Ticket() {
                Id = record["Id"].ToString(),
                TicketHolder = record["TicketHolder"].ToString(),
                TicketHolderEmail = record["TicketHolderEmail"].ToString(),
                TicketType = type,
                Amount = Int32.Parse(record["Amount"].ToString())
            };
        }

        //Aantal verkochte tickets berekenen en teruggeven
        public static int GetAmountSoldTickets() 
        {
            int aantalVerkochte = 0;
            try
            {
                DbDataReader reader = Database.GetData("SELECT Amount FROM Ticket");

                while (reader.Read())
                {
                    int plus = Int32.Parse(reader["Amount"].ToString());
                    aantalVerkochte += plus;
                }
                reader.Close();
                return aantalVerkochte;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return 0; }
        }

        //Een bestaand ticket bewerken
        public static int EditTicketHolder(Ticket holder)
        {
            try
            {
                String sSQL = "Update Ticket Set TicketHolder=@TicketHolder,TicketHolderEmail=@TicketHolderEmail,Amount=@Amount WHERE ID=@ID";

                DbParameter par1 = Database.AddParameter("@Ticketholder", holder.TicketHolder);
                if (par1.Value == null) par1.Value = DBNull.Value;

                DbParameter par2 = Database.AddParameter("@TicketHolderEmail", holder.TicketHolderEmail);
                if (par2.Value == null) par2.Value = DBNull.Value;

                DbParameter par4 = Database.AddParameter("@Amount", holder.Amount);
                if (par4.Value == null) par4.Value = DBNull.Value;

                DbParameter par5 = Database.AddParameter("@ID", holder.Id);
                if (par5.Value == null) par5.Value = DBNull.Value;

                DbParameter[] pars = new DbParameter[] { par1, par2, par4, par5 };
                int affected = Database.ModifyData(sSQL, pars);

                return affected;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return 0; }
        }

        //Een nieuw ticket toevoegen in database
        public static int AddTicketHolder(Ticket holder)
        {
            try
            {
                String sSQL = "INSERT INTO Ticket(TicketHolder, TicketHolderEmail, TicketType, Amount) VALUES(@TicketHolder, @TicketHolderEmail, @TicketType, @Amount)";

                DbParameter par1 = Database.AddParameter("@TicketHolder", holder.TicketHolder);
                if (par1.Value == null) par1.Value = DBNull.Value;

                DbParameter par2 = Database.AddParameter("@TicketHolderEmail", holder.TicketHolderEmail);
                if (par2.Value == null) par2.Value = DBNull.Value;

                DbParameter par3 = Database.AddParameter("@TicketType", holder.TicketType.Id);
                if (par3.Value == null) par3.Value = DBNull.Value;

                DbParameter par4 = Database.AddParameter("@Amount", holder.Amount);
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
