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
    public class Stage
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

        public static ObservableCollection<Stage> GetStages() 
        {
            ObservableCollection<Stage> stages = new ObservableCollection<Stage>();

            DbDataReader reader = Database.GetData("SELECT * FROM Stage");

            while (reader.Read()) 
            {
                Stage stage = Create(reader);
                stages.Add(stage);
            }
            reader.Close();
            return stages;
        }

        private static Stage Create(IDataRecord record)
        {
            return new Stage() 
            {
                Id = record["Id"].ToString(),
                Name = record["Name"].ToString()
            };
        }

        public static Stage GetStageById(ObservableCollection<Stage> l, int IdStage) 
        {
            foreach (Stage stage in l) 
            {
                if (stage.Id == IdStage.ToString()) 
                {
                    return stage;
                }
            }
            return null;
        }

    }
}
