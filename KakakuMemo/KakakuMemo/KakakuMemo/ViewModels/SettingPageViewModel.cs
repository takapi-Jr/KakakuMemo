using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KakakuMemo.ViewModels
{
	public class SettingPageViewModel : ViewModelBase
    {
        #region コマンド

        /// <summary>
        /// LicensePageへ画面遷移するコマンド
        /// </summary>
        private DelegateCommand _gotoLicensePageCommand;
        public DelegateCommand GotoLicensePageCommand
        {
            get
            {
                if (this._gotoLicensePageCommand != null)
                {
                    return this._gotoLicensePageCommand;
                }

                this._gotoLicensePageCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.NavigateAsync("LicensePage");
                });
                return this._gotoLicensePageCommand;
            }
        }

        #endregion


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "価格メモ Setting Page";
        }
    }
}
