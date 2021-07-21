#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <sys/ptrace.h>
#include <iostream>
#include <set>
#include <filesystem>
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
static long long int currentLogLineNumber=0;
static IOConfig ioConfig;
static RestServerConnector* restServerConnector = new RestServerConnector();
static OpenSSLAesEncryptor* openSSLCryptoUtil = new OpenSSLAesEncryptor();
static std::set<std::string> filePaths;
static int filePathsInitSize;
static int fd,wd1,wd2;

void continueExecution();
void endExecution(int status);
void processNewlyCreatedLabFiles();
//void readContentOfFile(std::string);
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
    findLastLineInLogFile();

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
        filePathsInitSize = filePaths.size();
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

void findLastLineInLogFile(){
    std::string tmpStr;
    std::ifstream logsFile(logsFilePath);
    while (std::getline(logsFile, tmpStr)) {
      currentLogLineNumber++;
    }  
    std::cout << "\nCount init lines in a log file: " << currentLogLineNumber << "\n";
}

void processNewLogRecords(){
    std::string tmpStr;
    long long int it=1;
    std::ifstream logsFile(logsFilePath);
    while (std::getline(logsFile, tmpStr)) {
        if(it>currentLogLineNumber){
            //std::cout << "Linia nr: " << it << " " << tmpStr << " " << std::endl;
            std::string decryptedRegistryContent = openSSLCryptoUtil->decryptAES256WithOpenSSL(tmpStr);
            std::string decryptedIndexNr = openSSLCryptoUtil->decryptAES256WithOpenSSL(readEncryptedIndexNrFromFile());
            restServerConnector->sendData(decryptedIndexNr,decryptedRegistryContent);
        }
        it++;
    }  
    currentLogLineNumber = --it;
}

std::string readEncryptedIndexNrFromFile(){
    std::string idxStr;
    std::ifstream encIndexFile(registrationFilePath);
    std::getline(encIndexFile,idxStr);
    return idxStr;
}

void init(){
    if(!ioConfig.doesFileExist(registrationFilePath)){
        system("notify-send -u critical -t 600000 [\"STATUS: ERROR - CREDENTIALS NOT SENT\"] \"Machine was not initialized. \nDo relog and send credentials again. \nIf error occurs download different linux image.\"");
        cleanup();
        exit(-1);
    }else if(ioConfig.isSecretFileEmpty(registrationFilePath)){
        system("notify-send -u critical -t 600000 [\"STATUS: ERROR - CREDENTIALS NOT SENT\"] \"Machine was not initialized. \nDo relog and send credentials again. \nIf error occurs download different linux image.\"");
        cleanup();
        exit(-1);
    }
    system("notify-send -u normal -t 60000 [\"STATUS: OK\"] \"\nMachine is ready to be used.\"");
}

void cleanup(){
    delete restServerConnector;
    delete openSSLCryptoUtil;
}

void processNewlyCreatedLabFiles() {
    std::string fileName;
    std::string newFileNameToBeSent;
    for (const auto &entry : fs::directory_iterator(labFilesFolderPath)) {
        fileName = entry.path().string();
        fileName = fileName.substr(fileName.find_last_of("/")+1,fileName.size());

        if (entry.is_regular_file()) {
            filePaths.insert(fileName);
            if(filePaths.size() != filePathsInitSize){
                newFileNameToBeSent = fileName;
                std::string decryptedRegistryContent = "Plik [" + newFileNameToBeSent + "] zostal stworzony w folderze laboratoryjnym";
                std::string decryptedIndexNr = openSSLCryptoUtil->decryptAES256WithOpenSSL(readEncryptedIndexNrFromFile());
                restServerConnector->sendData(decryptedIndexNr,decryptedRegistryContent);

                filePathsInitSize++;
            }
        }
    }
}

// void readContentOfFile(std::string pathOfFile) {
//     std::ifstream tmpFileStream(pathOfFile);
//     std::string fileContents((std::istreambuf_iterator<char>(tmpFileStream)),
//                              std::istreambuf_iterator<char>());
//     std::cout << fileContents << std::endl;
//     // NEXT PARSE CONTENTS OF THE FILE TO e.g. JSON FORMAT AND SEND IT ON SERVER
//                             processNewlyCreatedLabFiles();
//                         for (auto i = filePaths.begin(); i != filePaths.end(); i++) {
//                             readContentOfFile(*i);
//                         }
// }