using System.Collections.Generic;
using System.Linq;

namespace Coins
{
    public class CoinMaster
    {
        public List<int> Denominations { get; set; }

        public Dictionary<int, int> GetChange(int val)
        {
            var resultset = new Dictionary<int, int>();
            foreach (var coin in Denominations.OrderByDescending(d => d))
            {
                //Ideally i would rather use % ( mod ) functions to calculate this kind of thing but
                // with certain value typing doing division can end up with the incorrect values
                // This would require messy casts to and from doubles and rounding adding to complexity and points of failure
                // For the use case this while loop will not add more overhead than the above. Realistically one would never reqire
                // more than a few loops for any given dinomination

                //In adition to reducing points of failure this code is also much simpler to read and understand.

                while (val >= coin)
                {
                    if (!resultset.ContainsKey(coin))
                    {
                        resultset.Add(coin, 0);
                    }
                    resultset[coin]++;
                    val -= coin;
                }
            }
            if (val > 0)
            {
                resultset.Add(-1, val);
            }
            return resultset;
        }

        public void SetDenominations(IEnumerable<int> denominations)
        {
            Denominations = new List<int>();
            Denominations.AddRange(denominations);
        }
    }
}