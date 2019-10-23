using KakakuMemo.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KakakuMemo.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        #region ■画面遷移用キー

        public static readonly string INPUT_KEY_SELECTED_PRODUCT = "SelectedProduct";

        #endregion



        #region ■プロパティ

        /// <summary>
        /// 選択した製品データ
        /// </summary>
        public ReactiveProperty<ProductData> SelectedProduct { get; } = new ReactiveProperty<ProductData>();

        #endregion



        #region ■コマンド

        /// <summary>
        /// 価格情報追加画面へ移動コマンド
        /// </summary>
        public AsyncReactiveCommand GotoAddPricePageCommand { get; } = new AsyncReactiveCommand();

        /// <summary>
        /// 価格情報を(Android)長押し/(iOS)スワイプで編集コマンド
        /// </summary>
        public AsyncReactiveCommand<PriceData> EditPriceDataCommad { get; } = new AsyncReactiveCommand<PriceData>();

        /// <summary>
        /// 価格情報を(Android)長押し/(iOS)スワイプで削除コマンド
        /// </summary>
        public AsyncReactiveCommand<PriceData> DeletePriceDataCommad { get; } = new AsyncReactiveCommand<PriceData>();

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public DetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "製品詳細";

            ////////////////////////////////////////////////////////////////////////////////
            // 価格情報追加画面に移動コマンド
            GotoAddPricePageCommand.Subscribe(async () =>
            {
                try
                {
                    var navigationParameters = new NavigationParameters()
                    {
                        //{ "キー", 値 },
                        { AddPricePageViewModel.INPUT_KEY_SELECTED_PRODUCT, this.SelectedProduct.Value },
                    };
                    await this.NavigationService.NavigateAsync("AddPricePage", navigationParameters);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });

            ////////////////////////////////////////////////////////////////////////////////
            // 価格情報を(Android)長押し/(iOS)スワイプで編集コマンド
            EditPriceDataCommad.Subscribe(async selectedPrice =>
            {
                try
                {
                    var navigationParameters = new NavigationParameters()
                    {
                        //{ "キー", 値 },
                        { AddPricePageViewModel.INPUT_KEY_EDIT_PRICE, selectedPrice },
                        { AddPricePageViewModel.INPUT_KEY_SELECTED_PRODUCT, this.SelectedProduct.Value },
                    };
                    await this.NavigationService.NavigateAsync("AddPricePage", navigationParameters);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });

            ////////////////////////////////////////////////////////////////////////////////
            // 価格情報を(Android)長押し/(iOS)スワイプで削除コマンド
            DeletePriceDataCommad.Subscribe(async selectedPrice =>
            {
                try
                {
                    // アラート表示
                    var retFlag = await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"下記の価格情報を削除しますか？\n\n{selectedPrice.Price}\n{selectedPrice.Date.ToString("yyyy/MM/dd")}\n{selectedPrice.StoreName}\n{selectedPrice.OtherMemo}", "OK", "キャンセル");
                    if (retFlag)
                    {
                        Common.ProductList.Remove(SelectedProduct.Value);
                        SelectedProduct.Value.PriceList.Remove(selectedPrice);
                        if (SelectedProduct.Value.PriceList.Count == 0)
                        {
                            // 価格リストが空の場合
                            SelectedProduct.Value.CheapestData = null;
                        }
                        Common.ProductList.Insert(0, SelectedProduct.Value);

                        // 製品リストファイルを上書き保存
                        Common.UpdateProductsFile();
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
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
            if (parameters.ContainsKey(DetailPageViewModel.INPUT_KEY_SELECTED_PRODUCT))
            {
                // プロパティに格納
                this.SelectedProduct.Value = (ProductData)parameters[DetailPageViewModel.INPUT_KEY_SELECTED_PRODUCT];
            }
        }
    }
}
