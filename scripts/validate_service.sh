#!/bin/bash
# Check if the service is running
if systemctl status hotelsistem.service | grep -q "active (running)"; then
    echo "Service is running"
else
    echo "Service is not running"
    exit 1
fi
