using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KakakuMemo.ViewModels
{
    public class LicensePageViewModel : ViewModelBase
    {
        #region ■プロパティ

        /// <summary>
        /// ライブラリ名一覧
        /// </summary>
        public List<string> LibNameList { get; } = new List<string>
        {
            LicenseDetailPageViewModel.AiFormsEffects_Key,
            LicenseDetailPageViewModel.AiFormsSettingsView_Key,
            LicenseDetailPageViewModel.NETStandardLibrary_Key,
            LicenseDetailPageViewModel.NewtonsoftJson_Key,
            LicenseDetailPageViewModel.PrismUnityForms_Key,
            LicenseDetailPageViewModel.ReactiveProperty_Key,
            LicenseDetailPageViewModel.XamarinAndroidSupport_Key,
            LicenseDetailPageViewModel.XamarinEssentials_Key,
            LicenseDetailPageViewModel.XamarinForms_Key,
            LicenseDetailPageViewModel.XamarinFormsBehaviorsPack_Key,
        };

        #endregion



        #region ■コマンド

        /// <summary>
        /// ライセンス情報詳細画面へ移動コマンド
        /// </summary>
        public AsyncReactiveCommand<string> GotoLicenseDetailPageCommand { get; } = new AsyncReactiveCommand<string>();

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public LicensePageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "ライセンス情報";

            GotoLicenseDetailPageCommand.Subscribe(async libName =>
            {
                try
                {
                    var navigationParameters = new NavigationParameters()
                    {
                        //{ "キー", 値 },
                        { LicenseDetailPageViewModel.INPUT_KEY_LIB_NAME, libName },
                    };
                    await this.NavigationService.NavigateAsync("LicenseDetailPage", navigationParameters);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });
        }
    }
}
