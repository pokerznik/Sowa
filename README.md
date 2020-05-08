# Sowa coding test
## Few notices before review

Hi guys,

I have chosen Order Books exercise which is hopefully solved good enough.
There are two projects 
* ZanP.OrderBooks (application in .NET Core),
* ZanP.OrderBooks.Tests (XUnit tests for application)

**ZanP.OrderBooks** starts from Program.cs, where are also two basic examples how to use it. Be aware of custom balance which is set for every cryptoexchange. It can be defined inside `OrderHandler` constructor, but it's not required. In that case random balances will be applied for every cryptoexchange.
If there will be multiple selling/buying orders made sequentially, you may need `orderHandler.ResetData()` method which reset all existing exchange ballances and set new ones.

**ZanP.OrderBooks.Tests** nothing special there to be honest. Please just keep in mind that actual prices in test data (data file in test project) for BTC are diferent from real ones. It was made for simplified testing. *e.g. 0.5 BTC price -> 100 EUR each, 0.2 BTC -> 200 EUR, 300 EUR, 400 EUR, 500 EUR each. Refer to data file inside `bin/Debug/netcoreapp3.0` for more details.*