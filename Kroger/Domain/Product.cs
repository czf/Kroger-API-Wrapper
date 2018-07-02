using System;
using System.Collections.Generic;
using System.Text;

namespace Czf.ApiWrapper.Kroger.Domain
{
    public class ProductDetail
    {
        public string BrandName { get; set; }
        public bool ClickListItem { get; set; }
        public string CountryOfOrigin { get; set; }
        public string CustomerFacingSize { get; set; }
        public string Description { get; set; }
        public object ForceSize { get; set; } //unknown type
        public bool HomeDeliveryItem { get; set; }
        public List<ProductImage> Images { get; set; }
        public string MainImagePerspective { get; set; }
        public bool MultipackItem { get; set; }
        public int MultipackQuantity { get; set; }
        public List<object> options { get; set; } // unknown type
        public string RomanceDescription { get; set; }
        public string SeoDescription { get; set; }
        public object ServiceCounter { get; set; } // unknown type
        public bool ShipToHomeItem { get; set; }
        public bool SoldInStore { get; set; }
        public string TemperatureIndicator { get; set; }
        public bool Verified { get; set; }
        public string MainImage { get; set; }
        public string Slug { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public decimal? CalculatedPromoPrice { get; set; } // guessed type
        public decimal? CalculateRegularPrice { get; set; }
        public decimal? CalculatedReferencePrice { get; set; }
        public string DisplayTemplate { get; set; }
        public string Division { get; set; }
        public decimal? MinimumAdvertisedPrice { get; set; }
        public string OrderBy { get; set; }
        public string RegularNFor { get; set; }
        public string ReferenceNFor { get; set; }
        public decimal? ReferencePrice { get; set; }
        public string Store { get; set; }
        public string EndDate { get; set; } //guessed type
        public decimal? PriceNormal { get; set; }
        public decimal? PriceSale { get; set; }
        public string PromoDescription { get; set; }
        public string PromoType { get; set; } // guessed type
        public string UPC { get; set; }
        public string CouponId { get; set; }
        public List<object> Offers { get; set; } //unknown [] type
        public List<string> CouponIds { get; set; } // guessed [] type
        public bool HasPrice { get; set; }
        public bool LoyalMember { get; set; }
        public string PrimaryIndex { get; set; } // unknown type
        public bool CurbsidePickupEligible { get; set; }
    }
}
