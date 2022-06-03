from abc import ABC, abstractmethod
from feeds import FeedEntry

class FeedParser(ABC):
    def __init__(self):
        super().__init__()

    @abstractmethod
    async def parse(self) -> FeedEntry:
        pass