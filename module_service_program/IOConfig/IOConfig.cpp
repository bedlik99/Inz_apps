#include <string>
#include <iostream>
#include <fstream>
#include <sys/stat.h>
#include <dirent.h>
#include "IOConfig.h"

IOConfig::IOConfig(){}

bool IOConfig::doesFileExist(const std::string& name) {
  struct stat buffer;   
  return (stat (name.c_str(), &buffer) == 0); 
}

bool IOConfig::isFileEmpty(const std::string filePath){
    std::ifstream t(filePath);
    std::string secretHash((std::istreambuf_iterator<char>(t)),std::istreambuf_iterator<char>());
    std::string trimmedSecretFile = trim(secretHash);
    if(trimmedSecretFile.length() == 0){
        return true;
    }
    return false;
}

std::string IOConfig::ltrim(std::string str){
    str.erase(0, str.find_first_not_of(chars));
    return str;
}

std::string IOConfig::rtrim(std::string str){
    str.erase(str.find_last_not_of(chars) + 1);
    return str;
}

std::string IOConfig::trim(std::string str){
    std::string lrTrimmed = ltrim(rtrim(str));
    std::string fullyTrimmed="";
    int ascii_char_code;
    for(int i=0;i<lrTrimmed.length();i++){
      ascii_char_code = (int)lrTrimmed.at(i);
      if((ascii_char_code < 9 || ascii_char_code > 13) && ascii_char_code!=32){
        fullyTrimmed = fullyTrimmed+lrTrimmed.at(i);
      }
    }
    return fullyTrimmed;
}

//It will return the size of the file in bytes OR -1 in case that it cannot get any status information for it
std::string IOConfig::get_file_size(const char *filename) {
  //Specialised struct that can hold status attributes of files.
  struct stat st;
 
  //Gets file attributes for filename and puts them in the stat buffer.
  // Upon successful completion, it returns 0, otherwise and errno will be set to indicate the error.
  if (stat(filename, &st) == 0) {
    std::string fileSize = std::to_string(st.st_size);
    int delimeterPos;
    if(fileSize.length() < 4){
      fileSize=fileSize+" B";
    }else if(fileSize.length() >= 4 && fileSize.length() < 7){
      delimeterPos = fileSize.length()-3;
      fileSize = fileSize.substr(0,delimeterPos) + "," + fileSize.substr(delimeterPos,1) + " kB";
    }else{
      delimeterPos = fileSize.length()-6;
      fileSize = fileSize.substr(0,delimeterPos) + "," + fileSize.substr(delimeterPos,1) + " MB";
    }
    //Size of file, in bytes.
    return fileSize;
  }
 
  return "-1";
}

int IOConfig::getProcIdByName(std::string procName){
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
                std::string cmdPath = std::string("/proc/") + dirp->d_name + "/cmdline";
                std::ifstream cmdFile(cmdPath.c_str());
                std::string cmdLine;
                std::getline(cmdFile, cmdLine);
                if (!cmdLine.empty()){
                    // Keep first cmdline item which contains the program path
                    size_t pos = cmdLine.find('\0');
                    if (pos != std::string::npos)
                        cmdLine = cmdLine.substr(0, pos);
                    // Keep program name only, removing the path
                    pos = cmdLine.rfind('/');
                    if (pos != std::string::npos)
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

bool IOConfig::areCredentialsPresent(){
  std::string credentials;
  std::ifstream logsFile("/etc/identify_lab_data/logs_dir/main_logs");
  std::getline(logsFile,credentials);
  logsFile.close();
  if(trim(credentials).empty()){
    return false;
  }
  return true;
}