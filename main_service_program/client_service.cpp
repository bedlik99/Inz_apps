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

#define EVENT_SIZE (sizeof(struct inotify_event))
#define BUF_LEN (1024 * (EVENT_SIZE + 16))

namespace fs = std::filesystem;

const std::string path = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/outputFiles";
const char *watchedPath = "/home/jan/Documents/inz_dyp/Projekty_C++/working_folder_inz/outputFiles";
static std::set<std::string> filePaths;

void addFileNamesToSet();
void readContentOfFile(std::string);
void sig_handler(int);

int fd, wd;

int main(void) {

    signal(SIGINT, sig_handler);
    /* Step 1. Initialize inotify */
    fd = inotify_init();

    if (fcntl(fd, F_SETFL, O_NONBLOCK) < 0) // error checking for fcntl
        exit(2);

    /* Step 2. Add Watch */
    wd = inotify_add_watch(fd, watchedPath, IN_MODIFY | IN_CREATE);

    if (wd == -1){
        printf("Could not watch : %s\n", watchedPath);
    } else {
        printf("Watching : %s\n", watchedPath);
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
                if (event->mask & IN_CREATE) {
                    if (event->mask & IN_ISDIR) {
                        printf("The directory %s was created.\n", event->name);
                    }
                    else {
                        printf("The file %s was created.\n", event->name);
                        // PLIK TWORZY SIE PUSTY!

                    }
                } else if (event->mask & IN_MODIFY) {
                    if (event->mask & IN_ISDIR) {
                        printf("The directory %s was modified.\n", event->name);

                    } else {
                        printf("The file %s was modified.\n", event->name);
                        // DANE SA ZAPISYWANE I MOZNA NA NICH PRACOWAC
                        // TUTAJ RACZEJ BEDZIEMY TYLKO KORZYSTAC EW. TWORZENIE PLIKU

                        //DALSZE INSTRUKCJE - (REST-KLIENT) - WYSYLANIE DANYCH NA SERWER.
                        addFileNamesToSet();
                        for (auto i = filePaths.begin(); i != filePaths.end(); i++) {
                            readContentOfFile(*i);
                        }

                    }
                }
            }
            i += EVENT_SIZE + event->len;
        }
        
    }

    sig_handler(-1);
    return -1;
}

void sig_handler(int sig) {
    /* Step 5. Remove the watch descriptor and close the inotify instance*/
    inotify_rm_watch(fd, wd);
    close(fd);
    exit(sig);
}

void addFileNamesToSet() {
    std::string filePath;
    for (const auto &entry : fs::directory_iterator(path)) {
        filePath = entry.path().string();

        if (entry.is_regular_file() && filePath.substr(filePath.size() - 3, filePath.size() - 1) == "txt") {
            filePaths.insert(filePath);
        }
    }
}

void readContentOfFile(std::string pathOfFile) {
    std::ifstream tmpFileStream(pathOfFile);
    std::string fileContents((std::istreambuf_iterator<char>(tmpFileStream)),
                             std::istreambuf_iterator<char>());
    std::cout << fileContents << std::endl;
    // NEXT PARSE CONTENTS OF THE FILE TO e.g. JSON FORMAT AND SEND IT ON SERVER
}
