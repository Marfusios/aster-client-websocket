![Logo](asterdex-logo-alt.png)
# Aster websocket API client

[![NuGet version](https://img.shields.io/nuget/v/Aster.Client.Websocket?style=flat-square)](https://www.nuget.org/packages/Aster.Client.Websocket)
[![Nuget downloads](https://img.shields.io/nuget/dt/Aster.Client.Websocket?style=flat-square)](https://www.nuget.org/packages/Aster.Client.Websocket)
[![CI build](https://img.shields.io/github/check-runs/marfusios/aster-client-websocket/master?style=flat-square&label=build)](https://github.com/Marfusios/aster-client-websocket/actions/workflows/dotnet-core.yml)

This library is a .NET client for the **Aster DEX** public websocket API found here: 

https://github.com/asterdex/api-docs/blob/master/aster-finance-futures-api-v3.md#partial-book-depth-streams

### License: 
    Apache License 2.0

### Highlights

- available on NuGet as [`Aster.Client.Websocket`](https://www.nuget.org/packages/Aster.Client.Websocket)
- covers all documented market-data streams: trades, aggregate trades, kline, mark price, mini tickers, tickers,
  book tickers, partial and diff order books, liquidation alerts
- authenticated user streams for margin calls, balance/position updates, order trade updates, and config changes
- ready for private streams once the authenticated API is published
- integrates with [System.Reactive](https://github.com/dotnet/reactive) for composable stream handling
- targets `netstandard2.1`, `net6`, `net7`, `net8`

### Quick start

```csharp
var exitEvent = new ManualResetEvent(false);
var url = AsterValues.ApiWebsocketUrl;

using var communicator = new AsterWebsocketCommunicator(url);
using var client = new AsterWebsocketClient(communicator);

client.Streams.TradesStream.Subscribe(resp =>
{
    var trade = resp.Data;
    Console.WriteLine($"[{trade.Symbol}] {trade.Price} x {trade.Quantity}");
});

client.SetSubscriptions(
    new TradeSubscription("btcusdt"),
    new OrderBookPartialSubscription("btcusdt", 20)
);

await communicator.Start();
exitEvent.WaitOne(TimeSpan.FromSeconds(30));
```

### User data streams

Authenticate the websocket communicator with your API key and secret to receive private events. The client refreshes
the listen key for you every 55 minutes and exposes dedicated observables for each event category.

```csharp
var communicator = new AsterWebsocketCommunicator(AsterValues.FuturesApiWebsocketUrl);
await communicator.Authenticate(apiKey, new AsterHmac(apiSecret));

var client = new AsterWebsocketClient(communicator);
client.Streams.AccountUpdateStream.Subscribe(update =>
{
    foreach (var balance in update.Data.Balances)
    {
        Console.WriteLine($"Balance for {balance.Asset}: {balance.WalletBalance}");
    }
});

client.Streams.OrderUpdateStream.Subscribe(evt =>
{
    var order = evt.Order;
    Console.WriteLine($"Order {order.Type} {order.Side} -> status {order.Status}, filled {order.QuantityFilled}");
});

client.Streams.ListenKeyExpiredStream.Subscribe(_ =>
    Console.WriteLine("Listen key expired – call Authenticate() again to obtain a fresh key."));

await communicator.Start();
```

More samples:
- console app ([link](test_integration/Aster.Client.Websocket.Sample/Program.cs))

### Working with streams

- **Raw stream**: `new Uri("wss://fstream.asterdex.com/ws/btcusdt@aggTrade")`
- **Combined stream**: call `client.SetSubscriptions(...)` with the desired subscriptions – the client will build `/stream?streams=a/b`.
- **Live subscribe/unsubscribe**: use `client.Send(new { method = "SUBSCRIBE", params = ..., id = ... })`. The JSON contracts mirror the official docs.
- Ping/pong handling is automatic; unsolicited `PONG` frames are allowed by the server.
- One connection may subscribe to at most 200 streams, with an inbound limit of 10 messages/sec.

### Backtesting support

`AsterFileCommunicator` replays delimited files and feeds the client with historical data:

```csharp
var communicator = new AsterFileCommunicator
{
    FileNames = new[] { "data/aster_raw_btcusdt_2018-11-13.txt" },
    Delimiter = ";;"
};

var client = new AsterWebsocketClient(communicator);
client.Streams.AggregateTradesStream.Subscribe(resp =>
{
    // analyse resp.Data
});

await communicator.Start();
```

The tests look for uncompressed files in the `data/` folder and skip automatically if they are missing.

### Reconnection strategy

`AsterWebsocketCommunicator` retries automatically. Tunable properties:
- `ReconnectTimeout` – inactivity watchdog (default 1 minute)
- `ErrorReconnectTimeout` – cooldown after failures (default 1 minute)
- `DisconnectionHappened` / `ReconnectionHappened` – observables that surface connection status

### Multi-threading tips

Every stream is an Rx observable. Use `ObserveOn` to off-load work:

```csharp
client.Streams.TradesStream
    .ObserveOn(TaskPoolScheduler.Default)
    .Subscribe(HandleTrade);
```

To preserve ordering across multiple subscriptions, share a gate:

```csharp
var gate = new object();
client.Streams.TradesStream
      .ObserveOn(TaskPoolScheduler.Default)
      .Synchronize(gate)
      .Subscribe(HandleTrade);

client.Streams.OrderBookDiffStream
      .ObserveOn(TaskPoolScheduler.Default)
      .Synchronize(gate)
      .Subscribe(HandleBookDiff);
```

### Need help?

Consulting and support are available – contact **m@mkotas.cz**.
