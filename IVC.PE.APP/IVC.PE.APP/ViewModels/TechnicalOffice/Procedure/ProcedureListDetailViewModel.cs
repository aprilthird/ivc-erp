using GalaSoft.MvvmLight.Command;
using IVC.PE.APP.Views.TechnicalOffice.Procedures;
using IVC.PE.BINDINGRESOURCES.Areas.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace IVC.PE.APP.ViewModels.TechnicalOffice.Procedure
{
   public class ProcedureListDetailViewModel : BaseViewModel
    {
        #region PdfProvidersCommand
        public ICommand PdfProceduresCommand => new RelayCommand<ProcedureResourceModel>(this.PdfProcedures);
        private async void PdfProcedures(ProcedureResourceModel obj)
        {
            MainViewModel.GetInstance().ProcedurePdfViewModel = new ProcedurePdfViewModel(obj.FileUrl);
            await App.Navigator.PushAsync(new ProcedurePdfPage());
        }
        #endregion


        #region BimList
        private ObservableCollection<ProcedureResourceModel> bpList;
        public ObservableCollection<ProcedureResourceModel> BPList
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
        public ProcedureListDetailViewModel(ObservableCollection<ProcedureResourceModel> list)
        {

            this.BPList = list;

        }
    }
}
