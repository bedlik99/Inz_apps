#include <string>
#include "FinalStudentDetails.h"

using namespace std;

    FinalStudentDetails::FinalStudentDetails(){}

    FinalStudentDetails::FinalStudentDetails(string secretIndex,string indxNum,string uniqueCode,int grade,string date) :
    secretIndexNum(secretIndex),indexNum(indxNum),uniqueCode(uniqueCode),grade(grade),dateTime(date) {}

    string FinalStudentDetails::getSecretIndexNum(){
        return secretIndexNum;
    }

    void FinalStudentDetails::setSecretIndexNum(string secretIndex){
        secretIndexNum = secretIndex;
    }

    string FinalStudentDetails::getIndexNum(){
       return indexNum;
    }

    void FinalStudentDetails::setIndexNum(string indexNumb){
        indexNum = indexNumb;
    }

    string FinalStudentDetails::getUniqueCode(){
        return uniqueCode;
    }

    void FinalStudentDetails::setUniqueCode(string code){
        uniqueCode = code;
    }

    int FinalStudentDetails::getGrade(){
       return grade;
    }

    void FinalStudentDetails::setGrade(int gr){
        grade = gr;
    }

    string FinalStudentDetails::getDateTime(){
        return dateTime;
    }

    void FinalStudentDetails::setDateTime(string custDate){
        dateTime=custDate;
    }

    string FinalStudentDetails::toJson(){
        return "{\"secretIndexNum\":\""+secretIndexNum
        +"\",\"uniqueCode\":\""+uniqueCode
        +"\",\"indexNum\":\""+indexNum
        +"\",\"grade\":"+to_string(grade)
        +",\"dateTime\":\""+dateTime
        +"\"}";
    }






