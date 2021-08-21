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

***

# EDEN Architecture

The following is a diagram representing the architecture of EDEN and EXODUS. EXODUS is the market data dissemination platform that will be built alongside EDEN. 

![Architecture](/resources/architecture.png)

**Note:** Currently, EDEN is being worked on. Work on EXODUS has not started. The Instrument Flat File detailed above does not yet exist.

## Description

*EDEN and all trading clients reference a flat file detailing which instruments are supported for trading. Orders submitted from trading clients for instruments not contained in the flat file will be rejected by EDEN.*

1. Trading client connects to EDEN via TCP, leveraging gRPC. The trading client now has a private communication channel open between itself and EDEN.
2. Trading client can submit New Order, Cancel Order, and Modify Order requests, receving a corresponding acknowledgement for each via the gRPC bi-directional stream.
3. Upon receipt of a New Order, Cancel Order, or Modify Order, EDEN persists the order's information to Cassandra.
4. If a trading client's order matches against a resting order, they receive a Fill via the gRPC bi-directional stream.
5. Given a match, a Trade Summary, Incremental Orderbook Update and Trade Statistic message is generated and sent to EXODUS.
6. EXODUS persists this information in a seperate instance of Cassandra dedicated to storing market data. Whether it persists it to a seperate instance of Cassandra or the same instance that EDEN uses is still up for discussion.
7. Said market data is sent to all listening clients. Listening applications include:
    i) Applications with order entry capabilities listening to market data.
    ii) Applications **without** order entry capabilities listening to market data. In other words, one does not need to connect to EDEN to connect to EXODUS.