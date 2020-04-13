---
name: Tech Spezifikation
about: Beschreibt das technische Umsetzten von Feature Issues
title: 'Tech: <Beschreibung>'
labels: 'tech spec'
assignees: ''
---

## Feature Issues
- Issue1
- Issue2

// Die jeweiligen Komponeneten die nicht benötigt werden herrauslöschen.
// Generell überall wo es geht, ein bis zwei Sätze beschreibenden Text.

## Client
### Statisches Modell
### Dynamisches Modell

## Communikation
### Api Schnittstelle

```
# get
http://game.mudhub.de/api/rooms
# post
http://game.mudhub.de/api/rooms
```

### SignalR


Client => Server
```csharp
public interface IServer {
  Task SendMessage(string msg);
}
```


Server => Client
```csharp
public interface IClient {
  Task ReceiveMessage(string msg, string name);  
}
```

## Sever
### Statisches Modell
### Dynamisches Modell

## Offene Fragen
-[ ] Beschreibe wo es noch offene Punkte zum Diskutieren gibt
-[ ] Welche Fragen noch zu klären sind
