using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Newtonsoft.Json;
using KakakuMemo.Models;

namespace KakakuMemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region 定数

        // 製品リストを格納するJSONファイル名
        public static readonly string ProductsFileName = "Products.json";
        // パラメータ取得用キー
        public static readonly string InputKey_AddProduct = "AddProduct";

        #endregion



        #region プロパティ

        // 自作プロパティの命名規則メモ
        // privateのプロパティ名   →   _hogeHoge
        // publicのプロパティ名    →   HogeHoge

        // 製品リスト
        private IList<ProductData> _products;
        public IList<ProductData> Products
        {
            get { return _products; }
            set { SetProperty(ref _products, value); }
        }

        // データディレクトリパス
        public string DataDirPath { get; set; }

        #endregion



        #region コマンド

        /// <summary>
        /// SettingPageへ画面遷移するコマンド
        /// </summary>
        private DelegateCommand _gotoSettingPageCommand;
        public DelegateCommand GotoSettingPageCommand
        {
            get
            {
                if (this._gotoSettingPageCommand != null)
                {
                    return this._gotoSettingPageCommand;
                }

                this._gotoSettingPageCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.NavigateAsync("SettingPage");
                });
                return this._gotoSettingPageCommand;
            }
        }

        /// <summary>
        /// AddProductPageへ画面遷移するコマンド
        /// </summary>
        //public ICommand GotoAddProductPageCommand => new Command<List<ProductData>>(() =>
        //{
        //    var navigationParameters = new NavigationParameters()
        //    {
        //        //{ "キー", 値 },
        //        { AddProductPageViewModel.InputKey_Products, Products },
        //    };

        //    this.NavigationService.NavigateAsync("AddProductPage", navigationParameters);
        //});

        private DelegateCommand _gotoAddProductPageCommand;
        public DelegateCommand GotoAddProductPageCommand
        {
            get
            {
                //if (this._gotoAddProductPageCommand != null)
                //{
                //    return this._gotoAddProductPageCommand;
                //}

                var navigationParameters = new NavigationParameters()
                {
                    //{ "キー", 値 },
                    { AddProductPageViewModel.InputKey_Products, Products },
                };

                this._gotoAddProductPageCommand = new DelegateCommand(() =>
                {
                    this.NavigationService.NavigateAsync("AddProductPage", navigationParameters);
                });
                return this._gotoAddProductPageCommand;
            }
        }

        /// <summary>
        /// 製品リスト選択して、DetailPageへ画面遷移するコマンド
        /// </summary>
        public ICommand SelectedProductItemCommand => new Command<ProductData>(product => 
        {
            var navigationParameters = new NavigationParameters()
            {
                //{ "キー", 値 },
                { DetailPageViewModel.InputKey_Product, product },
            };

            this.NavigationService.NavigateAsync("DetailPage", navigationParameters);
        });

        #endregion



        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "価格メモ Main Page";

            InitProcess();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private void InitProcess()
        {
            // データディレクトリの取得
            DataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal, Environment.SpecialFolderOption.Create);
            var ProductsFilePath = Path.Combine(DataDirPath, ProductsFileName);

            // テスト用データ
            ProductData testData1 = new ProductData
            {
                ProductName = "デジタルカメラ",
                TypeNumber = "Nikon B700",
                Prices = new List<PriceData>()
                {
                    new PriceData() { Price = 40000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "購入済み" },
                    new PriceData() { Price = 50000, Date = new DateTime(2018, 4, 1), StoreName = "ヤマダ電機", OtherMemo = "hogehoge" },
                    new PriceData() { Price = 60000, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                },
            };
            testData1.CheapestData = testData1.Prices.MinBy(x => x.Price).First();

            ProductData testData2 = new ProductData
            {
                ProductName = "スマホ",
                TypeNumber = "ASUS Zenfone4",
                Prices = new List<PriceData>()
                {
                    new PriceData() { Price = 40000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "hogehoge" },
                    new PriceData() { Price = 30000, Date = new DateTime(2018, 4, 1), StoreName = "ネット(IIJmio)", OtherMemo = "Amazonギフト券(5000円分)付き" },
                    new PriceData() { Price = 35000, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35100, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35200, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35300, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35400, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35500, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35600, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35700, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35800, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 35900, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36000, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36100, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36200, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36300, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36400, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36500, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36600, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36700, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36800, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 36900, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 37000, Date = new DateTime(2018, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
                },
            };
            testData2.CheapestData = testData2.Prices.MinBy(x => x.Price).First();

            ProductData testData3 = new ProductData
            {
                ProductName = "ノートパソコン",
                TypeNumber = "Lenovo X1 Carbon",
                Prices = new List<PriceData>()
                {
                    new PriceData() { Price = 200000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "hogehoge" },
                    new PriceData() { Price = 150000, Date = new DateTime(2018, 4, 1), StoreName = "ヤマダ電機", OtherMemo = "fugafuga" },
                    new PriceData() { Price = 100000, Date = new DateTime(2018, 12, 31), StoreName = "ネット(Lenovoサイト)", OtherMemo = "(長文テスト)50%OFFクーポン使用。クーポンの利用は来週金曜日23時59分まで。" },
                },
            };
            testData3.CheapestData = testData3.Prices.MinBy(x => x.Price).First();

            List<ProductData> testProducts = new List<ProductData>();
            testProducts.Add(testData1);
            testProducts.Add(testData2);
            testProducts.Add(testData3);

            Products = testProducts;






            //// 製品リストの取得
            //if (!File.Exists(ProductsFilePath))
            //{
            //    // ファイルが存在しなければ作成
            //    using (var writer = new StreamWriter(ProductsFilePath, true, Encoding.UTF8))
            //    {
            //        // テスト用データ
            //        ProductData testData1 = new ProductData
            //        {
            //            ProductName = "デジタルカメラ",
            //            TypeNumber = "Nikon B700",
            //            Prices = new List<PriceData>()
            //            {
            //                new PriceData() { Price = 40000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "購入済み" },
            //                new PriceData() { Price = 50000, Date = new DateTime(2018, 4, 1), StoreName = "ヤマダ電機", OtherMemo = "hogehoge" },
            //                new PriceData() { Price = 60000, Date = new DateTime(2010, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
            //            },
            //        };
            //        testData1.CheapestData = testData1.Prices.MinBy(x => x.Price).First();

            //        ProductData testData2 = new ProductData
            //        {
            //            ProductName = "スマホ",
            //            TypeNumber = "ASUS Zenfone4 ",
            //            Prices = new List<PriceData>()
            //            {
            //                new PriceData() { Price = 40000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "hogehoge" },
            //                new PriceData() { Price = 30000, Date = new DateTime(2018, 4, 1), StoreName = "ネット(IIJmio)", OtherMemo = "Amazonギフト券(5000円分)付き" },
            //                new PriceData() { Price = 35000, Date = new DateTime(2010, 12, 31), StoreName = "ビックカメラ", OtherMemo = "fugafuga" },
            //            },
            //        };
            //        testData2.CheapestData = testData2.Prices.MinBy(x => x.Price).First();

            //        ProductData testData3 = new ProductData
            //        {
            //            ProductName = "ノートパソコン",
            //            TypeNumber = "Lenovo X1 Carbon",
            //            Prices = new List<PriceData>()
            //            {
            //                new PriceData() { Price = 200000, Date = DateTime.Now, StoreName = "ヨドバシ博多", OtherMemo = "hogehoge" },
            //                new PriceData() { Price = 150000, Date = new DateTime(2018, 4, 1), StoreName = "ヤマダ電機", OtherMemo = "fugafuga" },
            //                new PriceData() { Price = 100000, Date = new DateTime(2018, 12, 31), StoreName = "ネット(Lenovoサイト)", OtherMemo = "(長文テスト)50%OFFクーポン使用。クーポンの利用は来週金曜日23時59分まで。" },
            //            },
            //        };
            //        testData3.CheapestData = testData3.Prices.MinBy(x => x.Price).First();

            //        List<ProductData> testProducts = new List<ProductData>();
            //        testProducts.Add(testData1);
            //        testProducts.Add(testData2);
            //        testProducts.Add(testData3);

            //        // テスト用データ書き込み
            //        var json = JsonConvert.SerializeObject(testProducts);
            //        writer.WriteLine($"{json}");
            //    }
            //}

            //// ファイル読み込み
            //using (var reader = new StreamReader(ProductsFilePath, Encoding.UTF8))
            //{
            //    var json = reader.ReadToEnd();
            //    Products = JsonConvert.DeserializeObject<List<ProductData>>(json);
            //}
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
            // NavigationParametersに「InputKey_AddProduct」をキーとした
            // パラメーターを持っているかどうかの確認
            if (parameters.ContainsKey(InputKey_AddProduct))
            {
                // プロパティに格納
                var tempProduct = (ProductData)parameters[InputKey_AddProduct];
                Products.Add(tempProduct);
                Products = Products.ToList();
            }
        }
    }
}
