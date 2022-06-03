import logging
import asyncio

from config import Configuration
from discord import Client, TextChannel, Streaming
from feeds import FeedWatcher, FeedEntry, HolodexLiveFeedParser, HolodexClipFeedParser
from watcher import WatcherArgs

if __name__ == '__main__':
    cfg = Configuration()
    evt_loop = asyncio.new_event_loop()

    client = Client(loop=evt_loop)

    live_feed_args = WatcherArgs(name='LiveFeedWatcher', loop=evt_loop, delay_ms=cfg.live_delay_ms)
    clip_feed_args = WatcherArgs(name='ClipFeedWatcher', loop=evt_loop, delay_ms=cfg.clip_delay_ms)

    live_feed = FeedWatcher(args=live_feed_args, parser=HolodexLiveFeedParser(cfg.yt_channel_id, cfg.api_key))
    clip_feed = FeedWatcher(args=clip_feed_args, parser=HolodexClipFeedParser(cfg.yt_channel_id, cfg.api_key))

    async def send_to_channel(channel, message):
        messages = await channel.history(limit=cfg.search_history_limit).flatten()
        if any([message in m.content for m in messages]):
            logging.info('Duplicate message. Not sending: {0}'.format(message))
        else:
            logging.info('Sending message: {0}'.format(message))
            await channel.send(message)

    @client.event
    async def on_ready():
        logging.info('Discord client connect.')
        channel = client.get_channel(cfg.discord_channel_id)

        if channel == None:
            logging.error('Channel {0} not found.'.format(cfg.discord_channel_id))
            return

        @live_feed.event
        async def on_notify(entry: FeedEntry):
            if entry == None:
                return

            video_url = 'https://www.youtube.com/watch?v={0}'.format(entry.key)
            message = 'ENNA STREAM LETS GOOOOOO\n{0}'.format(video_url)
            
            await send_to_channel(channel, message)

            activity = Streaming(name='YouTube', url=video_url)
            logging.info('Changing status to: {0}'.format(activity))
            await client.change_presence(activity=activity)

        @live_feed.event
        async def on_idle(entry: FeedEntry):
            await client.change_presence()
            
        @clip_feed.event
        async def on_notify(entry: FeedEntry):
            if entry == None:
               return

            video_url = 'https://www.youtube.com/watch?v={0}'.format(entry.key)
            message = video_url

            await send_to_channel(channel, message)
        
        live_feed.start()
        clip_feed.start()
    
    @client.event
    async def on_resumed():
        logging.info('Discord client resumed.')
        live_feed.start()
        clip_feed.start()

    @client.event
    async def on_disconnect():
        logging.info('Discord client disconnect.')
        live_feed.stop()
        clip_feed.stop()

    client.run(cfg.bot_token)
    live_feed.stop()
    clip_feed.stop()