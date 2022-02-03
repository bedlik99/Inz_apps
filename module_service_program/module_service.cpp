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

const static std::string thisProcessName = "ModuleService";
const static std::string logsFilePath = "/home/cerber/Documents/lab_supervision/identify_lab_data/logs_dir/main_logs";
const static std::string bashHistoryFilePath = "/home/stud/.bash_history";
static IOConfig ioConfig;
static ModuleManager* moduleManager = nullptr;
static int fd;
static long long int bashHistoryLines=0;
static std::vector<int> wd;
static std::vector<string> scanFolderPaths,fullFileNames,prefixFileNames,suffixFileNames;
static std::string currentTimestamp, lastValidTimestamp;
void continueExecutionV();
int continueExecutionI();
void endExecution(int status);
void processChangedLabFiles(std::string labFilesFolderPath,std::string eventFileName);
bool isFileNameAcceptable(std::string fileName);
bool hasEnding(std::string const &fullString, std::string const &ending);
void findLastLineInFile(std::string filePath,long long int &lines);
void processBashLogs(std::string filePath,long long int &lines,std::vector <std::string> &fileContents);
void processEventName(std::stringstream &eventNameStream,std::string &eventName);
bool hasChangeTimeElapsed();
int timeToSeconds(int hours,int minutes,int seconds);
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
    std::stringstream eventNameStream;
    std::string eventFileName;
    
    signal(SIGINT, sig_handler);
    /* Step 1. Initialize inotify */
    fd = inotify_init();
    if (fcntl(fd, F_SETFL, O_NONBLOCK) < 0) // error checking for fcntl
        exit(2);

    /* Step 2. Add Watch */
    for(std::vector<string>::iterator it = scanFolderPaths.begin(); it != scanFolderPaths.end(); ++it) {
        wd.push_back(inotify_add_watch(fd,(*it).c_str(), IN_MODIFY));
        if(wd[wd.size()-1] == -1){
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
                    if ( !(event->mask & IN_ISDIR) ) {
                        // dodano nowy plik do folderu z plikami lab.  
                        eventNameStream << event->name;
                        eventFileName = eventNameStream.str();
                        processEventName(eventNameStream,eventFileName);
                        eventFileName.empty() ? continueExecutionV() : processChangedLabFiles(scanFolderPaths[(event->wd)-1],eventFileName);
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

void processChangedLabFiles(std::string labFilesFolderPath,std::string eventFileName) {
    std::string fullFilePath,fileSize;
    std::vector <std::string> fileContents; 
    std::ofstream outputLogsFile;
    outputLogsFile.open(logsFilePath,std::ios::app);

    if(outputLogsFile.is_open()){
        for (const auto &entry : it::directory_iterator(labFilesFolderPath)){
            fullFilePath = labFilesFolderPath+"/"+eventFileName;
            fileSize = ioConfig.get_file_size(fullFilePath.c_str());

            if(ioConfig.areCredentialsPresent()){
                if (isFileNameAcceptable(eventFileName)){              
                    if(eventFileName.compare(".bash_history") == 0){
                        processBashLogs(bashHistoryFilePath,bashHistoryLines,fileContents);
                        for(std::vector<std::string>::iterator it = fileContents.begin(); it != fileContents.end(); ++it) {
                            outputLogsFile << "bash_command>" << *it << std::endl;   
                        }    
                    }else{
                        currentTimestamp = ioConfig.currentDateTime();
                        if(hasChangeTimeElapsed()){
                            lastValidTimestamp = currentTimestamp;
                            std::string registryContent = "Modyfikacja pliku: [" + eventFileName + "] - Rozmiar pliku: " + fileSize;
                            outputLogsFile << registryContent << std::endl;  
                        } 
                    }       
                    fileContents.clear();
                }
            }else{
                if(isFileNameAcceptable(eventFileName)){
                    showError();
                }
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

void findLastLineInFile(std::string filePath,long long int &lines) {
    std::string tmpStr;
    std::ifstream customFile(filePath);
    while (std::getline(customFile, tmpStr)) {
      lines++;
    }
    customFile.close();
}

void processBashLogs(std::string filePath,long long int &lines,std::vector <std::string> &fileContents) {
    std::string tmpStr;
    long long int it=1;
    std::ifstream file(filePath);
    while (std::getline(file, tmpStr)) {
        if(it>lines && !ioConfig.trim(tmpStr).empty()){
            fileContents.push_back(tmpStr);
        }
        it++;
    }  
    file.close();
    lines = --it;
}

void processEventName(std::stringstream &eventNameStream,std::string &eventFileName){
    eventNameStream.clear();
    eventNameStream.str(std::string());
    eventNameStream.clear();
    eventFileName = (eventFileName.substr(eventFileName.length()-4,4)==".swp") ? eventFileName : eventFileName;
}

bool hasChangeTimeElapsed(){
    if(lastValidTimestamp.empty()){
        return true;  
    }                          
    int currentTimestampInSeconds = timeToSeconds(std::stoi(currentTimestamp.substr(11,2)),std::stoi(currentTimestamp.substr(14,2)),std::stoi(currentTimestamp.substr(17,2)));
    int lastTimestampInSeconds = timeToSeconds(std::stoi(lastValidTimestamp.substr(11,2)),std::stoi(lastValidTimestamp.substr(14,2)),std::stoi(lastValidTimestamp.substr(17,2)));
    if(abs(currentTimestampInSeconds-lastTimestampInSeconds) >= 25){
        return true;
    }
    return false;
}

int timeToSeconds(int hours,int minutes,int seconds){
    return (hours*3600+minutes*60+seconds);
}

void init(){
    moduleManager = new ModuleManager();
    scanFolderPaths = moduleManager->getScanFolderPaths();
    fullFileNames = moduleManager->getFullFileNames();
    prefixFileNames = moduleManager->getPrefixFileNames();
    suffixFileNames = moduleManager->getSuffixFileNames();
    findLastLineInFile(bashHistoryFilePath,bashHistoryLines);
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
    std::ofstream outfile("/home/stud/Desktop/ERROR_"+ioConfig.currentDateTime());
    outfile << "Maszyna nie zostala prawidlowo skonfigurowana. Przed korzystaniem z maszyny musi zostac wykonana rejestracja.\n Zrestartuj maszyne i zarejestruj jeszcze raz. Jezeli rejestracja jest niemozliwa i kolejne pliki bledu pojawiaja sie na pulpicie - zmien obraz maszyny" << std::endl;
    outfile.close();
}