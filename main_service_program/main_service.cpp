#include <cpprest/http_client.h>
#include <cpprest/filestream.h>
#include <filesystem>
#include <iostream>
#include <time.h>
#include <sys/inotify.h>    
#include <dirent.h>
#include "RESTConnector/IOConfig/IOConfig.h"
#include "RESTConnector/RestServerConnector.h"

#define EVENT_SIZE (sizeof(struct inotify_event))
#define BUF_LEN (1024 * (EVENT_SIZE + 16))

namespace fs = std::filesystem;

const static std::string thisProcessName = "MainService";
const static std::string logsFolderPath = "/home/cerber/Documents/lab_supervision/identify_lab_data/logs_dir";
const static std::string logsFilePath = logsFolderPath+"/main_logs";
static long long int lastLogLineNumber=0;
static bool isRegistration;
static IOConfig ioConfig;
static RestServerConnector* restServerConnector = nullptr;
static int fd,wd1;

void continueExecutionV();
int continueExecutionI();
void endExecution(int status);
void sig_handler(int);
void processNewLogRecords();
void findLastLineInLogFile();
void readCredentials(std::string &email,std::string &uniqueCode);
void clearCredentials(int httpReturnCode);
void resume_registration();
void init();
void cleanup();
void showError();

void continueExecutionV(){}
int continueExecutionI(){return 0;}
void endExecution(int status){
    cleanup();
    exit(status);
}

int main(void) {
    init();
    srand(time(NULL));

    signal(SIGINT, sig_handler);
    /* Step 1. Initialize inotify */
    fd = inotify_init();

    if (fcntl(fd, F_SETFL, O_NONBLOCK) < 0) // error checking for fcntl
        exit(2);

    /* Step 2. Add Watch */
    wd1 = inotify_add_watch(fd, logsFolderPath.c_str(), IN_MODIFY);
    
    if (wd1 == -1 && !ioConfig.doesFileExist("/home/stud/Desktop/system_ERROR")){
        showError();
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
                        if(wd1 == event->wd){
                            // zmodyfikowano plik z logami
                            processNewLogRecords();                    
                        }
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
    inotify_rm_watch(fd, wd1);
    close(fd);
}

void findLastLineInLogFile() {
    std::string tmpStr;
    std::ifstream logsFile(logsFilePath);
    while (std::getline(logsFile, tmpStr)) {
      lastLogLineNumber++;
    }  
    isRegistration = lastLogLineNumber > 0 ? false : true;
    logsFile.close();
}

void processNewLogRecords() {
    std::string tmpStr(""),email(""),uniqueCode("");
    long long int it=1;
    int httpReturnCode=0;
    std::ifstream logsFile(logsFilePath);
    while (std::getline(logsFile, tmpStr)) {
        if(it>lastLogLineNumber && !ioConfig.trim(tmpStr).empty()){
            readCredentials(email,uniqueCode);
            if(isRegistration && !ioConfig.trim(email).empty() && !ioConfig.trim(uniqueCode).empty()){
                httpReturnCode = restServerConnector->sendData(email,uniqueCode,tmpStr,isRegistration);
                std::string command = "ps -q "+std::to_string(ioConfig.getProcIdByName("IdentifyOnStart"))+" -o state --no-headers";
                std::string commandValue;
                while(true){
                    std::vector <std::string> resultSet;
                    ioConfig.getCommandOutput(command,resultSet);
                    for(std::string &el: resultSet){
                        if(el.compare("T")==0){
                            commandValue = el;
                            break;
                        }
                    }
                    if(commandValue.compare("T")==0)
                        break;
                    sleep(1);
                }
                switch (httpReturnCode){
                    case 200:
                        isRegistration = false;
                        kill(ioConfig.getProcIdByName("IdentifyOnStart"),SIGCONT);
                        break;
                    case 401:
                        clearCredentials(httpReturnCode);
                        it--;
                        kill(ioConfig.getProcIdByName("IdentifyOnStart"),SIGCONT);
                        break;
                    default:
                        //problemy z serwerem (np. wylaczony)
                        clearCredentials(httpReturnCode);
                        it--;
                        kill(ioConfig.getProcIdByName("IdentifyOnStart"),SIGCONT);
                        break;
                }
            }else if(ioConfig.trim(email).length()==18 && ioConfig.trim(uniqueCode).length()==8){
                httpReturnCode = restServerConnector->sendData(email,uniqueCode,tmpStr,isRegistration);
            }
        }
        if(!ioConfig.trim(tmpStr).empty())
            if(ioConfig.trim(tmpStr).compare("500")!=0 || lastLogLineNumber!=0)
                it++;
    }  
    logsFile.close();
    lastLogLineNumber = --it;
}

void readCredentials(std::string &email,std::string &uniqueCode){
    std::string credentials("");
    std::ifstream encIndexFile(logsFilePath);
    std::getline(encIndexFile,credentials);
    encIndexFile.close();
    if(ioConfig.trim(credentials).length()==26){
        email = credentials.substr(0,18);
        uniqueCode = credentials.substr(19,8);
    }else if(ioConfig.trim(credentials).compare("500")!=0){
        showError();
    }
}

void clearCredentials(int httpReturnCode){
    std::ofstream outputLogsFile;
    outputLogsFile.open(logsFilePath,std::ios::trunc);
    if(httpReturnCode!=401)
        outputLogsFile << "500";
    outputLogsFile.close();
}

void init(){
    if(ioConfig.currentDateTime().compare(ioConfig.readLabEndDate()) >= 0){
        system("systemctl disable eiti-main.service");
        kill(getpid(),SIGSTOP);
        endExecution(0);
    }
    restServerConnector = new RestServerConnector();
    findLastLineInLogFile();
}

void showError(){
    std::ofstream outfile("/home/stud/Desktop/ERROR_"+ioConfig.currentDateTime());
    outfile << "Maszyna nie zostala prawidlowo skonfigurowana. Przed korzystaniem z maszyny musi zostac wykonana rejestracja.\n Zrestartuj maszyne i zarejestruj jeszcze raz. Jezeli rejestracja jest niemozliwa lub kolejne pliki bledu pojawiaja sie na pulpicie - zmien obraz maszyny" << std::endl;
    outfile.close();
}

void cleanup(){
    if(restServerConnector != nullptr){
        delete restServerConnector;
        restServerConnector = nullptr;
    }
}

void resume_registration(){
    (ioConfig.getProcIdByName("IdentifyOnStart") != -1 && ioConfig.getProcIdByName("ModuleService") != -1) ? 
        kill(ioConfig.getProcIdByName("IdentifyOnStart"),SIGCONT) : continueExecutionI();
}