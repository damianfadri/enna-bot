import logging
from aiohttp import ClientSession
from feeds import FeedEntry, FeedParser

HOLODEX_LIVE_API_URL = 'https://holodex.net/api/v2/live'

class HolodexLiveFeedParser(FeedParser):
    def __init__(self, channel_id, api_key):
        self.params = { 
            'channel_id': channel_id, 
            'status': 'live' 
        }

        self.headers = {
            'X-APIKEY': api_key
        }

        self.url = HOLODEX_LIVE_API_URL

    async def parse(self) -> FeedEntry:
        async with ClientSession() as session:
            async with session.get(self.url, params=self.params, headers=self.headers) as resp:
                live_streams = await resp.json()

                if not len(live_streams) > 0:
                    return None

                live_stream = live_streams[0]
                video_id = live_stream['id']

                logging.info('Found livestream video ID: {0}'.format(video_id))
                return FeedEntry(live_stream, video_id)
        
        return None