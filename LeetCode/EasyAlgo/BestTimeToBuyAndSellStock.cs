public class BestTimeToBuyAndSellStock
{
    /// <summary>
    /// Restituisce il massimo profitto che può essere ottenuto dall'acquisto e vendita di azioni.
    /// Utilizza due puntatori per trovare il punto di acquisto e vendita ottimale.
    /// </summary>
    /// <param name="prices">Array di prezzi delle azioni.</param>
    /// <returns>Massimo profitto.</returns>
    public int MaxProfit(int[] prices)
    {
        int maxProfit = 0;
        int minPrice = int.MaxValue;

        foreach (int price in prices)
        {
            if (price < minPrice)
            {
                minPrice = price;
            }
            else
            {
                maxProfit = Math.Max(maxProfit, price - minPrice);
            }
        }
    }
}