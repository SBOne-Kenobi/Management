using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Management
{
    public struct Demand : IComparable<Demand>
    {
        public int Price { get; }
        public int UMat { get; }
        public int Priority { get; }

        public Demand(int price, int mat, int pr)
        {
            this.Price = price;
            this.UMat = mat;
            this.Priority = pr;
        }

        public int CompareTo(Demand other)
        {
            if (this.Price.CompareTo(other.Price) != 0)
                return -this.Price.CompareTo(other.Price);
            else
            {
                if (this.Priority.CompareTo(other.Priority) != 0)
                    return this.Priority.CompareTo(other.Priority);
                else
                    return this.UMat.CompareTo(other.UMat);

            }
        }
    }

    public struct Offer : IComparable<Offer>
    {
        public int Price { get; }
        public int UProd { get; }
        public int Priority { get; }

        public Offer(int price, int prod, int pr)
        {
            this.Price = price;
            this.UProd = prod;
            this.Priority = pr;
        }

        public int CompareTo(Offer other)
        {
            if (this.Price.CompareTo(other.Price) != 0)
                return this.Price.CompareTo(other.Price);
            else
            {
                if (this.Priority.CompareTo(other.Priority) != 0)
                    return this.Priority.CompareTo(other.Priority);
                else
                    return this.UProd.CompareTo(other.UProd);
            }
        }
    }

    public struct DemandOffer
    {
        public double UMat { get; } //units of material per one.
        public int MinPrice { get; }//minimal price of mat.

        public double UProd { get; }//units of product per one.
        public int MaxPrice { get; }//maximal price of prod.

        public DemandOffer(double mat, int minPr, double prod, int maxPr)
        {
            this.UMat = mat;
            this.MinPrice = minPr;
            this.UProd = prod;
            this.MaxPrice = maxPr;
        }
    }

    public class Bank
    {
        public static float[,] ProbOfPriceChange { get; } =
        {
            {1f/3f, 1f/3f, 1f/6f, 1f/12f, 1f/12f},
            {1f/4f, 1f/3f, 1f/4f, 1f/12f, 1f/12f},
            {1f/12f, 1f/4f, 1f/3f, 1f/4f, 1f/12f},
            {1f/12f, 1f/12f, 1f/4f, 1f/3f, 1f/4f},
            {1f/12f, 1f/12f, 1f/6f, 1f/3f, 1f/3f}
        };

        public static DemandOffer[] PriceLevelInfo { get; } =
        {
            new DemandOffer(1.0f, 800, 3.0f, 6500),
            new DemandOffer(1.5f, 650, 2.5f, 6000),
            new DemandOffer(2.0f, 500, 2.0f, 5500),
            new DemandOffer(2.5f, 400, 1.5f, 5000),
            new DemandOffer(3.0f, 300, 1.0f, 4500)
        };

        private int _priceLevel;
        public int PriceLevel => _priceLevel; 

        private int _materials;
        private int _product;

        private DemandOffer[] _report;
        public DemandOffer[] Report => _report;

        public Bank(int N)
        {
            _priceLevel = 2;
            _product = (int)Math.Floor(PriceLevelInfo[PriceLevel].UProd * N);
            _materials = (int)Math.Floor(PriceLevelInfo[PriceLevel].UMat * N);
        }

        public void SetNewPriceLevel(int N)
        {
            float Rem = 1.0f;
            for (int i = 0; i < ProbOfPriceChange.GetLength(_priceLevel); i++)
            {
                if (UnityEngine.Random.Range(0.0f, Rem) <= ProbOfPriceChange[_priceLevel, i])
                {
                    _priceLevel = i;
                    break;
                }
                Rem -= ProbOfPriceChange[_priceLevel, i];
            }
            _materials = (int)Math.Floor(PriceLevelInfo[_priceLevel].UMat * N);
            _product = (int)Math.Floor(PriceLevelInfo[_priceLevel].UProd * N);
        }

        public DemandOffer GetInfo => PriceLevelInfo[_priceLevel];

        public List<Demand> RequestOfMat(List<Demand> demands) //returns array of getting mat.
        {
            List<Demand> res = new List<Demand>();
            demands.Sort();
            foreach (Demand d in demands)
            {
                int got = Math.Min(_materials, d.UMat);
                if (d.Price < GetInfo.MinPrice)
                    got = 0;
                _materials -= got;
                res.Add(new Demand(d.Price, got, d.Priority));
            }
            return res;
        }

        public List<Offer> RequestOfProd(List<Offer> offers) //returns array of getting prod.
        {
            List<Offer> res = new List<Offer>();
            offers.Sort();
            foreach (Offer o in offers)
            {
                int got = Math.Min(_product, o.UProd);
                if (o.Price > GetInfo.MaxPrice)
                    got = 0;
                _product -= got;
                res.Add(new Offer(o.Price, got, o.Priority));
            }
            return res;
        }
    }
}