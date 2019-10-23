using KakakuMemo.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KakakuMemo.ViewModels
{
    public class AddProductPageViewModel : ViewModelBase
    {
        #region ■画面遷移用キー

        public static readonly string INPUT_KEY_EDIT_PRODUCT = "EditProduct";

        #endregion



        #region ■プロパティ

        /// <summary>
        /// 製品名
        /// </summary>
        [Required(ErrorMessage = "必須項目")]
        public ReactiveProperty<string> ProductName { get; }
        public ReadOnlyReactiveProperty<string> ProductNameError { get; }

        /// <summary>
        /// 型番
        /// </summary>
        [Required(ErrorMessage = "必須項目")]
        public ReactiveProperty<string> TypeNumber { get; }
        public ReadOnlyReactiveProperty<string> TypeNumberError { get; }

        /// <summary>
        /// 編集前製品情報
        /// </summary>
        public ProductData BeforeEditProduct { get; set; }

        #endregion



        #region ■コマンド

        /// <summary>
        /// 製品情報追加コマンド
        /// </summary>
        public AsyncReactiveCommand AddProductCommand { get; } = new AsyncReactiveCommand();

        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public AddProductPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "製品追加";

            //////////////////////////////////////////////////////////////////
            // 初期化,バリデーション属性設定
            this.ProductName = new ReactiveProperty<string>().SetValidateAttribute(() => this.ProductName);
            this.TypeNumber = new ReactiveProperty<string>().SetValidateAttribute(() => this.TypeNumber);

            //////////////////////////////////////////////////////////////////
            // バリデーションエラー表示設定
            this.ProductNameError = this.ProductName.ObserveErrorChanged
                .Select(x => x?.Cast<string>()?.FirstOrDefault())
                .ToReadOnlyReactiveProperty();
            this.TypeNumberError = this.TypeNumber.ObserveErrorChanged
                .Select(x => x?.Cast<string>()?.FirstOrDefault())
                .ToReadOnlyReactiveProperty();

            //////////////////////////////////////////////////////////////////
            // 追加コマンドのアクティブ設定
            this.AddProductCommand =
                new[]
                {
                    this.ProductName.ObserveHasErrors,
                    this.TypeNumber.ObserveHasErrors,
                }
                .CombineLatestValuesAreAllFalse()       // すべてエラーなしであればアクティブ設定
                .ToAsyncReactiveCommand();

            ////////////////////////////////////////////////////////////////////////////////
            // 製品情報追加コマンド
            AddProductCommand.Subscribe(async () =>
            {
                try
                {
                    var tempProduct = new ProductData
                    {
                        ProductName = this.ProductName.Value,
                        TypeNumber = this.TypeNumber.Value,
                    };

                    // 製品リストとのかぶりをチェック
                    //if (!Common.ProductList.Any(x => x.ProductName == tempProduct.ProductName && x.TypeNumber == tempProduct.TypeNumber))
                    if (!Common.ProductList.Any(x => x.Equals(tempProduct)))
                    {
                        // 編集前製品情報の有無確認
                        if (this.BeforeEditProduct != null)
                        {
                            //if (Common.ProductList.Any(x => x.ProductName == this.BeforeEditProduct.ProductName && x.TypeNumber == this.BeforeEditProduct.TypeNumber))
                            if (Common.ProductList.Any(x => x.Equals(this.BeforeEditProduct)))
                            {
                                // 編集前製品情報を削除
                                Common.ProductList.Remove(this.BeforeEditProduct);
                                tempProduct.CheapestData = this.BeforeEditProduct.CheapestData;
                                tempProduct.PriceList = this.BeforeEditProduct.PriceList;
                            }
                        }

                        // かぶりがなければリストに追加して戻る
                        Common.ProductList.Insert(0, tempProduct);

                        // 製品リストファイルを上書き保存
                        Common.UpdateProductsFile();

                        await this.NavigationService.GoBackAsync();
                    }
                    else
                    {
                        // かぶりがあればアラート表示
                        await Application.Current.MainPage.DisplayAlert(AppInfo.Name, "同じ製品情報が既に存在します。", "OK");
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
            BeforeEditProduct = null;

            // NavigationParametersに同じキーのパラメーターを持っているかどうかの確認
            if (parameters.ContainsKey(AddProductPageViewModel.INPUT_KEY_EDIT_PRODUCT))
            {
                // 編集前製品情報の設定
                this.BeforeEditProduct = (ProductData)parameters[AddProductPageViewModel.INPUT_KEY_EDIT_PRODUCT];

                // 編集前データを画面に反映
                this.ProductName.Value = this.BeforeEditProduct.ProductName;
                this.TypeNumber.Value = this.BeforeEditProduct.TypeNumber;
            }
        }
    }
}
