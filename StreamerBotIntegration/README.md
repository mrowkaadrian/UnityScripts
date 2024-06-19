# Integrating Unity with Streamer.bot
## Instructions

I suppose you have Streamer.bot installed and launched. In Unity you may need to install `com.github-glitchenzo.nugetforunity`,
restart the editor and then using that plugin install NuGet package: `System.Text.Json`. For me, version
`8.0.3` is fine.

### In Streamer.bot: 
1. Create new *Command*
2. Create new *Action*, use *Command* you created as a trigger
3. In the *Sub-actions* create new, select Core -> Network -> UDP Broadcast
4. Select free port of your choice, 5501 is going to be the default
5. In the payload data put:
```
{ 
    "Event":"<Your event name>",
    "User":"%user%",
    "Message":"%rawInput%",
    "Amount":1
}
```
`%rawInput%` is going to catch the "arguments" of the Command you got from the chat, eg. `!join abcd efgh` 
is going to set the `rawInput` to `abcd efgh`

### In Unity Editor
1. Create a directory somewhere in your `Assets` directory and name it `StreamerBotIntegration`
2. Put the *.cs* files from this repo into created directory
3. Create empty GameObject somewhere in your scene
4. Assign `UDPEventManager.cs` script to that GameObject

Inside the script, you will need to modify:

```
private void RegisterEvents()
{
    _udpConsumer.RegisterEvent("<Your event name>", SomeFunction);
}

private void SomeFunction(UDPEventData requestData)
{
    GetComponent<YOUR_COMPONENT>().DO_STUFF(requestData);
}
```

In your components you can use all the data you get from Streamer.bot UDP request:
`eventData.Event`, `eventData.User`, `eventData.Message` and `eventData.Amount`


