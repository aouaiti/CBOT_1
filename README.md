# TrendFollowerBot

## Overview
TrendFollowerBot is an algorithmic trading robot built using cAlgo (part of cTrader). It follows a simple trend-following strategy based on two Moving Averages (Fast and Slow) and Relative Strength Index (RSI) to identify potential buy and sell opportunities. It also features a customizable stop loss, take profit, and optional trailing stop functionality.

## Key Features
- **Trend Following**: The bot uses a fast and slow moving average to determine the current market trend (bullish or bearish).
- **RSI Indicator**: The Relative Strength Index is used to identify overbought and oversold conditions to improve entry signals.
- **Customizable Parameters**: The bot's parameters, such as Moving Average periods, RSI settings, stop loss, take profit, and trailing stop, can be adjusted to suit your trading strategy.
- **Trailing Stop**: If enabled, the bot manages positions with a dynamic trailing stop to lock in profits as the market moves in your favor.
- **Volume Management**: The volume of each trade can be specified by the user.
- **Risk Management**: It includes configurable stop loss and take profit in pips, as well as trailing stop distance.

## Parameters
- **Fast MA Period**: Period for the fast moving average (default: 50).
- **Slow MA Period**: Period for the slow moving average (default: 200).
- **RSI Period**: Period for the RSI (default: 14).
- **RSI Overbought**: RSI level for identifying overbought conditions (default: 70).
- **RSI Oversold**: RSI level for identifying oversold conditions (default: 30).
- **Volume**: Trading volume for each order (default: 10000, min: 1000).
- **Stop Loss (pips)**: Stop loss distance in pips (default: 50).
- **Take Profit (pips)**: Take profit distance in pips (default: 100).
- **Enable Trailing Stop**: Option to enable trailing stop functionality (default: false).
- **Trailing Stop (pips)**: Distance in pips for the trailing stop (default: 30, min: 10).

## Strategy Logic
1. **Buy Signal**:
   - A bullish trend is confirmed if the Fast MA is above the Slow MA.
   - RSI crosses from below the oversold level (RSI < 30) to above the oversold level (RSI >= 30).
   - If the conditions are met and no long position is open, the bot opens a buy order.

2. **Sell Signal**:
   - A bearish trend is confirmed if the Fast MA is below the Slow MA.
   - RSI crosses from above the overbought level (RSI > 70) to below the overbought level (RSI <= 70).
   - If the conditions are met and no short position is open, the bot opens a sell order.

3. **Trailing Stop**:
   - If enabled, a trailing stop is applied to positions once the market moves in favor of the position by a defined number of pips. The trailing stop moves with the market to lock in profits.

## Installation & Usage
1. Open cTrader and go to the **Automate** tab.
2. Create a new cBot and paste the code from this repository into the editor.
3. Compile the cBot and apply it to a chart.
4. Adjust the parameters as needed through the settings in cTrader.
5. Start the bot and monitor its performance.

## Risk Warning
Trading in the financial markets carries a risk of loss. Always use appropriate risk management techniques and test the bot in a demo account before applying it to live trading.

## License
This bot is provided under the MIT License. Feel free to modify and use it at your own risk.

## Author
- **Created by**: Aouaiti Ahmed
- **Email**: mr.aouaiti.ahmed@gmail.com
