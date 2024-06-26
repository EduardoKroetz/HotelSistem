#!/bin/bash
# Create the directory for the application if it doesn't exist
mkdir -p /var/www/api

# Copy the service file to the systemd directory
cp /var/www/hotelsistem/scripts/hotelsistem.service /etc/systemd/system/hotelsistem.service

# Reload systemd to recognize the new service
systemctl daemon-reload

# Enable the service to start on boot
systemctl enable hotelsistem.service
