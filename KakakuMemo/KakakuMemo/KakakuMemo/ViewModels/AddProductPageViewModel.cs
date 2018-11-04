﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using KakakuMemo.Models;
using System.Text.RegularExpressions;

namespace KakakuMemo.ViewModels
{
	public class AddProductPageViewModel : ViewModelBase
    {
        #region 定数

        // パラメータ取得用キー
        public static readonly string InputKey_Products = "Products";

        #endregion



        #region 正規表現パターン

        // 正の整数
        //private static readonly Regex RegIntPattern = new Regex(@"^[0-9]{0,9}$");

        // 正の整数+小数点第4位まで
        //private static readonly Regex RegDoublePattern = new Regex(@"^[0-9]+(\.[0-9]{1,4})?$");

        #endregion



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
            //set
            //{
            //    //@@@@@@@@@@@@@@@@@
            //    // 正規表現に適合していれば設定
            //    if (string.Equals(value, string.Empty) || RegIntPattern.IsMatch(value))
            //    {
            //        SetProperty(ref _priceEntry, value);
            //    }
            //}
        }

        // 日付
        private DateTime _dateEntry;
        public DateTime DateEntry
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

        // 参照専用製品リスト
        private List<ProductData> _products;
        public List<ProductData> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        #endregion



        #region コマンド

        public ICommand AddProductCommand => new Command<ProductData>(product =>
        {
            try
            {
                //@@@@@@@@@@@@
                // 入力項目のエラー判定
                

                // 追加予定変数に設定
                var tempProduct = new ProductData()
                {
                    ProductName = ProductNameEntry,
                    TypeNumber = TypeNumberEntry,
                    Prices = new List<PriceData>(),
                };
                var tempPrice = new PriceData()
                {
                    Price = int.MaxValue,       // 価格が正しく取得できなければ整数の最大値
                    Date = DateEntry,
                    StoreName = StoreNameEntry,
                    OtherMemo = OtherMemoEntry,
                };

                // 価格が取得できる形であれば設定
                var temp = 0;
                if (int.TryParse(PriceEntry, out temp))
                {
                    tempPrice.Price = temp;
                }
                tempProduct.Prices.Add(tempPrice);

                // 最安値情報を作成
                tempProduct.CheapestData = tempPrice;

                // MainPageの製品リストに追加
                // 同じ製品名、同じ型番のものがあれば追加しない
                if (!Products.Any(item => (item.ProductName == tempProduct.ProductName) && (item.TypeNumber == tempProduct.TypeNumber)))
                {
                    // かぶりなしの場合は追加してMainPageへ遷移
                    var navigationParameters = new NavigationParameters()
                    {
                        //{ "キー", 値 },
                        { MainPageViewModel.InputKey_AddProduct, tempProduct },
                    };

                    // ルートに戻る
                    this.NavigationService.GoBackToRootAsync(navigationParameters);
                }

            }
            catch (Exception ex)
            {
                //DisplayAlert("タイトル", "メッセージ", "OK");
            }

            // プロパティの値をリセット
            ClearEntry();

            //NotifyCompletedRequest.Request();
        });


        public ICommand ClearEntryCommand => new Command(() =>
        {
            // プロパティの値をリセット
            ClearEntry();
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

            // プロパティの初期値設定
            ClearEntry();
        }

        /// <summary>
        /// 入力エントリーの内容をクリア
        /// </summary>
        private void ClearEntry()
        {
            this.ProductNameEntry = string.Empty;
            this.TypeNumberEntry = string.Empty;
            this.PriceEntry = string.Empty;
            this.DateEntry = DateTime.Now;
            this.StoreNameEntry = string.Empty;
            this.OtherMemoEntry = string.Empty;

            return;
        }

        /// <summary>
        /// OnNavigatingTo後呼び出し(このページ"から"画面遷移時に実行)
        /// </summary>
        public override void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        /// <summary>
        /// 画面表示後呼び出し(このページ"に"画面遷移後に実行)
        /// </summary>
        public override void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        /// <summary>
        /// 画面表示前呼び出し(このページ"に"画面遷移時に実行)
        /// </summary>
        public override void OnNavigatingTo(NavigationParameters parameters)
        {
            // NavigationParametersに「InputKey_Products」をキーとした
            // パラメーターを持っているかどうかの確認
            if (parameters.ContainsKey(InputKey_Products))
            {
                // プロパティに格納
                Products = (List<ProductData>)parameters[InputKey_Products];
            }
        }
    }
}