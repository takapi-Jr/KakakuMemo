using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KakakuMemo.ViewModels
{
    public class LicenseDetailPageViewModel : ViewModelBase
    {
        #region ■画面遷移用キー

        public static readonly string INPUT_KEY_LIB_NAME = "LibName";

        #endregion



        #region ■ライセンス文

        // MITライセンス
        public static readonly string AiFormsEffects_Key = "AiForms.Effects";
        public static readonly string AiFormsSettingsView_Key = "AiForms.SettingsView";
        public static readonly string NETStandardLibrary_Key = "NETStandard.Library";
        public static readonly string NewtonsoftJson_Key = "Newtonsoft.Json";
        public static readonly string PrismUnityForms_Key = "Prism.Unity.Forms";
        public static readonly string ReactiveProperty_Key = "ReactiveProperty";
        public static readonly string XamarinAndroidSupport_Key = "Xamarin.Android.Support";
        public static readonly string XamarinEssentials_Key = "Xamarin.Essentials";
        public static readonly string XamarinForms_Key = "Xamarin.Forms";
        public static readonly string XamarinFormsBehaviorsPack_Key = "Xamarin.Forms.BehaviorsPack";



        /// <summary>
        /// MITライセンス
        /// </summary>
        public string MITLicenseText { get; } = @"
The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


--------------------------------";

        public string AiFormsEffects_LicenseText { get; } = @"
■[AiForms.Effects]
The MIT License (MIT)
Copyright (c) 2016 kamu


--------------------------------";



        public string AiFormsSettingsView_LicenseText { get; } = @"
■[AiForms.SettingsView]
The MIT License (MIT)
Copyright (c) 2017 kamu


--------------------------------";



        public string NETStandardLibrary_LicenseText { get; } = @"
■[NETStandard.Library]
The MIT License (MIT)
Copyright (c) .NET Foundation and Contributors
All rights reserved.


--------------------------------";

        public string NewtonsoftJson_LicenseText { get; } = @"
■[Newtonsoft.Json]
The MIT License (MIT)
Copyright (c) 2007 James Newton-King


--------------------------------";

        public string PrismUnityForms_LicenseText { get; } = @"
■[Prism.Unity.Forms]
The MIT License (MIT)
Copyright (c) .NET Foundation


--------------------------------";

        public string ReactiveProperty_LicenseText { get; } = @"
■[ReactiveProperty]
The MIT License (MIT)
Copyright (c) 2018 neuecc, xin9le, okazuki


--------------------------------";

        public string XamarinAndroidSupport_LicenseText { get; } = @"
■[Xamarin.Android.Support.CustomTabs]
■[Xamarin.Android.Support.Design]
■[Xamarin.Android.Support.v4]
■[Xamarin.Android.Support.v7.AppCompat]
■[Xamarin.Android.Support.v7.CardView]
The MIT License (MIT)
Copyright (c) .NET Foundation Contributors


--------------------------------";

        public string XamarinEssentials_LicenseText { get; } = @"
■[Xamarin.Essentials]
The MIT License (MIT)
Copyright (c) Microsoft Corporation
All rights reserved.


--------------------------------";

        public string XamarinForms_LicenseText { get; } = @"
■[Xamarin.Forms]
The MIT License (MIT)
Copyright (c) .NET Foundation Contributors
All rights reserved.


--------------------------------";

        public string XamarinFormsBehaviorsPack_LicenseText { get; } = @"
■[Xamarin.Forms.BehaviorsPack]
The MIT License (MIT)
Copyright (c) 2017 Atsushi Nakamura


--------------------------------";

        #endregion



        #region ■プロパティ

        /// <summary>
        /// ライブラリコピーライト
        /// </summary>
        public ReactiveProperty<string> LibCopyright { get; } = new ReactiveProperty<string>();

        /// <summary>
        /// ライブラリライセンス
        /// </summary>
        public ReactiveProperty<string> LibLicense { get; } = new ReactiveProperty<string>();

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="navigationService"></param>
        public LicenseDetailPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            //Title = "LicenseDetail Page";
        }

        /// <summary>
        /// 画面表示前呼び出し(このページ"に"画面遷移時に実行)
        /// </summary>
        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            // NavigationParametersに同じキーのパラメーターを持っているかどうかの確認
            // ライブラリ名を確認
            if (parameters.ContainsKey(LicenseDetailPageViewModel.INPUT_KEY_LIB_NAME))
            {
                var libName = (string)parameters[LicenseDetailPageViewModel.INPUT_KEY_LIB_NAME];
                this.Title = libName;

                // コピーライト文、ライセンス文を設定
                LibCopyright.Value = string.Empty;
                LibLicense.Value = string.Empty;
                if (libName.Equals(LicenseDetailPageViewModel.AiFormsEffects_Key))
                {
                    LibCopyright.Value = this.AiFormsEffects_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.AiFormsSettingsView_Key))
                {
                    LibCopyright.Value = this.AiFormsSettingsView_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.NETStandardLibrary_Key))
                {
                    LibCopyright.Value = this.NETStandardLibrary_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.NewtonsoftJson_Key))
                {
                    LibCopyright.Value = this.NewtonsoftJson_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.PrismUnityForms_Key))
                {
                    LibCopyright.Value = this.PrismUnityForms_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.ReactiveProperty_Key))
                {
                    LibCopyright.Value = this.ReactiveProperty_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.XamarinAndroidSupport_Key))
                {
                    LibCopyright.Value = this.XamarinAndroidSupport_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.XamarinEssentials_Key))
                {
                    LibCopyright.Value = this.XamarinEssentials_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.XamarinForms_Key))
                {
                    LibCopyright.Value = this.XamarinForms_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
                else if (libName.Equals(LicenseDetailPageViewModel.XamarinFormsBehaviorsPack_Key))
                {
                    LibCopyright.Value = this.XamarinFormsBehaviorsPack_LicenseText;
                    LibLicense.Value = this.MITLicenseText;
                }
            }
        }
    }
}
