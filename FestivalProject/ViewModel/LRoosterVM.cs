using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject.ViewModel
{
    class LRoosterVM:ObservableObject, IPage
    {
        public string Name
        {
            get { return "Rooster"; }
        }
    }
}
