using Prism;
using Prism.Ioc;
using KakakuMemo.ViewModels;
using KakakuMemo.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using KakakuMemo.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace KakakuMemo
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            // 製品リストの読み込み
            LoadProductsFile();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SettingPage, SettingPageViewModel>();
            containerRegistry.RegisterForNavigation<LicensePage, LicensePageViewModel>();
            containerRegistry.RegisterForNavigation<LicenseDetailPage, LicenseDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<DetailPage, DetailPageViewModel>();
            containerRegistry.RegisterForNavigation<AddProductPage, AddProductPageViewModel>();
            containerRegistry.RegisterForNavigation<AddPricePage, AddPricePageViewModel>();
        }

        /// <summary>
        /// 製品リストの読み込み
        /// </summary>
        private void LoadProductsFile()
        {
            ////////////////////////////////////////////////////////////////////////////////
            // データ格納用フォルダの作成
            Common.DataDirPath = Xamarin.Essentials.FileSystem.AppDataDirectory;
            Common.ProductsFilePath = Path.Combine(Common.DataDirPath, Common.ProductsFileName);

            ////////////////////////////////////////////////////////////////////////////////
            // 製品リストの取得
            if (!File.Exists(Common.ProductsFilePath))
            {
                // ファイルが存在しなければ作成
                using (var writer = new StreamWriter(Common.ProductsFilePath, true, Encoding.UTF8))
                {
#if false

                    // テスト用データ
                    ProductData testData1 = new ProductData
                    {
                        ProductName = "デジタルカメラ",
                        TypeNumber = "Nikon B700",
                        PriceList = new ObservableCollection<PriceData>()
                        {
                            new PriceData() { Price = 40000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "購入済み" },
                            new PriceData() { Price = 50000, Date = new DateTime(2018, 4, 1), StoreName = "ヤマダ電機", OtherMemo = "hogehoge" },
                            new PriceData() { Price = 60000, Date = new DateTime(2010, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                        },
                    };
                    testData1.CheapestData = testData1.PriceList.MinBy(x => x.Price).FirstOrDefault();

                    ProductData testData2 = new ProductData
                    {
                        ProductName = "スマホ",
                        TypeNumber = "ASUS Zenfone4 ",
                        PriceList = new ObservableCollection<PriceData>()
                        {
                            new PriceData() { Price = 40000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "hogehoge" },
                            new PriceData() { Price = 30000, Date = new DateTime(2018, 4, 1), StoreName = "ネット(IIJmio)", OtherMemo = "Amazonギフト券(5000円分)付き" },
                            new PriceData() { Price = 35000, Date = new DateTime(2010, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                        },
                    };
                    testData2.CheapestData = testData2.PriceList.MinBy(x => x.Price).FirstOrDefault();

                    ProductData testData3 = new ProductData
                    {
                        ProductName = "ノートパソコン",
                        TypeNumber = "Lenovo X1 Carbon",
                        PriceList = new ObservableCollection<PriceData>()
                        {
                            new PriceData() { Price = 200000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "hogehoge" },
                            new PriceData() { Price = 150000, Date = new DateTime(2018, 4, 1), StoreName = "ヤマダ電機", OtherMemo = "fugafuga" },
                            new PriceData() { Price = 100000, Date = new DateTime(2018, 12, 31), StoreName = "ネット(Lenovoサイト)", OtherMemo = "(長文テスト)50%OFFクーポン使用。クーポンの利用は来週金曜日23時59分まで。" },
                        },
                    };
                    testData3.CheapestData = testData3.PriceList.MinBy(x => x.Price).FirstOrDefault();

                    ProductData testData4 = new ProductData
                    {
                        ProductName = "あいう",
                        TypeNumber = "かきく",
                        PriceList = new  ObservableCollection<PriceData>()
                        {
                            new PriceData() { Price = 789, Date = DateTime.Now, StoreName = "aaa", OtherMemo = "hogehoge" },
                            new PriceData() { Price = 456, Date = new DateTime(2018, 4, 1), StoreName = "bbb", OtherMemo = "fugafuga" },
                            new PriceData() { Price = 123, Date = new DateTime(2018, 12, 31), StoreName = "ccc", OtherMemo = "fugofugo" },
                        },
                    };
                    testData4.CheapestData = testData4.PriceList.MinBy(x => x.Price).FirstOrDefault();

                    ObservableCollection<ProductData> testProducts = new ObservableCollection<ProductData>();
                    testProducts.Add(testData1);
                    testProducts.Add(testData2);
                    testProducts.Add(testData3);
                    testProducts.Add(testData4);
#endif

                    // テスト用データ書き込み
                    //var json = JsonConvert.SerializeObject(testProducts, Formatting.Indented);
                    var json = JsonConvert.SerializeObject(new ObservableCollection<ProductData>(), Formatting.Indented);
                    writer.WriteLine($"{json}");
                }
            }

            // ファイル読み込み
            using (var reader = new StreamReader(Common.ProductsFilePath, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                Common.ProductList = JsonConvert.DeserializeObject<ObservableCollection<ProductData>>(json);
            }
        }
    }
}
