# Eden

![Eden](/resources/Eden.png)

Eden is an algorithmic trading platform, whose core is a trading engine and limit orderbook, implemented in C# and built on .netstandard2.1. 
All the code under `src/` is being built upon in real-time on my [Algo Trading Platform Series](https://youtube.com/playlist?list=PLIkrF4j3_p-3fA9LyzSpT6yFPnqvJ02LS).

***

## Currently Supported Features

The following features are currently supported.

### Order Types

1. New Order
2. Modify Order
3. Cancel Order

### Order Responses

1. New Order Acknowledgement
2. Modify Order Acknowledgement
3. Cancel Order Acknowledgement
4. Fill

### Matching Algorithms

1. First-In-First-Out (FIFO)
2. Last-In-First-Out (LIFO)
3. Pro-Rata

### Market Data

1. Trade Summary
2. Market-By-Order Incremental Orderbook Update

***

## Planned Features

The following features are on the roadmap.

### Communication

Private gRPC stream-based communication for order entry between trading clients and the algorithmic trading platform.

### Market Data Dissemination

Seperate market data dissemination platform.

### Market Data

1. Session Statistics
2. Daily Statistics

***

# Building Eden

The following steps will allow you to build and run Eden.

1. Download [Visual Studio 2019](https://visualstudio.microsoft.com/vs/).
2. Download this repository.
3. Open `TradingEngine.sln` under `src/TradingEngine`
4. Hit F5 to build and run the solution.
