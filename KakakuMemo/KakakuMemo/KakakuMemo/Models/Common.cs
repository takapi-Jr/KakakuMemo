using Newtonsoft.Json;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace KakakuMemo.Models
{
    public class Common
    {
        #region ■文字列関連

        // 製品リストを格納するJSONファイル名
        public static readonly string ProductsFileName = "Products.json";

        #endregion



        #region ■プロパティ

        /// <summary>
        /// データディレクトリパス
        /// </summary>
        public static string DataDirPath { get; set; }

        /// <summary>
        /// 製品リストファイルパス
        /// </summary>
        public static string ProductsFilePath { get; set; }

        /// <summary>
        /// 製品リストファイルに書き込む用
        /// </summary>
        public static ObservableCollection<ProductData> ProductList { get; set; }

        #endregion



        #region ■ファイル操作関連

        /// <summary>
        /// 製品リストファイルを上書き保存
        /// </summary>
        public static void UpdateProductsFile()
        {
            using (var writer = new StreamWriter(Common.ProductsFilePath, false, Encoding.UTF8))
            {
                var json = JsonConvert.SerializeObject(Common.ProductList, Formatting.Indented);
                writer.WriteLine($"{json}");
            }

            return;
        }

        #endregion
    }
}
