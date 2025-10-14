using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Aster.Client.Websocket.Responses;
using Aster.Client.Websocket.Responses.AggregateTrades;
using Aster.Client.Websocket.Responses.Books;
using Aster.Client.Websocket.Responses.BookTickers;
using Aster.Client.Websocket.Responses.Kline;
using Aster.Client.Websocket.Responses.MarkPrice;
using Aster.Client.Websocket.Responses.MiniTicker;
using Aster.Client.Websocket.Responses.Orders;
using Aster.Client.Websocket.Responses.Trades;
using Aster.Client.Websocket.Responses.UserData;

namespace Aster.Client.Websocket.Client
{
    /// <summary>
    /// All provided streams.
    /// You need to set subscriptions in advance (via method `SetSubscriptions()` on AsterWebsocketClient)
    /// </summary>
    public class AsterClientStreams
    {
        internal readonly Subject<PongResponse> PongSubject = new Subject<PongResponse>();

        internal readonly Subject<TradeResponse> TradesSubject = new Subject<TradeResponse>();
        internal readonly Subject<AggregatedTradeResponse> TradeBinSubject = new Subject<AggregatedTradeResponse>();

        internal readonly Subject<OrderBookPartialResponse> OrderBookPartialSubject =
            new Subject<OrderBookPartialResponse>();

        internal readonly Subject<OrderBookDiffResponse> OrderBookDiffSubject = new Subject<OrderBookDiffResponse>();
        internal readonly Subject<FundingResponse> FundingSubject = new Subject<FundingResponse>();

        internal readonly Subject<BookTickerResponse> BookTickerSubject = new Subject<BookTickerResponse>();

        internal readonly Subject<KlineResponse> KlineSubject = new Subject<KlineResponse>();

        internal readonly Subject<MiniTickerResponse> MiniTickerSubject = new Subject<MiniTickerResponse>();
        internal readonly Subject<AllMarketMiniTickerResponse> AllMarketMiniTickerSubject = new Subject<AllMarketMiniTickerResponse>();

        internal readonly Subject<OrderUpdate> OrderUpdateSubject = new Subject<OrderUpdate>();
        internal readonly Subject<MarginCallEvent> MarginCallSubject = new Subject<MarginCallEvent>();
        internal readonly Subject<AccountUpdateEvent> AccountUpdateSubject = new Subject<AccountUpdateEvent>();
        internal readonly Subject<AccountConfigUpdateEvent> AccountConfigUpdateSubject = new Subject<AccountConfigUpdateEvent>();
        internal readonly Subject<ListenKeyExpiredEvent> ListenKeyExpiredSubject = new Subject<ListenKeyExpiredEvent>();

        // PUBLIC

        /// <summary>
        /// Response stream to every ping request
        /// </summary>
        public IObservable<PongResponse> PongStream => PongSubject.AsObservable();

        /// <summary>
        /// Trades stream - emits every executed trade on Aster
        /// </summary>
        public IObservable<TradeResponse> TradesStream => TradesSubject.AsObservable();

        /// <summary>
        /// Chunk of trades - emits grouped trades together
        /// </summary>
        public IObservable<AggregatedTradeResponse> AggregateTradesStream => TradeBinSubject.AsObservable();

        /// <summary>
        /// Partial order book stream - emits small snapshot of the order book
        /// </summary>
        public IObservable<OrderBookPartialResponse> OrderBookPartialStream => OrderBookPartialSubject.AsObservable();

        /// <summary>
        /// Order book difference stream - emits small snapshot of the order book
        /// </summary>
        public IObservable<OrderBookDiffResponse> OrderBookDiffStream => OrderBookDiffSubject.AsObservable();

        /// <summary>
        /// Mark price and funding rate stream - emits mark price and funding rate for a single symbol pushed every 3 seconds or every second
        /// </summary>
        public IObservable<FundingResponse> FundingStream => FundingSubject.AsObservable();

        /// <summary>
        ///  The best bid or ask's price or quantity in real-time for a specified symbol
        /// </summary>
        public IObservable<BookTickerResponse> BookTickerStream => BookTickerSubject.AsObservable();

        /// <summary>
        /// The Kline/Candlestick subscription, provide symbol and chart intervals
        /// </summary>
        public IObservable<KlineResponse> KlineStream => KlineSubject.AsObservable();

        /// <summary>
        /// Mini-ticker specified symbol statistics for the previous 24hrs
        /// </summary>
        public IObservable<MiniTickerResponse> MiniTickerStream => MiniTickerSubject.AsObservable();
        
        /// <summary>
        /// Mini-ticker all symbol statistics for the previous 24hrs
        /// </summary>
        public IObservable<AllMarketMiniTickerResponse> AllMarketMiniTickerStream => AllMarketMiniTickerSubject.AsObservable();
        
        
        // PRIVATE

        /// <summary>
        /// Order update stream - emits every update to the private order,
        /// you need to be subscribed to authenticated API
        /// </summary>
        public IObservable<OrderUpdate> OrderUpdateStream => OrderUpdateSubject.AsObservable();

        /// <summary>
        /// Margin call warnings for leveraged positions
        /// </summary>
        public IObservable<MarginCallEvent> MarginCallStream => MarginCallSubject.AsObservable();

        /// <summary>
        /// Account balance and position updates
        /// </summary>
        public IObservable<AccountUpdateEvent> AccountUpdateStream => AccountUpdateSubject.AsObservable();

        /// <summary>
        /// Account configuration changes such as leverage or multi-asset mode
        /// </summary>
        public IObservable<AccountConfigUpdateEvent> AccountConfigUpdateStream => AccountConfigUpdateSubject.AsObservable();

        /// <summary>
        /// Notification that the current listen key expired
        /// </summary>
        public IObservable<ListenKeyExpiredEvent> ListenKeyExpiredStream => ListenKeyExpiredSubject.AsObservable();
    }
}
