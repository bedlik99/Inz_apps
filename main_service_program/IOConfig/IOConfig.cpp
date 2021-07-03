#include <string>
#include <iostream>
#include <fstream>
#include <sys/stat.h>
#include "IOConfig.h"

IOConfig::IOConfig(){}

bool IOConfig::doesFileExist(const std::string& name) {
  struct stat buffer;   
  return (stat (name.c_str(), &buffer) == 0); 
}

bool IOConfig::isSecretFileEmpty(const std::string secretFile, bool &wasSecretFileInitProperly){
    std::ifstream t(secretFile);
    std::string secretHash((std::istreambuf_iterator<char>(t)),std::istreambuf_iterator<char>());
    std::string trimmedSecretFile = trim(secretHash);

    if(trimmedSecretFile.length() == 0){
        return true;
    }

    wasSecretFileInitProperly = true;
    return false;
}

std::string& IOConfig::ltrim(std::string& str)
{
    str.erase(0, str.find_first_not_of(chars));
    return str;
}
 
std::string& IOConfig::rtrim(std::string& str)
{
    str.erase(str.find_last_not_of(chars) + 1);
    return str;
}
 
std::string& IOConfig::trim(std::string& str)
{
    return ltrim(rtrim(str));
}