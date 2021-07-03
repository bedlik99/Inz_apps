#include <string>
#include "InitStudentDetails.h"

using namespace std;

    InitStudentDetails::InitStudentDetails(){}

    InitStudentDetails::InitStudentDetails(string indxNum,string code,string date):
    indexNum(indxNum),uniqueCode(code),dateTime(date){}

    string InitStudentDetails::getIndexNum(){
       return indexNum;
    }

    void InitStudentDetails::setIndexNum(string indexNumb){
        indexNum = indexNumb;
    }

    string InitStudentDetails::getUniqueCode(){
        return uniqueCode;
    }

    void InitStudentDetails::setUniqueCode(string code){
        uniqueCode = code;
    }

    string InitStudentDetails::getDateTime(){
        return dateTime;
    }

    void InitStudentDetails::setDateTime(string custDate){
        dateTime=custDate;
    }


    string InitStudentDetails::toJson(){
        return "{\"indexNum\":\""+indexNum+"\",\"dateTime\":\""+dateTime+"\"}";
    }