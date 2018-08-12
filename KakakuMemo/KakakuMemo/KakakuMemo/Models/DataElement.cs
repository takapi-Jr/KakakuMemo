using System;
using System.Collections.Generic;
using System.Text;

namespace KakakuMemo.Models
{
    public class ProductData
    {
        public string ProductName { get; set; }             // 名前
        public string TypeNumber { get; set; }              // 型番
        public PriceData CheapestData { get; set; }         // 最安値情報(MainPageで使用)
        public List<PriceData> Prices { get; set; }         // 価格詳細リスト(DetailPageで使用)
    }

    public class PriceData
    {
        public int Price { get; set; }                      // 価格
        public DateTime Date { get; set; }                  // 日付
        public string StoreName { get; set; }               // 店舗名
        public string OtherMemo { get; set; }               // その他メモ
    }
}
