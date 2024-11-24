using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class TrendFollowerBot : Robot
    {
        // Parameters
        [Parameter("Fast MA Period", DefaultValue = 50)]
        public int FastMAPeriod { get; set; }

        [Parameter("Slow MA Period", DefaultValue = 200)]
        public int SlowMAPeriod { get; set; }

        [Parameter("RSI Period", DefaultValue = 14)]
        public int RSIPeriod { get; set; }

        [Parameter("RSI Overbought", DefaultValue = 70)]
        public double RSIOverbought { get; set; }

        [Parameter("RSI Oversold", DefaultValue = 30)]
        public double RSIOversold { get; set; }

        [Parameter("Volume", DefaultValue = 10000, MinValue = 1000)]
        public int Volume { get; set; }

        [Parameter("Stop Loss (pips)", DefaultValue = 50)]
        public int StopLossPips { get; set; }

        [Parameter("Take Profit (pips)", DefaultValue = 100)]
        public int TakeProfitPips { get; set; }

        [Parameter("Enable Trailing Stop", DefaultValue = false)]
        public bool EnableTrailingStop { get; set; }

        [Parameter("Trailing Stop (pips)", DefaultValue = 30, MinValue = 10)]
        public int TrailingStopPips { get; set; }

        // Indicators
        private MovingAverage _fastMA;
        private MovingAverage _slowMA;
        private RelativeStrengthIndex _rsi;

        protected override void OnStart()
        {
            // Initialize Indicators
            _fastMA = Indicators.SimpleMovingAverage(Bars.ClosePrices, FastMAPeriod);
            _slowMA = Indicators.SimpleMovingAverage(Bars.ClosePrices, SlowMAPeriod);
            _rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, RSIPeriod);
        }

        protected override void OnBar()
        {
            // Determine trend
            bool isBullish = _fastMA.Result.Last(1) > _slowMA.Result.Last(1);
            bool isBearish = _fastMA.Result.Last(1) < _slowMA.Result.Last(1);

            // Get RSI value
            double currentRSI = _rsi.Result.Last(1);
            double previousRSI = _rsi.Result.Last(2);

            // Check for existing positions
            bool hasLong = Positions.Find("TrendFollowerBot", SymbolName, TradeType.Buy) != null;
            bool hasShort = Positions.Find("TrendFollowerBot", SymbolName, TradeType.Sell) != null;

            // Buy Signal
            if (isBullish && previousRSI < RSIOversold && currentRSI >= RSIOversold && !hasLong)
            {
                ExecuteMarketOrder(TradeType.Buy, SymbolName, Volume, "TrendFollowerBot", StopLossPips, TakeProfitPips);
            }

            // Sell Signal
            if (isBearish && previousRSI > RSIOverbought && currentRSI <= RSIOverbought && !hasShort)
            {
                ExecuteMarketOrder(TradeType.Sell, SymbolName, Volume, "TrendFollowerBot", StopLossPips, TakeProfitPips);
            }

            // Trailing Stop Management
            if (EnableTrailingStop)
            {
                foreach (var position in Positions.FindAll("TrendFollowerBot", SymbolName))
                {
                    double trailingStopPrice;
                    if (position.TradeType == TradeType.Buy)
                    {
                        trailingStopPrice = Symbol.Bid - TrailingStopPips * Symbol.PipSize;
                        if (Symbol.Bid > position.EntryPrice + TrailingStopPips * Symbol.PipSize)
                        {
                            if (position.StopLoss < trailingStopPrice)
                                ModifyPosition(position, trailingStopPrice, position.TakeProfit);
                        }
                    }
                    else if (position.TradeType == TradeType.Sell)
                    {
                        trailingStopPrice = Symbol.Ask + TrailingStopPips * Symbol.PipSize;
                        if (Symbol.Ask < position.EntryPrice - TrailingStopPips * Symbol.PipSize)
                        {
                            if (position.StopLoss > trailingStopPrice)
                                ModifyPosition(position, trailingStopPrice, position.TakeProfit);
                        }
                    }
                }
            }
        }

        protected override void OnStop()
        {
            // Optional: Close all positions when the bot stops
            foreach (var position in Positions.FindAll("TrendFollowerBot", SymbolName))
            {
                ClosePosition(position);
            }
        }
    }
}