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

const static std::string logsFolderPath = "/etc/identify_lab_data/logs_dir";
const static std::string logsFilePath = "/etc/identify_lab_data/logs_dir/main_logs";
static long long int lastLogLineNumber=0;
static bool isRegistration;
static IOConfig ioConfig;
static RestServerConnector* restServerConnector = nullptr;
static OpenSSLAesEncryptor* openSSLCryptoUtil = nullptr;
static int fd,wd1;

void continueExecutionV();
int continueExecutionI();
void endExecution(int status);
void sig_handler(int);
void processNewLogRecords();
void findLastLineInLogFile();
std::string readCredentials();
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
    std::string tmpStr;
    long long int it=1;
    std::ifstream logsFile(logsFilePath);
    while (std::getline(logsFile, tmpStr)) {
        if(it>lastLogLineNumber && !ioConfig.trim(tmpStr).empty()){
            std::string user_credentials = readCredentials();
            if(lastLogLineNumber==0 && isRegistration && !user_credentials.empty()){
                restServerConnector->sendData(user_credentials,tmpStr,isRegistration);
                isRegistration = false;
            }else if(!user_credentials.empty()){
                restServerConnector->sendData(user_credentials,tmpStr,isRegistration);
            }
        }
        it++;
    }  
    logsFile.close();
    lastLogLineNumber = --it;
}

std::string readCredentials(){
    std::string credentials;
    std::ifstream encIndexFile(logsFilePath);
    std::getline(encIndexFile,credentials);
    encIndexFile.close();
    if(ioConfig.trim(credentials).empty()){
        showError();
        return "";
    }
    return (credentials.substr(0,6)+credentials.substr(7,6));
}

void init(){
    restServerConnector = new RestServerConnector();
    openSSLCryptoUtil = new OpenSSLAesEncryptor();
    findLastLineInLogFile();
}

void showError(){
    system("notify-send -u critical [\"STATUS: ERROR\"] \"Maszyna nie jest prawidlowo zidentyfikowana. \nNie korzystaj z tej maszyny.\"");
    std::ofstream outfile("/home/stud/Desktop/user_Error");
    outfile << "Maszyna nie zostala prawidlowo zidentyfikowana. Nie korzystaj z tej maszyny." << std::endl;
    outfile.close();
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

void resume_registration(){
    (ioConfig.getProcIdByName("IdentifyOnStart") != -1 && ioConfig.getProcIdByName("ModuleService") != -1) ? 
        kill(ioConfig.getProcIdByName("IdentifyOnStart"),SIGCONT) : continueExecutionI();
}