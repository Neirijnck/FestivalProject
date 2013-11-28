using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace FestivalProject.ViewModel
{
    class ApplicationVM:ObservableObject
    {

        public ApplicationVM()
        {
            Pages.Add(new CPersonenVM());
            Pages.Add(new CTypesVM());
            Pages.Add(new LInstellingenVM());
            Pages.Add(new LBandsVM());
            Pages.Add(new LRoosterVM());
            Pages.Add(new LOverzichtVM());
            Pages.Add(new TTypesVM());
            Pages.Add(new TBestellenVM());
            CurrentPage = Pages[0];
        }

        private IPage _currentPage;

        public IPage CurrentPage
        {
            get { return _currentPage; }
            set { _currentPage = value; OnPropertyChanged("CurrentPage"); }
        }
        private List<IPage> _pages;

        public List<IPage> Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new List<IPage>();
                return _pages;
            }
        }
        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }
        private void ChangePage(IPage page)
        {
            CurrentPage = page;
        }
    }
}
