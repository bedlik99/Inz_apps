#include "UserDetailsDTO.h"

using namespace std;

UserDetailsDTO::UserDetailsDTO(){}

UserDetailsDTO::UserDetailsDTO(std::string inputIndex,std::string inputUniqueCode):
indexNr(inputIndex),uniqueCode(inputUniqueCode){}

const string& UserDetailsDTO::getIndex(){
    return indexNr;
}

const string& UserDetailsDTO::getUniqueCode(){
    return uniqueCode;
}

void UserDetailsDTO::setIndex(string inputIndex){
    indexNr = inputIndex;
}

void UserDetailsDTO::setUniqueCode(string code){
    uniqueCode = code;
}

const string UserDetailsDTO::toJson(){
        return "{\"uniqueCode\":\""+uniqueCode
        +"\",\"indexNr\":\""+indexNr
        +"\"}";
}



