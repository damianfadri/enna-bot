import logging
from feeds import FeedParser
from watcher import Watcher, WatcherArgs

NOTIFY_EVENT = 'on_notify'
IDLE_EVENT = 'on_idle'

class FeedWatcher(Watcher):
    def __init__(self, args: WatcherArgs, parser: FeedParser):
        super(FeedWatcher, self).__init__(args)

        self.__parser = parser
        self.__prev_entry = None

    async def execute(self):
        curr_entry = await self.__parser.parse()
        if curr_entry == None:
            await self.notify(IDLE_EVENT, None)
            return

        logging.info('Comparing {0} to {1}'.format(curr_entry, self.__prev_entry))
        if self.__prev_entry != curr_entry:
            self.__prev_entry = curr_entry
            await self.notify(NOTIFY_EVENT, curr_entry)