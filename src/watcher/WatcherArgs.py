DEFAULT_DELAY_MS = 5000
DEFAULT_NAME = "DefaultWatcher"

class WatcherArgs():
    def __init__(self, name = DEFAULT_NAME, loop = None, delay_ms = DEFAULT_DELAY_MS):
        self.loop = loop
        self.name = name
        self.delay_ms = delay_ms