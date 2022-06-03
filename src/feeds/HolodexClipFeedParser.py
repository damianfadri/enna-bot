import logging
from aiohttp import ClientSession
from feeds import FeedEntry, FeedParser

HOLODEX_CLIP_API_URL = 'https://holodex.net/api/v2/channels/{0}/clips'

class HolodexClipFeedParser(FeedParser):
    def __init__(self, channel_id, api_key):
        self.params = {
            'limit': 1,
            'lang': 'en'
        }

        self.headers = {
            'X-APIKEY': api_key
        }

        self.url = HOLODEX_CLIP_API_URL.format(channel_id)
    
    async def parse(self) -> FeedEntry:
        async with ClientSession() as session:
            async with session.get(self.url, params=self.params, headers=self.headers) as resp:
                clips = await resp.json()

                if not len(clips):
                    return None

                clip = clips[0]
                video_id = clip['id']

                logging.info('Found clip video ID: {0}'.format(video_id))
                return FeedEntry(clip, video_id)
        
        return None
                
