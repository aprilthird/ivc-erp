using IVC.PE.BINDINGRESOURCES.Areas.Warehouse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace IVC.PE.APP.ViewModels.Warehouse.Techos
{
   public class TechoListDetailViewModel :BaseViewModel
    {
        #region BluePrintList
        private ObservableCollection<TechoResourceModel> bpList;
        public ObservableCollection<TechoResourceModel> BPList
        {
            get { return this.bpList; }
            set { this.SetValue(ref this.bpList, value); }
        }
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        public TechoListDetailViewModel(ObservableCollection<TechoResourceModel> list)
        {

            this.BPList = list;

        }
    }
}
