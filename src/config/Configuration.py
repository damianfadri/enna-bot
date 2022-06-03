import logging
from os import environ

class Configuration():
    def __init__(self):
        env = environ.get('ENV')

        level = logging.WARNING
        if env == 'develop':
            level = logging.INFO

        logging.basicConfig(level=level)
        logging.info('Environment is {0}'.format(env))

        self.bot_token = environ.get('DISCORD_BOT_TOKEN')
        self.api_key = environ.get('HOLODEX_API_KEY')
        self.yt_channel_id = environ.get('YT_CHANNEL_ID')
        self.discord_channel_id = int(environ.get('DISCORD_CHANNEL_ID'))
        self.live_delay_ms = int(environ.get('QUERY_LIVE_DELAY_MS'))
        self.clip_delay_ms = int(environ.get('QUERY_CLIP_DELAY_MS'))
        self.search_history_limit = int(environ.get('SEARCH_HISTORY_LIMIT'))

        logging.info('DISCORD_BOT_TOKEN={0}'.format(self.bot_token))
        logging.info('HOLODEX_API_KEY={0}'.format(self.api_key))
        logging.info('YT_CHANNEL_ID={0}'.format(self.yt_channel_id))
        logging.info('DISCORD_CHANNEL_ID={0}'.format(self.discord_channel_id))
        logging.info('LIVE_DELAY_MS={0}'.format(self.live_delay_ms))
        logging.info('CLIP_DELAY_MS={0}'.format(self.clip_delay_ms))
        logging.info('SEARCH_HISTORY_LIMIT={0}'.format(self.search_history_limit))