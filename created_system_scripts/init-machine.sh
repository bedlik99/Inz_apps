#!/bin/bash
LOGGED_AS=`whoami`
PIDS=`pgrep IdentifyOnStart`
RUN=1
if [ $LOGGED_AS = stud ];then
    if [ -z "$PIDS" ];then
        sudo /home/cerber/Documents/lab_supervision/identify_machine_program/./IdentifyOnStart </dev/null &>/dev/null &
    else
        for PID in $PIDS
        do
            STATUS=`ps -q $PID -o state --no-header`
            if [ $STATUS != Z ];then
                RUN=0
            fi
        done
        if [ $RUN = 1 ];then
            sudo /home/cerber/Documents/lab_supervision/identify_machine_program/./IdentifyOnStart </dev/null &>/dev/null &
        fi
    fi
fi
