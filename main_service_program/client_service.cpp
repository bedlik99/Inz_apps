#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <sys/ptrace.h>
#include <iostream>
#include <set>
#include <filesystem>
#include <algorithm>
#include <stdio.h>
#include <sys/inotify.h>
#include <unistd.h>
#include <stdlib.h>
#include <signal.h>
#include <fcntl.h>          
#include "IOConfig/IOConfig.h"
#include "RESTConnector/RestServerConnector.h"

#define EVENT_SIZE (sizeof(struct inotify_event))
#define BUF_LEN (1024 * (EVENT_SIZE + 16))

namespace fs = std::filesystem;

const static std::string logsFolderPath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/output_files";
const std::string registrationFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/rgstr_trace/rqrm";
const static std::string logsFilePath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/output_files/main_logs";
const static std::string labFilesFolderPath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/output_files/lab_files";
const char* errorNotifyMsg = "notify-send -u critical -t 600000 [\"STATUS: ERROR - CREDENTIALS NOT SENT\"] \"Machine was not initialized. \nDo relog and send credentials again. \nIf error occurs download different linux image.\"";
const char* successNotifyMsg = "notify-send -u normal -t 30000 [\"STATUS: OK\"] \"\nMachine is ready to be used.\"";
static long long int currentLogLineNumber=0;
static IOConfig ioConfig;
static RestServerConnector* restServerConnector = nullptr;
static OpenSSLAesEncryptor* openSSLCryptoUtil = nullptr;
static std::set<std::string> fileNames;
static int fd,wd1,wd2;

void continueExecution();
void endExecution(int status);
void processNewlyCreatedLabFiles();
void sig_handler(int);
void processNewLogRecords();
void findLastLineInLogFile();
std::string readEncryptedIndexNrFromFile();
void init();
void cleanup();

void continueExecution(){}
void endExecution(int status){
    cleanup();
    kill(getpid(), SIGTERM);
    exit(status);
}

int main(void) {
    ptrace(PTRACE_TRACEME, 0, 0, 0) < 0 ? endExecution(0) : continueExecution();
    init();

    signal(SIGINT, sig_handler);
    /* Step 1. Initialize inotify */
    fd = inotify_init();

    if (fcntl(fd, F_SETFL, O_NONBLOCK) < 0) // error checking for fcntl
        exit(2);

    /* Step 2. Add Watch */
    wd1 = inotify_add_watch(fd, logsFolderPath.c_str(), IN_MODIFY);
    wd2 = inotify_add_watch(fd, labFilesFolderPath.c_str(), IN_MODIFY);

    if (wd1 == -1 || wd2 == -1){
        exit(-1);
    } 

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
                        if(wd1 == event->wd){
                            // zmodyfikowano plik z logami
                            processNewLogRecords();                         
                        }else if(wd2 == event->wd){
                            // dodano nowy plik do folderu z plikami lab.    
                            processNewlyCreatedLabFiles();
                        }
                    }
                }
            }
            i += EVENT_SIZE + event->len;
        }

    }
    cleanup();
    sig_handler(-1);
    return -1;
}

void sig_handler(int status) {
    /* Step 5. Remove the watch descriptor and close the inotify instance*/
    inotify_rm_watch(fd, wd1);
    inotify_rm_watch(fd, wd2);
    close(fd);
}

void findLastLineInLogFile() {
    std::string tmpStr;
    std::ifstream logsFile(logsFilePath);
    while (std::getline(logsFile, tmpStr)) {
      currentLogLineNumber++;
    }  
    logsFile.close();
}

void processNewLogRecords() {
    std::string tmpStr;
    long long int it=1;
    std::ifstream logsFile(logsFilePath);
    while (std::getline(logsFile, tmpStr)) {
        if(it>currentLogLineNumber && tmpStr != "\n" && !tmpStr.empty()){
            //std::cout << "\nLinia nr: " << it << " " << tmpStr << std::endl;
            std::string decryptedRegistryContent = openSSLCryptoUtil->decryptAES256WithOpenSSL(tmpStr);
            std::string decryptedIndexNr = openSSLCryptoUtil->decryptAES256WithOpenSSL(readEncryptedIndexNrFromFile());
            restServerConnector->sendData(decryptedIndexNr,decryptedRegistryContent);
        }
        it++;
    }  
    logsFile.close();
    currentLogLineNumber = --it;
}

std::string readEncryptedIndexNrFromFile(){
    std::string idxStr;
    std::ifstream encIndexFile(registrationFilePath);
    std::getline(encIndexFile,idxStr);
    encIndexFile.close();
    return idxStr;
}

void processNewlyCreatedLabFiles() {
    std::string fileName;
    std::ofstream outputLogsFile;
    outputLogsFile.open(logsFilePath,std::ios::app);

    if(outputLogsFile.is_open()){
        for (const auto &entry : fs::directory_iterator(labFilesFolderPath)) {
            fileName = entry.path().string();
            fileName = fileName.substr(fileName.find_last_of("/")+1,fileName.size());
            if (entry.is_regular_file() && outputLogsFile.is_open()  && fileNames.find(fileName) == fileNames.end()) {
                fileNames.insert(fileName);
                std::string registryContent = "Plik [" + fileName + "] zostal stworzony w folderze laboratoryjnym";
                std::string encryptedRegistryContent = openSSLCryptoUtil->encryptAES256WithOpenSSL(registryContent);
                outputLogsFile << encryptedRegistryContent << std::endl;
            }
        }
        outputLogsFile.close();
    }
}

void checkAlreadyCreatedFilesOnStart(){
    std::string fileName;
    for (const auto & file : std::filesystem::directory_iterator(labFilesFolderPath)){
        fileName = file.path().string();
        fileName = fileName.substr(fileName.find_last_of("/")+1,fileName.size());
        fileNames.insert(fileName);
    }
}

void init(){
    restServerConnector = new RestServerConnector();
    openSSLCryptoUtil = new OpenSSLAesEncryptor();
    if(!ioConfig.doesFileExist(registrationFilePath)){
        system(errorNotifyMsg);
        cleanup();
        exit(-1);
    }else if(ioConfig.isSecretFileEmpty(registrationFilePath)){
        system(errorNotifyMsg);
        cleanup();
        exit(-1);
    }
    findLastLineInLogFile();
    checkAlreadyCreatedFilesOnStart();
    system(successNotifyMsg);
}

void cleanup(){
    if(restServerConnector != nullptr){
        delete restServerConnector;
        restServerConnector = nullptr;
    }
    if(openSSLCryptoUtil != nullptr){
        delete openSSLCryptoUtil;
        openSSLCryptoUtil = nullptr;
    }
}

