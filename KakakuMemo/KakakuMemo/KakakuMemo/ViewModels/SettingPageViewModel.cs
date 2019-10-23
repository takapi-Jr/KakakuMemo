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
    public class SettingPageViewModel : ViewModelBase
    {
        #region ■プロパティ

        /// <summary>
        /// アプリ名
        /// </summary>
        public ReactiveProperty<string> AppName { get; } = new ReactiveProperty<string>();

        /// <summary>
        /// バージョン番号
        /// </summary>
        public ReactiveProperty<string> Version { get; } = new ReactiveProperty<string>();

        #endregion



        #region ■コマンド

        /// <summary>
        /// ライセンス情報画面へ移動コマンド
        /// </summary>
        public AsyncReactiveCommand GotoLicensePageCommand { get; } = new AsyncReactiveCommand();

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public SettingPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "設定/製品情報";

            // バージョン情報設定
            AppName.Value = AppInfo.Name;
            Version.Value = AppInfo.VersionString;

            ////////////////////////////////////////////////////////////////////////////////
            // ライセンス情報画面に移動コマンド
            GotoLicensePageCommand.Subscribe(async () =>
            {
                try
                {
                    await this.NavigationService.NavigateAsync("LicensePage");
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert(AppInfo.Name, $"例外が発生しました。\n{ex}", "OK");
                }
            });
        }
    }
}
