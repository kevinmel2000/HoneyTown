﻿using HarvestMoon.Entities.General;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarvestMoon.Entities.Town
{
    internal class FloristStore: UpfrontStore
    {
        static List<string> GetItems()
        {
            if(HarvestMoon.Instance.Season == "Spring")
            {
                return new List<string> { "Grass", "Turnip", "Potato" };
            }
            else
            {
                return new List<string> { "Grass", "Corn", "Tomato" };
            }
        }

        static List<string> GetClasses()
        {
            return new List<string> { "Crop", "Crop", "Crop" }; 
        }

        static List<int> GetPrices()
        {
            if (HarvestMoon.Instance.Season == "Spring")
            {
                return new List<int> { 300, 120, 180};
            }
            else
            {
                return new List<int> { 300, 280, 240 };
            }

        }

        static Func<List<string>, List<int>, int, string> GetPurchaseCallback()
        {
            return (List<string> purchases, List<int> amounts, int total) =>
            {
                if (purchases.Count == 0)
                {
                    ConfirmPurchase = false;
                    return "There is nothing to buy";
                }

                if (total > HarvestMoon.Instance.Gold)
                {
                    ConfirmPurchase = false;
                    return "Not enough Gold";
                }

                if (!ConfirmPurchase)
                {
                    ConfirmPurchase = true;
                    return "Please confirm purchase";
                }
                else
                {
                    ConfirmPurchase = false;
                }


                for (int i = 0; i < purchases.Count; ++i)
                {
                    string purchase = purchases[i].ToLower();

                    if (purchase == "grass")
                    {
                        HarvestMoon.Instance.GrassSeeds += amounts[i];
                    }


                    if (purchase == "turnip")
                    {
                        HarvestMoon.Instance.TurnipSeeds += amounts[i];
                    }


                    if (purchase == "potato")
                    {
                        HarvestMoon.Instance.PotatoSeeds += amounts[i];
                    }

                    if (purchase == "corn")
                    {
                        HarvestMoon.Instance.CornSeeds += amounts[i];
                    }


                    if (purchase == "tomato")
                    {
                        HarvestMoon.Instance.TomatoSeeds += amounts[i];
                    }

                }

                HarvestMoon.Instance.Gold -= total;

                return "Thanks for your purchase";
            };
        }

        public FloristStore(Vector2 initialPosition,
                            Size2 size,
                            string message,
                            string title):
           base(initialPosition,
                size,
                message,
                title,
                GetItems(),
                GetClasses(),
                GetPrices(),
                GetPurchaseCallback())
        {
            ConfirmPurchase = false;
        }
    }
}
