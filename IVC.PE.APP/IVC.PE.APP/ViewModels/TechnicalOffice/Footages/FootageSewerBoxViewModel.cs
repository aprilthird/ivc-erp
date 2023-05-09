using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Common.Models;
using IVC.PE.APP.Common.Services;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.Footages
{
    public class FootageSewerBoxViewModel : BaseViewModel
    {
        #region Picker Tipo Buzon
        public SelectItemInt SewerBoxTypeSelected { get; set; }
        private ObservableCollection<SelectItemInt> sewerBoxTypes;
        public ObservableCollection<SelectItemInt> SewerBoxTypes
        {
            get { return this.sewerBoxTypes; }
            set { this.SetValue(ref this.sewerBoxTypes, value); }
        }
        private async void LoadSewerboxTypes()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItemInt>(
                url,
                "/select",
                "/buzones-tipos");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myTypes = (List<SelectItemInt>)response.Result;
            this.SewerBoxTypes = new ObservableCollection<SelectItemInt>(myTypes);
        }
        #endregion

        #region Picker Rangos
        public SelectItemInt SewerBoxRangeSelected { get; set; }
        private ObservableCollection<SelectItemInt> sewerBoxRanges;
        public ObservableCollection<SelectItemInt> SewerBoxRanges
        {
            get { return this.sewerBoxRanges; }
            set { this.SetValue(ref this.sewerBoxRanges, value); }
        }
        private async void LoadSewerboxRanges()
        {
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<SelectItemInt>(
                url,
                "/select",
                "/buzones-rangos");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myRanges = (List<SelectItemInt>)response.Result;
            this.SewerBoxRanges = new ObservableCollection<SelectItemInt>(myRanges);
        }
        #endregion        

        #region ListView Buzon Detalle
        public class FootageSewerBoxItemGroup : ObservableCollection<FootageSewerBoxItemResourceModel>
        {
            public string Name { get; private set; }

            public FootageSewerBoxItemGroup(string name) : base()
            {
                this.Name = name;
            }

            public FootageSewerBoxItemGroup(string name, IEnumerable<FootageSewerBoxItemResourceModel> source) : base(source)
            {
                this.Name = name;
            }
        }
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        private ObservableCollection<FootageSewerBoxItemGroup> footageSewerBoxItem;
        public ObservableCollection<FootageSewerBoxItemGroup> FootageSewerBoxItems
        {
            get { return this.footageSewerBoxItem; }
            set { this.SetValue(ref this.footageSewerBoxItem, value); }
        }
        #endregion

        #region Search Command
        public ICommand SewerBoxSearchCommand => new RelayCommand(SewerBoxSearch);
        private async void SewerBoxSearch()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetAsync<FootageSewerBoxResourceModel>(
                url,
                "/api/oficina-tecnica/metrados/buzones",
                $"/listar?sewerboxtype={SewerBoxTypeSelected.Id.ToString()}&sewerboxrange={SewerBoxRangeSelected.Id.ToString()}",
                "bearer",
                MainViewModel.GetInstance().Token.Access_Token);

            this.IsRefreshing = false;
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Entendido"
                    );
                return;
            }

            var myFootage = (FootageSewerBoxResourceModel)response.Result;
            this.FootageSewerBoxItems = new ObservableCollection<FootageSewerBoxItemGroup>();
            this.FootageSewerBoxItems.Add(new FootageSewerBoxItemGroup("Total",
                myFootage.SewerBoxFootageItems.Where(x => x.Group == 0).OrderBy(x => x.Type).ToList()));
            this.FootageSewerBoxItems.Add(new FootageSewerBoxItemGroup("Techo",
                myFootage.SewerBoxFootageItems.Where(x => x.Group == 1).OrderBy(x => x.Type).ToList()));
            this.FootageSewerBoxItems.Add(new FootageSewerBoxItemGroup("Muro",
                myFootage.SewerBoxFootageItems.Where(x => x.Group == 2).OrderBy(x => x.Type).ToList()));
            this.FootageSewerBoxItems.Add(new FootageSewerBoxItemGroup("Losa",
                myFootage.SewerBoxFootageItems.Where(x => x.Group == 3).OrderBy(x => x.Type).ToList()));
            this.FootageSewerBoxItems.Add(new FootageSewerBoxItemGroup("M.C.+DADO",
                myFootage.SewerBoxFootageItems.Where(x => x.Group == 4).OrderBy(x => x.Type).ToList()));
        }
        #endregion

        private readonly ApiService apiService;

        public FootageSewerBoxViewModel()
        {
            this.apiService = new ApiService();
            LoadSewerboxTypes();
            LoadSewerboxRanges();
            this.IsRefreshing = false;
        }
    }
}
