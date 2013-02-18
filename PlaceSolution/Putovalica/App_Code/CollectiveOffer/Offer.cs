using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace Collective
{
    public class Offer
    {
        public int OfferId { get; private set; }
        public int? AdvertId { get; private set; }
        public int? AgencyId { get; private set; }
        public int CollectiveCategoryId { get; private set; }
        public string OfferName { get; private set; }
        public double PriceReal { get; private set; }
        public double PriceSave { get; private set; }
        public double Price { get; private set; }
        public int CurrencyId { get; private set; }
        public OfferStatusType OfferStatus { get; private set; }
        public string CategoryName { get; private set; }
        public double? Discount { get; private set; }
        public DateTime DateStart { get; private set; }
        public DateTime DateEnd { get; private set; }
        public DateTime DateCouponStart { get; private set; }
        public DateTime DateCouponEnd { get; private set; }
        public int? NumberOfPersons { get; private set; }
        public int? NumberOfCouponsPerUser { get; private set; }
        public int MinBoughtCount { get; private set; }
        public int MaxBoughtCount { get; private set; }
        public int BoughtCount { get; private set; }
        public int ClickCount { get; private set; }
        public string OfferType { get; private set; }
        public double? Longitude { get; private set; }
        public double? Latitude { get; private set; }
        public int Priority { get; private set; }
        public bool Active { get; private set; }


        // propertyji koji trebaju samo za klijenta i ne popunjavaju se u metodama koje se koriste za admina

        public bool IsClientDataSet { get; private set; }

        public string FirstImgSrc { get; private set; }
        public OfferTranslation Translation { get; private set; }
        public string CurrencySymbol { get; private set; }


        public Offer() { }  // dummy za prazan grid

        private Offer(
                int offerId,
                int? advertId,
                int? agencyId,
                int collectiveCategoryId,
                string name,
                double priceReal,
                double priceSave,
                double price,
                int currencyId,
                OfferStatusType status,
                string categoryName,
                double? discount,
                DateTime dateStart,
                DateTime dateEnd,
                DateTime dateCouponStart,
                DateTime dateCouponEnd,
                int? numberOfPersons,
                int? numberOfCouponsPerUser,
                int minBoughtCount,
                int maxBoughtCount,
                int boughtCount,
                int clickCount,
                string offerType,
                double? longitude,
                double? latitude,
                int priority,
                bool active
                )
        {
            this.OfferId = offerId;
            this.AdvertId = advertId;
            this.AgencyId = agencyId;
            this.CollectiveCategoryId = collectiveCategoryId;
            this.OfferName = name;
            this.PriceReal = priceReal;
            this.PriceSave = priceSave;
            this.Price = price;
            this.CurrencyId = currencyId;
            this.OfferStatus = status;
            this.CategoryName = categoryName;
            this.Discount = discount;
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
            this.DateCouponStart = dateCouponStart;
            this.DateCouponEnd = dateCouponEnd;
            this.NumberOfPersons = numberOfPersons;
            this.NumberOfCouponsPerUser = numberOfCouponsPerUser;
            this.MinBoughtCount = minBoughtCount;
            this.MaxBoughtCount = maxBoughtCount;
            this.BoughtCount = boughtCount;
            this.ClickCount = clickCount;
            this.OfferType = offerType;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Priority = priority;
            this.Active = active;            
        }


        public void SetClientData(int langId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_SetOfferClientData"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@OfferId", SqlDbType.Int, 4) { Value = this.OfferId });
                    cmd.Parameters.Add(new SqlParameter("@Langid", SqlDbType.Int, 4) { Value = langId });

                    SqlParameter parFirstImgSrc = new SqlParameter("@FirstImgSrc", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parOfferTitle = new SqlParameter("@OfferTitle", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parContentShort = new SqlParameter("@ContentShort", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parContentText = new SqlParameter("@ContentText", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    SqlParameter parReservationText = new SqlParameter("@ReservationText", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    SqlParameter parMetaDesc = new SqlParameter("@MetaDesc", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parMetaKW = new SqlParameter("@MetaKeywords", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parUrlTag = new SqlParameter("@UrlTag", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parCurrencySymbol = new SqlParameter("@CurrencySymbol", SqlDbType.NVarChar, 5) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(parFirstImgSrc);
                    cmd.Parameters.Add(parOfferTitle);
                    cmd.Parameters.Add(parContentShort);
                    cmd.Parameters.Add(parContentText);
                    cmd.Parameters.Add(parReservationText);
                    cmd.Parameters.Add(parMetaDesc);
                    cmd.Parameters.Add(parMetaKW);
                    cmd.Parameters.Add(parUrlTag);
                    cmd.Parameters.Add(parCurrencySymbol);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    this.FirstImgSrc = parFirstImgSrc.Value != DBNull.Value ? parFirstImgSrc.Value.ToString() : null;
                    this.Translation = new OfferTranslation(this.OfferId, langId,
                        parOfferTitle.Value != DBNull.Value ? parOfferTitle.Value.ToString() : null,
                        parContentShort.Value != DBNull.Value ? parContentShort.Value.ToString() : null,
                        parContentText.Value != DBNull.Value ? parContentText.Value.ToString() : null,
                        parReservationText.Value != DBNull.Value ? parReservationText.Value.ToString() : null,
                        parMetaDesc.Value != DBNull.Value ? parMetaDesc.Value.ToString() : null,
                        parMetaKW.Value != DBNull.Value ? parMetaKW.Value.ToString() : null,
                        parUrlTag.Value.ToString());
                    this.CurrencySymbol = parCurrencySymbol.Value != DBNull.Value ? parCurrencySymbol.Value.ToString() : null;

                    this.IsClientDataSet = true;
                }
            }
        }


        public static Offer CreateOffer(
                            int? advertId,
                            int? agencyId,
                            int collectiveCategoryId,
                            string name,
                            double priceReal,
                            double priceSave,
                            double price,
                            int currencyId,
                            OfferStatusType status,
                            double? discount,
                            DateTime dateStart,
                            DateTime dateEnd,
                            DateTime dateCouponStart,
                            DateTime dateCouponEnd,
                            int? numberOfPersons,
                            int? numberOfCouponsPerUser,
                            int minBoughtCount,
                            int maxBoughtCount,
                            string offerType,
                            double? longitude,
                            double? latitude,
                            int priority,
                            bool active,
                            List<int> lstPlacesIds
                            )
        {
            // da budem siguran da je vrijeme 0:00:00
            dateStart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day);
            dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day);
            dateCouponStart = new DateTime(dateCouponStart.Year, dateCouponStart.Month, dateCouponStart.Day);
            dateCouponEnd = new DateTime(dateCouponEnd.Year, dateCouponEnd.Month, dateCouponEnd.Day);

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreateOffer"
                })
                {
                    SqlParameter parRetId = new SqlParameter("@Id", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parRetId);
                    if (advertId.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@AdvertId", SqlDbType.Int, 4) { Value = advertId.Value });
                    if (agencyId.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@AgencyId", SqlDbType.Int, 4) { Value = agencyId.Value });
                    cmd.Parameters.Add(new SqlParameter("@CollectiveCategoryId", SqlDbType.Int, 4) { Value = collectiveCategoryId });
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 250) { Value = name });
                    cmd.Parameters.Add(new SqlParameter("@PriceReal", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Value = priceReal });
                    cmd.Parameters.Add(new SqlParameter("@PriceSave", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Value = priceSave });
                    cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Value = price });
                    cmd.Parameters.Add(new SqlParameter("@CurrencyId", SqlDbType.Int, 4) { Value = currencyId });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char, 1) { Value = (char)status });
                    if (discount.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Value = discount.Value });
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.SmallDateTime, 4) { Value = dateStart });
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.SmallDateTime, 4) { Value = dateEnd });
                    cmd.Parameters.Add(new SqlParameter("@DateCouponStart", SqlDbType.SmallDateTime, 4) { Value = dateCouponStart });
                    cmd.Parameters.Add(new SqlParameter("@DateCouponEnd", SqlDbType.SmallDateTime, 4) { Value = dateCouponEnd });
                    if (numberOfPersons.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@NumberOfPersons", SqlDbType.Int, 4) { Value = numberOfPersons.Value });
                    if (numberOfCouponsPerUser.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@NumberOfCouponsPerUser", SqlDbType.Int, 4) { Value = numberOfCouponsPerUser.Value });
                    cmd.Parameters.Add(new SqlParameter("@MinBoughtCount", SqlDbType.Int, 4) { Value = minBoughtCount });
                    cmd.Parameters.Add(new SqlParameter("@MaxBoughtCount", SqlDbType.Int, 4) { Value = maxBoughtCount });
                    SqlParameter parBoughtCount = new SqlParameter("@BoughtCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parBoughtCount);
                    SqlParameter parClickCount = new SqlParameter("@ClickCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parClickCount);
                    if (!String.IsNullOrEmpty(offerType))
                        cmd.Parameters.Add(new SqlParameter("@OfferType", SqlDbType.NVarChar, 250) { Value = offerType });
                    if (longitude.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@Longitude", SqlDbType.Decimal, 8) { Precision = 10, Scale = 6, Value = longitude.Value });
                    if (latitude.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@Latitude", SqlDbType.Decimal, 8) { Precision = 10, Scale = 6, Value = latitude.Value });
                    cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int, 4) { Value = priority });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });
                    SqlParameter parCategoryName = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCategoryName);

                    XDocument xdocPlaces = new XDocument(new XElement("root"));

                    foreach (int plId in lstPlacesIds)
                    {
                        xdocPlaces.Root.Add(new XElement("place",
                            new XAttribute("id", plId.ToString())));
                    }

                    cmd.Parameters.Add(new SqlParameter("@Places", SqlDbType.Xml) { Value = xdocPlaces.ToString() });
                    con.Open();

                    cmd.ExecuteNonQuery();
                    con.Close();

                    return new Offer(
                            Convert.ToInt32(parRetId.Value),
                            advertId,
                            agencyId,
                            collectiveCategoryId,
                            name,
                            priceReal,
                            priceSave,
                            price,
                            currencyId,
                            status,
                            parCategoryName.Value.ToString(),
                            discount,
                            dateStart,
                            dateEnd,
                            dateCouponStart,
                            dateCouponEnd,
                            numberOfPersons,
                            numberOfCouponsPerUser,
                            minBoughtCount,
                            maxBoughtCount,
                            Convert.ToInt32(parBoughtCount.Value),
                            Convert.ToInt32(parClickCount.Value),
                            offerType,
                            longitude,
                            latitude,
                            priority,
                            active
                        ) { IsClientDataSet = false };
                }
            }
        }


        public int GetUserBoughtCount(int userId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetUserBoughtCount"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int, 4) { Value = userId });
                    cmd.Parameters.Add(new SqlParameter("@OfferId", SqlDbType.Int, 4) { Value = this.OfferId });

                    SqlParameter parRetValue = new SqlParameter("@ReturnValue", SqlDbType.Int, 4) { Direction = ParameterDirection.ReturnValue };
                    cmd.Parameters.Add(parRetValue);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return Convert.ToInt32(parRetValue.Value);
                }
            }
        }
        
        public static Offer UpdateOffer(
                            int id,
                            int? advertId,
                            int? agencyId,
                            int collectiveCategoryId,
                            string name,
                            double priceReal,
                            double priceSave,
                            double price,
                            int currencyId,
                            OfferStatusType status,
                            double? discount,
                            DateTime dateStart,
                            DateTime dateEnd,
                            DateTime dateCouponStart,
                            DateTime dateCouponEnd,
                            int? numberOfPersons,
                            int? numberOfCouponsPerUser,
                            int minBoughtCount,
                            int maxBoughtCount,
                            string offerType,
                            double? longitude,
                            double? latitude,
                            int priority,
                            bool active,
                            List<int> lstPlacesIds
                            )
        {
            // da budem siguran da je vrijeme 0:00:00
            dateStart = new DateTime(dateStart.Year, dateStart.Month, dateStart.Day);
            dateEnd = new DateTime(dateEnd.Year, dateEnd.Month, dateEnd.Day);
            dateCouponStart = new DateTime(dateCouponStart.Year, dateCouponStart.Month, dateCouponStart.Day);
            dateCouponEnd = new DateTime(dateCouponEnd.Year, dateCouponEnd.Month, dateCouponEnd.Day);

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_UpdateOffer"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = id });

                    if (advertId.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@AdvertId", SqlDbType.Int, 4) { Value = advertId.Value });
                    if (agencyId.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@AgencyId", SqlDbType.Int, 4) { Value = agencyId.Value });
                    cmd.Parameters.Add(new SqlParameter("@CollectiveCategoryId", SqlDbType.Int, 4) { Value = collectiveCategoryId });
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 250) { Value = name });
                    cmd.Parameters.Add(new SqlParameter("@PriceReal", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Value = priceReal });
                    cmd.Parameters.Add(new SqlParameter("@PriceSave", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Value = priceSave });
                    cmd.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Value = price });
                    cmd.Parameters.Add(new SqlParameter("@CurrencyId", SqlDbType.Int, 4) { Value = currencyId });
                    cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.Char, 1) { Value = (char)status });
                    SqlParameter parCategoryName = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCategoryName);
                    if (discount.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@Discount", SqlDbType.Decimal, 8) { Value = discount.Value });
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.SmallDateTime, 4) { Value = dateStart });
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.SmallDateTime, 4) { Value = dateEnd });
                    cmd.Parameters.Add(new SqlParameter("@DateCouponStart", SqlDbType.SmallDateTime, 4) { Value = dateCouponStart });
                    cmd.Parameters.Add(new SqlParameter("@DateCouponEnd", SqlDbType.SmallDateTime, 4) { Value = dateCouponEnd });
                    if (numberOfPersons.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@NumberOfPersons", SqlDbType.Int, 4) { Value = numberOfPersons.Value });
                    if (numberOfCouponsPerUser.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@NumberOfCouponsPerUser", SqlDbType.Int, 4) { Value = numberOfCouponsPerUser.Value });
                    cmd.Parameters.Add(new SqlParameter("@MinBoughtCount", SqlDbType.Int, 4) { Value = minBoughtCount });
                    cmd.Parameters.Add(new SqlParameter("@MaxBoughtCount", SqlDbType.Int, 4) { Value = maxBoughtCount });
                    SqlParameter parBoughtCount = new SqlParameter("@BoughtCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parBoughtCount);
                    SqlParameter parClickCount = new SqlParameter("@ClickCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parClickCount);
                    if (!String.IsNullOrEmpty(offerType))
                        cmd.Parameters.Add(new SqlParameter("@OfferType", SqlDbType.NVarChar, 250) { Value = offerType });
                    if (longitude.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@Longitude", SqlDbType.Decimal, 8) { Precision = 10, Scale = 6, Value = longitude.Value });
                    if (latitude.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@Latitude", SqlDbType.Decimal, 8) { Precision = 10, Scale = 6, Value = latitude.Value });
                    cmd.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int, 4) { Value = priority });
                    cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active });


                    XDocument xdocPlaces = new XDocument(new XElement("root"));

                    foreach (int plId in lstPlacesIds)
                    {
                        xdocPlaces.Root.Add(new XElement("place",
                            new XAttribute("id", plId.ToString())));
                    }

                    cmd.Parameters.Add(new SqlParameter("@Places", SqlDbType.Xml) { Value = xdocPlaces.ToString() });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return new Offer(
                            id,
                            advertId,
                            agencyId,
                            collectiveCategoryId,
                            name,
                            priceReal,
                            priceSave,
                            price,
                            currencyId,
                            status,
                            parCategoryName.Value.ToString(),
                            discount,
                            dateStart,
                            dateEnd,
                            dateCouponStart,
                            dateCouponEnd,
                            numberOfPersons,
                            numberOfCouponsPerUser,
                            minBoughtCount,
                            maxBoughtCount,
                            Convert.ToInt32(parBoughtCount.Value),
                            Convert.ToInt32(parClickCount.Value),
                            offerType,
                            longitude,
                            latitude,
                            priority,
                            active
                        ) { IsClientDataSet = false };
                }
            }
        }

        public static Offer GetOffer(int id, int? langId)   // ako se posalje langId imat ce postavljen clientdata, ako ne, nece
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetOffer"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@OfferId", SqlDbType.Int, 4) { Value = id });

                    // ako se ne posalje langId nece ima polja iz prijevoda
                    if(langId.HasValue)
                        cmd.Parameters.Add(new SqlParameter("@LangId", SqlDbType.Int, 4) { Value = langId.Value });

                    SqlParameter parAdvertId = new SqlParameter("@AdvertId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };                    
                    SqlParameter parAgencyId = new SqlParameter("@AgencyId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parCollectiveCategoryId = new SqlParameter("@CollectiveCategoryId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parOfferName = new SqlParameter("@OfferName", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parPriceReal = new SqlParameter("@PriceReal", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Direction = ParameterDirection.Output };
                    SqlParameter parPriceSave = new SqlParameter("@PriceSave", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Direction = ParameterDirection.Output };
                    SqlParameter parPrice = new SqlParameter("@Price", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Direction = ParameterDirection.Output };
                    SqlParameter parCurrencyId = new SqlParameter("@CurrencyId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parStatus = new SqlParameter("@Status", SqlDbType.Char, 1) { Direction = ParameterDirection.Output };
                    SqlParameter parCategoryName = new SqlParameter("@CategoryName", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parDiscount = new SqlParameter("@Discount", SqlDbType.Decimal, 8) { Precision = 15, Scale = 2, Direction = ParameterDirection.Output };
                    SqlParameter parDateStart = new SqlParameter("@DateStart", SqlDbType.SmallDateTime, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parDateEnd = new SqlParameter("@DateEnd", SqlDbType.SmallDateTime, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parDateCouponStart = new SqlParameter("@DateCouponStart", SqlDbType.SmallDateTime, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parDateCouponEnd = new SqlParameter("@DateCouponEnd", SqlDbType.SmallDateTime, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parNumberOfPersons = new SqlParameter("@NumberOfPersons", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parNumberOfCouponsPerUser = new SqlParameter("@NumberOfCouponsPerUser", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parMinBoughtCount = new SqlParameter("@MinBoughtCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parMaxBoughtCount = new SqlParameter("@MaxBoughtCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parBoughtCount = new SqlParameter("@BoughtCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parClickCount = new SqlParameter("@ClickCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parOfferType = new SqlParameter("@OfferType", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parLongitude = new SqlParameter("@Longitude", SqlDbType.Decimal, 8) { Precision = 10, Scale = 6, Direction = ParameterDirection.Output };
                    SqlParameter parLatitude = new SqlParameter("@Latitude", SqlDbType.Decimal, 8) { Precision = 10, Scale = 6, Direction = ParameterDirection.Output };
                    SqlParameter parPriority = new SqlParameter("@Priority", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    SqlParameter parActive = new SqlParameter("@Active", SqlDbType.Bit, 1) { Direction = ParameterDirection.Output };

                    // client data parametars
                    SqlParameter parFirstImgSrc = new SqlParameter("@FirstImgSrc", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parOfferTitle = new SqlParameter("@OfferTitle", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parContentShort = new SqlParameter("@ContentShort", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parContentText = new SqlParameter("@ContentText", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    SqlParameter parReservationText = new SqlParameter("@ReservationText", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    SqlParameter parMetaDesc = new SqlParameter("@MetaDesc", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parMetaKW = new SqlParameter("@MetaKeywords", SqlDbType.NVarChar, 1000) { Direction = ParameterDirection.Output };
                    SqlParameter parUrlTag = new SqlParameter("@UrlTag", SqlDbType.NVarChar, 250) { Direction = ParameterDirection.Output };
                    SqlParameter parCurrencySymbol = new SqlParameter("@CurrencySymbol", SqlDbType.NVarChar, 5) { Direction = ParameterDirection.Output };

                    // language parametars

                    cmd.Parameters.Add(parAdvertId);
                    cmd.Parameters.Add(parAgencyId);
                    cmd.Parameters.Add(parCollectiveCategoryId);
                    cmd.Parameters.Add(parOfferName);
                    cmd.Parameters.Add(parPriceReal);
                    cmd.Parameters.Add(parPriceSave);
                    cmd.Parameters.Add(parPrice);
                    cmd.Parameters.Add(parCurrencyId);
                    cmd.Parameters.Add(parStatus);
                    cmd.Parameters.Add(parCategoryName);
                    cmd.Parameters.Add(parDiscount);
                    cmd.Parameters.Add(parDateStart);
                    cmd.Parameters.Add(parDateEnd);
                    cmd.Parameters.Add(parDateCouponStart);
                    cmd.Parameters.Add(parDateCouponEnd);
                    cmd.Parameters.Add(parNumberOfPersons);
                    cmd.Parameters.Add(parNumberOfCouponsPerUser);
                    cmd.Parameters.Add(parMinBoughtCount);
                    cmd.Parameters.Add(parMaxBoughtCount);
                    cmd.Parameters.Add(parBoughtCount);
                    cmd.Parameters.Add(parClickCount);
                    cmd.Parameters.Add(parOfferType);
                    cmd.Parameters.Add(parLongitude);
                    cmd.Parameters.Add(parLatitude);
                    cmd.Parameters.Add(parPriority);
                    cmd.Parameters.Add(parActive);

                    cmd.Parameters.Add(parFirstImgSrc);
                    cmd.Parameters.Add(parOfferTitle);
                    cmd.Parameters.Add(parContentShort);
                    cmd.Parameters.Add(parContentText);
                    cmd.Parameters.Add(parReservationText);
                    cmd.Parameters.Add(parMetaDesc);
                    cmd.Parameters.Add(parMetaKW);
                    cmd.Parameters.Add(parUrlTag);
                    cmd.Parameters.Add(parCurrencySymbol);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parCollectiveCategoryId.Value == DBNull.Value)  // ovaj je obavezan, pa ako je null...
                        return null;

                    return new Offer(
                        id,
                        parAdvertId.Value != DBNull.Value ? (int?)Convert.ToInt32(parAdvertId.Value) : null,
                        parAgencyId.Value != DBNull.Value ? (int?)Convert.ToInt32(parAgencyId.Value) : null,
                        Convert.ToInt32(parCollectiveCategoryId.Value),
                        parOfferName.Value.ToString(),
                        Convert.ToDouble(parPriceReal.Value),
                        Convert.ToDouble(parPriceSave.Value),
                        Convert.ToDouble(parPrice.Value),
                        Convert.ToInt32(parCurrencyId.Value),
                        (OfferStatusType)Convert.ToChar(parStatus.Value),
                        parCategoryName.Value.ToString(),
                        parDiscount.Value != DBNull.Value ? (double?)Convert.ToDouble(parDiscount.Value) : null,
                        Convert.ToDateTime(parDateStart.Value),
                        Convert.ToDateTime(parDateEnd.Value),
                        Convert.ToDateTime(parDateCouponStart.Value),
                        Convert.ToDateTime(parDateCouponEnd.Value),
                        parNumberOfPersons.Value != DBNull.Value ? (int?)Convert.ToInt32(parNumberOfPersons.Value) : null,
                        parNumberOfCouponsPerUser.Value != DBNull.Value ? (int?)Convert.ToInt32(parNumberOfCouponsPerUser.Value) : null,
                        Convert.ToInt32(parMinBoughtCount.Value),
                        Convert.ToInt32(parMaxBoughtCount.Value),
                        Convert.ToInt32(parBoughtCount.Value),
                        Convert.ToInt32(parClickCount.Value),
                        parOfferType.Value != DBNull.Value ? parOfferType.Value.ToString() : null,
                        parLongitude.Value != DBNull.Value ? (double?)Convert.ToDouble(parLongitude.Value) : null,
                        parLatitude.Value != DBNull.Value ? (double?)Convert.ToDouble(parLatitude.Value) : null,
                        Convert.ToInt32(parPriority.Value),
                        Convert.ToBoolean(parActive.Value)
                        )
                        {
                            // client data
                            FirstImgSrc = parFirstImgSrc.Value != DBNull.Value ? parFirstImgSrc.Value.ToString() : null,

                            Translation = langId.HasValue ? new OfferTranslation(id, langId.Value,
                                parOfferTitle.Value != DBNull.Value ? parOfferTitle.Value.ToString() : null,
                                parContentShort.Value != DBNull.Value ? parContentShort.Value.ToString() : null,
                                parContentText.Value != DBNull.Value ? parContentText.Value.ToString() : null,
                                parReservationText.Value != DBNull.Value ? parReservationText.Value.ToString() : null,
                                parMetaDesc.Value != DBNull.Value ? parMetaDesc.Value.ToString() : null,
                                parMetaKW.Value != DBNull.Value ? parMetaKW.Value.ToString() : null,
                                parUrlTag.Value.ToString()) : null,

                            CurrencySymbol = parCurrencySymbol.Value != DBNull.Value ? parCurrencySymbol.Value.ToString() : null,

                            IsClientDataSet = langId.HasValue
                        };
                }
            }
        }


        public static void ToggleActive(int offerId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ToggleOfferActive"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = offerId });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static void DeleteOffer(int offerId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeleteOffer"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = offerId });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public static List<Offer> ListOffersForClient(int? categoryId, int? placeId, int langId, int count)
        {
            List<Offer> lst = new List<Offer>();

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ListOffersForClient"
                })
                {
                    if (categoryId.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.Int, 4) { Value = categoryId.Value });
                    }

                    if (placeId.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("@PlaceId", SqlDbType.Int, 4) { Value = placeId.Value });
                    }

                    cmd.Parameters.Add(new SqlParameter("@LanguageId", SqlDbType.Int, 4) { Value = langId });
                    cmd.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int, 4) { Value = count });

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Offer(
                                Convert.ToInt32(rdr["Id"]),
                                rdr["AdvertId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["AdvertId"]) : null,
                                rdr["AgencyId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["AgencyId"]) : null,
                                Convert.ToInt32(rdr["CollectiveCategoryId"]),
                                rdr["OfferName"].ToString(),
                                Convert.ToDouble(rdr["PriceReal"]),
                                Convert.ToDouble(rdr["PriceSave"]),
                                Convert.ToDouble(rdr["Price"]),
                                Convert.ToInt32(rdr["CurrencyId"]),
                                (OfferStatusType)Convert.ToChar(rdr["Status"]),
                                rdr["CategoryName"].ToString(),
                                rdr["Discount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Discount"]) : null,
                                Convert.ToDateTime(rdr["DateStart"]),
                                Convert.ToDateTime(rdr["DateEnd"]),
                                Convert.ToDateTime(rdr["DateCouponStart"]),
                                Convert.ToDateTime(rdr["DateCouponEnd"]),
                                rdr["NumberOfPersons"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["NumberOfPersons"]) : null,
                                rdr["NumberOfCouponsPerUser"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["NumberOfCouponsPerUser"]) : null,
                                Convert.ToInt32(rdr["MinBoughtCount"]),
                                Convert.ToInt32(rdr["MaxBoughtCount"]),
                                Convert.ToInt32(rdr["BoughtCount"]),
                                Convert.ToInt32(rdr["ClickCount"]),
                                rdr["OfferType"] != DBNull.Value ? rdr["OfferType"].ToString() : null,
                                rdr["Longitude"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Longitude"]) : null,
                                rdr["Latitude"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Latitude"]) : null,
                                Convert.ToInt32(rdr["Priority"]),
                                Convert.ToBoolean(rdr["Active"])
                                )
                                {
                                    FirstImgSrc = rdr["FirstImgSrc"] != DBNull.Value ? rdr["FirstImgSrc"].ToString() : null,
                                    Translation = new OfferTranslation(Convert.ToInt32(rdr["Id"]), langId,
                                        rdr["OfferTitle"] != DBNull.Value ? rdr["OfferTitle"].ToString() : null,
                                        rdr["ContentShort"] != DBNull.Value ? rdr["ContentShort"].ToString() : null,
                                        rdr["ContentText"] != DBNull.Value ? rdr["ContentText"].ToString() : null,
                                        rdr["ReservationText"] != DBNull.Value ? rdr["ReservationText"].ToString() : null,
                                        rdr["MetaDesc"] != DBNull.Value ? rdr["MetaDesc"].ToString() : null,
                                        rdr["MetaKeywords"] != DBNull.Value ? rdr["MetaKeywords"].ToString() : null,
                                        rdr["UrlTag"].ToString()),
                                    CurrencySymbol = rdr["CurrencySymbol"] != DBNull.Value ? rdr["CurrencySymbol"].ToString() : null,
                                    IsClientDataSet = true
                                });
                        }

                        rdr.Close();
                    }
                    con.Close();
                }
            }

            return lst;
        }

        // vratit ce current page ako je veci od maksimalnog broja stranica
        // ova metoda koristi se u adminu pa nece popuniti property-e: FirstImgSrc, OfferTitle i ContentShort
        public static List<Offer> PagOffersForAdmin(SortType sortBy, bool isAsc, int pageNum, int numOfRows, int? active, out int totalPageCount, out int validPagNum)
        {
            List<Offer> lst = new List<Offer>();

            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_PagOffersForAdmin"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@SortBy", SqlDbType.VarChar, 50) { Value = sortBy.ToString() });
                    cmd.Parameters.Add(new SqlParameter("@SortAsc", SqlDbType.Bit, 1) { Value = isAsc });
                    cmd.Parameters.Add(new SqlParameter("@RowCount", SqlDbType.Int, 4) { Value = numOfRows });

                    if (active.HasValue)
                    {
                        cmd.Parameters.Add(new SqlParameter("@Active", SqlDbType.Bit, 1) { Value = active.Value });
                    }

                    SqlParameter parCurrPage = new SqlParameter("@PageNum", SqlDbType.Int, 4) { Value = pageNum, Direction = ParameterDirection.InputOutput };
                    cmd.Parameters.Add(parCurrPage);

                    SqlParameter parTotalPageCount = new SqlParameter("@TotalPageCount", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parTotalPageCount);

                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lst.Add(new Offer(
                                Convert.ToInt32(rdr["OfferId"]),
                                rdr["AdvertId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["AdvertId"]) : null,
                                rdr["AgencyId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["AgencyId"]) : null,
                                Convert.ToInt32(rdr["CollectiveCategoryId"]),
                                rdr["OfferName"].ToString(),
                                Convert.ToDouble(rdr["PriceReal"]),
                                Convert.ToDouble(rdr["PriceSave"]),
                                Convert.ToDouble(rdr["Price"]),
                                Convert.ToInt32(rdr["CurrencyId"]),
                                (OfferStatusType)Convert.ToChar(rdr["OfferStatus"]),
                                rdr["CategoryName"].ToString(),
                                rdr["Discount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Discount"]) : null,
                                Convert.ToDateTime(rdr["DateStart"]),
                                Convert.ToDateTime(rdr["DateEnd"]),
                                Convert.ToDateTime(rdr["DateCouponStart"]),
                                Convert.ToDateTime(rdr["DateCouponEnd"]),
                                rdr["NumberOfPersons"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["NumberOfPersons"]) : null,
                                rdr["NumberOfCouponsPerUser"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["NumberOfCouponsPerUser"]) : null,
                                Convert.ToInt32(rdr["MinBoughtCount"]),
                                Convert.ToInt32(rdr["MaxBoughtCount"]),
                                Convert.ToInt32(rdr["BoughtCount"]),
                                Convert.ToInt32(rdr["ClickCount"]),
                                rdr["OfferType"] != DBNull.Value ? rdr["OfferType"].ToString() : null,
                                rdr["Longitude"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Longitude"]) : null,
                                rdr["Latitude"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Latitude"]) : null,
                                Convert.ToInt32(rdr["Priority"]),
                                Convert.ToBoolean(rdr["Active"])
                                )
                                {
                                    IsClientDataSet = false
                                });
                        }

                        rdr.Close();

                        totalPageCount = Convert.ToInt32(parTotalPageCount.Value);
                        validPagNum = Convert.ToInt32(parCurrPage.Value);
                    }
                    con.Close();
                }
            }

            return lst;
        }

        public enum SortType
        {
            OfferId,
            OfferName,
            CategotyName,
            BoughtCount,
            DateStart
        }
    }

    public enum OfferStatusType
    {
        Inactive = 'I',
        Active = 'A',
        Completed = 'C',
        Failed = 'F'
    }
}