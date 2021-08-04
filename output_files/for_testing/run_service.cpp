#include <stdlib.h>
#include <iostream>
#include <unistd.h>
#include <sys/types.h>
#include <dirent.h>
#include <errno.h>
#include <vector>
#include <string>
#include <fstream>
#include <stdio.h>
//---
using namespace std;
//---
int getProcIdByName(string procName);

//Aby zdetachowac proces(dodaj na koniec komendy): </dev/null &>/dev/null &   
int main(void){
    while(true){
        sleep(10);
        if(getProcIdByName("HttpRestClient") == -1){
            if(getProcIdByName("IdentifyOnStartup") == -1){
                cout << "Run MainService" << endl;
                system("~/Documents/inz_dyp/working_folder_inz/main_service_program/HttpRestClient");
            }else{
                cout << "Kill Identify" << endl;
                system("killall -9 IdentifyOnStartup");
            }
        }
        cout << "Loop" << endl;   
    }
    return -1;
}

int getProcIdByName(string procName){
    int pid = -1;
    // Open the /proc directory
    DIR *dp = opendir("/proc");
    if (dp != NULL){
        // Enumerate all entries in directory until process found
        struct dirent *dirp;
        while (pid < 0 && (dirp = readdir(dp))){
            // Skip non-numeric entries
            int id = atoi(dirp->d_name);
            if (id > 0){
                // Read contents of virtual /proc/{pid}/cmdline file
                string cmdPath = string("/proc/") + dirp->d_name + "/cmdline";
                ifstream cmdFile(cmdPath.c_str());
                string cmdLine;
                getline(cmdFile, cmdLine);
                if (!cmdLine.empty()){
                    // Keep first cmdline item which contains the program path
                    size_t pos = cmdLine.find('\0');
                    if (pos != string::npos)
                        cmdLine = cmdLine.substr(0, pos);
                    // Keep program name only, removing the path
                    pos = cmdLine.rfind('/');
                    if (pos != string::npos)
                        cmdLine = cmdLine.substr(pos + 1);
                    // Compare against requested process name
                    if (procName == cmdLine)
                        pid = id;
                }
            }
        }
    }
    closedir(dp);
    return pid;
}