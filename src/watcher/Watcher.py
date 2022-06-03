import logging
import asyncio
from typing import Coroutine
from abc import ABC, abstractmethod
from watcher import WatcherArgs

class Watcher(ABC):
    def __init__(self, args: WatcherArgs):
        super().__init__()

        self.__loop = args.loop
        self.__name = args.name
        self.__delay_ms = args.delay_ms

        self.__watching = False
        self.__listeners = {}
        self.__running_task = None

    @abstractmethod
    async def execute(self):
        pass

    async def __poll(self):
        logging.info('Polling start.')
        while self.__watching:
            try:
                await self.execute()
            except Exception as e:
                logging.error(e)
            finally:        
                await asyncio.sleep(self.__delay_ms / 1000)
        logging.info('Polling end.')

    async def notify(self, evt_name, evt_data):
        logging.info('{0}: Sending {1} event to listeners'.format(self.__name, evt_name))
        if evt_name in self.__listeners:
            await self.__listeners[evt_name](evt_data)   
    
    def __start_polling(self):
        if self.__running_task is None \
                or self.__running_task.done() \
                or self.__running_task.cancelled():
            self.__running_task = self.__loop.create_task(self.__poll())

    def start(self):
        logging.info('{0}: Starting watch.'.format(self.__name))
        self.__watching = True
        self.__start_polling()

    def event(self, coro: Coroutine) -> Coroutine:
        self.__listeners[coro.__name__] = coro
        return coro

    def stop(self):
        logging.info('{0}: Stopping watch.'.format(self.__name))
        self.__watching = False