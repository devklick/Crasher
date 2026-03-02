<h1 align="center">
    Crasher
</h1>

<p align="center">
    A simple crash game built using <a href='https://spacetimedb.com/'>SpacetimeDB</a>.
</p>


## Running locally

Because this game uses SpacetimeDB, it can be run entirely locally without having 
to actually provision or configure any services. However, before starting, a few 
things need to be available on your system.

### Pre-requisites 

- [SpacetimeDB CLI](https://spacetimedb.com/install)
- [.Net 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [wasi-sdk](https://github.com/WebAssembly/wasi-sdk)
- [wasi-experimental .Net workload](https://devblogs.microsoft.com/dotnet/extending-web-assembly-to-the-cloud/)

### SpacetimeDB Login

Even though you may only be running the game locally, you will still need to log
in using your spacetime cli. 

```
spacetime login
```

### Start local SpacetimeDB server

In a separate console, you can start the local development spacetime server. 
While the server is running, this console will display various logs.

```
spacetime start
```

### Publish Server

Now, in another console, we can publish the server (which contains the database, functions etc) 
to the local development server. 

```
spacetime publish --server local --module-path server/spacetimedb crasher
```
> [!NOTE]  
> We specify `--module-path server/spacetimedb` here to tell the spacetime CLI
> where the project which contains the backend can be found.
>
> We specify `crasher` here, which is the name of the database.

After a few seconds, this should run to completion and the backend will be successfully 
published. 

Note that as soon as the backend is published, it is initialized, and the core game loop starts.
See the [Init reducer](./server/spacetimedb/Reducers/Module.Init.cs) for more.

### Check Logs

Since the core game logic is automatically started when the backend is published, 
we can check the server logs to see what's going on.

```
spacetime logs --server local crasher --follow
```
> [!NOTE]  
> We specify `crasher` here, which is the name of the database.
>
> We specify the `--follow` argument to keep the terminal connected so we see 
> realtime logs.

Here, you should see that the game:

- Begins a countdown
- Starts incrementing the multiplier once the countdown reaches 0
- Crashes when it reaches a random multiplier
- Schedules the next game
- Repeats infinitely 

### Run the UI

A very basic UI has been created which simply connects to the backend and displays
information about the current round. You can run it with:

```
npm run --ws dev
```
Once the UI is running, you can browse to the URL that it's hosted on
>http://localhost:5173/

Here, you can see basically the same information that was shown in the logs.

## To do

- Reduce server ticks during countdown + ended (only need fast ticks while in-play / multiplier increasing)
- Player auth
- Place bet
- Cancel bet
- Cash out
- View historic rounds
- View bet history
