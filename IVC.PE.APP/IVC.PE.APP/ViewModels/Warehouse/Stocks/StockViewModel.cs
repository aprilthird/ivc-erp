namespace IVC.PE.APP.ViewModels.Warehouse.Stocks
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using IVC.PE.APP.Common.Services;
    using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
    using Xamarin.Forms;

    public class StockViewModel : BaseViewModel
    {
        #region ListView Stocks
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        private ObservableCollection<StockResourceModel> stocks;
        public ObservableCollection<StockResourceModel> Stocks
        {
            get { return this.stocks; }
            set { this.SetValue(ref this.stocks, value); }
        }
        private async void LoadStocks()
        {
            this.IsRefreshing = true;
            var url = Application.Current.Resources["UrlAPI"].ToString();
            var response = await this.apiService.GetListAsync<StockResourceModel>(
                url,
                "/api/almacenes/existencias",
                "/listar",
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

            var myStocks = (List<StockResourceModel>)response.Result;
            this.Stocks = new ObservableCollection<StockResourceModel>(myStocks);
        }
        #endregion

        private readonly ApiService apiService;

        public StockViewModel()
        {
            this.apiService = new ApiService();
            LoadStocks();
            this.IsRefreshing = false;
        }
    }
}
