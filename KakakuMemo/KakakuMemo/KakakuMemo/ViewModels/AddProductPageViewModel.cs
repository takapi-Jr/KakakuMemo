using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace KakakuMemo.ViewModels
{
	public class AddProductPageViewModel : ViewModelBase
    {
        #region プロパティ

        // 製品名
        private string _productNameEntry;
        public string ProductNameEntry
        {
            get { return _productNameEntry; }
            set { SetProperty(ref _productNameEntry, value); }
        }

        // 型番
        private string _typeNumberEntry;
        public string TypeNumberEntry
        {
            get { return _typeNumberEntry; }
            set { SetProperty(ref _typeNumberEntry, value); }
        }

        // 価格
        private string _priceEntry;
        public string PriceEntry
        {
            get { return _priceEntry; }
            set { SetProperty(ref _priceEntry, value); }
        }

        // 日付
        private string _dateEntry;
        public string DateEntry
        {
            get { return _dateEntry; }
            set { SetProperty(ref _dateEntry, value); }
        }

        // 店舗名
        private string _storeNameEntry;
        public string StoreNameEntry
        {
            get { return _storeNameEntry; }
            set { SetProperty(ref _storeNameEntry, value); }
        }

        // その他メモ
        private string _otherMemoEntry;
        public string OtherMemoEntry
        {
            get { return _otherMemoEntry; }
            set { SetProperty(ref _otherMemoEntry, value); }
        }

        #endregion



        #region コマンド

        public ICommand AddProductCommand => new Command(() =>
        {
            //NotifyCompletedRequest.Request();
        });


        public ICommand ClearEntryCommand => new Command(() =>
        {
            //NotifyCompletedRequest.Request();
        });

        //public InteractionRequest NotifyCompletedRequest { get; } = new InteractionRequest();

        
        
        //public NotificationRequest DisplayRequest { get; } = new NotificationRequest();

        //private void Foo()
        //{
        //    DisplayRequest.Raise(
        //        "登録確認",
        //        "入力情報を登録してもよろしいですか？",
        //        new AlertButton
        //        {
        //            Message = "OK",
        //            Action = () =>
        //            {
        //                // OKクリック時の処理
        //            }
        //        },
        //        new AlertButton
        //        {
        //            Message = "Cancel",
        //        }
        //    );
        //}

        #endregion
        
        
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddProductPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "価格メモ Add Product Page";

            // プロパティの値をリセット
            this.ProductNameEntry = string.Empty;
            this.TypeNumberEntry = string.Empty;
            this.PriceEntry = string.Empty;
            this.DateEntry = string.Empty;
            this.StoreNameEntry = string.Empty;
            this.OtherMemoEntry = string.Empty;
        }
	}
}
