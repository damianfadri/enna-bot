FROM python:3.10-slim-bullseye
WORKDIR /enna-bot
COPY . .
RUN pip install -r requirements.txt
CMD ["python", "src/index.py"]