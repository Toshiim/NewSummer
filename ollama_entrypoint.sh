#!/bin/sh
ollama serve &

until ollama list > /dev/null 2>&1; do
  echo "Waiting for Ollama server..."
  sleep 1
done

ollama pull qwen3.5:4b
wait