#include <string>

class InitStudentDetails {
private:
    std::string indexNum;
    std::string uniqueCode;
    std::string dateTime;

public:
    InitStudentDetails();
    InitStudentDetails(std::string indexNum,std::string uniqueCode,std::string dateTime);

    std::string getIndexNum();
    void setIndexNum(std::string index);

    std::string getUniqueCode();
    void setUniqueCode(std::string code);

    std::string getDateTime();
    void setDateTime(std::string dateTime);

    std::string toJson();

};