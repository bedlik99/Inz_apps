#!/bin/bash
java -jar /home/stud/Public/server_run/localServer_example.jar </dev/null &>/dev/null &
if [ -z `pidof IdentifyOnStart` ]; then
     /home/cerber/Documents/lab_supervision/identify_machine_program/./IdentifyOnStart </dev/null &>/dev/null &
fi
