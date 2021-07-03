#include <string>

class FinalStudentDetails {
private:
    std::string secretIndexNum;
    std::string indexNum;
    std::string uniqueCode;
    int grade;
    std::string dateTime;

public:
    FinalStudentDetails();
    FinalStudentDetails(std::string secretIndexNum,std::string indexNum,std::string uniqueCode,int grade,std::string dateTime);

    std::string getSecretIndexNum();
    void setSecretIndexNum(std::string secretIndex);

    std::string getIndexNum();
    void setIndexNum(std::string index);

    std::string getUniqueCode();
    void setUniqueCode(std::string code);

    int getGrade();
    void setGrade(int grade);

    std::string getDateTime();
    void setDateTime(std::string dateTime);

    std::string toJson();

};