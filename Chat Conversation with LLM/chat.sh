#!/bin/bash

HISTORY_FILE="conversation.txt"

# Append user message to history
if [[ "$1" == "" ]]; then
    echo "$1" >> $HISTORY_FILE
else
    echo "User: $1" >> $HISTORY_FILE
fi

# Run the model and capture output
RESPONSE=$(docker model run ai/gemma3 $(cat $HISTORY_FILE))

# Append model response to history
# if [[ "${RESPONSE:0:9}" == "Assistant" ]]; then
if [[ "$RESPONSE" == Agent* ]]; then
    echo "$RESPONSE" >> $HISTORY_FILE
else
    echo "Agent: $RESPONSE" >> $HISTORY_FILE
fi

# Show model response
echo "$RESPONSE"
