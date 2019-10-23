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
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KakakuMemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region ■画面遷移用キー

        #endregion



        #region ■プロパティ

        /// <summary>
        /// 製品リスト
        /// </summary>
        public ReactiveProperty<ObservableCollection<ProductData>> ProductList { get; } = new ReactiveProperty<ObservableCollection<ProductData>>();

        #endregion



        #region ■コマンド

        /// <summary>
        /// 設定画面に移動コマンド
        /// </summary>
        public AsyncReactiveCommand GotoSettingPageCommand { get; } = new AsyncReactiveCommand();

        /// <summary>
        /// 製品情報追加画面に移動コマンド
        /// </summary>
        public AsyncReactiveCommand GotoAddProductPageCommand { get; } = new AsyncReactiveCommand();

        /// <summary>
        /// 製品情報詳細画面に移動コマンド
        /// </summary>
        public AsyncReactiveCommand<ProductData> GotoDetailPageCommand { get; } = new AsyncReactiveCommand<ProductData>();

        /// <summary>
        /// 製品情報を(Android)長押し/(iOS)スワイプで編集コマンド
        /// </summary>
        public AsyncReactiveCommand<ProductData> EditProductDataCommad { get; } = new AsyncReactiveCommand<ProductData>();

        /// <summary>
        /// 製品情報を(Android)長押し/(iOS)スワイプで削除コマンド
        /// </summary>
        public AsyncReactiveCommand<ProductData> DeleteProductDataCommad { get; } = new AsyncReactiveCommand<ProductData>();

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "製品リスト";

            // 製品リスト読み込み
            this.ProductList.Value = Common.ProductList;

            ////////////////////////////////////////////////////////////////////////////////
            // 設定画面に移動コマンド
            GotoSettingPageCommand.Subscribe(async () =>
            {
                try
                {
                    await this.NavigationService.NavigateAsync("SettingPage");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });

            ////////////////////////////////////////////////////////////////////////////////
            // 製品情報追加画面に移動コマンド
            GotoAddProductPageCommand.Subscribe(async () =>
            {
                try
                {
                    await this.NavigationService.NavigateAsync("AddProductPage");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });

            ////////////////////////////////////////////////////////////////////////////////
            // 製品情報詳細画面に移動コマンド
            GotoDetailPageCommand.Subscribe(async selectedProduct =>
            {
                try
                {
                    var navigationParameters = new NavigationParameters()
                    {
                        //{ "キー", 値 },
                        { DetailPageViewModel.INPUT_KEY_SELECTED_PRODUCT, selectedProduct },
                    };
                    await this.NavigationService.NavigateAsync("DetailPage", navigationParameters);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });

            ////////////////////////////////////////////////////////////////////////////////
            // 製品情報を(Android)長押し/(iOS)スワイプで編集コマンド
            EditProductDataCommad.Subscribe(async selectedProduct =>
            {
                try
                {
                    var navigationParameters = new NavigationParameters()
                    {
                        //{ "キー", 値 },
                        { AddProductPageViewModel.INPUT_KEY_EDIT_PRODUCT, selectedProduct },
                    };
                    await this.NavigationService.NavigateAsync("AddProductPage", navigationParameters);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });

            ////////////////////////////////////////////////////////////////////////////////
            // 製品情報を(Android)長押し/(iOS)スワイプで削除コマンド
            DeleteProductDataCommad.Subscribe(async selectedProduct =>
            {
                try
                {
                    // アラート表示
                    var retFlag = await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"下記の製品情報を削除しますか？\n\n{selectedProduct.ProductName}\n{selectedProduct.TypeNumber}", "OK", "キャンセル");
                    if (retFlag)
                    {
                        ProductList.Value.Remove(selectedProduct);
                        Common.ProductList.Remove(selectedProduct);

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
    }
}
