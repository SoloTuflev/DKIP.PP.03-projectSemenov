namespace ReviewSamples.Modules.Variants.Fixed
{
    public class WarehouseManager
    {
        private readonly Dictionary<string, int> _stock = new Dictionary<string, int>();

        private void UpdateStock(string productName, int quantity)
        {
            
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Название товара не может быть пустым.", nameof(productName));

            if (quantity == 0)
                return;

            if (quantity < 0 && _stock.TryGetValue(productName, out int currentQty) && currentQty + quantity < 0)
                throw new InvalidOperationException($"Недостаточно товара '{productName}' на складе.");

            _stock[productName] = _stock.TryGetValue(productName, out int current) ? current + quantity : quantity;
        }

        // Приход товара
        public void Add(string productName, int quantity)
        {
            
            if (quantity <= 0)
                throw new ArgumentException("Количество для добавления должно быть больше нуля.", nameof(quantity));

            UpdateStock(productName, quantity);
        }

        // Расход товара. 
        public bool Remove(string productName, int quantity)
        {
            
            if (quantity <= 0)
                throw new ArgumentException("Количество для списания должно быть больше нуля.", nameof(quantity));

            if (!_stock.ContainsKey(productName) || _stock[productName] < quantity)
                return false; 

            UpdateStock(productName, -quantity);
            return true;
        }

        // Метод для получения копии данных
        public IReadOnlyDictionary<string, int> GetCurrentStock()
        {
            return new Dictionary<string, int>(_stock);
        }
    }

    public static class StockPrinter
    {
        public static void PrintStock(IReadOnlyDictionary<string, int> stock)
        {
            Console.WriteLine("--- Stock ---");
            foreach (var pair in stock)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value}");
            }
        }
    }
}
