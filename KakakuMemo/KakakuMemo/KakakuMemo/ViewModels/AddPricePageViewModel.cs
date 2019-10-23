using KakakuMemo.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KakakuMemo.ViewModels
{
    public class AddPricePageViewModel : ViewModelBase
    {
        #region ■画面遷移用キー

        public static readonly string INPUT_KEY_EDIT_PRICE = "EditPrice";
        public static readonly string INPUT_KEY_SELECTED_PRODUCT = "SelectedProduct";

        #endregion



        #region ■プロパティ

        /// <summary>
        /// 選択中製品情報
        /// </summary>
        public ProductData SelectedProduct { get; set; }

        /// <summary>
        /// 価格
        /// </summary>
        [Required(ErrorMessage = "必須項目")]
        [RegularExpression(@"\+?[1-9][0-9]*", ErrorMessage = "正の整数を入力してください")]
        public ReactiveProperty<int> Price { get; }
        public ReadOnlyReactiveProperty<string> PriceError { get; }

        /// <summary>
        /// 日付
        /// </summary>
        [Required(ErrorMessage = "必須項目")]
        public ReactiveProperty<DateTime> Date { get; }
        public ReadOnlyReactiveProperty<string> DateError { get; }

        /// <summary>
        /// 店舗名
        /// </summary>
        [Required(ErrorMessage = "必須項目")]
        public ReactiveProperty<string> StoreName { get; }
        public ReadOnlyReactiveProperty<string> StoreNameError { get; }

        /// <summary>
        /// その他メモ
        /// </summary>
        public ReactiveProperty<string> OtherMemo { get; }
        public ReadOnlyReactiveProperty<string> OtherMemoError { get; }

        /// <summary>
        /// 編集前価格情報
        /// </summary>
        public PriceData BeforeEditPrice { get; set; }

        #endregion



        #region ■コマンド

        /// <summary>
        /// 価格情報追加コマンド
        /// </summary>
        public AsyncReactiveCommand AddPriceCommand { get; } = new AsyncReactiveCommand();

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public AddPricePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "価格追加";

            //////////////////////////////////////////////////////////////////
            // 初期化,バリデーション属性設定
            this.Price = new ReactiveProperty<int>().SetValidateAttribute(() => this.Price);
            this.Date = new ReactiveProperty<DateTime>().SetValidateAttribute(() => this.Date);
            this.StoreName = new ReactiveProperty<string>().SetValidateAttribute(() => this.StoreName);
            this.OtherMemo = new ReactiveProperty<string>().SetValidateAttribute(() => this.OtherMemo);

            //////////////////////////////////////////////////////////////////
            // バリデーションエラー表示設定
            this.PriceError = this.Price.ObserveErrorChanged
                .Select(x => x?.Cast<string>()?.FirstOrDefault())
                .ToReadOnlyReactiveProperty();
            this.DateError = this.Date.ObserveErrorChanged
                .Select(x => x?.Cast<string>()?.FirstOrDefault())
                .ToReadOnlyReactiveProperty();
            this.StoreNameError = this.StoreName.ObserveErrorChanged
                .Select(x => x?.Cast<string>()?.FirstOrDefault())
                .ToReadOnlyReactiveProperty();
            this.OtherMemoError = this.OtherMemo.ObserveErrorChanged
                .Select(x => x?.Cast<string>()?.FirstOrDefault())
                .ToReadOnlyReactiveProperty();

            //////////////////////////////////////////////////////////////////
            // 追加コマンドのアクティブ設定
            this.AddPriceCommand =
                new[]
                {
                    this.Price.ObserveHasErrors,
                    this.Date.ObserveHasErrors,
                    this.StoreName.ObserveHasErrors,
                    this.OtherMemo.ObserveHasErrors,
                }
                .CombineLatestValuesAreAllFalse()       // すべてエラーなしであればアクティブ設定
                .ToAsyncReactiveCommand();

            ////////////////////////////////////////////////////////////////////////////////
            // 価格情報追加コマンド
            AddPriceCommand.Subscribe(async () =>
            {
                try
                {
                    var tempPrice = new PriceData
                    {
                        Price = this.Price.Value,
                        Date = this.Date.Value,
                        StoreName = this.StoreName.Value,
                        OtherMemo = this.OtherMemo.Value,
                    };

                    // 価格リストとのかぶりをチェック
                    //if (!this.SelectedProduct.PriceList.Any(x => x.Price == tempPrice.Price && x.Date == tempPrice.Date && x.StoreName == tempPrice.StoreName && x.OtherMemo == tempPrice.OtherMemo))
                    if (!this.SelectedProduct.PriceList.Any(x => x.Equals(tempPrice)))
                    {
                        Common.ProductList.Remove(this.SelectedProduct);

                        // 編集前価格情報の有無確認
                        if (this.BeforeEditPrice != null)
                        {
                            //if (this.SelectedProduct.PriceList.Any(x => x.Price == this.BeforeEditPrice.Price && x.Date == this.BeforeEditPrice.Date && x.StoreName == this.BeforeEditPrice.StoreName && x.OtherMemo == this.BeforeEditPrice.OtherMemo))
                            if (this.SelectedProduct.PriceList.Any(x => x.Equals(this.BeforeEditPrice)))
                            {
                                // 編集前製品情報を削除
                                this.SelectedProduct.PriceList.Remove(this.BeforeEditPrice);
                            }
                        }

                        // 挿入するインデックスを検索
                        var tempList = this.SelectedProduct.PriceList.ToList();
                        tempList.Add(tempPrice);
                        tempList = tempList.OrderBy(x => x.Price).ToList();
                        var index = tempList.IndexOf(tempPrice);

                        // かぶりがなければリストに挿入して戻る
                        this.SelectedProduct.PriceList.Insert(index, tempPrice);
                        this.SelectedProduct.CheapestData = this.SelectedProduct.PriceList.MinBy(x => x.Price).FirstOrDefault();
                        Common.ProductList.Insert(0, this.SelectedProduct);

                        // 製品リストファイルを上書き保存
                        Common.UpdateProductsFile();

                        var navigationParameters = new NavigationParameters()
                        {
                            //{ "キー", 値 },
                            { DetailPageViewModel.INPUT_KEY_SELECTED_PRODUCT, this.SelectedProduct },
                        };
                        await this.NavigationService.GoBackAsync(navigationParameters);
                    }
                    else
                    {
                        // かぶりがあればアラート表示
                        await Application.Current.MainPage.DisplayAlert(AppInfo.Name, "同じ価格情報が既に存在します。", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, ex.Message, "OK");
                }
            });
        }

        /// <summary>
        /// 画面表示前呼び出し(このページ"に"画面遷移時に実行)
        /// </summary>
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            // NavigationParametersに同じキーのパラメーターを持っているかどうかの確認
            if (parameters.ContainsKey(AddPricePageViewModel.INPUT_KEY_EDIT_PRICE))
            {
                // プロパティに格納
                this.BeforeEditPrice = (PriceData)parameters[AddPricePageViewModel.INPUT_KEY_EDIT_PRICE];

                // 編集前データを画面に反映
                this.Price.Value = this.BeforeEditPrice.Price;
                this.Date.Value = this.BeforeEditPrice.Date;
                this.StoreName.Value = this.BeforeEditPrice.StoreName;
                this.OtherMemo.Value = this.BeforeEditPrice.OtherMemo;
            }
            if (parameters.ContainsKey(AddPricePageViewModel.INPUT_KEY_SELECTED_PRODUCT))
            {
                // プロパティに格納
                this.SelectedProduct = (ProductData)parameters[AddPricePageViewModel.INPUT_KEY_SELECTED_PRODUCT];
            }
        }
    }
}
