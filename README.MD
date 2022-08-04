# CycliusAlarm

A cyclius switching alarm for a game named cookie clicker.

[![996.icu](https://img.shields.io/badge/link-996.icu-red.svg)](https://996.icu)
[![LICENSE](https://img.shields.io/badge/license-Anti%20996-blue.svg)](https://github.com/996icu/996.ICU/blob/master/LICENSE)

---

### How to use

Download from Github Releases and run it then do nothing and dont close the window.

### Compile

##### Install .Net Core 6 SDK and run above command

```
$ dotnet publish -c Release
```

### Known problems

##### Result inaccurate caused by my time calculating algorithm is not accurate enough
```csharp
private float GetCurrentTimeInHours()
{
    return (float) Math.Round(DateTime.Now.Hour + (double) DateTime.Now.Minute / 60 + (double) DateTime.Now.Second / 3600, 2);
}
```