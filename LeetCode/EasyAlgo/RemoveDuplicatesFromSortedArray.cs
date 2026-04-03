public class RemoveDuplicatesFromSortedArray
{
    /// <summary>
    /// Rimuove i duplicati da un array ordinato e restituisce il numero di elementi univoci.
    /// Utilizza due puntatori per trovare i duplicati e rimuoverli.
    /// </summary>
    /// <param name="nums">Array di numeri ordinati.</param>
    /// <returns>Numero di elementi univoci.</returns>
    public int RemoveDuplicates(int[] nums)
    {
        // slow è l'indice dell'ultimo elemento univoco
        int slow = 0;

        // fast è l'indice dell'elemento corrente
        for (int fast = 1; fast < nums.Length; fast++)
        {
            // se l'elemento corrente è diverso dall'ultimo elemento univoco, lo copiamo nella posizione slow + 1
            if (nums[fast] != nums[slow])
            {
                // incrementiamo slow e copiamo l'elemento corrente nella posizione slow + 1
                nums[++slow] = nums[fast];
            }


        }
        // returniamo il numero di elementi univoci
        return slow + 1;
    }
}