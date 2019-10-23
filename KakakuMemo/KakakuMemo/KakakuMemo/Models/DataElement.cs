using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace KakakuMemo.Models
{
    public class ProductData
    {
        /// <summary>
        /// 製品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 型番
        /// </summary>
        public string TypeNumber { get; set; }

        /// <summary>
        /// 最安値情報(MainPageで使用)
        /// </summary>
        public PriceData CheapestData { get; set; }

        /// <summary>
        /// 価格詳細リスト(DetailPageで使用)
        /// </summary>
        public ObservableCollection<PriceData> PriceList { get; set; } = new ObservableCollection<PriceData>();

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var rhs = obj as ProductData;
            return (this.ProductName == rhs.ProductName) &&
                (this.TypeNumber == rhs.TypeNumber);
        }

        public override int GetHashCode()
        {
            return this.ProductName.GetHashCode() ^
                this.TypeNumber.GetHashCode();
        }
    }

    public class PriceData
    {
        /// <summary>
        /// 価格
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 日付
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>
        /// 店舗名
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// その他メモ
        /// </summary>
        public string OtherMemo { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var rhs = obj as PriceData;
            return (this.Price == rhs.Price) &&
                (this.Date == rhs.Date) &&
                (this.StoreName == rhs.StoreName) &&
                (this.OtherMemo == rhs.OtherMemo);
        }

        public override int GetHashCode()
        {
            return this.Price.GetHashCode() ^
                this.Date.GetHashCode() ^
                this.StoreName.GetHashCode() ^
                this.OtherMemo.GetHashCode();
        }
    }
}
