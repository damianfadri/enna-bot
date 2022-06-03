class FeedEntry():
    def __init__(self, data, key):
        self.data = data
        self.key = key

    def __eq__(self, obj):
        return isinstance(obj, FeedEntry) and self.key == obj.key

    def __ne__(self, obj):
        return not self == obj

    def __str__(self):
        return "{{ key: {0}, data: {1} }}".format(self.key, self.data)