using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using KakakuMemo.Models;

namespace KakakuMemo.ViewModels
{
	public class DetailPageViewModel : ViewModelBase
    {
        #region 定数

        // パラメータ取得用キー
        public static readonly string InputKey_Product = "Product";

        #endregion



        #region プロパティ

        // 選択した製品データ
        private ProductData _selectedProduct;
        public ProductData SelectedProduct
        {
            get { return _selectedProduct; }
            set { SetProperty(ref _selectedProduct, value); }
        }

        #endregion



        #region イベント

        /// <summary>
        /// MainPageへ画面遷移するコマンド
        /// </summary>
        //private DelegateCommand _goBackCommand;
        //public DelegateCommand GoBackCommand
        //{
        //    get
        //    {
        //        if (this._goBackCommand != null)
        //        {
        //            return this._goBackCommand;
        //        }

        //        this._goBackCommand = new DelegateCommand(() =>
        //        {
        //            this.NavigationService.GoBackAsync();
        //        });
        //        return this._goBackCommand;
        //    }
        //}

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "価格メモ Detail Page";
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
            // NavigationParametersに「InputKey_Product」をキーとした
            // パラメーターを持っているかどうかの確認
            if (parameters.ContainsKey(InputKey_Product))
            {
                // プロパティに格納
                SelectedProduct = (ProductData)parameters[InputKey_Product];
            }
        }
    }
}
