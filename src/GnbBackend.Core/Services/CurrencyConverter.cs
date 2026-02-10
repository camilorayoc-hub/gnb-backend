using GnbBackend.Core.Interfaces;
using GnbBackend.Core.Models;
using Microsoft.Extensions.Logging;

namespace GnbBackend.Core.Services;

public class CurrencyConverter : ICurrencyConverter
{
    private readonly Dictionary<string, List<(string Currency, decimal Rate)>> _conversionGraph;
    private readonly ILogger<CurrencyConverter> _logger;

    public CurrencyConverter(IEnumerable<ExchangeRate> rates, ILogger<CurrencyConverter> logger)
    {
        _logger = logger;
        _conversionGraph = BuildConversionGraph(rates);
    }

    private Dictionary<string, List<(string Currency, decimal Rate)>> BuildConversionGraph(IEnumerable<ExchangeRate> rates)
    {
        var graph = new Dictionary<string, List<(string, decimal)>>();

        foreach(var rate in rates){
            if (!graph.ContainsKey(rate.From)) graph[rate.From] = new List<(string, decimal)>();
            graph[rate.From].Add((rate.To, rate.Rate)); 

            if (!graph.ContainsKey(rate.To)) graph[rate.To] = new List<(string, decimal)>();
            graph[rate.To].Add((rate.From, 1m / rate.Rate)); 
        }
        return graph;
    }

    public decimal? Convert(decimal amount, string fromCurrency, string toCurrency)
    {
        if (fromCurrency == toCurrency) return Math.Round(amount, 2, MidpointRounding.ToEven);

        if (!_conversionGraph.ContainsKey(fromCurrency)){
            _logger.LogWarning("Currency {From} not found", fromCurrency);
            return null;
        }

        var path = FindConversionPath(fromCurrency, toCurrency);
        if (path == null){
            _logger.LogWarning("No path from {From} to {To}", fromCurrency, toCurrency);
            return null;
        }

        decimal result = amount;
        for (int i = 0; i < path.Count - 1; i++){
            var current = path[i];
            var next = path[i + 1];

            var rate = _conversionGraph[current].First(x => x.Currency == next).Rate;

            result *= rate;
            result = Math.Round(result, 2, MidpointRounding.ToEven);
        }
        return result;
    }

    private List<string>? FindConversionPath(string from, string to)
    {
        if (!_conversionGraph.ContainsKey(from) || !_conversionGraph.ContainsKey(to)) return null;

        var queue = new Queue<(string Currency, List<string> Path)>();
        var visited = new HashSet<string>();

        queue.Enqueue((from, new List<string>{from}));
        visited.Add(from);

        while (queue.Count > 0)
        {
            var (current, path) = queue.Dequeue();

            if (current == to) return path;

            if (_conversionGraph.ContainsKey(current)){
                foreach(var (nextCurrency, _) in _conversionGraph[current]){
                    if (!visited.Contains(nextCurrency)){
                        visited.Add(nextCurrency);
                        var newPath = new List<string>(path) { nextCurrency };
                        queue.Enqueue((nextCurrency, newPath));
                    }
                }
            }
        }
        return null; 
    }
}