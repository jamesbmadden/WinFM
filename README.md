# WinFM

## Running/Building
To be able to compile and run the app yourself, [create an API account from last.fm](https://www.last.fm/api/account/create). Then, once you've recieved an API key, add it to ```ApiKey.cs``` so the app can use it to access the API:
```cs
namespace WinFM {
  class Api {
    static string Key = "ADD YOUR KEY HERE";
  }
}
```
After the API has been setup, the app can be run or built from Visual Studio.