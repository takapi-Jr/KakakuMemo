using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using KakakuMemo.Models;

namespace KakakuMemo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
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
        // 製品リストを格納するJSONファイル名
        public static readonly string ProductsFileName = "Products.json";

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
            var FilePath = Path.Combine(DataDirPath, ProductsFileName);

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
            //if (!File.Exists(FilePath))
            //{
            //    // ファイルが存在しなければ作成
            //    using (var writer = new StreamWriter(FilePath, true, Encoding.UTF8))
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

            //        List<ProductData> testProducts = new List<ProductData>();
            //        testProducts.Add(testData1);
            //        testProducts.Add(testData2);

            //        // テスト用データ書き込み
            //        var json = JsonConvert.SerializeObject(testProducts);
            //        writer.WriteLine($"{json}");
            //    }
            //}

            //// ファイル読み込み
            //using (var reader = new StreamReader(FilePath, Encoding.UTF8))
            //{
            //    var json = reader.ReadToEnd();
            //    Products = JsonConvert.DeserializeObject<List<ProductData>>(json);
            //}
        }
    }
}
