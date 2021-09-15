#include <set>
#include <vector>
#include <filesystem>
#include <iostream>
#include <fstream>
#include <string>
#include <unistd.h>
#include <sys/inotify.h>
#include <stdio.h>
#include <stdlib.h>
#include <signal.h>
#include <fcntl.h>
#include <dirent.h>
#include "IOConfig/IOConfig.h"
#include "ModuleManager/ModuleManager.h"

#define EVENT_SIZE (sizeof(struct inotify_event))
#define BUF_LEN (1024 * (EVENT_SIZE + 16))

namespace it = std::filesystem;

const static std::string logsFilePath = "/etc/identify_lab_data/logs_dir/main_logs";
static IOConfig ioConfig;
static ModuleManager* moduleManager = nullptr;
static int fd;
static std::set<std::string> fileNames;
static std::vector<int> wd;
static std::vector<string> scanFolderPaths,fullFileNames,prefixFileNames,suffixFileNames;

void continueExecutionV();
int continueExecutionI();
void endExecution(int status);
void processNewlyCreatedLabFiles(std::string labFilesFolderPath);
bool isFileNameAcceptable(std::string fileName);
bool hasEnding(std::string const &fullString, std::string const &ending);

void checkAlreadyCreatedFiles();
void resume_registration();
void sig_handler(int);
void showError();
void init();
void cleanup();

void continueExecutionV(){}
int continueExecutionI(){return 0;}
void endExecution(int status){
    cleanup();
    exit(status);
}

int main(void) {
    init();
    
    signal(SIGINT, sig_handler);
    /* Step 1. Initialize inotify */
    fd = inotify_init();

    if (fcntl(fd, F_SETFL, O_NONBLOCK) < 0) // error checking for fcntl
        exit(2);

    /* Step 2. Add Watch */
    for(std::vector<string>::iterator it = scanFolderPaths.begin(); it != scanFolderPaths.end(); ++it) {
        wd.push_back(inotify_add_watch(fd,(*it).c_str(), IN_MODIFY));
        if(wd[wd.size()-1] == -1 && !ioConfig.doesFileExist("/home/stud/Desktop/system_ERROR")){
            showError();
        } 
    }

    resume_registration();
    // Do below instructions if content of directory has changed.
    while (true) {
        int i = 0, length;
        char buffer[BUF_LEN];

        /* Step 3. Read buffer*/
        length = read(fd, buffer, BUF_LEN);
        /* Step 4. Process the events which has occurred */
        while (i < length) {
            struct inotify_event *event = (struct inotify_event *)&buffer[i];
            if (event->len) {
                if (event->mask & IN_MODIFY) {
                    if (event->mask & IN_ISDIR) {
                    } else {
                        // dodano nowy plik do folderu z plikami lab.    
                        processNewlyCreatedLabFiles(scanFolderPaths[(event->wd)-1]);
                    }
                }
            }
            i += EVENT_SIZE + event->len;
        }
        sleep(2);
    }
    cleanup();
    sig_handler(-1);
    return -1;
}

void sig_handler(int status) {
    /* Step 5. Remove the watch descriptor and close the inotify instance*/
    for(std::vector<int>::iterator it = wd.begin(); it != wd.end(); ++it) {
        inotify_rm_watch(fd,*it);
    }
    close(fd);
}

void processNewlyCreatedLabFiles(std::string labFilesFolderPath) {
    std::string fileName,fileSize;
    std::ofstream outputLogsFile;
    outputLogsFile.open(logsFilePath,std::ios::app);

    if(outputLogsFile.is_open()){
        for (const auto &entry : it::directory_iterator(labFilesFolderPath)){
            fileName = entry.path().string();
            fileSize = ioConfig.get_file_size(fileName.c_str());
            fileName = fileName.substr(fileName.find_last_of("/")+1,fileName.size());
            if(ioConfig.areCredentialsPresent()){
                if (fileNames.find(fileName) == fileNames.end() && isFileNameAcceptable(fileName)){
                    fileNames.insert(fileName);
                    std::string registryContent = "Plik [" + fileName + "] zostal stworzony. Rozmiar pliku: " + fileSize;
                    outputLogsFile << registryContent << std::endl;                 
                }
            }else{
                if(!ioConfig.doesFileExist("/home/stud/Desktop/system_ERROR"))
                    showError();
            }
        }
        outputLogsFile.close();
    }
}

bool isFileNameAcceptable(std::string fileName){
    for(std::vector<string>::iterator it = fullFileNames.begin(); it != fullFileNames.end();++it) {
        if(fileName.compare(*it)==0)
            return true;  
    }
    for(std::vector<string>::iterator it = prefixFileNames.begin(); it != prefixFileNames.end();++it) {
        if(fileName.rfind(*it,0) == 0)
            return true;
    }       
    for(std::vector<string>::iterator it = suffixFileNames.begin(); it != suffixFileNames.end();++it) {
        if(hasEnding(fileName,*it))
            return true;
    } 
    return false;
}

bool hasEnding(std::string const &fullString,std::string const &ending){
    if (fullString.length() >= ending.length()) {
        return (0 == fullString.compare(fullString.length() - ending.length(), ending.length(), ending));
    }
    return false;
}

void checkAlreadyCreatedFiles(){
    std::string fileName;
    for(std::vector<string>::iterator it = scanFolderPaths.begin(); it != scanFolderPaths.end(); ++it) {
        for (const auto & file : it::directory_iterator(*it)){
            fileName = file.path().string();
            fileName = fileName.substr(fileName.find_last_of("/")+1,fileName.size());
            fileNames.insert(fileName);
        }
    }
}

void init(){
    moduleManager = new ModuleManager();
    scanFolderPaths = moduleManager->getScanFolderPaths();
    fullFileNames = moduleManager->getFullFileNames();
    prefixFileNames = moduleManager->getPrefixFileNames();
    suffixFileNames = moduleManager->getSuffixFileNames();
    checkAlreadyCreatedFiles();
}

void resume_registration(){
    (ioConfig.getProcIdByName("IdentifyOnStart") != -1 && ioConfig.getProcIdByName("MainService") != -1) ?
        kill(ioConfig.getProcIdByName("IdentifyOnStart"),SIGCONT) : continueExecutionI();
}

void cleanup(){
    if(moduleManager != nullptr){
        delete moduleManager;
        moduleManager = nullptr;
    }
}

void showError(){
    system("notify-send -u critical [\"STATUS: ERROR\"] \"Maszyna nie jest prawidlowo skonfigurowana.\nNie korzystaj z tej maszyny.\"");
    std::ofstream outfile("/home/stud/Desktop/system_ERROR");
    outfile << "Maszyna nie zostala prawidlowo skonfigurowana. Nie korzystaj z tej maszyny." << std::endl;
    outfile.close();
}