using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KakakuMemo.ViewModels
{
	public class AddProductPageViewModel : ViewModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AddProductPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "価格メモ Add Product Page";
        }
	}
}
