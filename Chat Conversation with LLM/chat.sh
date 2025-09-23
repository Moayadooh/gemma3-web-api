#!/bin/bash

HISTORY_FILE="conversation.txt"

# Append user message to history
echo "User: $1" >> $HISTORY_FILE

# Run the model and capture output
RESPONSE=$(docker model run ai/gemma3 $(cat $HISTORY_FILE))

# Append model response to history
echo "Assistant: $RESPONSE" >> $HISTORY_FILE

# Show model response
echo "$RESPONSE"
