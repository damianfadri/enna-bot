# Enna Bot
## Overview
Enna Bot is a streamer notifier for Discord. It sends a customizable message to the channel whenever a specific streamer goes live.

## Prerequisites
* .NET 8
* SQL Server
* Discord Application (see: [Discord Developer Portal](https://discord.com/developers/applications/))

## Running the Project
1. Clone this project locally.
2. Restore dependencies.
```bash
dotnet restore
```

3. Set the following environment variables:

* Discord Bot Token (see: [Creating a Discord Bot](https://discord.com/developers/docs/getting-started#step-1-creating-an-app))

```bash
$env:BotOptions__Token = "OTQ0...DiscordBotToken"
```

* SQL Server Connection String
```bash
$env:ConnectionStrings__EnnaDatabase = "ConnectionStringTo=YourDatabase;"
```

4. Apply the database schema
```bash
dotnet ef database update 
    --project src/Enna.Bot.Infrastructure.Mssql/Enna.Bot.Infrastructure.Mssql.csproj 
    --startup-project src/Enna.Bot/Enna.Bot.csproj 
    --context StreamerContext

dotnet ef database update 
    --project src/Enna.Bot.Infrastructure.Mssql/Enna.Bot.Infrastructure.Mssql.csproj 
    --startup-project ./src/Enna.Bot/Enna.Bot.csproj 
    --context TenantContext
```

5. Run the project
```bash
dotnet run --project src/Enna.Bot
```

## Commands
### Add Streamer
```bash
/add-streamer <name> <link> [channel] [template]
```
Adds a streamer to the streamer list. 

Parameters:
| Name | Description | Required |
| --- | --- | --- |
| `name` | Friendly name for the streamer being added | Yes |
| `link` | Channel link of the streamer. It can be a YouTube link or a Twitch link. | Yes |
| `channel` | Discord text channel within the current Discord server. Defaults to the current text channel where this command is invoked. | No |
| `template` | The message to be sent when the streamer goes live. For more info on constructing the template, see [Constructing the Message Template](#constructing-the-message-template). | No |

### List Streamers
```bash
/list-streamers
```
Lists all streamers in the streamer list. General information regarding the streamer is returned such as the name, channel link, and its unique id.

### Remove Streamer
```bash
/remove-streamer <id>
```
Removes a streamer from the streamer list.

Parameters:
| Name | Description | Required |
| --- | --- | --- |
| `id` | Unique ID of the streamer. | Yes |

## Constructing the Message Template
You can set a custom message for the message that will be sent when a streamer goes live. Anything goes as long as the command parameter field accepts it.

There are also some reserved substrings that are replaced to a specific value before the message is sent to the text channel.
| Substring | Replaced with |
| --- | --- |
| `{link}` | The link to the live stream. Template is set to this by default when left as blank. | 
| `\n` | A newline character. Use this in case you want a multiline message to be sent.








