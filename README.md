scriptcs-net
============

# Net Script Pack

## What is it?
A script pack that can be used to create TCP servers. This was originally created for demo purposes, but I will probably continue working on it to make it more usable. Any contributions/feedback about features to implement or API improvements are welcome.

## Usage
Install the [nuget package](https://nuget.org/packages/ScriptCs.Net/0.2) by running `scriptcs -install ScriptCs.Net`.

### Creating a TCP Server
```csharp
var net = Require<Net>();

var server = net.CreateServer(socket =>
{
    Console.WriteLine("New connection");

    socket.On(
        data: bytes => Console.Write(bytes.AsString()),
        close: () => Console.WriteLine("Connection closed"),
        error: e => Console.WriteLine("Error: {0}\r\nStackTrace: {1}", e.Message, e.StackTrace));
});

Console.WriteLine("Listening at 127.0.0.1:8080");

server.Listen(8080, "127.0.0.1").Wait();

Console.WriteLine("Closing server");

server.Close();
```

### Creating a TCP Client
```csharp
var net = Require<Net>();

var client = net.Connect(8080, "127.0.0.1", onConnect: () => Console.WriteLine("Connected to chat room at 127.0.0.1:8080"));

client.On(
        data: data => Console.WriteLine(data.AsString()),
        close: () => Console.WriteLine("Connection closed"),
        error: e => Console.WriteLine("Error: {0} \r\nStackTrace: {1}", e.Message, e.StackTrace));

var line = Console.ReadLine();

while (line != ":close"){
	client.WriteAsync(line.AsBytes());
	line = Console.ReadLine();
}

// still have to troubleshoot an issue when closing the socket
client.Close();
```

## What's next
* Add more features, such as error handling.
* Listen to community feedback.